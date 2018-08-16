using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using TestAddin.Properties;
using Microsoft.Win32;
using TestAddin.ACTIONS;

namespace TestAddin.UI
{
	public partial class ImportTaskPane : UserControl
	{
		private IEnumerable<IndexServerRecord> _isr;
		private IEnumerable<DataSourceRecord> _dsr;
		private IEnumerable<Handler> _h;
		private string _host;

		private IndexServerRecord SelectedIndexServer { get => (IndexServerRecord)(cbxIndexServer.SelectedItem ?? default(IndexServerRecord)); }
		private DataSourceInfo SelectedDataSource { get => (DataSourceInfo)(cbxDataSource.SelectedItem ?? default(DataSourceInfo)); }
		private Handler SelectedHandler { get => (Handler)(cbxHandler.SelectedItem ?? default(Handler)); }

		private bool TypeFilterCheckboxesEnabled
		{
			set
			{
				gbxDataSourceTypes.Enabled =
				chkTypeDatabase.Enabled =
				chkTypeWebsite.Enabled =
				chkTypeCSV.Enabled =
				chkTypeFileSystem.Enabled =
				chkTypeNAS.Enabled =
				chkTypeEloqua.Enabled =
				chkTypeSalesforce.Enabled =
				chkTypeLDAP.Enabled =
				chkTypeFederatedSearch.Enabled =
				chkTypeOpenDataSource.Enabled =
				chkTypeHTTP.Enabled =
				chkTypeFacebook.Enabled =
				chkTypeTwitter.Enabled = value;
			}
		}

		private ImportTaskPane()
		{
			InitializeComponent();
			SetControlColorScheme();
			Localize();
		}
		public ImportTaskPane(string host) : this()
		{
			_host = host;
			InternalCtorRefresh();
		}
		public ImportTaskPane(ImportTaskPane other) : this()
		{
			#region State Preservation

			_isr = other._isr;
			_dsr = other._dsr;
			_h = other._h;

			_host = other._host;

			UpdateIndexServers();
			cbxIndexServer.SelectedIndex = cbxIndexServer.IndexOf(other.SelectedIndexServer);

			UpdateDataSources();
			cbxDataSource.SelectedIndex = cbxDataSource.IndexOf(other.SelectedDataSource);

			UpdateHandlers();
			cbxDataSource.SelectedIndex = cbxDataSource.IndexOf(other.SelectedDataSource);

			chkNoTypeFilter.Checked = other.chkNoTypeFilter.Checked;
			chkMakeLive.Checked = other.chkMakeLive.Checked;
			chkMakeLive.Visible = other.chkMakeLive.Visible;
			gbxDataSourceTypes.Enabled = other.gbxDataSourceTypes.Enabled;
			btnRefresh.Enabled = other.btnRefresh.Enabled;
			btnImport.Enabled = other.btnImport.Enabled;

			chkTypeDatabase.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeWebsite.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeCSV.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeFileSystem.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeNAS.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeEloqua.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeSalesforce.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeLDAP.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeFederatedSearch.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeOpenDataSource.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeHTTP.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeFacebook.CheckedChanged -= TypeFilterCheckboxStateChanged;
			chkTypeTwitter.CheckedChanged -= TypeFilterCheckboxStateChanged;

			chkTypeDatabase.Checked = other.chkTypeDatabase.Checked;
			chkTypeWebsite.Checked = other.chkTypeWebsite.Checked;
			chkTypeCSV.Checked = other.chkTypeCSV.Checked;
			chkTypeFileSystem.Checked = other.chkTypeFileSystem.Checked;
			chkTypeNAS.Checked = other.chkTypeNAS.Checked;
			chkTypeEloqua.Checked = other.chkTypeEloqua.Checked;
			chkTypeSalesforce.Checked = other.chkTypeSalesforce.Checked;
			chkTypeLDAP.Checked = other.chkTypeLDAP.Checked;
			chkTypeFederatedSearch.Checked = other.chkTypeFederatedSearch.Checked;
			chkTypeOpenDataSource.Checked = other.chkTypeOpenDataSource.Checked;
			chkTypeHTTP.Checked = other.chkTypeHTTP.Checked;

			chkTypeDatabase.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeWebsite.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeCSV.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeFileSystem.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeNAS.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeEloqua.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeSalesforce.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeLDAP.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeFederatedSearch.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeOpenDataSource.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeHTTP.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeFacebook.CheckedChanged += TypeFilterCheckboxStateChanged;
			chkTypeTwitter.CheckedChanged += TypeFilterCheckboxStateChanged;

			#endregion
			InternalCtorRefresh();
		}

