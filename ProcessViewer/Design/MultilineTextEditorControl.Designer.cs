/*
*	File: MultilineTextEditorControl.Designer.cs
*	Written by Ozcan DEGIRMENCI - 2008
*	http://www.ozcandegirmenci.com
*/

namespace ProcessViewer.Design
{
	partial class MultilineTextEditorControl
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
			if (disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultilineTextEditorControl));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnRefresh = new System.Windows.Forms.ToolStripButton();
			this.btnCancel = new System.Windows.Forms.ToolStripButton();
			this.btnOkey = new System.Windows.Forms.ToolStripButton();
			this.txtText = new System.Windows.Forms.TextBox();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.btnCancel,
            this.btnOkey});
			this.toolStrip1.Name = "toolStrip1";
			// 
			// btnRefresh
			// 
			this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRefresh.Image = global::ProcessViewer.Properties.Resources.refresh_16;
			resources.ApplyResources(this.btnRefresh, "btnRefresh");
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCancel.Image = global::ProcessViewer.Properties.Resources.cancel_16;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOkey
			// 
			this.btnOkey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOkey.Image = global::ProcessViewer.Properties.Resources.confirm_16;
			resources.ApplyResources(this.btnOkey, "btnOkey");
			this.btnOkey.Name = "btnOkey";
			this.btnOkey.Click += new System.EventHandler(this.btnOkey_Click);
			// 
			// txtText
			// 
			this.txtText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.txtText, "txtText");
			this.txtText.Name = "txtText";
			// 
			// MultilineTextEditorControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtText);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MultilineTextEditorControl";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnRefresh;
		private System.Windows.Forms.ToolStripButton btnCancel;
		private System.Windows.Forms.ToolStripButton btnOkey;
		private System.Windows.Forms.TextBox txtText;
	}
}
