using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using TestAddin.Properties;

namespace TestAddin
{
	//This class has some 'Useful' functions/properties in it
	static class Useful
	{
		// These four numbers are valid if(Excel >= 2007 && Excels <= 365)
		public const int EXCEL_MAX_STR_LEN = 32767; // Max chars allowed in a cell
		public const int EXCEL_MAX_CELL_LF = 253; // Max line feeds (LF) allowed in a cell, painful restriction !! UNCHECKED !!
		public const int EXCEL_MAX_ROWS = 1048576; // Max rows in a sheet
		public const int EXCEL_MAX_COLS = 16384; // Max cols in a sheet

		// This is the largest integer you can fit in an Excel cell and still have it render correctly, See the IEEE double spec for why
		public const long EXCEL_MAX_INT_AS_DOUBLE = 1000000000000000;

		// Should all be self-explainative except Hwnd
		public static Workbook ActiveWorkbook { get => Globals.ThisAddIn.Application.ActiveWorkbook; }
		public static Range ActiveRange { get => (Range)Globals.ThisAddIn.Application.Selection; }
		public static Worksheet ActiveSheet { get => (Worksheet)Globals.ThisAddIn.Application.ActiveSheet; }
		public static WorksheetFunction Functions { get => Globals.ThisAddIn.Application.WorksheetFunction; }
		public static CustomTaskPaneCollection CustomTaskPanes { get => Globals.ThisAddIn.CustomTaskPanes; }
		public static Sheets Worksheets { get => Globals.ThisAddIn.Application.Worksheets; }
		public static Workbooks Workbooks { get => Globals.ThisAddIn.Application.Workbooks; }
		public static IntPtr Hwnd { get => (IntPtr)Globals.ThisAddIn.Application.Hwnd; } // This HWND is a Handle to WiNDow: It refers to the Excel application window.

		public static bool IsEmpty(this Range r) => ((int)Functions.CountA(ActiveSheet.UsedRange)) == 0;
		public static IEnumerable<Schema> GetSchemata(this SchemaCollection sc) => sc.collections.Select(a => a.schema);
		public static bool Contains(this ComboBox cbx, object x) => cbx.Items.Contains(x);
		public static int IndexOf(this ComboBox cbx, object x) => cbx.Items.IndexOf(x);
		public static int IndexOfSingle<Type>(this ComboBox cbx, Func<Type, bool> p) => cbx.IndexOf(cbx.Single(p));
		public static int Add(this ComboBox cbx, object x) => cbx.Items.Add(x);
		public static void Clear(this ComboBox cbx) => cbx.Items.Clear();
		public static Type Single<Type>(this ComboBox cbx, Func<Type, bool> p) => cbx.Items.Cast<Type>().Where(p).Single(); // throws at least one benign exception when used in the plugin -- list.Count != 1 or list.Count == 0
		public static IEnumerable<DataSourceRecord> FilterByIndexServer(this IEnumerable<DataSourceRecord> dsr, IndexServerRecord isr) => isr == default ? dsr : dsr.Where(q => isr.cloudID == q.cloudId);
		public static IEnumerable<DataSourceInfo> GetAllDataSources(this IEnumerable<DataSourceRecord> dsr) => dsr.SelectMany(q => q.datasources);
		public static IEnumerable<Handler> FilterByDSID(this IEnumerable<Handler> h, int dsid) => h.Where(hh => hh.dataSourceId == dsid);
		public static byte[] ToByteArray(this string s) => Encoding.Unicode.GetBytes(s);
		public static string MakeIntoString(this byte[] b) => Encoding.Unicode.GetString(b);
		public static string ToBase64(this string s) => Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		public static string ToBase64(this byte[] b) => Convert.ToBase64String(b);
		public static object[,] ToFlat2DArray(this string csv, int rows, ref bool truncated, ref bool numtext, ref bool dateoob, bool skip_first_row = false)
		{
			List<object>[] the_array = new List<object>[rows + 1];
			StringBuilder sb = new StringBuilder();
			int subarray = 0;
			bool f = false;

