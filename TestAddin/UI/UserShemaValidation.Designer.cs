namespace TestAddin.UI
{
	partial class UserSchemaValidation
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.propgrid = new System.Windows.Forms.PropertyGrid();
			this.lblInformation = new System.Windows.Forms.Label();
			this.btnContinue = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// propgrid
			// 
			this.propgrid.HelpVisible = false;
			this.propgrid.Location = new System.Drawing.Point(12, 47);
			this.propgrid.Name = "propgrid";
			this.propgrid.Size = new System.Drawing.Size(410, 362);
			this.propgrid.TabIndex = 0;
			this.propgrid.ToolbarVisible = false;
			// 
			// lblInformation
			// 
			this.lblInformation.AutoSize = true;
			this.lblInformation.Location = new System.Drawing.Point(12, 9);
			this.lblInformation.Name = "lblInformation";
			this.lblInformation.Size = new System.Drawing.Size(69, 13);
			this.lblInformation.TabIndex = 1;
			this.lblInformation.Text = "lblInformation";
			// 
			// btnContinue
			// 
			this.btnContinue.Location = new System.Drawing.Point(347, 415);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 23);
			this.btnContinue.TabIndex = 2;
			this.btnContinue.Text = "btnContinue";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.Continue);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(266, 415);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.Cancel);
			// 
			// UserShemaValidation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 450);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.lblInformation);
			this.Controls.Add(this.propgrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UserShemaValidation";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FrmUserShemaValidation";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PropertyGrid propgrid;
		private System.Windows.Forms.Label lblInformation;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Button btnCancel;
	}
}