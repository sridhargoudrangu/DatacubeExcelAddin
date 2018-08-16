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
			Value = 0;
		}

		public int Value
		{
			get => progressBar1.Value;
			set
			{
				progressBar1.Value = value;
				lblProcessing.Text = string.Format(Resources.LoadingBarText, progressBar1.Value, progressBar1.Maximum);
				Invalidate();
			}
		}

		// The idea here is to never actually use binary + or - but to use += and -= instead
		// Also: minus should not be used
		public static Loading operator +(Loading l, int v)
		{
			l.Value += v;
			return l;
		}
		public static Loading operator ++(Loading l)
		{
			l.Value++;
			return l;
		}
		public static Loading operator -(Loading l, int v)
		{
			l.Value -= v;
			return l;
		}
		public static Loading operator --(Loading l)
		{
			l.Value--;
			return l;
		}

		private void Cancel(object sender, EventArgs e) => ACTIONS.Import.Underway = false;
	}
}