			the_array[0] = new List<object>();

			// local functions
			object[,] CreateFlatArray()
			{
				// There MUST be some way of doing this which is more efficient than copying these references

				int r = the_array.Length;
				int c = the_array[0].Count;
				object[,] flat = new object[skip_first_row ? r - 1 : r, c];

				for(r = skip_first_row ? 1 : 0; r < the_array.Length; r++)
					for(c = 0; c < the_array[r].Count; c++)
						flat[skip_first_row ? r - 1 : r, c] = the_array[r][c];

				return flat;
			}
			void WriteValue(ref bool _truncated, ref bool _numtext, ref bool _dateoob, bool addNewRow = false)
			{
				string s = sb.ToString();

				/*
				Do some checks before placing the value in the_array.
				This should detect:
				  - strings starting with '='
				  - integers beyond 1000000000000000
				  - strings whose length is greater than 32767
				This does not detect (but it should):
				  - strings with more than 253 newlines   ** use _truncated to impl
				  - dates before DateTime.MinValue        ** use _dateoob to impl
				  - dates beyond DateTime.MaxValue        ** use _dateoob to impl
				*/

				if(s.StartsWith("="))
					s = '\'' + s;

				if(s.Length > EXCEL_MAX_STR_LEN)
					_truncated = true;

				if(long.TryParse(s, out long l))
				{
					if(l > EXCEL_MAX_INT_AS_DOUBLE)
					{
						if(the_array[0][the_array[subarray].Count] as string != "_version_")
						{
							// this if statement is such a hack:
							// _version_, a system field that gets removed, tends to be above EXCEL_MAX_INT_AS_DOUBLE
							// so we check if that's the field we're writing because the user shouldn't get a notification
							// saying "number stored as string" when that column doesn't exist!

							_numtext = true;
						}

						s = '\'' + s;

						if(s.Length > EXCEL_MAX_STR_LEN)
							_truncated = true;

						the_array[subarray].Add(s);
					}
					else
						the_array[subarray].Add(l);
				}
				else
					the_array[subarray].Add(s);

				if(addNewRow && subarray < the_array.Length - 1)
					the_array[++subarray] = new List<object>();
				sb.Clear();
			}

			// loop over each char of the csv, converting/placing it into sb
			// This needs Microsoft.VisualBasic.FileIO.TextFieldParser so badly
			for(int i = 0; i < csv.Length; i++)
			{
				switch(csv[i])
				{
					case '\n':
						if(f)
							goto default;
						else
							WriteValue(ref truncated, ref numtext, ref dateoob, i + 1 < csv.Length); // initialize the next row each newline that isn't the last one
						continue;

					case ',':
						if(f)
							goto default;
						else
							WriteValue(ref truncated, ref numtext, ref dateoob);
						continue;

					case '"':
						if(f && i + 1 < csv.Length && csv[i + 1] == '"')
						{
							i = i + 1;
							goto default;
						}
						else
							f = !f;
						continue;

					default:
						sb.Append(csv[i]);
						continue;
				}
			}

			// Unmatched double quote check
			// This *should be* impossible since the server *should be* sending valid CSV
			if(f)
				throw new Exception();

