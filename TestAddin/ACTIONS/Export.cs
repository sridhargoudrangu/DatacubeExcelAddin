using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAddin.Properties;
using TestAddin.UI;

namespace TestAddin.ACTIONS
{
	static class Export
	{
		public static void Execute(string host, int datasourceid, bool useSelection = false)
		{
			if(!new Api.GetSchema(host, datasourceid).ExecuteLoggedIn(out string result))
			{
				Useful.Error(Resources.UnableToRetrieveSchema, Resources.Error);
				return;
			}

			SchemaModel model = new SchemaModel();
			bool UserValidation = false;

			Range r = useSelection ? Useful.ActiveRange : Useful.ActiveSheet.UsedRange;
			if(!r.ValidateAgainstShemata(SchemaCollection.Deserialize(result).GetSchemata(), ref UserValidation, ref model))
			{
				Useful.Error(Resources.SchemaValidationFailure, Resources.Error);
				return;
			}

			if(UserValidation)
			{
				if(UserSchemaValidation.Launch(model, false) == DialogResult.Cancel)
					return;

				if(!new Api.UpdateSchema(host, datasourceid, model).ExecuteLoggedIn(out result))
				{
					Useful.Error(string.Format(Resources.UnableToUpdateRemoteSchema, result), Resources.Error);
					return;
				}
			}

			if(!new Api.Post(host, datasourceid, r.ToCSV(), false).ExecuteLoggedIn(out result))
			{
				Useful.Error(string.Format(Resources.PostErrorText, result), Resources.Error);
				return;
			}

			MessageBox.Show(Resources.PostSuccess, Resources.Success);
		}
	}
}
