namespace ProcessViewer
{
	partial class FormBreakpoints
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
			if (disposing)
			{
				boltFont.Dispose();
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBreakpoints));
            this.lvwMessages = new System.Windows.Forms.ListBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnMouseMessages = new System.Windows.Forms.Button();
            this.btnKeyboardMessages = new System.Windows.Forms.Button();
            this.btnAllMessages = new System.Windows.Forms.Button();
            this.btnNonClientMessages = new System.Windows.Forms.Button();
            this.btnAddCustomMessages = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.chkOnlyWactheds = new System.Windows.Forms.CheckBox();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvwMessages
            // 
            this.lvwMessages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lvwMessages.FormattingEnabled = true;
            resources.ApplyResources(this.lvwMessages, "lvwMessages");
            this.lvwMessages.Name = "lvwMessages";
            this.lvwMessages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lvwMessages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lvwMessages_DrawItem);
            this.lvwMessages.SelectedValueChanged += new System.EventHandler(this.lvwMessages_SelectedValueChanged);
            // 
            // propertyGrid
            // 
            resources.ApplyResources(this.propertyGrid, "propertyGrid");
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDeleteAll
            // 
            resources.ApplyResources(this.btnDeleteAll, "btnDeleteAll");
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnMouseMessages
            // 
            resources.ApplyResources(this.btnMouseMessages, "btnMouseMessages");
            this.btnMouseMessages.Name = "btnMouseMessages";
            this.btnMouseMessages.UseVisualStyleBackColor = true;
            this.btnMouseMessages.Click += new System.EventHandler(this.btnMouseMessages_Click);
            // 
            // btnKeyboardMessages
            // 
            resources.ApplyResources(this.btnKeyboardMessages, "btnKeyboardMessages");
            this.btnKeyboardMessages.Name = "btnKeyboardMessages";
            this.btnKeyboardMessages.UseVisualStyleBackColor = true;
            this.btnKeyboardMessages.Click += new System.EventHandler(this.btnKeyboardMessages_Click);
            // 
            // btnAllMessages
            // 
            resources.ApplyResources(this.btnAllMessages, "btnAllMessages");
            this.btnAllMessages.Name = "btnAllMessages";
            this.btnAllMessages.UseVisualStyleBackColor = true;
            this.btnAllMessages.Click += new System.EventHandler(this.btnAllMessages_Click);
            // 
            // btnNonClientMessages
            // 
            resources.ApplyResources(this.btnNonClientMessages, "btnNonClientMessages");
            this.btnNonClientMessages.Name = "btnNonClientMessages";
            this.btnNonClientMessages.UseVisualStyleBackColor = true;
            this.btnNonClientMessages.Click += new System.EventHandler(this.btnNonClientMessages_Click);
            // 
            // btnAddCustomMessages
            // 
            resources.ApplyResources(this.btnAddCustomMessages, "btnAddCustomMessages");
            this.btnAddCustomMessages.Name = "btnAddCustomMessages";
            this.btnAddCustomMessages.UseVisualStyleBackColor = true;
            this.btnAddCustomMessages.Click += new System.EventHandler(this.btnAddCustomMessages_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ProcessViewer.Properties.Resources.save_16;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::ProcessViewer.Properties.Resources.import_16;
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // chkOnlyWactheds
            // 
            resources.ApplyResources(this.chkOnlyWactheds, "chkOnlyWactheds");
            this.chkOnlyWactheds.Name = "chkOnlyWactheds";
            this.chkOnlyWactheds.UseVisualStyleBackColor = true;
            this.chkOnlyWactheds.CheckedChanged += new System.EventHandler(this.chkOnlyWactheds_CheckedChanged);
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            // 
            // lblFilter
            // 
            resources.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            // 
            // FormBreakpoints
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.chkOnlyWactheds);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddCustomMessages);
            this.Controls.Add(this.btnAllMessages);
            this.Controls.Add(this.btnNonClientMessages);
            this.Controls.Add(this.btnKeyboardMessages);
            this.Controls.Add(this.btnMouseMessages);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.lvwMessages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBreakpoints";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lvwMessages;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnDeleteAll;
		private System.Windows.Forms.Button btnMouseMessages;
		private System.Windows.Forms.Button btnKeyboardMessages;
		private System.Windows.Forms.Button btnAllMessages;
		private System.Windows.Forms.Button btnNonClientMessages;
		private System.Windows.Forms.Button btnAddCustomMessages;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.CheckBox chkOnlyWactheds;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label lblFilter;
	}
}