			return CreateFlatArray();
		}
		public static string ToCSV(this Range r)
		{
			StringBuilder csv = new StringBuilder();
			object[,] arr = r.Value;

			/* The following algorithm SHOULD be compliant with https://tools.ietf.org/html/rfc4180 */

			for(int y = arr.GetLowerBound(0); y <= arr.GetUpperBound(0); y++)
			{
				bool first = true; // used to control when we place commas
				for(int x = arr.GetLowerBound(1); x <= arr.GetUpperBound(1); x++)
				{
					switch(arr[y, x])
					{
						case null:
						default:
							csv.Append(first ? "" : ",");
							break;

						case bool b:
							csv.Append((first ? "" : ",") + b.ToString());
							break;

						case double d:
							csv.Append((first ? "" : ",") + d.ToString());
							break;

						case string s:
							if(s.Contains(' ') || s.Contains('\n') || s.Contains('\t') || s.Contains('"') || s.Contains(','))
							{
								s = s.Replace("\"", "\"\"");
								s = '"' + s + '"';
							}
							csv.Append((first ? "" : ",") + s);
							break;

						case DateTime date:
							csv.Append((first ? "" : ",") + date.ToUniversalTime().ToString("u").Replace(' ', 'T'));
							break;
					}
					first = false;
				}
				csv.Append('\n');
			}

			return csv.ToString();
		}
		/**<summary>Returns true if the range is a single column.</summary>*/
		public static bool IsColumnar(this Range r) => r.Columns.Count == 1;
		/**<summary>Returns true if the range is a single row.</summary>*/
		public static bool IsTabular(this Range r) => r.Rows.Count == 1;
		public static bool ValidateAgainstShemata(this Range _range, IEnumerable<Schema> schemas, ref bool userNeedsToConfirmAdditionalTypes, ref SchemaModel model)
		{
			Dictionary<string, string> fieldmap = new Dictionary<string, string>();

			{
				IEnumerator<string> names = schemas.SelectMany(s => s.schemaFields.Select(f => f.fieldName)).GetEnumerator();
				IEnumerator<string> types = schemas.SelectMany(s => s.schemaFields.Select(f => f.type)).GetEnumerator();

				/*
				from https://msdn.microsoft.com/en-us/library/system.collections.ienumerator.current(v=vs.110).aspx
				The enumerator is positioned before the first element in the collection, immediately after the enumerator is created.
				MoveNext must be called to advance the enumerator to the first element of the collection before reading the value of Current.
				*/

				while(names.MoveNext() && types.MoveNext())
					fieldmap.Add(names.Current, types.Current);
			} //Setup Region -- Building the dictionary

			// Get the entire range as an 2D object array
			object[,] range = _range.Value;
			for(int c = range.GetLowerBound(1); c <= range.GetUpperBound(1); c++)
			{
				if(range[1, c] is string fieldname)
				{
					if(fieldmap.ContainsKey(fieldname))
					{
						for(int r = range.GetLowerBound(0) + 1; r <= range.GetUpperBound(0); r++)
						{
							//Checks if range[r,c] is of type `typename`
							bool IsOfType(string typename)
							{
								switch(typename)
								{
									case "boolean":
										if(range[r, c] is bool)
											return true;
										break;
									case "currency":
										if(decimal.TryParse(range[r, c] as string, out decimal d))
											return true;
										break;
									case "date":
									case "tdate":
										if(range[r, c] is DateTime dt0)
											return true;
										break;
									case "double":
									case "tdouble":
										if(range[r, c] is double)
											return true;
										break;
									case "epoch":
										if(range[r, c] is DateTime dt1)
											return true;
										break;
									case "float":
									case "tfloat":
										if(range[r, c] is double)
											return true;
										break;
									case "int":
									case "tint":
										if(range[r, c] is double dbl0 && ((int)dbl0) == dbl0)
											return true;
										break;
									case "long":
									case "tlong":
										if(range[r, c] is double dbl1 && ((int)dbl1) == dbl1)
											return true;
										break;
									case "longstring":
										if(range[r, c] is string)
											return true;
										break;
									case "lowercase":
										if(range[r, c] is string s0 && s0.ToLower() == s0)
											return true;
										break;
									case "string":
										if(range[r, c] is string)
											return true;
										break;
									case "uuid":
										if(range[r, c] is string s1 && Guid.TryParse(s1, out Guid guid))
											return true;
										break;

									#region Unsupported Types
									
								case "ignored":
								case "_bbox_coord":
								case "alphaOnlySort":
								case "ancestor_path":
								case "bbox":
								case "binary":
								case "commaDelimited":
								case "descendent_path":
								case "entity_field":
								case "filePath":
								case "location":
								case "location_rpt":
								case "managed_en":
								case "payloads":
								case "phonetic":
								case "point":
								case "random":
								case "textSpell":

								case "text_ar":
								case "text_bg":
								case "text_ca":
								case "text_cjk":
								case "text_ckb":
								case "text_cz":
								case "text_da":
								case "text_de":
								case "text_el":
								case "text_en":
								case "text_en_splitting":
								case "text_en_splitting_tight":
								case "text_es":
								case "text_eu":
								case "text_fa":
								case "text_fi":
								case "text_fr":
								case "text_ga":
								case "text_general":
								case "text_general_rev":
								case "text_gl":
								case "text_hi":
								case "text_hu":
								case "text_hy":
								case "text_id":
								case "text_it":
								case "text_ja":
								case "text_lv":
								case "text_nl":
								case "text_no":
								case "text_pt":
								case "text_ro":
								case "text_ru":
								case "text_sv":
								case "text_th":
								case "text_tr":
								case "text_ws":

									#endregion

									default:
										throw new InvalidOperationException(string.Format(Resources.ErrUnsupportedType, typename));
								}
								return false;
							}

							if(IsOfType(fieldmap[fieldname]))
								continue;
							else return false;
						}
					}
					else
					{
						model.Add(fieldname, (2 <= range.GetUpperBound(0)) ? InferServerSideTyping(range[2, c]) : DSFieldType.String);
						userNeedsToConfirmAdditionalTypes = true;
					}
				}
				else throw new InvalidOperationException(Resources.ErrNonTextHeader);
			}

			return true;
		}
		public static DSFieldType InferServerSideTyping(object x)
		{
			switch(x)
			{
				case string sv: return (sv.ToLower() != sv) ? DSFieldType.String : DSFieldType.LowercaseString;
				case bool bv: return DSFieldType.Boolean;
				case double dv: return ((int)dv == dv) ? DSFieldType.Integer : DSFieldType.Double;
				case DateTime datev: return DSFieldType.Date;
				default: return DSFieldType.String;
			}
		}
		public static object[,] CleanSystemColumns(this Worksheet ws)
		{
			Range used = ws.UsedRange;
			int end = used.Columns.Count;
			object[,] cols = used.Cell(1, 1).EntireRow.Value2;
			object[,] contentids = null;
			for(int i = 1, j = 1; i <= end; i++, j++)
			{
				string colhdr = cols[1, i] as string;

				if(colhdr.StartsWith("_"))
					ws.Cell(1, j--).EntireColumn.Delete();
				else if(colhdr == "contentid")
				{
					contentids = used.Cell(1, j).Resize[used.Rows.Count, 1].Value2;
					ws.Cell(1, j--).EntireColumn.Delete();
				}
			}
			return contentids;
		}
		public static Range Cell(this Worksheet ws, int r, int c) => ws.Cells[r, c];
		public static Range Cell(this Range rng, int r, int c) => rng.Cells[r, c];
		public static string ToServerTypeString(this DSFieldType f)
		{
			switch(f)
			{
				case DSFieldType.Boolean:
					return "boolean";
				case DSFieldType.Timestamp:
					return "epoch";
				case DSFieldType.Integer:
					return "int";
				case DSFieldType.LongString:
					return "longstring";
				case DSFieldType.LowercaseString:
					return "lowercase";
				case DSFieldType.String:
					return "string";
				case DSFieldType.Date:
					return "date";
				case DSFieldType.Double:
					return "double";
				case DSFieldType.Text:
				default:
					return "text_general";
				case DSFieldType.Float:
					return "float";
				case DSFieldType.Long:
					return "long";
			}
		}
		public static DSFieldType FromServerTypeString(this String s)
		{
			switch(s)
			{
				case "boolean":
					return DSFieldType.Boolean;
				case "epoch":
					return DSFieldType.Timestamp;
				case "int":
					return DSFieldType.Integer;
				case "longstring":
					return DSFieldType.LongString;
				case "lowercase":
					return DSFieldType.LowercaseString;
				case "string":
					return DSFieldType.String;
				case "date":
					return DSFieldType.Date;
				case "double":
					return DSFieldType.Double;
				case "text_general":
				default:
					return DSFieldType.Text;
				case "float":
					return DSFieldType.Float;
				case "long":
					return DSFieldType.Long;
			}
		}
		public static bool HasMoreData(this TextFieldParser p) => !p.EndOfData;

