using System;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using TestAddin.Properties;
using WindowsCredentialManagerLibrary;

// The Authentication class
// Communicates with a server to log a user into the rest API
namespace TestAddin
{
	internal static partial class Authentication
	{
		// Are we logged in?
		// Ease of Use AutoProperty
		public static bool LoggedIn { get => !(LoginToken is null); }

		public const string CredName = "commvault_exceldatacubeaddin_savedlogin";

		// Saved login token, gets zeroed when it gets garbage collected
		public static Str LoginToken { get; private set; } = null;

		// Saved hostname
		public static string _hostname = null;

		private static Credential Prompt(string hostname, ref bool save, bool err = false, bool cbx = true)
			=> Credential.GetGenericCredentialFromLoginPrompt(
					CredName,
					string.Format(Resources.ConnectingToHostname, hostname),
					Resources.PromptEnterCreds,
					ref save,
					cbx,
					err ? Err.LogonFailure : Err.Success,
					Resources.CredComment,
					(IntPtr)Globals.ThisAddIn.Application.Hwnd
				);

		private static bool ServerSideLogin(string hostname, Credential c, string commserver = "")
		{
			LoginInformation li;
			string response;

			// Spooky code ahead
			unsafe
			{
				string d1, d2, d3;

				//The password and its base64 are now in a string that cannot
				//move around in memory due to the fixed clause
				fixed (char* p1 = d1 = c.Password.ToString(), p2 = d2 = d1.ToBase64())
				{
					// We don't need the raw password in memory after making the base64
					// Zero it fast!
					for(int i = 0; i < d1.Length; p1[i++] = default) ;

					// Important note immediately following this declaration
					UserCreds u = new UserCreds()
					{
						domain = c.Domain,
						username = c.Username,
						password = d2,
						commserver = commserver
					};

					// Since the string password in the UserCreds struct is just a reference to the base64 string d2,
					// We only have to zero the memory at p2 = d2 in order to delete the password in the struct as well.

					/* Grab a pointer to the serialized struct -- it has the base64 password in it! */
					fixed (char* p3 = d3 = Json.Serializer.Serialize(u))
					{
						// Kill the base64 password that isn't in the serialized string
						for(int i = 0; i < d2.Length; p2[i++] = default) ;

						// HERE COMES THE FUN PART
						// We begin a region of no garbage collection since the response string produced
						// by RestAPI.Execute could POTENTIALLY be copied around in memory before we get
						// the chance to zero it. Here we pre-allocate 2 MiB for any objects that are
						// created in the no GC zone.
						GC.TryStartNoGCRegion(1048576 * 2);

						// We call the API and get a response
						bool success = new Api.Login(hostname, d3).Execute(out response);

						// Zero out the api call body since that has the base64 password in it.
						for(int i = 0; i < d3.Length; p3[i++] = default) ;

						if(!success)
						{
							// If we're in here, the api call failed and we don't have any secrets in the response.
							// End the no GC region since that string is 'safe'.
							GC.EndNoGCRegion();

							// We failed.
							return false;
						}
					}
				}
			}

			// NO GC REGION IS STILL IN EFFECT HERE

			try
			{
				// Quick, get the response deserialized so we can get out of the no GC zone!
				li = Json.Serializer.Deserialize<LoginInformation>(response);
				if(!(li.token is null))
				{
					/* Oh hey we have a login token in the response */
					unsafe
					{
						// Zero out the response string since it has a login token in it
						// We don't have to worry about fixing the response string in place
						// before this since we're in a no GC zone
						fixed (char* r = response)
							for(int i = 0; i < response.Length; r[i++] = default) ;

						// Make a pinned string with the login token in it.
						// This constructor zeros the input string as it copies it
						// so that's why we don't have another fixed block here.
						LoginToken = new Str(li.token);
					}
				}
			}
			catch(Exception e)
			{
				// Something went horribly wrong, this message may be helpful.
				MessageBox.Show(string.Format(Resources.ExceptionInLogon, hostname, e.Message));

				/*
					Don't worry about the No GC region! The finally block ALWAYS executes:
					"""Within a handled exception, the associated finally block is guaranteed to be run."""
					https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-finally
				*/

				// Failure
				return false;
			}
			finally
			{
				// Let's get rid of this
				GC.EndNoGCRegion();
			}

			/* No errors == We're logged in */
			if(li.errList.Length == 0)
			{
				// Save the hostname for later
				_hostname = hostname;
				return true; // Success
			}

			// Failure, we have at least one error in the errorlist
			return false;
		}

