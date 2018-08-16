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
	public partial class UserSchemaValidation : Form
	{
		private UserSchemaValidation() => InitializeComponent();
		private UserSchemaValidation(SchemaModel model, bool isCreate) : this()
		{
			Localize(isCreate);
			propgrid.SelectedObject = model;
		}

		public static DialogResult Launch(SchemaModel model, bool isCreate) => new UserSchemaValidation(model, isCreate).ShowDialog(new ParentHwnd(Useful.Hwnd));

		private void Localize(bool isCreate)
		{
			Text = Resources.UserSchemaValidationFormTitle;
			lblInformation.Text = isCreate ? Resources.ValidationCreateNew : Resources.ValidationUseExistingNEWCOLUMNS;
			btnCancel.Text = Resources.Cancel;
			btnContinue.Text = Resources.Continue;
		}
		private void Continue(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
		private void Cancel(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
