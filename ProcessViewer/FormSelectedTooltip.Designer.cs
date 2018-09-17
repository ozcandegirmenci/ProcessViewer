namespace ProcessViewer
{
	partial class FormSelectedTooltip
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectedTooltip));
			this.label1 = new System.Windows.Forms.Label();
			this.lblHandle = new System.Windows.Forms.Label();
			this.lblParent = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblText = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblClass = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lvwParentList = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblThreadProcess = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// lblHandle
			// 
			resources.ApplyResources(this.lblHandle, "lblHandle");
			this.lblHandle.Name = "lblHandle";
			// 
			// lblParent
			// 
			resources.ApplyResources(this.lblParent, "lblParent");
			this.lblParent.Name = "lblParent";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// lblText
			// 
			this.lblText.AutoEllipsis = true;
			resources.ApplyResources(this.lblText, "lblText");
			this.lblText.Name = "lblText";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// lblClass
			// 
			this.lblClass.AutoEllipsis = true;
			resources.ApplyResources(this.lblClass, "lblClass");
			this.lblClass.Name = "lblClass";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// lvwParentList
			// 
			this.lvwParentList.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.lvwParentList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvwParentList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			resources.ApplyResources(this.lvwParentList, "lvwParentList");
			this.lvwParentList.Name = "lvwParentList";
			this.lvwParentList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lvwParentList_DrawItem);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// lblThreadProcess
			// 
			this.lblThreadProcess.AutoEllipsis = true;
			resources.ApplyResources(this.lblThreadProcess, "lblThreadProcess");
			this.lblThreadProcess.Name = "lblThreadProcess";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// FormSelectedTooltip
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ControlBox = false;
			this.Controls.Add(this.lblThreadProcess);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lvwParentList);
			this.Controls.Add(this.lblClass);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblParent);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblHandle);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FormSelectedTooltip";
			this.Opacity = 0.7;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblHandle;
		private System.Windows.Forms.Label lblParent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblClass;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListBox lvwParentList;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblThreadProcess;
		private System.Windows.Forms.Label label6;
	}
}