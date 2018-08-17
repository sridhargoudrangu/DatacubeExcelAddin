using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TestAddin.Properties;
using Microsoft.Win32;
using TestAddin.ACTIONS;
using TestAddin.UI;

namespace TestAddin.UI
{
	/*
	Originally, I thought this could be a single panel for both createnew/useexisting functionality.
	As I wrote code though, I realized I would have done better by making two separate panes.
	Despite this, I didn't want to throw away the existing code.
	This pane now uses two panels and swaps between them in order to simulate two panes.

	Aside from this, the pane is very similar in construction to ImportTaskPane.
	Many structures were copied verbatim from ImportTaskPane, so a bug here means there is a good chance of a bug in the import pane.
	*/
	public partial class ExportTaskPane : UserControl
	{
		private IEnumerable<IndexServerRecord> _isr;
		private IEnumerable<DataSourceRecord> _dsr;
		private bool _createnew = false; // use CreateNew to access this member
		private string _host;

		private IndexServerRecord SelectedIndexServer_cn { get => (IndexServerRecord)(cbxIndexServer_cn.SelectedItem ?? default(IndexServerRecord)); }
		private IndexServerRecord SelectedIndexServer_ue { get => (IndexServerRecord)(cbxIndexServer_ue.SelectedItem ?? default(IndexServerRecord)); }
		private DataSourceInfo SelectedDataSource { get => (DataSourceInfo)(cbxDataSource_ue.SelectedItem ?? default(DataSourceInfo)); }
		private bool CreateNew
		{
			get => _createnew;
			set
			{
				panCreate.Dock = value ? DockStyle.Fill : DockStyle.None;
				panExport.Dock = !value ? DockStyle.Fill : DockStyle.None;
				panCreate.Visible = panCreate.Enabled = value;
				panExport.Visible = panExport.Enabled = !value;
				_createnew = value;
			}
		}

		private ExportTaskPane()
		{
			InitializeComponent();
			SetControlColorScheme();
			Localize();
			panCreate.Location = panExport.Location = new Point(3, 3);
			CreateNew = false;
			INVISIBLE_LABEL_AUTODESTROY.Dispose(); // Gets rid of the developer label.
		}
		public ExportTaskPane(string host) : this()
		{
			_host = host;
			InternalCtorRefresh();
		}
		public ExportTaskPane(ExportTaskPane other) : this()
		{
			#region State Preservation

			_host = other._host;
			_isr = other._isr;
			_dsr = other._dsr;

			//chkTypeCSV_ue.Checked = other.chkTypeCSV_ue.Checked;
			//chkTypeOpenDataSource_ue.Checked = other.chkTypeOpenDataSource_ue.Checked;

			UpdateIndexServers();
			cbxIndexServer_cn.SelectedIndex = cbxIndexServer_cn.IndexOf(other.SelectedIndexServer_cn);
			cbxIndexServer_ue.SelectedIndex = cbxIndexServer_ue.IndexOf(other.SelectedIndexServer_ue);

			UpdateDataSources();
			cbxDataSource_ue.SelectedIndex = cbxDataSource_ue.IndexOf(other.SelectedDataSource);

			cbxDataSource_ue.Enabled = other.cbxDataSource_ue.Enabled;
			btnCreate.Enabled = other.btnCreate.Enabled;
			btnExport.Enabled = other.btnExport.Enabled;

			chkUseSelection_cn.Checked = other.chkUseSelection_cn.Checked;
			chkUseSelection_ue.Checked = other.chkUseSelection_ue.Checked;

			CreateNew = other.CreateNew;

			#endregion
			InternalCtorRefresh();
		}

