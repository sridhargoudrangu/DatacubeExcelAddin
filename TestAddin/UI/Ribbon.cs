using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TestAddin.Properties;
using Microsoft.Office.Core;

using IRC = Microsoft.Office.Core.IRibbonControl;
using IRUI = Microsoft.Office.Core.IRibbonUI;

namespace TestAddin
{
	[ComVisible(true)]
	public class Ribbon : IRibbonExtensibility
	{
		// Here's an ALMOST duplicate of Authentication._hostname
		// *This* string is updated everytime the content in the hostname 
		// textbox in the login group changes.
		// That makes this variable more volatile than the one in the Authentication
		// class. This one may contain a "hostname" that doesn't exist, for example.
		public static string _host = null;

		// This variable specifies whether the autologin failed.
		// If it did, then we want to communicate that to the user.
		public static bool _savedloginfail = false;

		private static IRUI ribbon;
		public static void Invalidate() => ribbon.Invalidate();

		#region Ribbon Callbacks

		#region Labels

		// These methods exist for localization purposes
		public string label_TaskButtonDataCube(IRC ctrl) => Resources.DataCube;
		public string label_grpDataCubeLogin(IRC ctrl) => Resources.NotLoggedIn;
		public string htext_grpDataCubeLogin(IRC ctrl) => _savedloginfail ? Resources.SavedLoginFailure : Resources.PleaseLogin;
		public string label_btnDataCubeLogin(IRC ctrl) => Resources.Login;
		public string label_edtDataCubeLoginHostname(IRC ctrl) => Resources.WebconsoleHostname;
		public string label_grpDataCubeImport(IRC ctrl) => Resources.ImportDataCube;
		public string htext_grpDataCubeImport(IRC ctrl) => Resources.ImportFlavorText;
		public string label_btnDataCubeImport(IRC ctrl) => Resources.Import;
		public string label_grpDataCubeExport(IRC ctrl) => Resources.ExportDataCube;
		public string htext_grpDataCubeExport(IRC ctrl) => IsExportEnabled(null) ? Resources.ExportFlavorText : Resources.NoActiveWorkbook;
		public string label_btnDataCubeExport(IRC ctrl) => Resources.Export;
		public string label_btnDataCubeLogout(IRC ctrl) => Resources.Logout;
		public string label_lblDataCubeLogoutInfo(IRC ctrl) => Resources.YouWillBeLoggedOut;
		public string label_btnDataCubeForget(IRC ctrl) => Resources.ForgetCreds;
		public string label_lblDataCubeForgetInfo(IRC ctrl) => Resources.PlaceholderText;
		public string label_ribbonGroupLblDatacube(IRC ctrl) => Resources.DataCube;
		public string label_ribbonGroupLblImport(IRC ctrl) => Resources.Import;
		public string label_ribbonGroupLblExport(IRC ctrl) => Resources.Export;

		#endregion

		#region Functions

		// These methods provide interactivity to the ribbon and allow it to have dynamic
		// enabling/disabling of buttons/groups

		public void Ribbon_Load(IRUI r) => ribbon = r;

		public void HostnameChanged(IRC ctrl, string host) => _host = host;

		public bool IsExportEnabled(IRC ctrl) => Useful.Workbooks.Count > 0;

		public void RibbonButtonClick(IRC ctrl)
		{
			if(Authentication.LoggedIn)
				ThisAddIn.CreateTaskPane(ctrl.Tag[0], _host);
			else if(Globals.ThisAddIn.Application.InputBox(Resources.NeedHostname, Resources.HostnameCaps) is string hostname && Authentication.Login(hostname))
				ThisAddIn.CreateTaskPane(ctrl.Tag[0], _host = hostname);
		}
		public void BackstageButtonClick(IRC ctrl) => ThisAddIn.CreateTaskPane(ctrl.Tag[0], _host);

		public void Login(IRC ctrl) => Authentication.Login(_host);
		public void Logout(IRC ctrl) => Authentication.Logout();

		public bool AreWeLoggedIn(IRC ctrl) => Authentication.LoggedIn;
		public bool AreWeLoggedOut(IRC ctrl) => !Authentication.LoggedIn;
		public bool AreCredentialsSaved(IRC ctrl) => Settings.Default.SaveLoginInformation;

		public void ForgetCredentials(IRC ctrl) => Authentication.DeleteCreds();

		#endregion

		#endregion

		public string GetCustomUI(string ribbonID)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			foreach(string s in asm.GetManifestResourceNames())
				if(string.Compare("TestAddin.UI.Ribbon.xml", s, StringComparison.OrdinalIgnoreCase) == 0)
					using(StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(s)))
						return resourceReader?.ReadToEnd();
			return null;
		}
	}
}