		public static void DeleteCreds()
		{
			Credential.Delete(CredName); // Remove the credential from the windows cred manager
			Settings.Default.SaveLoginInformation = false; // We are no longer saving login information
			Settings.Default.Save(); // Make the program remember the above comment
			Ribbon.Invalidate(); // update the ui
		}

		public static bool AutoLogin()
		{
			// Credential class has some memory management in it so it can be disposed
			using(Credential c = Credential.Read(CredName))
			{
				if(c is null) // Read failed or no credential
				{
					DeleteCreds(); // No credential = no longer saving creds
					return false; // login fails
				}

				// We have a credential, do we have a login?
				if(ServerSideLogin(c[Resources.Hostname].MakeIntoString(), c))
				{
					// yes, update ui and return true
					Ribbon.Invalidate();
					return true;
				}
				else
				{
					// no, we have 0 logins, the old credential doesn't work??
					// user updated their password?
					// Either way, we don't have a fresh username/password combo
					// so we are not going to save the old ones.
					DeleteCreds();
					return false;
				}
			}
		}

		public static bool ReEstablishLogin()
		{
			// This is a specialized version of AutoLogin that was made with the 30 minute
			// idle-kick in mind.
			// 
			//     >>> After 30 minutes of inactivity, your login token will expire.
			// 
			// That being said, if we're remembering your password, we should automatically
			// log you back in. It'd be real bad if, when we were remembering your login creds,
			// a login box popped up and asked for them a second time.
			using(Credential c = Credential.Read(CredName))
			{
				if(c is null)
				{
					// This is the main change from AutoLogin
					// If we don't have a credential saved, then we might have a hostname
					// from an earlier login -- here we'll try to use that
					if(_hostname is null)
					{
						// We don't have one, give up
						LoginToken = null;
						return false;
					}

					// Try logging into the saved hostname
					return Login(_hostname);
				}

				// See AutoLogin, this is a copy/paste
				if(ServerSideLogin(c[Resources.Hostname].MakeIntoString(), c))
				{
					Ribbon.Invalidate();
					return true;
				}
				else
				{
					LoginToken = null;
					DeleteCreds();
					return false;
				}
			}
		}

		public static bool Login(string hostname, bool cbx = true)
		{
			// Main entry point for logging in.

			// This bool controls the checkbox on the login form.
			bool save = cbx ? Settings.Default.SaveLoginInformation : false;

			// Get the username/password from the user in the form of a credential
			Credential c = Prompt(hostname, ref save, false, cbx);

			// This if is triggered when the user clicks "cancel" on the login box
			if(c is null)
				return false; // User no longer wishes to login

			// While we are unable to login on the serverside...
			while(!ServerSideLogin(hostname, c))
			{
				// Get rid of the old credential that doesn't work...
				c.Dispose();

				// Get a new one that isn't null
				if((c = Prompt(hostname, ref save, true, cbx)) is null)
					return false;

				// and retry serverside login
			}

			if(save)
			{
				// user wants us to save their id/pass
				Settings.Default.SaveLoginInformation = true;
				Settings.Default.Save();
				c[Resources.Hostname] = hostname.ToByteArray();
				c.Write();
			}

			// clear out the credential
			c.Dispose();

			// update the ui
			Ribbon.Invalidate();
			return true; // we're logged in
		}

		public static void Logout()
		{
			// there's no logout rest api, so this is the best we can do.
			LoginToken = null;
			Ribbon.Invalidate();
		}

	}
}
