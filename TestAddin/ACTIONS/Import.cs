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

	/*
		There's at least one optimization to be made here.

		Instead of calling out to the server every CHUNK_SIZE rows,
		use the special Api.ExecuteLoggedIn function that has 'out Stream'
		and read from the stream that way.

		Use a Microsoft.VisualBasic.FileIO.TextFieldParser to bypass my garbage
		CSV parsing code in Useful.cs; See the commented out ExecuteEx below for 
		approximate usage.

		** NOTE: the ExecuteEx function DOES NOT WORK as intended **

		Using the above comments, you can get rid of a lot of wasted runtime
		when the addin is waiting for a server response.

		I had only four or five days left in my internship when all of this
		became apparent to me so I left it unimplemented in favor of cleaning up
		the rest of the code.

		Brian Gembara
	*/

	static class Import
	{
		private const int CHUNK_SIZE = 4096;

		/*public static void ExecuteEx(string host, Handler h, bool makelive)
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

				string[] headers = parser.ReadFields();

				if(headers.Length > Useful.EXCEL_MAX_COLS)
					throw new InternalBufferOverflowException(string.Format(Resources.ErrTooManyColumns, Useful.EXCEL_MAX_COLS));

				while(parser.HasMoreData())
				{
					object[,] objs = new object[Useful.EXCEL_MAX_ROWS, headers.Length];

					for(int i = 0; i < headers.Length; i++)
						objs[0, i] = headers[i];

					for(int i = 1; parser.HasMoreData() && i < Useful.EXCEL_MAX_ROWS; i++)
					{
						string[] data = parser.ReadFields();
						for(int j = 0; j < data.Length; j++) // [string -> *] type conversion
						{
							if(long.TryParse(data[i], out long _long))
							{
								if(_long > Useful.EXCEL_MAX_INT_AS_DOUBLE)
								{
									numtext = true;
									data[i] = '\'' + data[i];

									if(data[i].Length > Useful.EXCEL_MAX_STR_LEN)
										truncated = true;
								}
								objs[j, i] = _long;
							}
							//else if(DateTime.TryParse(data[i], out DateTime _datetime))
							//{
							//	if(_datetime.Year < 1904 || _datetime.Year > 9999)
							//	{
							//		dateoob = true;
							//		objs[i, j] = _datetime.ToString();
							//	}
							//	else
							//		objs[j, i] = _datetime;
							//}
							else if(double.TryParse(data[i], out double _double))
							{
								objs[j, i] = _double;
							}
							else
							{
								if(data[i].StartsWith("="))
									data[i] = '\'' + data[i];

								if(data[i].Length > Useful.EXCEL_MAX_STR_LEN)
									truncated = true;

								objs[j, i] = data[i];
							}
						}
					}

					Useful.ActiveSheet.Cell(1, 1).Resize[Useful.EXCEL_MAX_ROWS, objs.GetUpperBound(1) + 1].Value2 = objs;
					FinalizeWorksheet(parser.HasMoreData());
				}
			}

			if(truncated) Useful.Error(Resources.TruncationWarning, Resources.TruncationWarningCaption, MessageBoxIcon.Information);
			if(numtext) Useful.Error(Resources.NumTextWarning, Resources.NumTextWarningCaption, MessageBoxIcon.Information);
			if(dateoob) Useful.Error(Resources.DateOOBWarning, Resources.DateOOBWarningCaption, MessageBoxIcon.Information);
		}
		*/

		public static bool Underway = false;
		
		public static void Execute(string host, Handler h, bool makelive = false)
		{
			if(!new Api.GetNumRecords(host, h).ExecuteLoggedIn(out int max)
			|| !new Api.GetTimestamp(host, h.dataSourceId).ExecuteLoggedIn(out DateTime timestamp))
			{
				Useful.Error(Resources.UnableToGetRemoteDataAmbiguous, Resources.Error);
				return;
			}

			if(Useful.ActiveWorkbook is null)
			{
				/* Adding a workbook implies adding a worksheet */
				Useful.Workbooks.Add();
			}
			else if(!Useful.ActiveSheet.UsedRange.IsEmpty())
			{
				/* Adding a worksheet gives that worksheet focus */
				Useful.Worksheets.Add();
			}

			Loading progress = new Loading(max);
			progress.Show(new ParentHwnd(Useful.Hwnd));
			bool truncated = false, numtext = false, dateoob = false;
			object[,] cids;
			Underway = true;

			void EmitToWorksheet(object[,] objects, Worksheet target, int sheetrow = 1)
				=> target.Cell(sheetrow, 1).Resize[objects.GetUpperBound(0) + 1, objects.GetUpperBound(1) + 1].Value2 = objects;
			void GetAndEmitData(int rows, int skip, int offset)
			{
				new Api.Handler(host, h, maxrows: rows, skip: skip).ExecuteLoggedIn(out string result);
				object[,] flat = result.ToFlat2DArray(rows, ref truncated, ref numtext, ref dateoob, (offset % Useful.EXCEL_MAX_ROWS != 0));
				EmitToWorksheet(flat, Useful.ActiveSheet, 1 + (offset % Useful.EXCEL_MAX_ROWS));
			}

			using(new ExcelNoScreenUpdate())
			{
				int written = 0, processed = 0;

				// While there is still data left unprocessed...
				while(max > 0 && Underway)
				{
					if(max > CHUNK_SIZE)
					{
						// we can get a full chunk and still have leftover
						if(written % Useful.EXCEL_MAX_ROWS == 0)
						{
							GetAndEmitData(CHUNK_SIZE - 1, processed, written);
							processed += CHUNK_SIZE - 1;
							progress += CHUNK_SIZE - 1;
							max -= CHUNK_SIZE - 1;
						}
						else
						{
							GetAndEmitData(CHUNK_SIZE, processed, written);
							processed += CHUNK_SIZE;
							progress += CHUNK_SIZE;
							max -= CHUNK_SIZE;
						}

						written += CHUNK_SIZE;
						if(written % Useful.EXCEL_MAX_ROWS == 0)
						{
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
							Useful.Worksheets.Add();
						}
					}
					else
					{
						// This is the last 
						GetAndEmitData(max, processed, written);
						progress += max;
						max = 0;
					}
				}

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
			}

			progress.Dispose();
			Underway = false;
			if(truncated) Useful.Error(Resources.TruncationWarning, Resources.TruncationWarningCaption, MessageBoxIcon.Information);
			if(numtext) Useful.Error(Resources.NumTextWarning, Resources.NumTextWarningCaption, MessageBoxIcon.Information);
			if(dateoob) Useful.Error(Resources.DateOOBWarning, Resources.DateOOBWarningCaption, MessageBoxIcon.Information);
		}
	}
}
