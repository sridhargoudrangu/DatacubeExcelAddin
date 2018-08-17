namespace TestAddin.UI
{
	partial class ExportTaskPane
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnRefresh_cn = new System.Windows.Forms.Button();
			this.lblIndexServer_cn = new System.Windows.Forms.Label();
			this.cbxIndexServer_cn = new System.Windows.Forms.ComboBox();
			this.btnCreate = new System.Windows.Forms.Button();
			this.lblDataSource_cn = new System.Windows.Forms.Label();
			this.txtDataSourceName_cn = new System.Windows.Forms.TextBox();
			this.btnRefresh_ue = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.lblDataSource_ue = new System.Windows.Forms.Label();
			this.cbxDataSource_ue = new System.Windows.Forms.ComboBox();
			this.lblIndexServer_ue = new System.Windows.Forms.Label();
			this.cbxIndexServer_ue = new System.Windows.Forms.ComboBox();
			this.panCreate = new System.Windows.Forms.Panel();
			this.chkUseSelection_cn = new System.Windows.Forms.CheckBox();
			this.btnExportInstead = new System.Windows.Forms.Button();
			this.panExport = new System.Windows.Forms.Panel();
			this.chkUseSelection_ue = new System.Windows.Forms.CheckBox();
			this.btnCreateInstead = new System.Windows.Forms.Button();
			this.INVISIBLE_LABEL_AUTODESTROY = new System.Windows.Forms.Label();
			this.panCreate.SuspendLayout();
			this.panExport.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnRefresh_cn
			// 
			this.btnRefresh_cn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh_cn.Location = new System.Drawing.Point(234, 52);
			this.btnRefresh_cn.Name = "btnRefresh_cn";
			this.btnRefresh_cn.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh_cn.TabIndex = 10;
			this.btnRefresh_cn.Text = "btnRefresh_cn";
			this.btnRefresh_cn.UseVisualStyleBackColor = true;
			this.btnRefresh_cn.Click += new System.EventHandler(this.BtnRefresh);
			// 
			// lblIndexServer_cn
			// 
			this.lblIndexServer_cn.AutoSize = true;
			this.lblIndexServer_cn.Location = new System.Drawing.Point(3, 0);
			this.lblIndexServer_cn.Name = "lblIndexServer_cn";
			this.lblIndexServer_cn.Size = new System.Drawing.Size(92, 13);
			this.lblIndexServer_cn.TabIndex = 9;
			this.lblIndexServer_cn.Text = "lblIndexServer_cn";
			// 
			// cbxIndexServer_cn
			// 
			this.cbxIndexServer_cn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxIndexServer_cn.FormattingEnabled = true;
			this.cbxIndexServer_cn.Location = new System.Drawing.Point(3, 21);
			this.cbxIndexServer_cn.Name = "cbxIndexServer_cn";
			this.cbxIndexServer_cn.Size = new System.Drawing.Size(306, 21);
			this.cbxIndexServer_cn.TabIndex = 8;
			this.cbxIndexServer_cn.SelectedIndexChanged += new System.EventHandler(this.CanWeCreateANewDataSource);
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.Enabled = false;
			this.btnCreate.Location = new System.Drawing.Point(234, 134);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(75, 23);
			this.btnCreate.TabIndex = 6;
			this.btnCreate.Text = "btnCreate";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.BtnCreate);
			// 
			// lblDataSource_cn
			// 
			this.lblDataSource_cn.AutoSize = true;
			this.lblDataSource_cn.Location = new System.Drawing.Point(3, 80);
			this.lblDataSource_cn.Name = "lblDataSource_cn";
			this.lblDataSource_cn.Size = new System.Drawing.Size(92, 13);
			this.lblDataSource_cn.TabIndex = 4;
			this.lblDataSource_cn.Text = "lblDataSource_cn";
			// 
			// txtDataSourceName_cn
			// 
			this.txtDataSourceName_cn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDataSourceName_cn.Location = new System.Drawing.Point(3, 101);
			this.txtDataSourceName_cn.Name = "txtDataSourceName_cn";
			this.txtDataSourceName_cn.Size = new System.Drawing.Size(306, 20);
			this.txtDataSourceName_cn.TabIndex = 3;
			this.txtDataSourceName_cn.TextChanged += new System.EventHandler(this.CanWeCreateANewDataSource);
			// 
			// btnRefresh_ue
			// 
			this.btnRefresh_ue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh_ue.Location = new System.Drawing.Point(234, 43);
			this.btnRefresh_ue.Name = "btnRefresh_ue";
			this.btnRefresh_ue.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh_ue.TabIndex = 7;
			this.btnRefresh_ue.Text = "btnRefresh_ue";
			this.btnRefresh_ue.UseVisualStyleBackColor = true;
			this.btnRefresh_ue.Click += new System.EventHandler(this.BtnRefresh);
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Enabled = false;
			this.btnExport.Location = new System.Drawing.Point(234, 132);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 6;
			this.btnExport.Text = "btnExport";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.BtnExport);
			// 
			// lblDataSource_ue
			// 
			this.lblDataSource_ue.AutoSize = true;
			this.lblDataSource_ue.Location = new System.Drawing.Point(3, 76);
			this.lblDataSource_ue.Name = "lblDataSource_ue";
			this.lblDataSource_ue.Size = new System.Drawing.Size(92, 13);
			this.lblDataSource_ue.TabIndex = 3;
			this.lblDataSource_ue.Text = "lblDataSource_ue";
			// 
			// cbxDataSource_ue
			// 
			this.cbxDataSource_ue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxDataSource_ue.Enabled = false;
			this.cbxDataSource_ue.FormattingEnabled = true;
			this.cbxDataSource_ue.Location = new System.Drawing.Point(3, 92);
			this.cbxDataSource_ue.Name = "cbxDataSource_ue";
			this.cbxDataSource_ue.Size = new System.Drawing.Size(306, 21);
			this.cbxDataSource_ue.TabIndex = 2;
			this.cbxDataSource_ue.SelectedIndexChanged += new System.EventHandler(this.DataSourceSelected_ue);
			// 
			// lblIndexServer_ue
			// 
			this.lblIndexServer_ue.AutoSize = true;
			this.lblIndexServer_ue.Location = new System.Drawing.Point(3, 0);
			this.lblIndexServer_ue.Name = "lblIndexServer_ue";
			this.lblIndexServer_ue.Size = new System.Drawing.Size(92, 13);
			this.lblIndexServer_ue.TabIndex = 1;
			this.lblIndexServer_ue.Text = "lblIndexServer_ue";
			// 
			// cbxIndexServer_ue
			// 
			this.cbxIndexServer_ue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxIndexServer_ue.FormattingEnabled = true;
			this.cbxIndexServer_ue.Location = new System.Drawing.Point(3, 16);
			this.cbxIndexServer_ue.Name = "cbxIndexServer_ue";
			this.cbxIndexServer_ue.Size = new System.Drawing.Size(306, 21);
			this.cbxIndexServer_ue.TabIndex = 0;
			this.cbxIndexServer_ue.SelectedIndexChanged += new System.EventHandler(this.IndexServerSelected_ue);
			// 
			// panCreate
			// 
			this.panCreate.Controls.Add(this.chkUseSelection_cn);
			this.panCreate.Controls.Add(this.btnExportInstead);
			this.panCreate.Controls.Add(this.lblIndexServer_cn);
			this.panCreate.Controls.Add(this.btnRefresh_cn);
			this.panCreate.Controls.Add(this.lblDataSource_cn);
			this.panCreate.Controls.Add(this.btnCreate);
			this.panCreate.Controls.Add(this.txtDataSourceName_cn);
			this.panCreate.Controls.Add(this.cbxIndexServer_cn);
			this.panCreate.Enabled = false;
			this.panCreate.Location = new System.Drawing.Point(3, 321);
			this.panCreate.Name = "panCreate";
			this.panCreate.Size = new System.Drawing.Size(319, 276);
			this.panCreate.TabIndex = 11;
			this.panCreate.Visible = false;
			// 
			// chkUseSelection_cn
			// 
			this.chkUseSelection_cn.AutoSize = true;
			this.chkUseSelection_cn.Location = new System.Drawing.Point(3, 163);
			this.chkUseSelection_cn.Name = "chkUseSelection_cn";
			this.chkUseSelection_cn.Size = new System.Drawing.Size(125, 17);
			this.chkUseSelection_cn.TabIndex = 14;
			this.chkUseSelection_cn.Text = "chkUseSelection_cn";
			this.chkUseSelection_cn.UseVisualStyleBackColor = true;
			// 
			// btnExportInstead
			// 
			this.btnExportInstead.Location = new System.Drawing.Point(3, 134);
			this.btnExportInstead.Name = "btnExportInstead";
			this.btnExportInstead.Size = new System.Drawing.Size(100, 23);
			this.btnExportInstead.TabIndex = 14;
			this.btnExportInstead.Text = "btnExportInstead";
			this.btnExportInstead.UseVisualStyleBackColor = true;
			this.btnExportInstead.Click += new System.EventHandler(this.ExportInstead);
			// 
			// panExport
			// 
			this.panExport.Controls.Add(this.chkUseSelection_ue);
			this.panExport.Controls.Add(this.btnCreateInstead);
			this.panExport.Controls.Add(this.lblIndexServer_ue);
			this.panExport.Controls.Add(this.cbxIndexServer_ue);
			this.panExport.Controls.Add(this.cbxDataSource_ue);
			this.panExport.Controls.Add(this.btnRefresh_ue);
			this.panExport.Controls.Add(this.lblDataSource_ue);
			this.panExport.Controls.Add(this.btnExport);
			this.panExport.Location = new System.Drawing.Point(3, 3);
			this.panExport.Name = "panExport";
			this.panExport.Size = new System.Drawing.Size(319, 312);
			this.panExport.TabIndex = 12;
			// 
			// chkUseSelection_ue
			// 
			this.chkUseSelection_ue.AutoSize = true;
			this.chkUseSelection_ue.Location = new System.Drawing.Point(3, 161);
			this.chkUseSelection_ue.Name = "chkUseSelection_ue";
			this.chkUseSelection_ue.Size = new System.Drawing.Size(125, 17);
			this.chkUseSelection_ue.TabIndex = 15;
			this.chkUseSelection_ue.Text = "chkUseSelection_ue";
			this.chkUseSelection_ue.UseVisualStyleBackColor = true;
			// 
			// btnCreateInstead
			// 
			this.btnCreateInstead.Location = new System.Drawing.Point(3, 132);
			this.btnCreateInstead.Name = "btnCreateInstead";
			this.btnCreateInstead.Size = new System.Drawing.Size(100, 23);
			this.btnCreateInstead.TabIndex = 13;
			this.btnCreateInstead.Text = "btnCreateInstead";
			this.btnCreateInstead.UseVisualStyleBackColor = true;
			this.btnCreateInstead.Click += new System.EventHandler(this.CreateInstead);
			// 
			// INVISIBLE_LABEL_AUTODESTROY
			// 
			this.INVISIBLE_LABEL_AUTODESTROY.AutoSize = true;
			this.INVISIBLE_LABEL_AUTODESTROY.Location = new System.Drawing.Point(3, 600);
			this.INVISIBLE_LABEL_AUTODESTROY.Name = "INVISIBLE_LABEL_AUTODESTROY";
			this.INVISIBLE_LABEL_AUTODESTROY.Size = new System.Drawing.Size(234, 65);
			this.INVISIBLE_LABEL_AUTODESTROY.TabIndex = 13;
			this.INVISIBLE_LABEL_AUTODESTROY.Text = "!! NOTE !!\r\n1. Only one of the two panels above will ever be\r\n    displayed to th" +
    "e user at any time.\r\n2. This label does not appear on the form at\r\n    runtime.";
			// 
			// ExportTaskPane
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.INVISIBLE_LABEL_AUTODESTROY);
			this.Controls.Add(this.panExport);
			this.Controls.Add(this.panCreate);
			this.Name = "ExportTaskPane";
			this.Size = new System.Drawing.Size(325, 766);
			this.panCreate.ResumeLayout(false);
			this.panCreate.PerformLayout();
			this.panExport.ResumeLayout(false);
			this.panExport.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblDataSource_cn;
		private System.Windows.Forms.TextBox txtDataSourceName_cn;
		private System.Windows.Forms.Label lblIndexServer_ue;
		private System.Windows.Forms.ComboBox cbxIndexServer_ue;
		private System.Windows.Forms.Label lblDataSource_ue;
		private System.Windows.Forms.ComboBox cbxDataSource_ue;
		private System.Windows.Forms.Button btnRefresh_ue;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnRefresh_cn;
		private System.Windows.Forms.Label lblIndexServer_cn;
		private System.Windows.Forms.ComboBox cbxIndexServer_cn;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Panel panCreate;
		private System.Windows.Forms.Panel panExport;
		private System.Windows.Forms.Button btnExportInstead;
		private System.Windows.Forms.Button btnCreateInstead;
		private System.Windows.Forms.Label INVISIBLE_LABEL_AUTODESTROY;
		private System.Windows.Forms.CheckBox chkUseSelection_cn;
		private System.Windows.Forms.CheckBox chkUseSelection_ue;
	}
}
