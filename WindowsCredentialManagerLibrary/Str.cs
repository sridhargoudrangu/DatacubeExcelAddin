using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCredentialManagerLibrary
{
	public class Str : IDisposable
	{
		private GCHandle _pin;
		private char[] _buf;

		public char this[int index]
		{
			get => _buf[index];
			set => _buf[index] = value;
		}

		public int Length { get => _buf.Length; }

		public Str(int length)
		{
			_pin = GCHandle.Alloc(_buf = new char[length], GCHandleType.Pinned);
		}

		public Str(StringBuilder str) : this(str.Length)
		{
			for(int i = 0; i < str.Length; str[i++] = default)
				_buf[i] = str[i];
		}

		public Str(string s) : this(s.Length)
		{
			unsafe
			{
				fixed (char* ss = s)
					for(int i = 0; i < s.Length; ss[i++] = default)
						_buf[i] = ss[i];
			}
		}

		~Str()
		{
			Dispose();
			_pin.Free();
		}

		public void Dispose()
		{
			for(int i = 0; i < _buf.Length; _buf[i++] = default) ;
		}

		public override string ToString() => new string(_buf);
	}
}
