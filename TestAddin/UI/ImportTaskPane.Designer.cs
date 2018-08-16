namespace TestAddin.UI
{
	partial class ImportTaskPane
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> Clean up any resources being used. </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary> Required method for Designer support - do not modify the contents of this method with the code editor. </summary>
		private void InitializeComponent()
		{
			this.cbxIndexServer = new System.Windows.Forms.ComboBox();
			this.cbxDataSource = new System.Windows.Forms.ComboBox();
			this.cbxHandler = new System.Windows.Forms.ComboBox();
			this.btnImport = new System.Windows.Forms.Button();
			this.lblIndexServer = new System.Windows.Forms.Label();
			this.lblDataSource = new System.Windows.Forms.Label();
			this.lblHandler = new System.Windows.Forms.Label();
			this.lblInformation = new System.Windows.Forms.Label();
			this.gbxDataSourceTypes = new System.Windows.Forms.GroupBox();
			this.chkTypeTwitter = new System.Windows.Forms.CheckBox();
			this.chkTypeFacebook = new System.Windows.Forms.CheckBox();
			this.chkTypeHTTP = new System.Windows.Forms.CheckBox();
			this.chkTypeOpenDataSource = new System.Windows.Forms.CheckBox();
			this.chkTypeFederatedSearch = new System.Windows.Forms.CheckBox();
			this.chkTypeLDAP = new System.Windows.Forms.CheckBox();
			this.chkTypeSalesforce = new System.Windows.Forms.CheckBox();
			this.chkTypeEloqua = new System.Windows.Forms.CheckBox();
			this.chkTypeNAS = new System.Windows.Forms.CheckBox();
			this.chkTypeFileSystem = new System.Windows.Forms.CheckBox();
			this.chkTypeCSV = new System.Windows.Forms.CheckBox();
			this.chkTypeWebsite = new System.Windows.Forms.CheckBox();
			this.chkTypeDatabase = new System.Windows.Forms.CheckBox();
			this.chkNoTypeFilter = new System.Windows.Forms.CheckBox();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.chkMakeLive = new System.Windows.Forms.CheckBox();
			this.gbxDataSourceTypes.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbxIndexServer
			// 
			this.cbxIndexServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxIndexServer.FormattingEnabled = true;
			this.cbxIndexServer.Location = new System.Drawing.Point(3, 42);
			this.cbxIndexServer.Name = "cbxIndexServer";
			this.cbxIndexServer.Size = new System.Drawing.Size(294, 21);
			this.cbxIndexServer.TabIndex = 0;
			this.cbxIndexServer.SelectedIndexChanged += new System.EventHandler(this.IndexServerChanged);
			// 
			// cbxDataSource
			// 
			this.cbxDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxDataSource.Enabled = false;
			this.cbxDataSource.FormattingEnabled = true;
			this.cbxDataSource.Location = new System.Drawing.Point(3, 453);
			this.cbxDataSource.Name = "cbxDataSource";
			this.cbxDataSource.Size = new System.Drawing.Size(294, 21);
			this.cbxDataSource.TabIndex = 1;
			this.cbxDataSource.SelectedIndexChanged += new System.EventHandler(this.DataSourceChanged);
			// 
			// cbxHandler
			// 
			this.cbxHandler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxHandler.Enabled = false;
			this.cbxHandler.FormattingEnabled = true;
			this.cbxHandler.Location = new System.Drawing.Point(3, 511);
			this.cbxHandler.Name = "cbxHandler";
			this.cbxHandler.Size = new System.Drawing.Size(294, 21);
			this.cbxHandler.TabIndex = 2;
			this.cbxHandler.SelectedIndexChanged += new System.EventHandler(this.HandlerChanged);
			// 
			// btnImport
			// 
			this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnImport.Enabled = false;
			this.btnImport.Location = new System.Drawing.Point(222, 569);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 3;
			this.btnImport.Text = "btnImport";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// lblIndexServer
			// 
			this.lblIndexServer.AutoSize = true;
			this.lblIndexServer.Location = new System.Drawing.Point(3, 21);
			this.lblIndexServer.Name = "lblIndexServer";
			this.lblIndexServer.Size = new System.Drawing.Size(74, 13);
			this.lblIndexServer.TabIndex = 4;
			this.lblIndexServer.Text = "lblIndexServer";
			// 
			// lblDataSource
			// 
			this.lblDataSource.AutoSize = true;
			this.lblDataSource.Location = new System.Drawing.Point(3, 432);
			this.lblDataSource.Name = "lblDataSource";
			this.lblDataSource.Size = new System.Drawing.Size(74, 13);
			this.lblDataSource.TabIndex = 5;
			this.lblDataSource.Text = "lblDataSource";
			// 
			// lblHandler
			// 
			this.lblHandler.AutoSize = true;
			this.lblHandler.Location = new System.Drawing.Point(3, 490);
			this.lblHandler.Name = "lblHandler";
			this.lblHandler.Size = new System.Drawing.Size(54, 13);
			this.lblHandler.TabIndex = 6;
			this.lblHandler.Text = "lblHandler";
			// 
			// lblInformation
			// 
			this.lblInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblInformation.Location = new System.Drawing.Point(-614, 569);
			this.lblInformation.Name = "lblInformation";
			this.lblInformation.Size = new System.Drawing.Size(830, 23);
			this.lblInformation.TabIndex = 7;
			this.lblInformation.Text = "lblInformation";
			this.lblInformation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gbxDataSourceTypes
			// 
			this.gbxDataSourceTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbxDataSourceTypes.AutoSize = true;
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeTwitter);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeFacebook);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeHTTP);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeOpenDataSource);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeFederatedSearch);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeLDAP);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeSalesforce);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeEloqua);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeNAS);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeFileSystem);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeCSV);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeWebsite);
			this.gbxDataSourceTypes.Controls.Add(this.chkTypeDatabase);
			this.gbxDataSourceTypes.Enabled = false;
			this.gbxDataSourceTypes.Location = new System.Drawing.Point(0, 98);
			this.gbxDataSourceTypes.Name = "gbxDataSourceTypes";
			this.gbxDataSourceTypes.Size = new System.Drawing.Size(297, 328);
			this.gbxDataSourceTypes.TabIndex = 19;
			this.gbxDataSourceTypes.TabStop = false;
			this.gbxDataSourceTypes.Text = "gbxDataSourceTypes";
			// 
			// chkTypeTwitter
			// 
			this.chkTypeTwitter.AutoSize = true;
			this.chkTypeTwitter.Checked = true;
			this.chkTypeTwitter.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeTwitter.Enabled = false;
			this.chkTypeTwitter.Location = new System.Drawing.Point(6, 292);
			this.chkTypeTwitter.Name = "chkTypeTwitter";
			this.chkTypeTwitter.Size = new System.Drawing.Size(100, 17);
			this.chkTypeTwitter.TabIndex = 31;
			this.chkTypeTwitter.Text = "chkTypeTwitter";
			this.chkTypeTwitter.UseVisualStyleBackColor = true;
			// 
			// chkTypeFacebook
			// 
			this.chkTypeFacebook.AutoSize = true;
			this.chkTypeFacebook.Checked = true;
			this.chkTypeFacebook.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeFacebook.Enabled = false;
			this.chkTypeFacebook.Location = new System.Drawing.Point(6, 269);
			this.chkTypeFacebook.Name = "chkTypeFacebook";
			this.chkTypeFacebook.Size = new System.Drawing.Size(116, 17);
			this.chkTypeFacebook.TabIndex = 30;
			this.chkTypeFacebook.Text = "chkTypeFacebook";
			this.chkTypeFacebook.UseVisualStyleBackColor = true;
			// 
			// chkTypeHTTP
			// 
			this.chkTypeHTTP.AutoSize = true;
			this.chkTypeHTTP.Checked = true;
			this.chkTypeHTTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeHTTP.Enabled = false;
			this.chkTypeHTTP.Location = new System.Drawing.Point(6, 246);
			this.chkTypeHTTP.Name = "chkTypeHTTP";
			this.chkTypeHTTP.Size = new System.Drawing.Size(97, 17);
			this.chkTypeHTTP.TabIndex = 29;
			this.chkTypeHTTP.Text = "chkTypeHTTP";
			this.chkTypeHTTP.UseVisualStyleBackColor = true;
			this.chkTypeHTTP.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeOpenDataSource
			// 
			this.chkTypeOpenDataSource.AutoSize = true;
			this.chkTypeOpenDataSource.Checked = true;
			this.chkTypeOpenDataSource.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeOpenDataSource.Enabled = false;
			this.chkTypeOpenDataSource.Location = new System.Drawing.Point(6, 223);
			this.chkTypeOpenDataSource.Name = "chkTypeOpenDataSource";
			this.chkTypeOpenDataSource.Size = new System.Drawing.Size(151, 17);
			this.chkTypeOpenDataSource.TabIndex = 28;
			this.chkTypeOpenDataSource.Text = "chkTypeOpenDataSource";
			this.chkTypeOpenDataSource.UseVisualStyleBackColor = true;
			this.chkTypeOpenDataSource.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeFederatedSearch
			// 
			this.chkTypeFederatedSearch.AutoSize = true;
			this.chkTypeFederatedSearch.Checked = true;
			this.chkTypeFederatedSearch.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeFederatedSearch.Enabled = false;
			this.chkTypeFederatedSearch.Location = new System.Drawing.Point(6, 200);
			this.chkTypeFederatedSearch.Name = "chkTypeFederatedSearch";
			this.chkTypeFederatedSearch.Size = new System.Drawing.Size(150, 17);
			this.chkTypeFederatedSearch.TabIndex = 27;
			this.chkTypeFederatedSearch.Text = "chkTypeFederatedSearch";
			this.chkTypeFederatedSearch.UseVisualStyleBackColor = true;
			this.chkTypeFederatedSearch.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeLDAP
			// 
			this.chkTypeLDAP.AutoSize = true;
			this.chkTypeLDAP.Checked = true;
			this.chkTypeLDAP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeLDAP.Enabled = false;
			this.chkTypeLDAP.Location = new System.Drawing.Point(6, 177);
			this.chkTypeLDAP.Name = "chkTypeLDAP";
			this.chkTypeLDAP.Size = new System.Drawing.Size(96, 17);
			this.chkTypeLDAP.TabIndex = 26;
			this.chkTypeLDAP.Text = "chkTypeLDAP";
			this.chkTypeLDAP.UseVisualStyleBackColor = true;
			this.chkTypeLDAP.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeSalesforce
			// 
			this.chkTypeSalesforce.AutoSize = true;
			this.chkTypeSalesforce.Checked = true;
			this.chkTypeSalesforce.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeSalesforce.Location = new System.Drawing.Point(6, 154);
			this.chkTypeSalesforce.Name = "chkTypeSalesforce";
			this.chkTypeSalesforce.Size = new System.Drawing.Size(118, 17);
			this.chkTypeSalesforce.TabIndex = 25;
			this.chkTypeSalesforce.Text = "chkTypeSalesforce";
			this.chkTypeSalesforce.UseVisualStyleBackColor = true;
			this.chkTypeSalesforce.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeEloqua
			// 
			this.chkTypeEloqua.AutoSize = true;
			this.chkTypeEloqua.Checked = true;
			this.chkTypeEloqua.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeEloqua.Enabled = false;
			this.chkTypeEloqua.Location = new System.Drawing.Point(6, 131);
			this.chkTypeEloqua.Name = "chkTypeEloqua";
			this.chkTypeEloqua.Size = new System.Drawing.Size(101, 17);
			this.chkTypeEloqua.TabIndex = 24;
			this.chkTypeEloqua.Text = "chkTypeEloqua";
			this.chkTypeEloqua.UseVisualStyleBackColor = true;
			this.chkTypeEloqua.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeNAS
			// 
			this.chkTypeNAS.AutoSize = true;
			this.chkTypeNAS.Checked = true;
			this.chkTypeNAS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeNAS.Enabled = false;
			this.chkTypeNAS.Location = new System.Drawing.Point(6, 108);
			this.chkTypeNAS.Name = "chkTypeNAS";
			this.chkTypeNAS.Size = new System.Drawing.Size(90, 17);
			this.chkTypeNAS.TabIndex = 23;
			this.chkTypeNAS.Text = "chkTypeNAS";
			this.chkTypeNAS.UseVisualStyleBackColor = true;
			this.chkTypeNAS.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeFileSystem
			// 
			this.chkTypeFileSystem.AutoSize = true;
			this.chkTypeFileSystem.Checked = true;
			this.chkTypeFileSystem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeFileSystem.Enabled = false;
			this.chkTypeFileSystem.Location = new System.Drawing.Point(6, 85);
			this.chkTypeFileSystem.Name = "chkTypeFileSystem";
			this.chkTypeFileSystem.Size = new System.Drawing.Size(118, 17);
			this.chkTypeFileSystem.TabIndex = 22;
			this.chkTypeFileSystem.Text = "chkTypeFileSystem";
			this.chkTypeFileSystem.UseVisualStyleBackColor = true;
			this.chkTypeFileSystem.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeCSV
			// 
			this.chkTypeCSV.AutoSize = true;
			this.chkTypeCSV.Checked = true;
			this.chkTypeCSV.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeCSV.Enabled = false;
			this.chkTypeCSV.Location = new System.Drawing.Point(6, 62);
			this.chkTypeCSV.Name = "chkTypeCSV";
			this.chkTypeCSV.Size = new System.Drawing.Size(89, 17);
			this.chkTypeCSV.TabIndex = 21;
			this.chkTypeCSV.Text = "chkTypeCSV";
			this.chkTypeCSV.UseVisualStyleBackColor = true;
			this.chkTypeCSV.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeWebsite
			// 
			this.chkTypeWebsite.AutoSize = true;
			this.chkTypeWebsite.Checked = true;
			this.chkTypeWebsite.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeWebsite.Enabled = false;
			this.chkTypeWebsite.Location = new System.Drawing.Point(6, 39);
			this.chkTypeWebsite.Name = "chkTypeWebsite";
			this.chkTypeWebsite.Size = new System.Drawing.Size(107, 17);
			this.chkTypeWebsite.TabIndex = 20;
			this.chkTypeWebsite.Text = "chkTypeWebsite";
			this.chkTypeWebsite.UseVisualStyleBackColor = true;
			this.chkTypeWebsite.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkTypeDatabase
			// 
			this.chkTypeDatabase.AutoSize = true;
			this.chkTypeDatabase.Checked = true;
			this.chkTypeDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTypeDatabase.Location = new System.Drawing.Point(6, 16);
			this.chkTypeDatabase.Name = "chkTypeDatabase";
			this.chkTypeDatabase.Size = new System.Drawing.Size(114, 17);
			this.chkTypeDatabase.TabIndex = 19;
			this.chkTypeDatabase.Text = "chkTypeDatabase";
			this.chkTypeDatabase.UseVisualStyleBackColor = true;
			this.chkTypeDatabase.CheckedChanged += new System.EventHandler(this.TypeFilterCheckboxStateChanged);
			// 
			// chkNoTypeFilter
			// 
			this.chkNoTypeFilter.AutoSize = true;
			this.chkNoTypeFilter.Checked = true;
			this.chkNoTypeFilter.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNoTypeFilter.Location = new System.Drawing.Point(6, 75);
			this.chkNoTypeFilter.Name = "chkNoTypeFilter";
			this.chkNoTypeFilter.Size = new System.Drawing.Size(104, 17);
			this.chkNoTypeFilter.TabIndex = 30;
			this.chkNoTypeFilter.Text = "chkNoTypeFilter";
			this.chkNoTypeFilter.UseVisualStyleBackColor = true;
			this.chkNoTypeFilter.CheckedChanged += new System.EventHandler(this.NoTypeFilterCheckboxStateChanged);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Location = new System.Drawing.Point(222, 69);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 31;
			this.btnRefresh.Text = "btnRefresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// chkMakeLive
			// 
			this.chkMakeLive.AutoSize = true;
			this.chkMakeLive.Enabled = false;
			this.chkMakeLive.Location = new System.Drawing.Point(6, 549);
			this.chkMakeLive.Name = "chkMakeLive";
			this.chkMakeLive.Size = new System.Drawing.Size(91, 17);
			this.chkMakeLive.TabIndex = 32;
			this.chkMakeLive.Text = "chkMakeLive";
			this.chkMakeLive.UseVisualStyleBackColor = true;
			// 
			// ImportTaskPane
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.chkMakeLive);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.chkNoTypeFilter);
			this.Controls.Add(this.gbxDataSourceTypes);
			this.Controls.Add(this.lblInformation);
			this.Controls.Add(this.lblHandler);
			this.Controls.Add(this.lblDataSource);
			this.Controls.Add(this.lblIndexServer);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.cbxHandler);
			this.Controls.Add(this.cbxDataSource);
			this.Controls.Add(this.cbxIndexServer);
			this.Name = "ImportTaskPane";
			this.Size = new System.Drawing.Size(300, 766);
			this.gbxDataSourceTypes.ResumeLayout(false);
			this.gbxDataSourceTypes.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.ComboBox cbxIndexServer;
		private System.Windows.Forms.ComboBox cbxDataSource;
		private System.Windows.Forms.ComboBox cbxHandler;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Label lblIndexServer;
		private System.Windows.Forms.Label lblDataSource;
		private System.Windows.Forms.Label lblHandler;
		private System.Windows.Forms.Label lblInformation;
		private System.Windows.Forms.GroupBox gbxDataSourceTypes;
		private System.Windows.Forms.CheckBox chkTypeHTTP;
		private System.Windows.Forms.CheckBox chkTypeOpenDataSource;
		private System.Windows.Forms.CheckBox chkTypeFederatedSearch;
		private System.Windows.Forms.CheckBox chkTypeLDAP;
		private System.Windows.Forms.CheckBox chkTypeSalesforce;
		private System.Windows.Forms.CheckBox chkTypeEloqua;
		private System.Windows.Forms.CheckBox chkTypeNAS;
		private System.Windows.Forms.CheckBox chkTypeFileSystem;
		private System.Windows.Forms.CheckBox chkTypeCSV;
		private System.Windows.Forms.CheckBox chkTypeWebsite;
		private System.Windows.Forms.CheckBox chkTypeDatabase;
		private System.Windows.Forms.CheckBox chkNoTypeFilter;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.CheckBox chkTypeTwitter;
		private System.Windows.Forms.CheckBox chkTypeFacebook;
		private System.Windows.Forms.CheckBox chkMakeLive;
	}
}