		private void Localize()
		{
			lblInformation.Text = Useful.ActiveWorkbook is null ? Resources.NewWorkbook : ((Useful.ActiveSheet.UsedRange.IsEmpty()) ? Resources.WeWillUseThisSheet : Resources.NewSheet);
			lblIndexServer.Text = Resources.IndexServer;
			lblDataSource.Text = Resources.DataSource;
			lblHandler.Text = Resources.Handler;
			gbxDataSourceTypes.Text = Resources.ShowOnlyTheseTypes;
			chkNoTypeFilter.Text = Resources.ListAllDataSources;
			chkTypeDatabase.Text = Resources.Database;
			chkTypeWebsite.Text = Resources.Website;
			chkTypeCSV.Text = Resources.CSV;
			chkTypeFileSystem.Text = Resources.FileSystem;
			chkTypeNAS.Text = Resources.NAS;
			chkTypeEloqua.Text = Resources.Eloqua;
			chkTypeSalesforce.Text = Resources.Salesforce;
			chkTypeLDAP.Text = Resources.LDAP;
			chkTypeFederatedSearch.Text = Resources.FederatedSearch;
			chkTypeOpenDataSource.Text = Resources.OpenDataSource;
			chkTypeHTTP.Text = Resources.HTTP;
			chkTypeFacebook.Text = Resources.Facebook;
			chkTypeTwitter.Text = Resources.Twitter;
			chkMakeLive.Text = Resources.MakeLiveWorksheet;
			btnImport.Text = Resources.Import;
			btnRefresh.Text = Resources.Refresh;
		}
		private void SetControlColorScheme()
		{
			// Almost ripped off this but didn't:
			// https://stackoverflow.com/questions/4668248/office-2010-addin-development-can-i-from-code-behind-read-what-theme-the-user
			// I used that question to find the applicable registry key
			RegistryKey theme = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\16.0\Common");
			if(!(theme is null))
			{
				int? t;

				try
				{
					t = theme.GetValue("UI Theme") as int?;
				}
				finally
				{
					theme.Dispose();
				}

				if(!(t is null) && t > 1)
				{
					lblHandler.ForeColor = SystemColors.ControlLight;
					lblDataSource.ForeColor = SystemColors.ControlLight;
					lblIndexServer.ForeColor = SystemColors.ControlLight;
					lblInformation.ForeColor = SystemColors.ControlLight;
					gbxDataSourceTypes.ForeColor = SystemColors.ControlLight;
					chkTypeDatabase.ForeColor = SystemColors.ControlLight;
					chkTypeWebsite.ForeColor = SystemColors.ControlLight;
					chkTypeCSV.ForeColor = SystemColors.ControlLight;
					chkTypeFileSystem.ForeColor = SystemColors.ControlLight;
					chkTypeNAS.ForeColor = SystemColors.ControlLight;
					chkTypeEloqua.ForeColor = SystemColors.ControlLight;
					chkTypeSalesforce.ForeColor = SystemColors.ControlLight;
					chkTypeLDAP.ForeColor = SystemColors.ControlLight;
					chkTypeFederatedSearch.ForeColor = SystemColors.ControlLight;
					chkTypeOpenDataSource.ForeColor = SystemColors.ControlLight;
					chkTypeHTTP.ForeColor = SystemColors.ControlLight;
					chkMakeLive.ForeColor = SystemColors.ControlLight;
					btnImport.BackColor = SystemColors.ControlLightLight;
					btnRefresh.BackColor = SystemColors.ControlLightLight;
				}
			}
		}
		private void InternalCtorRefresh()
		{
			if(SelectedHandler == default)
				cbxDataSource.Enabled = cbxHandler.Enabled = btnImport.Enabled = false;

			if(QueryIndexServers().Any(i => !cbxIndexServer.Contains(i)))
				UpdateIndexServers();

			if(QueryDataSources().Any(d => !cbxDataSource.Contains(d)))
				UpdateDataSources();

			if(QueryHandlers().Any(d => !cbxHandler.Contains(d)))
				UpdateHandlers();
		}

		private IEnumerable<IndexServerRecord> QueryIndexServers()
		{
			if(!new Api.GetAnalyticsEngine(_host).ExecuteLoggedIn(out string result))
			{
				Useful.Error($"{Resources.CannotQueryIndexServers}\n{Resources.ErrorMsgCaps}\n{result}", Resources.Error);
				return null;
			}

			return _isr = IndexServers.Deserialize(result).listOfCIServer;
		}
		private IEnumerable<DataSourceInfo> QueryDataSources(bool filterByIndexServer = true)
		{
			if(!new Api.GetDataSources(_host).ExecuteLoggedIn(out string result))
			{
				Useful.Error($"{Resources.CannotQueryDataSources}\n{Resources.ErrorMsgCaps}\n{result}", Resources.Error);
				return null;
			}

			return (_dsr = DataSources.Deserialize(result).collections).FilterByIndexServer(SelectedIndexServer).GetAllDataSources();
		}
		private IEnumerable<Handler> QueryHandlers(bool filterByDataSource = true)
		{
			if(!new Api.GetHandlers(_host, filterByDataSource ? $"?datasourceId={SelectedDataSource.datasourceId}" : null).ExecuteLoggedIn(out string result))
			{
				Useful.Error($"{Resources.CannotQueryHandlers}\n{Resources.ErrorMsgCaps}\n{result}", Resources.Error);
				return null;
			}

			return _h = HandlerInfo.Deserialize(result).handlerInfos;
		}