		public static void Error(string msg, string cap, MessageBoxIcon i = MessageBoxIcon.Error) => MessageBox.Show(msg, cap, MessageBoxButtons.OK, i);
	}

	//	This class is all thanks to https://stackoverflow.com/questions/1928567/using-a-dictionary-in-a-propertygrid
	public class SchemaModel : ICustomTypeDescriptor, IDictionary<string, object>
	{
		private IDictionary<string, object> backing { get; }

		// Create an empty Dictionary to back the object
		public SchemaModel() => backing = new Dictionary<string, object>();
		public SchemaModel(IDictionary<string, object> backing) => this.backing = backing;

		// Guess type information based on range
		public SchemaModel(Range r) : this()
		{
			object[,] range = r.Value;
			DSFieldType[] types = new DSFieldType[range.GetLength(1)];

			for(int i = range.GetLowerBound(1); i <= range.GetUpperBound(1); i++) // loop over columns of range
				types[i - 1] = Useful.InferServerSideTyping(range[2, i]);

			// If any of the columns' j'th value doesn't match the type of their first value, then set their
			// perceived type to string.
			for(int i = range.GetLowerBound(1); i <= range.GetUpperBound(1); i++) // cols
			{
				for(int j = range.GetLowerBound(0) + 1; j <= range.GetUpperBound(0); j++) // rows
				{
					if(types[i - 1] != DSFieldType.String)
					{
						switch(range[j, i])
						{
							case string sv:
								if(sv.ToLower() != sv)
									types[i - 1] = DSFieldType.String;
								break;
							case bool bv:
								if(types[i - 1] != DSFieldType.Boolean)
									types[i - 1] = DSFieldType.String;
								break;
							case DateTime datev:
								if(types[i - 1] != DSFieldType.Date)
									types[i - 1] = DSFieldType.String;
								break;
							case double dv:
								if(types[i - 1] != DSFieldType.LowercaseString)
								{
									if((int)dv == dv)
									{
										if(types[i - 1] != DSFieldType.Double && types[i - 1] != DSFieldType.Integer)
											types[i - 1] = DSFieldType.String;
									}
									else if(types[i - 1] != DSFieldType.Double)
										types[i - 1] = DSFieldType.String;
								}
								break;
							default:
								types[i - 1] = DSFieldType.String;
								break;
						}
					}
				}
			}

			for(int i = range.GetLowerBound(1); i <= range.GetUpperBound(1); i++)
				backing.Add(range[1, i] as string, types[i - 1]);
		}

