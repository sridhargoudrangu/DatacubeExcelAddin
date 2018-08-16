using System;
using TestAddin.Properties;
using Microsoft.Office.Core;
using TestAddin.ACTIONS;

using CustomTaskPane = Microsoft.Office.Tools.CustomTaskPane;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Microsoft.Office.Interop.Excel;
using TestAddin.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TestAddin
{
	public sealed partial class ThisAddIn : Microsoft.Office.Tools.AddInBase
	{
		private static CustomTaskPane ExportPane = null;
		private static ExportTaskPane ExportPaneInst = null;
		private static CustomTaskPane ImportPane = null;
		private static ImportTaskPane ImportPaneInst = null;

		protected override IRibbonExtensibility CreateRibbonExtensibilityObject() => new Ribbon();

		internal static void CreateTaskPane(char type, string host)
		{
			switch(type)
			{
				case 'i':
					CreateImportTaskPane(host);
					break;
				case 'e':
					CreateExportTaskPane(host);
					break;
			}
		}
		private static void CreateExportTaskPane(string host)
		{
			/* Created after CreateImportTaskPane, refer to the comments there */
			if(ExportPane is null)
				ExportPane = Useful.CustomTaskPanes.Add(ExportPaneInst = new ExportTaskPane(host), Resources.ExportDataCube);
			else
			{
				ExportPaneInst = new ExportTaskPane(ExportPaneInst);
				Useful.CustomTaskPanes.Remove(ExportPane);
				ExportPane = Useful.CustomTaskPanes.Add(ExportPaneInst, Resources.ExportDataCube);
			}

			ExportPane.Width = 325 + 16;
			ExportPane.Visible = true;
		}
		private static void CreateImportTaskPane(string host)
		{
			if(ImportPane is null)
				ImportPane = Useful.CustomTaskPanes.Add(ImportPaneInst = new ImportTaskPane(host), Resources.ImportDataCube);
			else
			{
				ImportPaneInst = new ImportTaskPane(ImportPaneInst); // This statement must come before the "remove" -- it preserves the state of the task pane controls between invocations
				Useful.CustomTaskPanes.Remove(ImportPane); // This statement discards the state of ImportPane the ImportTaskPane assiociated with it
				ImportPane = Useful.CustomTaskPanes.Add(ImportPaneInst, Resources.ImportDataCube);
			}

			// Set appropriate width & set visible
			ImportPane.Width = 325 + 16;
			ImportPane.Visible = true;
		}

		private void Load(object sender, EventArgs e)
		{
			// If we've saved the user's credentials, try to login with them.
			if(Settings.Default.SaveLoginInformation)
			{
				if(Authentication.AutoLogin())
					// Inform the ribbon that we are now logged in.
					// Tell it the server we connected to, like that it can ask that server
					// for the names of other index servers later when he makes a task pane visible.
					Ribbon._host = Authentication._hostname;
				else
					// Tell the ribbon that our saved credentials failed login and we got rid of them.
					Ribbon._savedloginfail = true;
			}

			// Setup event handlers
			Application.WorkbookAfterSave += WorkbookSavedEvent;
			Application.WorkbookOpen += WorkbookOpenedEvent;
			Application.SheetChange += SheetChangedEvent;
		}

		// If synchronized, add changed range to list
		private void SheetChangedEvent(object Sh, Range Target)
		{
			if(Sh is Worksheet ws && ws.IsSynchronizedWorksheet())
			{
				if(Target.IsColumnar())
				{
					// USER'S TRYING TO DELETE A COLUMN IN A SYNCHRONIZED SHEET
					// ALERT THEM OF THE CONSEQUENCES.

					
				}
				else if(Target.IsTabular())
				{
					// user is deleting a whole row
					
					
				}
				else if(Target.Columns.Count > 1 || Target.Rows.Count > 1)
				{
					// user is deleting multiple contiguous cells

					
				}
				else
				{
					// user is changing a single cell
					// easy case

					
				}
			}
		}

		// If synchronized, and the user is ok with it, pull server data down unless we're not logged into that server
		private void WorkbookOpenedEvent(Workbook wb)
		{
			return;

			// Action needs to be taken here to implement live workbooks
			// ws.Pull doesn't exist

			if(Authentication.LoggedIn)
				foreach(Worksheet ws in wb.Sheets)
					if(ws.GetSyncValues(out string host, out int dsid, out int handlerid, out string handlername, out string dsname, out DateTime timestamp, out string linkedwsname))
						if(host == Authentication._hostname && new Api.GetTimestamp(host, dsid).ExecuteLoggedIn(out DateTime checktime) && timestamp < checktime)
							if(MessageBox.Show(string.Format(Resources.WorkbookOpenSyncNotification, ws.Name, host)) == DialogResult.Yes)
								; // ws.Pull(host, handlername, handlerid);
		}

		// If synchronized, push data up to server unless we're not logged into that server
		// Do this AFTER we save the workbook to disk
		private void WorkbookSavedEvent(Workbook wb, bool Success) 
		{
			if(Success && Authentication.LoggedIn)
				foreach(Worksheet ws in wb.Sheets)
					if(ws.GetSyncValues(out string host, out int dsid, out int handlerid, out string handlername, out string dsname, out DateTime timestamp, out string linkedwsname))
						if(host == Authentication._hostname)
							ws.Push(host, dsid);
						else
							Useful.Error(Resources.LiveWorkbookSyncError, Resources.Error);
		}

		///<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InternalStartup() => Startup += new EventHandler(Load);
	}
}
