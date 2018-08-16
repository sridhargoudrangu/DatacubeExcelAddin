using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAddin.Properties;

namespace TestAddin.UI
{
	public partial class Loading : Form
	{
		private Loading() => InitializeComponent();
		public Loading(int outOf) : this()
		{
			progressBar1.Maximum = outOf;
			Text = Resources.LoadingFormTitle;
			UpdateBar();
		}

		/*
			I looked into having the 'Windows 8/10 Loading Circle' animation play in an application.
			It turns out you can't do that in Win32, you have to do it in a UWP app.
			So, without some serious hacks, we aren't getting the animation to show up here.
		*/

		private void UpdateBar()
		{
			lblProcessing.Text = string.Format(Resources.LoadingBarText, progressBar1.Value, progressBar1.Maximum);
			Invalidate();
		}

		private int Value { get => progressBar1.Value; set => progressBar1.Value = value; }

		// The idea here is to never actually use binary + or - but to use += and -= instead
		// Also: minus should not be used since progress doesn't backtrack.
		public static Loading operator +(Loading l, int v)
		{
			l.Value += v;
			l.UpdateBar();
			return l;
		}
		public static Loading operator ++(Loading l)
		{
			l.Value++;
			l.UpdateBar();
			return l;
		}
		public static Loading operator -(Loading l, int v)
		{
			// for completion
			l.Value -= v;
			l.UpdateBar();
			return l;
		}
		public static Loading operator --(Loading l)
		{
			// for completion
			l.Value--;
			l.UpdateBar();
			return l;
		}
	}
}
