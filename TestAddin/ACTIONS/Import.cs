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

using Microsoft.VisualBasic.FileIO;

namespace TestAddin.ACTIONS
{

	static class Import
	{

		private const int DIVISION_FACTOR = 256;
		private const int CHUNK_SIZE = 4096;
		public static bool Underway = false;

		public static void Execute(string host, Handler h, bool makelive)
		{
			bool truncated = false, numtext = false, dateoob = false;
			object[,] cids = new object[Useful.EXCEL_MAX_ROWS, 1];
			DateTime timestamp;
			Stream input;
			int max;

			#region External API Calls

			if(!new Api.GetNumRecords(host, h).ExecuteLoggedIn(out max)
			|| !new Api.GetTimestamp(host, h.dataSourceId).ExecuteLoggedIn(out timestamp))
			{
				Useful.Error(Resources.UnableToGetRemoteDataAmbiguous, Resources.Error);
				return;
			}

			try
			{
				new Api.Handler(host, h, maxrows: max).ExecuteLoggedIn(out input);
			}
			catch
			{
				Useful.Error(Resources.UnableToContinue, Resources.Error);
				return;
			}

			#endregion
			#region Null/Empty Workbook/Worksheet Checks

			if(Useful.ActiveWorkbook is null)
			{
				// Adding a workbook implies adding a worksheet
				Useful.Workbooks.Add();
			}
			else if(!Useful.ActiveSheet.UsedRange.IsEmpty())
			{
				// Adding a worksheet gives that worksheet focus
				Useful.Worksheets.Add();
			}

			#endregion


			void FinalizeWorksheet(bool addANewWorksheet = true)
			{
				// This method cleans out the system columns we pulled down and sets up
				// information for live worksheet functionality if desired. Optionally
				// creates a new worksheet (Used in the rest of the import process).
				cids = Useful.ActiveSheet.CleanSystemColumns();
				if(makelive)
				{
					string cid_ws_name = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
					Useful.ActiveSheet.MarkSynchronized(h, host, timestamp, cid_ws_name);
					Useful.Worksheets.Add();
					Useful.ActiveSheet.Cell(1, 1).Resize[cids.GetLength(0), cids.GetLength(1)].Value2 = cids;
					Useful.ActiveSheet.Name = cid_ws_name;
					Useful.ActiveSheet.Visible = XlSheetVisibility.xlSheetVeryHidden;
				}
				if(addANewWorksheet) Useful.Worksheets.Add();
			}

			using(Loading progress = new Loading(max))
			using(TextFieldParser parser = new TextFieldParser(input))
			using(new ExcelNoScreenUpdate())
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				progress.Show(new ParentHwnd(Useful.Hwnd));

				string[] headers = parser.ReadFields();

				if(headers.Length > Useful.EXCEL_MAX_COLS)
					throw new InternalBufferOverflowException(string.Format(Resources.ErrTooManyColumns, Useful.EXCEL_MAX_COLS));

				while(parser.HasMoreData())
				{
					object[,] objs = new object[Useful.EXCEL_MAX_ROWS / DIVISION_FACTOR, headers.Length];

					for(int i = 0; i < headers.Length; i++)
						objs[0, i] = headers[i];

					int row;
					for(row = 1; parser.HasMoreData() && row < Useful.EXCEL_MAX_ROWS; row++, progress.Value++)
					{
						string[] data = parser.ReadFields();

						if(row % (Useful.EXCEL_MAX_ROWS / DIVISION_FACTOR) == 0)
							Useful.ActiveSheet.Cell(1 + row - (Useful.EXCEL_MAX_ROWS / DIVISION_FACTOR), 1).Resize[objs.GetUpperBound(0) + 1, objs.GetUpperBound(1) + 1].Value2 = objs;

						for(int col = 0; col < data.Length; col++) // [string -> *] type conversion
						{
							object InterpretType(string tgt)
							{
								if(long.TryParse(tgt, out long _long) && headers[col] != "_version_")
								{
									if(_long > Useful.EXCEL_MAX_INT_AS_DOUBLE)
									{
										numtext = true;
										tgt = '\'' + tgt;

										if(tgt.Length > Useful.EXCEL_MAX_STR_LEN)
											truncated = true;
									}
									return _long;
								}
								else if(double.TryParse(tgt, out double _double))
								{
									return _double;
								}
								else
								{
									if(tgt.StartsWith("="))
										tgt = '\'' + tgt;

									if(tgt.Length > Useful.EXCEL_MAX_STR_LEN)
										truncated = true;

									return tgt;
								}
							}

							objs[row % (Useful.EXCEL_MAX_ROWS / DIVISION_FACTOR), col] = InterpretType(data[col]);
						}
					}

					Useful.ActiveSheet.Cell(row < (Useful.EXCEL_MAX_ROWS / DIVISION_FACTOR) ? 1 : 1 + row - (Useful.EXCEL_MAX_ROWS / DIVISION_FACTOR), 1).Resize[objs.GetUpperBound(0) + 1, objs.GetUpperBound(1) + 1].Value2 = objs;
					FinalizeWorksheet(parser.HasMoreData());
				}
			}

			if(truncated) Useful.Error(Resources.TruncationWarning, Resources.TruncationWarningCaption, MessageBoxIcon.Information);
			if(numtext) Useful.Error(Resources.NumTextWarning, Resources.NumTextWarningCaption, MessageBoxIcon.Information);
			if(dateoob) Useful.Error(Resources.DateOOBWarning, Resources.DateOOBWarningCaption, MessageBoxIcon.Information);
		}

	}

}