		private void Localize()
		{
			lblIndexServer_cn.Text = Resources.CreateNewWhatIndexServer;
			lblDataSource_cn.Text = Resources.CreateNewDatasourceName;
			chkUseSelection_cn.Text = Resources.UseSelectedRegion;
			btnRefresh_cn.Text = Resources.Refresh;
			btnCreate.Text = Resources.Create;
			btnExportInstead.Text = Resources.ExportInstead;

			//lblDataSourceType_ue.Text = Resources.UseExistingTypeFilterText;
			lblIndexServer_ue.Text = Resources.UseExistingIndexServer;
			lblDataSource_ue.Text = Resources.UseExistingDataSource;
			//chkTypeOpenDataSource_ue.Text = Resources.OpenDataSource;
			//chkTypeCSV_ue.Text = Resources.CSV;
			chkUseSelection_ue.Text = Resources.UseSelectedRegion;
			btnRefresh_ue.Text = Resources.Refresh;
			btnExport.Text = Resources.Export;
			btnCreateInstead.Text = Resources.CreateInstead;
		}
		private void SetControlColorScheme()
		{
			// I used this question to find the applicable registry key:
			// https://stackoverflow.com/questions/4668248/office-2010-addin-development-can-i-from-code-behind-read-what-theme-the-user
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
					lblIndexServer_cn.ForeColor = SystemColors.ControlLight;
					lblDataSource_cn.ForeColor = SystemColors.ControlLight;
					//lblDataSourceType_ue.ForeColor = SystemColors.ControlLight;
					lblIndexServer_ue.ForeColor = SystemColors.ControlLight;
					lblDataSource_ue.ForeColor = SystemColors.ControlLight;
					//chkTypeOpenDataSource_ue.ForeColor = SystemColors.ControlLight;
					//chkTypeCSV_ue.ForeColor = SystemColors.ControlLight;
					btnRefresh_cn.BackColor = SystemColors.ControlLightLight;
					btnRefresh_ue.BackColor = SystemColors.ControlLightLight;
					btnCreateInstead.BackColor = SystemColors.ControlLightLight;
					btnExportInstead.BackColor = SystemColors.ControlLightLight;
					btnCreate.BackColor = SystemColors.ControlLightLight;
					btnExport.BackColor = SystemColors.ControlLightLight;
				}
			}
		}
		private void InternalCtorRefresh()
		{
			if(SelectedIndexServer_ue != default)
			{
				cbxDataSource_ue.Enabled = false;
				if(SelectedDataSource != default)
					btnExport.Enabled = true;
			}

			CanWeCreateANewDataSource(null, null);
			if(QueryIndexServers().Any(i => !cbxIndexServer_ue.Contains(i)) || _isr.Any(i => !cbxIndexServer_ue.Contains(i)))
				UpdateIndexServers();

			if(QueryDataSources().Any(d => !cbxDataSource_ue.Contains(d)))
				UpdateDataSources();
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
		private IEnumerable<DataSourceInfo> QueryDataSources()
		{
			if(!new Api.GetDataSources(_host).ExecuteLoggedIn(out string result))
			{
				Useful.Error($"{Resources.CannotQueryDataSources}\n{Resources.ErrorMsgCaps}\n{result}", Resources.Error);
				return null;
			}

			return (_dsr = DataSources.Deserialize(result).collections).FilterByIndexServer(SelectedIndexServer_ue).GetAllDataSources();
		}

		private void UpdateIndexServers()
		{
			IndexServerRecord active_ue = SelectedIndexServer_ue;
			IndexServerRecord active_cn = SelectedIndexServer_cn;

			cbxIndexServer_ue.Clear();
			cbxIndexServer_cn.Clear();
			cbxIndexServer_ue.Text = "";
			cbxIndexServer_cn.Text = "";
			foreach(IndexServerRecord i in _isr)
			{
				cbxIndexServer_ue.Add(i);
				cbxIndexServer_cn.Add(i);
			}

			if(active_ue != default)
				cbxIndexServer_ue.SelectedIndex = cbxIndexServer_ue.IndexOf(active_ue);
			if(active_cn != default)
				cbxIndexServer_cn.SelectedIndex = cbxIndexServer_cn.IndexOf(active_cn);
		}
		private void UpdateDataSources()
		{
			DataSourceInfo active = SelectedDataSource;
			cbxDataSource_ue.Clear();
			cbxDataSource_ue.Text = string.Empty;

			IEnumerable<DataSourceInfo> dsi = _dsr.FilterByIndexServer(SelectedIndexServer_ue).GetAllDataSources();

			foreach(DataSourceInfo i in dsi) // .Where(DataSourceMatchesFilters))
				cbxDataSource_ue.Add(i);

			if(active != default)
				cbxDataSource_ue.SelectedIndex = cbxDataSource_ue.IndexOf(active);
		}

		/*private bool DataSourceMatchesFilters(DataSourceInfo i)
			=> (chkTypeCSV_ue.Checked && i.datasourceType == DataSourceType.CSV)
			|| (chkTypeOpenDataSource_ue.Checked && i.datasourceType == DataSourceType.OpenDataSource);*/

		private void CanWeCreateANewDataSource(object sender, EventArgs e) => btnCreate.Enabled = (!string.IsNullOrWhiteSpace(txtDataSourceName_cn.Text) && SelectedIndexServer_cn != default);

		private void IndexServerSelected_ue(object sender, EventArgs e)
		{
			QueryDataSources();
			UpdateDataSources();
			cbxDataSource_ue.Enabled = true;
		}
		private void DataSourceSelected_ue(object sender, EventArgs e) => btnExport.Enabled = true;

		/*private void TypeFilterChanged_ue(object sender, EventArgs e)
		{
			QueryDataSources();
			UpdateDataSources();
		}*/

		private void BtnExport(object sender, EventArgs e) => Export.Execute(_host, SelectedDataSource.datasourceId, useSelection: chkUseSelection_ue.Checked);
		private void BtnCreate(object sender, EventArgs e) => Create.Execute(_host, txtDataSourceName_cn.Text, SelectedIndexServer_cn.cloudID, DataSourceType.OpenDataSource, useSelection: chkUseSelection_cn.Checked);
		private void BtnRefresh(object sender, EventArgs e)
		{
			if(QueryIndexServers().Any(i => !cbxIndexServer_ue.Contains(i)) || _isr.Any(i => !cbxIndexServer_cn.Contains(i)))
				UpdateIndexServers();

			if(QueryDataSources().Any(d => !cbxDataSource_ue.Contains(d)))
				UpdateDataSources();
		}

		private void CreateInstead(object sender, EventArgs e) => CreateNew = true;
		private void ExportInstead(object sender, EventArgs e) => CreateNew = false;
	}
}
