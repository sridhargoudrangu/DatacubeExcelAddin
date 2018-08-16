using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace TestAddin.ACTIONS
{
	/*
		This class supports the not-yet-implemented feature "Synchronized Worksheets"

		The dictionary cache maps the guid of an imported sheet to
		   a dictionary that maps the string contentid of a row to
			a dictionary that maps the string column header to
			the modified contents of the cell (as an object).
		
		Usage:
			cache[worksheet_guid][contentid][column_header] = obj;
			obj = cache[worksheet_guid][contentid][column_header];

		Note that this class should not be called into -- it is unfinished.
	*/
	static class SynchronizedWorksheet
	{
		public static IDictionary<Guid, IDictionary<string, IDictionary<string, object>>> cache;

		/// <summary>Check if the worksheet is synchronized using the presence of the cvlt_datacube_live flag</summary>
		public static bool IsSynchronizedWorksheet(this Worksheet ws)
		{
			try
			{
				// If the property exists, return the value of it. (true)
				return ws.CustomProperties.Item["cvlt_datacube_live"].Value;
			}
			catch
			{
				// If the property does not exist, as evidenced by the exception we're "handling", return false
				return false;
			}
		}

		/// <summary>Same as IsSynchronizedWorksheet without the out params, but passes informational data back to the caller.</summary>
		public static bool IsSynchronizedWorksheet(this Worksheet ws, out string host, out int dsid, out int handlerid, out string handlername, out string dsname, out DateTime timestamp, out string cid_ws_name)
		{
			if(ws.IsSynchronizedWorksheet())
			{
				host = ws.CustomProperties.Item["cvlt_sync_host"].Value;
				dsid = ws.CustomProperties.Item["cvlt_sync_dsid"].Value;
				dsname = ws.CustomProperties.Item["cvlt_sync_dsname"].Value;
				timestamp = ws.CustomProperties.Item["cvlt_sync_timestamp"].Value;
				handlerid = ws.CustomProperties.Item["cvlt_sync_handlerid"].Value;
				handlername = ws.CustomProperties.Item["cvlt_sync_handlername"].Value;
				cid_ws_name = ws.CustomProperties.Item["cvlt_sync_contentid_sheetname"].Value;
				return true;
			}
			else
			{
				host = default;
				dsid = default;
				dsname = default;
				timestamp = default;
				handlerid = default;
				handlername = default;
				cid_ws_name = default;
				return false;
			}
		}

		public static void MarkSynchronized(this Worksheet ws, Handler h, string host, DateTime timestamp, string cid_ws_name)
		{
			ws.CustomProperties.Add("cvlt_sync", true);
			ws.CustomProperties.Add("cvlt_sync_host", host);
			ws.CustomProperties.Add("cvlt_sync_dsid", h.dataSourceId);
			ws.CustomProperties.Add("cvlt_sync_dsname", h.dataSourceName);
			ws.CustomProperties.Add("cvlt_sync_timestamp", timestamp);
			ws.CustomProperties.Add("cvlt_sync_handlerid", h.handlerId);
			ws.CustomProperties.Add("cvlt_sync_handlername", h.handlerName);
			ws.CustomProperties.Add("cvlt_sync_contentid_sheetname", cid_ws_name);
		}

		public static string GetContentId(this Worksheet ws, int row)
			=> ws.IsSynchronizedWorksheet(out string host, out int dsid, out int handlerid, out string handlername, out string dsname, out DateTime timestamp, out string tgt) ? ((Worksheet)Useful.Worksheets[tgt]).Cell(row, 1).Value2 as string : null;

		// Push the cached changes to the server
		public static void Push(this Worksheet ws, string host, int dsid)
		{
			return;
			// Push action not yet implemented.
			StringBuilder sb = new StringBuilder();
			sb.Append('[');
			//foreach(Range r in caches[ws])
			{
				sb.Append('{');
				//sb.Append($"\"contentid\":\"{ws.GetContentId(r.Row)}\"");

				/* NOT DONE !*/

				sb.Append("},");
			}
			sb.Remove(sb.Length - 1, 1);
			sb.Append(']');
		}
	}
}
