using System;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

using dll = System.Runtime.InteropServices.DllImportAttribute;
using ptr = System.IntPtr;

namespace WindowsCredentialManagerLibrary
{
	internal static class Interop
	{
		#region Consts

		private const string advapi32 = "advapi32";
		private const string credui = "credui";

		#endregion
		#region WinApi

		[dll(advapi32, SetLastError = true, CharSet = CharSet.Unicode)] internal static extern bool CredRead(string name, CredentialType type, int flags, out ptr cred);
		[dll(advapi32, SetLastError = true, CharSet = CharSet.Unicode)] internal static extern bool CredWrite([In] ref CREDENTIAL cred, bool preserve = false);
		[dll(advapi32, SetLastError = true, CharSet = CharSet.Unicode)] internal static extern bool CredDelete(string name, CredentialType type, int flags = 0);
		[dll(advapi32)] internal static extern void CredFree(ptr p);
		[dll(credui, SetLastError = true, CharSet = CharSet.Unicode)] private static extern bool CredUnPackAuthenticationBuffer(CRED_PACK flags, ptr blob, int blobsz, StringBuilder un, ref int unsz, StringBuilder dn, ref int dnsz, StringBuilder pw, ref int pwsz);
		[dll(credui, CharSet = CharSet.Unicode)] internal static extern Err CredUIPromptForWindowsCredentials([In] ref CREDUI_INFO info, Err err, ref int authpkg, ptr inblob, int inblobsz, out ptr outblob, out int outblobsz, ref bool save, CREDUIWIN flags = CREDUIWIN.CHECKBOX | CREDUIWIN.GENERIC);

		#endregion

		internal static void ExtractSecrets(ptr blob, int blobsz, out string d1, out string d2, out Str d3)
		{
			int z1 = 0, z2 = 0, z3 = 0;
			CredUnPackAuthenticationBuffer(CRED_PACK.PROTECTED_CREDENTIALS, blob, blobsz, null, ref z1, null, ref z2, null, ref z3);
			StringBuilder s1 = new StringBuilder(z1), s2 = new StringBuilder(z2), s3 = new StringBuilder(z3);
			CredUnPackAuthenticationBuffer(CRED_PACK.PROTECTED_CREDENTIALS, blob, blobsz, s1, ref z1, s2, ref z2, s3, ref z3);
			d1 = s1.ToString();
			d2 = s2.ToString();
			d3 = new Str(s3);
		}
	}

	#region Structs

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CREDENTIAL
	{
		public CredentialFlags Flags;
		public CredentialType Type;
		public string TargetName;
		public string Comment;
		public long LastWritten;
		public int CredentialBlobSize;
		public ptr CredentialBlob;
		public CredentialPersistence Persist;
		public int AttributeCount;
		public ptr Attributes;
		public string TargetAlias;
		public string Username;

		public CREDENTIAL(ptr blob, int blobsz, string name, string comment = default) : this()
		{
			Type = CredentialType.Generic;
			TargetName = name;
			Comment = comment;
			CredentialBlobSize = blobsz;
			CredentialBlob = blob;
			Persist = CredentialPersistence.LocalMachine;
			Attributes = Marshal.AllocHGlobal(0);
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CREDENTIAL_ATTRIBUTE: IDisposable
	{
		public string Keyword;
		public int Flags;
		public int ValueSize;
		public ptr Value;

		public CREDENTIAL_ATTRIBUTE(string k, byte[] v) : this()
		{
			Keyword = k;
			GC.AddMemoryPressure(ValueSize = v.Length);
			Value = Marshal.AllocHGlobal(v.Length);

			unsafe
			{
				byte* x = (byte*) Value.ToPointer();
				for(int i = 0; i < v.Length; i++)
					x[i] = v[i];
			}
		}

		public void SetValueFromByteArray(byte[] a)
		{
			Value = Marshal.ReAllocHGlobal(Value, (ptr) a.Length);
			ValueSize = a.Length;

			unsafe
			{
				byte* y = (byte*) Value.ToPointer();
				for(int i = 0; i < ValueSize; i++)
					y[i] = a[i];
			}
		}

		public byte[] GetValueAsByteArray()
		{
			byte[] x = new byte[ValueSize];

			unsafe
			{
				byte* y = (byte*) Value.ToPointer();
				for(int i = 0; i < x.Length; i++)
					x[i] = y[i];
			}

			return x;
		}

		public void Dispose()
		{
			Marshal.FreeHGlobal(Value);
			GC.RemoveMemoryPressure(ValueSize);
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CREDUI_INFO
	{
		public int cbSize;
		public ptr hwndParent;
		public string pszMessageText;
		public string pszCaptionText;
		public ptr hbmBanner;

		public CREDUI_INFO(string caption, string message, ptr parent = default) : this()
		{
			cbSize = Marshal.SizeOf<CREDUI_INFO>();
			pszCaptionText = caption;
			pszMessageText = message;
			hwndParent = parent;
		}
	}

	#endregion
	#region Enums

	public enum Err
	{
		Success = 0,
		GeneralFailure = 31,
		Cancelled = 1223,
		NoSuchLogonSession = 1312,
		LogonFailure = 1326
	}

	[Flags]
	public enum CredentialFlags: int
	{
		None = 0x0,
		PromptNow = 0x2,
		UsernameTarget = 0x4
	}

	public enum CredentialType: int
	{
		Generic = 0x1,
		DomainPassword = 0x2,
		DomainCertificate = 0x3,
		DomainVisiblePassword = 0x4,
		GenericCertificate = 0x5,
		DomainExtended = 0x6,
		Maximum = 0x7,
		MaximumEx = Maximum + 1000
	}

	public enum CredentialPersistence: int
	{
		Session = 0x1,
		LocalMachine = 0x2,
		Enterprise = 0x3
	}

	[Flags]
	internal enum CRED_PACK: int
	{
		PROTECTED_CREDENTIALS = 0x1,
		WOW_BUFFER = 0x2,
		GENERIC_CREDENTIALS = 0x4,
		ID_PROVIDER_CREDENTIALS = 0x8
	}

	[Flags]
	internal enum CREDUIWIN: int
	{
		GENERIC = 0x1,
		CHECKBOX = 0x2,
		AUTHPACKAGE_ONLY = 0x10,
		IN_CRED_ONLY = 0x20,
		ENUMERATE_ADMINS = 0x100,
		ENUMERATE_CURRENT_USER = 0x200,
		SECURE_PROMPT = 0x1000,
		PREPROMPTING = 0x2000,
		PACK_32_WOW = 0x10000000
	}

	#endregion
}
