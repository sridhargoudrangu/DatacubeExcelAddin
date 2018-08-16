namespace TestAddin.UI
{
	partial class Loading
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblProcessing = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 26);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(648, 23);
			this.progressBar1.TabIndex = 0;
			this.progressBar1.UseWaitCursor = true;
			// 
			// lblProcessing
			// 
			this.lblProcessing.AutoSize = true;
			this.lblProcessing.Location = new System.Drawing.Point(12, 9);
			this.lblProcessing.Name = "lblProcessing";
			this.lblProcessing.Size = new System.Drawing.Size(69, 13);
			this.lblProcessing.TabIndex = 1;
			this.lblProcessing.Text = "lblProcessing";
			this.lblProcessing.UseWaitCursor = true;
			// 
			// Loading
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(672, 61);
			this.ControlBox = false;
			this.Controls.Add(this.lblProcessing);
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Loading";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmLoading";
			this.UseWaitCursor = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lblProcessing;
	}
}