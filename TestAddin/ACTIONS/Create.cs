using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAddin.Properties;
using TestAddin.UI;

namespace TestAddin.ACTIONS
{
	static class Create
	{
		public static void Execute(string host, string datasourcename, int cloudid, int type, string description = "", bool useSelection = false)
		{
			Range target = useSelection ? Useful.ActiveRange : Useful.ActiveSheet.UsedRange;

			object[,] headers = target.Cell(1, 1).EntireRow.Value2;
			for(int i = headers.GetLowerBound(1); i <= headers.GetUpperBound(1); i++)
				if(headers[1, i] is string q)
					for(int j = 0; j < q.Length; j++)
						// Regex.Match("", @"[_a-zA-Z][_a-zA-Z\d]*").Success; // Maybe use a regex?
						if(!((q[j] >= 'A' && q[j] <= 'Z') || (q[j] >= 'a' && q[j] <= 'z') || (q[j] >= '0' && q[j] <= '9') || q[j] == '_'))
						{
							Useful.Error(Resources.ErrorHeadersNeedAlphaNumericChars, Resources.Error);
							return;
						}

			SchemaModel model = new SchemaModel(target);
			if(UserSchemaValidation.Launch(model, true) == DialogResult.Cancel)
				return;

			if(!new Api.Create(host, new CreateDataSourceInfo(cloudid, datasourcename, type, description).Serialize()).ExecuteLoggedIn(out string result))
			{
				Useful.Error(Resources.CreateErrorText, Resources.Error);
				return;
			}

			int dsid = DataSources.Deserialize(result).collections.SelectMany(dsr => dsr.datasources).Single(dsi => dsi.datasourceName == datasourcename).datasourceId;

			if(!new Api.UpdateSchema(host, dsid, model).ExecuteLoggedIn(out result))
			{
				Useful.Error(string.Format(Resources.UnableToUpdateRemoteSchema, result), Resources.Error);
				return;
			}

			if(!new Api.Post(host, dsid, target.ToCSV(), false).ExecuteLoggedIn(out result))
			{
				Useful.Error(string.Format(Resources.PostErrorText, result), Resources.Error);
				return;
			}

			MessageBox.Show(Resources.CreateSuccess, Resources.Success);
		}
	}
}