		private void IndexServerChanged(object sender, EventArgs e)
		{
			QueryDataSources();
			UpdateDataSources();
			QueryHandlers(false);
			UpdateHandlers(false);
			cbxDataSource.Enabled = true;
		}
		private void DataSourceChanged(object sender, EventArgs e)
		{
			QueryHandlers();
			UpdateHandlers();
			cbxHandler.Enabled = true;
		}
		private void HandlerChanged(object sender, EventArgs e) => btnImport.Enabled = true;

		private void UpdateIndexServers()
		{
			IndexServerRecord active = SelectedIndexServer;
			cbxIndexServer.Clear();
			cbxIndexServer.Text = "";
			foreach(IndexServerRecord i in _isr)
				cbxIndexServer.Add(i);

			if(active != default)
				cbxIndexServer.SelectedIndex = cbxIndexServer.IndexOf(active);
		}
		private void UpdateDataSources()
		{
			DataSourceInfo active = SelectedDataSource;
			cbxDataSource.Clear();
			cbxDataSource.Text = string.Empty;

			IEnumerable<DataSourceInfo> dsi = _dsr.FilterByIndexServer(SelectedIndexServer).GetAllDataSources();

			// user wants to filter out some datasources
			if(!chkNoTypeFilter.Checked)
				foreach(DataSourceInfo i in dsi.Where(DataSourceMatchesFilters))
					cbxDataSource.Add(i);
			else // no filtering necessary
				foreach(DataSourceInfo i in dsi) // put all of them in the combobox
					cbxDataSource.Add(i);

			if(active != default)
				cbxDataSource.SelectedIndex = cbxDataSource.IndexOf(active);
		}
		private void UpdateHandlers(bool filterByDataSource = true)
		{
			cbxHandler.Items.Clear();
			cbxHandler.Text = string.Empty;
			foreach(Handler h in filterByDataSource ? _h.FilterByDSID(SelectedDataSource.datasourceId) : _h)
				cbxHandler.Add(h);

			try
			{
				cbxHandler.SelectedIndex = cbxHandler.IndexOfSingle<Handler>(h => h.handlerName == "default");
			}
			catch { }
		}

		private bool DataSourceMatchesFilters(DataSourceInfo i)
			=> (chkTypeDatabase.Checked && i.datasourceType == DataSourceType.Database)
			|| (chkTypeWebsite.Checked && i.datasourceType == DataSourceType.Website)
			|| (chkTypeCSV.Checked && i.datasourceType == DataSourceType.CSV)
			|| (chkTypeFileSystem.Checked && i.datasourceType == DataSourceType.FileSystem)
			|| (chkTypeNAS.Checked && i.datasourceType == DataSourceType.NAS)
			|| (chkTypeEloqua.Checked && i.datasourceType == DataSourceType.Eloqua)
			|| (chkTypeSalesforce.Checked && i.datasourceType == DataSourceType.Salesforce)
			|| (chkTypeLDAP.Checked && i.datasourceType == DataSourceType.LDAP)
			|| (chkTypeFederatedSearch.Checked && i.datasourceType == DataSourceType.FederatedSearch)
			|| (chkTypeOpenDataSource.Checked && i.datasourceType == DataSourceType.OpenDataSource)
			|| (chkTypeHTTP.Checked && i.datasourceType == DataSourceType.HTTP)
			|| (chkTypeFacebook.Checked && i.datasourceType == DataSourceType.Facebook)
			|| (chkTypeTwitter.Checked && i.datasourceType == DataSourceType.Twitter);

		private void btnImport_Click(object sender, EventArgs e)
		{
			Import.Execute(_host, SelectedHandler, chkMakeLive.Checked);
			lblInformation.Text = Resources.NewSheet;
		}
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			if(QueryIndexServers().Any(i => !cbxIndexServer.Contains(i)))
				UpdateIndexServers();

			if(QueryDataSources().Any(d => !cbxDataSource.Contains(d)))
				UpdateDataSources();

			QueryHandlers();
			UpdateHandlers();
		}

		private void NoTypeFilterCheckboxStateChanged(object sender, EventArgs e)
		{
			TypeFilterCheckboxesEnabled = !chkNoTypeFilter.Checked;
			UpdateDataSources();
			UpdateHandlers();
		}
		private void TypeFilterCheckboxStateChanged(object sender, EventArgs e)
		{
			UpdateDataSources();
			UpdateHandlers();
		}
	}
}