		#region ICustomTypeDescriptor
		public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
		public string GetClassName() => TypeDescriptor.GetClassName(this, true);
		public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);
		public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);
		public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);
		public PropertyDescriptor GetDefaultProperty() => null;
		public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);
		public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);
		public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);
		public PropertyDescriptorCollection GetProperties() => GetProperties(new Attribute[0]);
		public object GetPropertyOwner(PropertyDescriptor pd) => backing;
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			ArrayList s = new ArrayList(Count);
			foreach(KeyValuePair<string, object> kv in backing)
				s.Add(new DictionaryPropertyDescriptor(backing, kv.Key));

			return new PropertyDescriptorCollection((PropertyDescriptor[])s.ToArray(typeof(PropertyDescriptor)));
		}
		private class DictionaryPropertyDescriptor : PropertyDescriptor
		{
			private IDictionary<string, object> backing;
			private string key;

			public DictionaryPropertyDescriptor(IDictionary<string, object> backing, string key) : base(key, null)
			{
				this.backing = backing;
				this.key = key;
			}
			public override Type PropertyType { get => backing[key].GetType(); }
			public override void SetValue(object component, object value) => backing[key] = value;
			public override object GetValue(object component) => backing[key];
			public override bool IsReadOnly { get => backing.IsReadOnly; }
			public override Type ComponentType { get => null; }
			public override bool CanResetValue(object component) => false;
			public override void ResetValue(object component) { }
			public override bool ShouldSerializeValue(object component) => false;
		}
		#endregion
		#region IDictionary<string, object>
		public object this[string key]
		{
			get => backing[key];
			set => backing[key] = value;
		}
		public int Count
		{
			get => backing.Count;
		}
		public bool IsReadOnly => backing.IsReadOnly;
		public ICollection<string> Keys => backing.Keys;
		public ICollection<object> Values => backing.Values;
		public bool ContainsKey(string key) => backing.ContainsKey(key);
		bool IDictionary<string, object>.Remove(string key) => backing.Remove(key);
		public bool TryGetValue(string key, out object value) => backing.TryGetValue(key, out value);
		public void Add(string key, object value) => backing.Add(key, value);
		public void Add(KeyValuePair<string, object> item) => backing.Add(item);
		public void Remove(string key) => backing.Remove(key);
		public void Clear() => backing.Clear();
		public bool Contains(KeyValuePair<string, object> item) => backing.Contains(item);
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => backing.CopyTo(array, arrayIndex);
		public bool Remove(KeyValuePair<string, object> item) => backing.Remove(item);
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => backing.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => backing.GetEnumerator();
		#endregion

		/* I'm betting there's some prebuilt function to do this... */
		public string ToJson(int dsid)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("{\"datasourceId\":");
			sb.Append(dsid);
			sb.Append(",\"schema\":{\"schemaFields\":[");

			foreach(string k in backing.Keys)
			{
				sb.Append("{\"indexed\":true,\"stored\":true,\"fieldName\":\"");
				sb.Append(k);
				sb.Append("\",\"type\":\"");
				if(backing[k] is DSFieldType t)
					sb.Append(t.ToServerTypeString());
				else
					throw new InvalidOperationException(Resources.ErrSchemaModelInvalidUsage);
				sb.Append("\"},");
			}

			sb.Remove(sb.Length - 1, 1);
			sb.Append("]}}");

			return sb.ToString();
		}
	}

	// Possibly incomplete listing of serverside datatypes?
	public enum DSFieldType
	{
		Boolean = 0, // general
		Timestamp = 1, // ISO8601 datetime
		Integer = 2, // general/text
		LongString = 3, // general
		LowercaseString = 4, // general
		String = 5, // general
		Date = 6, // ISO8601 datetime
		Double = 7, // general
		Text = 8, // general
		Float = 9, // general
		Long = 10, // general/text
	}

	/* doing a using(...) block with an instance of this class will disable Excel's screen updating and event handling for the duration of the block. */
	sealed class ExcelNoScreenUpdate : IDisposable
	{
		public ExcelNoScreenUpdate()
		{
			Globals.ThisAddIn.Application.ScreenUpdating = false;
			Globals.ThisAddIn.Application.EnableEvents = false;
		}
		public void Dispose()
		{
			Globals.ThisAddIn.Application.ScreenUpdating = true;
			Globals.ThisAddIn.Application.EnableEvents = true;
		}
	}

	// This class wraps HWNDs so that we can use them in Form methods: annoying but seemingly necessary
	sealed class ParentHwnd : IWin32Window
	{
		public ParentHwnd(IntPtr p) => Handle = p;
		public IntPtr Handle { get; }
	}
}
