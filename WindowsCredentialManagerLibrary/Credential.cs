using System;
using System.Security;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

using ptr = System.IntPtr;

namespace WindowsCredentialManagerLibrary
{
	public sealed class Credential : IDisposable
	{

		public byte[] this[string keyword]
		{
			get
			{
				if(_disposed)
					throw new ObjectDisposedException($"{Properties.Resources.ExAttributeRead}: {keyword}");

				for(int i = 0; i < _cred.AttributeCount; i++)
				{
					CREDENTIAL_ATTRIBUTE a = Marshal.PtrToStructure<CREDENTIAL_ATTRIBUTE>(AttributeRef(i));
					if(a.Keyword == keyword)
						return a.GetValueAsByteArray();
				}
				return null;
			}

			set
			{
				if(_disposed)
					throw new ObjectDisposedException($"{Properties.Resources.ExAttributeWrite}: {keyword}");

				if(!_need_heavy_mem_mgmt)
					throw new NotImplementedException(string.Format(Properties.Resources.ExSettingAttributesOnRegisteredCredentialNotAllowed, keyword));

				for(int i = 0; i < _cred.AttributeCount; i++)
				{
					using(CREDENTIAL_ATTRIBUTE a = Marshal.PtrToStructure<CREDENTIAL_ATTRIBUTE>(AttributeRef(i)))
						if(a.Keyword == keyword)
						{
							a.SetValueFromByteArray(value);
							return;
						}
				}

				_cred.Attributes = Marshal.ReAllocHGlobal(_cred.Attributes, (ptr)(++_cred.AttributeCount * Marshal.SizeOf<CREDENTIAL_ATTRIBUTE>()));
				Marshal.StructureToPtr(new CREDENTIAL_ATTRIBUTE(keyword, value), _cred.Attributes + Marshal.SizeOf<CREDENTIAL_ATTRIBUTE>() * (_cred.AttributeCount - 1), false);
			}
		}

		public string Username
		{
			get
			{
				if(!_has_live_secrets)
				{
					Interop.ExtractSecrets(_cred.CredentialBlob, _cred.CredentialBlobSize, out _usernm, out _domain, out _passwd);
					_has_live_secrets = true;
				}
				return _usernm;
			}

			private set => _usernm = value;
		}

		public string Domain
		{
			get
			{
				if(!_has_live_secrets)
				{
					Interop.ExtractSecrets(_cred.CredentialBlob, _cred.CredentialBlobSize, out _usernm, out _domain, out _passwd);
					_has_live_secrets = true;
				}
				return _domain;
			}

			private set => _domain = value;
		}

		public Str Password
		{
			get
			{
				if(_disposed) throw new ObjectDisposedException(Properties.Resources.ExTriedToReadPasswordDisposed);
				if(!_has_live_secrets)
				{
					Interop.ExtractSecrets(_cred.CredentialBlob, _cred.CredentialBlobSize, out _usernm, out _domain, out _passwd);
					_has_live_secrets = true;
				}
				return _passwd;
			}
		}

		private CREDENTIAL _cred;

		private ptr _credp = default;
		private bool _need_heavy_mem_mgmt = false;
		private bool _has_live_secrets = false;
		private bool _disposed = false;
		private string _domain;
		private string _usernm;
		private Str _passwd;

		private Credential(ptr cred)
		{
			_credp = cred;
			_cred = Marshal.PtrToStructure<CREDENTIAL>(cred);

			int sum = 0;
			for(int i = 0; i < _cred.AttributeCount; i++)
			{
				CREDENTIAL_ATTRIBUTE a = Marshal.PtrToStructure<CREDENTIAL_ATTRIBUTE>(AttributeRef(i));
				sum += a.ValueSize;
			}

			GC.AddMemoryPressure(sum + Marshal.SizeOf<CREDENTIAL>() + Marshal.SizeOf<CREDENTIAL_ATTRIBUTE>() * _cred.AttributeCount);
		}

		private Credential(ptr blob, int blobsz, string name, string comment)
		{
			_cred = new CREDENTIAL(blob, blobsz, name, comment);
			_need_heavy_mem_mgmt = true;

			GC.AddMemoryPressure(blobsz);
		}

		~Credential() => Dispose();

		public void Dispose()
		{
			if(_disposed)
				throw new ObjectDisposedException(Properties.Resources.ExTriedToDisposeDisposedCred);

			if(_has_live_secrets)
				_passwd.Dispose();

			if(_need_heavy_mem_mgmt)
			{
				unsafe
				{
					byte* b = (byte*)_cred.CredentialBlob.ToPointer();
					for(int i = 0; i < _cred.CredentialBlobSize; b[i++] = 0) ;
				}

				Marshal.FreeCoTaskMem(_cred.CredentialBlob);
				_cred.CredentialBlob = ptr.Zero;

				for(int i = 0; i < _cred.AttributeCount; i++)
					Marshal.DestroyStructure<CREDENTIAL_ATTRIBUTE>(AttributeRef(i));
				Marshal.FreeHGlobal(_cred.Attributes);
			}
			else
				Interop.CredFree(_credp);

			GC.SuppressFinalize(this);
			_disposed = true;
		}

		private ptr AttributeRef(int i) => _cred.Attributes + Marshal.SizeOf<CREDENTIAL_ATTRIBUTE>() * i;

		public static Credential Read(string name, CredentialType type = CredentialType.Generic) => Interop.CredRead(name, type, 0, out ptr cred) ? new Credential(cred) : null;

		public bool Write(bool preserve = false) => Interop.CredWrite(ref _cred, preserve);

		public static bool Delete(string name, CredentialType type = CredentialType.Generic) => Interop.CredDelete(name, type);

		public bool Delete() => Interop.CredDelete(_cred.TargetName, _cred.Type);

		public static Credential GetGenericCredentialFromLoginPrompt(string name, string caption, string message, ref bool save, bool showSaveCbx = true, Err err = Err.Success, string comment = default, ptr parent = default)
		{
			CREDUI_INFO ci = new CREDUI_INFO(caption, message, parent);
			int authpkg = default;

			if(Interop.CredUIPromptForWindowsCredentials(ref ci, err, ref authpkg, default, default, out ptr blob, out int blobsz, ref save, CREDUIWIN.GENERIC | (showSaveCbx ? CREDUIWIN.CHECKBOX : 0)) != Err.Success)
				return null;

			return new Credential(blob, blobsz, name, comment);
		}
	}
}
