namespace ProcessViewer
{
	partial class FormMain
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
			if (_IconHelper != null)
				_IconHelper.Dispose();
			if (_Container != null)
				_Container.Dispose();
			if (_Highlighter != null)
				_Highlighter.Dispose();
			if (Finder != null)
				Finder.Dispose();
			_SubClass.Dispose();
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.statusBarMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbarWatcher = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnTopMost = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.trvResult = new System.Windows.Forms.TreeView();
			this.treeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnRefreshList = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.listenMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.killProcessToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.splitterBottom = new System.Windows.Forms.Splitter();
			this.pnlMessages = new System.Windows.Forms.Panel();
			this.txtMessages = new System.Windows.Forms.TextBox();
			this.toolStripWatch = new System.Windows.Forms.ToolStrip();
			this.btnWatcherOn = new System.Windows.Forms.ToolStripButton();
			this.btnWatcherOff = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.btnEditWatchs = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.btnClearMessages = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.btnCopyToClipboard = new System.Windows.Forms.ToolStripButton();
			this.btnShowMessages = new System.Windows.Forms.ToolStripButton();
			this.lblMessages = new System.Windows.Forms.Label();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.killProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.breakpointsManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.highlightSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showDescriptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showVerbsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.clearMessageLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyMessagesToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showMessagesLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.turkishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoOzcansWebSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoProcessViewerPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutProcessViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.standartToolBar = new System.Windows.Forms.ToolStrip();
			this.btnFindWindow = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.btnKill = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.tlbWindow = new System.Windows.Forms.ToolStrip();
			this.btnShowButton = new System.Windows.Forms.ToolStripButton();
			this.btnHideWindow = new System.Windows.Forms.ToolStripButton();
			this.btnEnableWindow = new System.Windows.Forms.ToolStripButton();
			this.btnDisableWindow = new System.Windows.Forms.ToolStripButton();
			this.btnHighlight = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.btnListenMessages = new System.Windows.Forms.ToolStripButton();
			this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.statusBar.SuspendLayout();
			this.splitContainerMain.Panel1.SuspendLayout();
			this.splitContainerMain.Panel2.SuspendLayout();
			this.splitContainerMain.SuspendLayout();
			this.treeContext.SuspendLayout();
			this.pnlMessages.SuspendLayout();
			this.toolStripWatch.SuspendLayout();
			this.mainMenu.SuspendLayout();
			this.standartToolBar.SuspendLayout();
			this.tlbWindow.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.BottomToolStripPanel
			// 
			this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusBar);
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainerMain);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitterBottom);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.pnlMessages);
			resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
			resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
			this.toolStripContainer1.Name = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tlbWindow);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.standartToolBar);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.mainMenu);
			// 
			// statusBar
			// 
			resources.ApplyResources(this.statusBar, "statusBar");
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarMessage,
            this.sbarWatcher,
            this.btnTopMost});
			this.statusBar.Name = "statusBar";
			// 
			// statusBarMessage
			// 
			this.statusBarMessage.Name = "statusBarMessage";
			resources.ApplyResources(this.statusBarMessage, "statusBarMessage");
			this.statusBarMessage.Spring = true;
			// 
			// sbarWatcher
			// 
			resources.ApplyResources(this.sbarWatcher, "sbarWatcher");
			this.sbarWatcher.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.sbarWatcher.DoubleClickEnabled = true;
			this.sbarWatcher.IsLink = true;
			this.sbarWatcher.Name = "sbarWatcher";
			this.sbarWatcher.Click += new System.EventHandler(this.sbarWatcher_Click);
			// 
			// btnTopMost
			// 
			resources.ApplyResources(this.btnTopMost, "btnTopMost");
			this.btnTopMost.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.btnTopMost.IsLink = true;
			this.btnTopMost.Name = "btnTopMost";
			this.btnTopMost.Click += new System.EventHandler(this.btnTopMost_Click);
			// 
			// splitContainerMain
			// 
			resources.ApplyResources(this.splitContainerMain, "splitContainerMain");
			this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainerMain.Name = "splitContainerMain";
			// 
			// splitContainerMain.Panel1
			// 
			this.splitContainerMain.Panel1.Controls.Add(this.trvResult);
			resources.ApplyResources(this.splitContainerMain.Panel1, "splitContainerMain.Panel1");
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.propertyGrid);
			resources.ApplyResources(this.splitContainerMain.Panel2, "splitContainerMain.Panel2");
			// 
			// trvResult
			// 
			this.trvResult.ContextMenuStrip = this.treeContext;
			resources.ApplyResources(this.trvResult, "trvResult");
			this.trvResult.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.trvResult.FullRowSelect = true;
			this.trvResult.HideSelection = false;
			this.trvResult.ItemHeight = 17;
			this.trvResult.MinimumSize = new System.Drawing.Size(150, 100);
			this.trvResult.Name = "trvResult";
			this.trvResult.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvResult_DrawNode);
			this.trvResult.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvResult_BeforeExpand);
			this.trvResult.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvResult_AfterSelect);
			this.trvResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvResult_MouseDown);
			// 
			// treeContext
			// 
			this.treeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshList,
            this.toolStripSeparator8,
            this.highlightToolStripMenuItem,
            this.toolStripSeparator10,
            this.showToolStripMenuItem,
            this.hideToolStripMenuItem,
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem,
            this.toolStripSeparator11,
            this.listenMessagesToolStripMenuItem,
            this.killProcessToolStripMenuItem1});
			this.treeContext.Name = "treeContext";
			resources.ApplyResources(this.treeContext, "treeContext");
			this.treeContext.Opening += new System.ComponentModel.CancelEventHandler(this.treeContext_Opening);
			// 
			// btnRefreshList
			// 
			this.btnRefreshList.Image = global::ProcessViewer.Properties.Resources.refresh_16;
			this.btnRefreshList.Name = "btnRefreshList";
			resources.ApplyResources(this.btnRefreshList, "btnRefreshList");
			this.btnRefreshList.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
			// 
			// highlightToolStripMenuItem
			// 
			this.highlightToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.pictures_16;
			this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
			resources.ApplyResources(this.highlightToolStripMenuItem, "highlightToolStripMenuItem");
			this.highlightToolStripMenuItem.Click += new System.EventHandler(this.highlightSelectedToolStripMenuItem_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.open_in_browser_16x16;
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			resources.ApplyResources(this.showToolStripMenuItem, "showToolStripMenuItem");
			this.showToolStripMenuItem.Click += new System.EventHandler(this.btnShowButton_Click);
			// 
			// hideToolStripMenuItem
			// 
			this.hideToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.tab_16x16;
			this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
			resources.ApplyResources(this.hideToolStripMenuItem, "hideToolStripMenuItem");
			this.hideToolStripMenuItem.Click += new System.EventHandler(this.btnHideWindow_Click);
			// 
			// enableToolStripMenuItem
			// 
			this.enableToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.horizontal_16x16;
			this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
			resources.ApplyResources(this.enableToolStripMenuItem, "enableToolStripMenuItem");
			this.enableToolStripMenuItem.Click += new System.EventHandler(this.btnEnableWindow_Click);
			// 
			// disableToolStripMenuItem
			// 
			this.disableToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.horizontal2_16x16;
			this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
			resources.ApplyResources(this.disableToolStripMenuItem, "disableToolStripMenuItem");
			this.disableToolStripMenuItem.Click += new System.EventHandler(this.btnDisableWindow_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
			// 
			// listenMessagesToolStripMenuItem
			// 
			this.listenMessagesToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.zoom_in_16;
			this.listenMessagesToolStripMenuItem.Name = "listenMessagesToolStripMenuItem";
			resources.ApplyResources(this.listenMessagesToolStripMenuItem, "listenMessagesToolStripMenuItem");
			this.listenMessagesToolStripMenuItem.Click += new System.EventHandler(this.btnListenMessages_Click);
			// 
			// killProcessToolStripMenuItem1
			// 
			this.killProcessToolStripMenuItem1.Image = global::ProcessViewer.Properties.Resources.cancel_16;
			this.killProcessToolStripMenuItem1.Name = "killProcessToolStripMenuItem1";
			resources.ApplyResources(this.killProcessToolStripMenuItem1, "killProcessToolStripMenuItem1");
			this.killProcessToolStripMenuItem1.Click += new System.EventHandler(this.btnKill_Click);
			// 
			// propertyGrid
			// 
			resources.ApplyResources(this.propertyGrid, "propertyGrid");
			this.propertyGrid.MinimumSize = new System.Drawing.Size(200, 200);
			this.propertyGrid.Name = "propertyGrid";
			// 
			// splitterBottom
			// 
			resources.ApplyResources(this.splitterBottom, "splitterBottom");
			this.splitterBottom.Name = "splitterBottom";
			this.splitterBottom.TabStop = false;
			// 
			// pnlMessages
			// 
			this.pnlMessages.Controls.Add(this.txtMessages);
			this.pnlMessages.Controls.Add(this.toolStripWatch);
			this.pnlMessages.Controls.Add(this.lblMessages);
			resources.ApplyResources(this.pnlMessages, "pnlMessages");
			this.pnlMessages.Name = "pnlMessages";
			// 
			// txtMessages
			// 
			resources.ApplyResources(this.txtMessages, "txtMessages");
			this.txtMessages.Name = "txtMessages";
			this.txtMessages.ReadOnly = true;
			// 
			// toolStripWatch
			// 
			this.toolStripWatch.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripWatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnWatcherOn,
            this.btnWatcherOff,
            this.toolStripSeparator5,
            this.btnEditWatchs,
            this.toolStripSeparator7,
            this.btnClearMessages,
            this.toolStripSeparator9,
            this.btnCopyToClipboard,
            this.btnShowMessages});
			resources.ApplyResources(this.toolStripWatch, "toolStripWatch");
			this.toolStripWatch.Name = "toolStripWatch";
			// 
			// btnWatcherOn
			// 
			this.btnWatcherOn.Checked = true;
			this.btnWatcherOn.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnWatcherOn.Image = global::ProcessViewer.Properties.Resources.go_16;
			resources.ApplyResources(this.btnWatcherOn, "btnWatcherOn");
			this.btnWatcherOn.Name = "btnWatcherOn";
			this.btnWatcherOn.Click += new System.EventHandler(this.btnWatcherOn_Click);
			// 
			// btnWatcherOff
			// 
			this.btnWatcherOff.Image = global::ProcessViewer.Properties.Resources.stop_16;
			resources.ApplyResources(this.btnWatcherOff, "btnWatcherOff");
			this.btnWatcherOff.Name = "btnWatcherOff";
			this.btnWatcherOff.Click += new System.EventHandler(this.btnWatcherOff_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			// 
			// btnEditWatchs
			// 
			this.btnEditWatchs.Image = global::ProcessViewer.Properties.Resources.help_16;
			resources.ApplyResources(this.btnEditWatchs, "btnEditWatchs");
			this.btnEditWatchs.Name = "btnEditWatchs";
			this.btnEditWatchs.Click += new System.EventHandler(this.btnEditWatchs_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
			// 
			// btnClearMessages
			// 
			this.btnClearMessages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnClearMessages.Image = global::ProcessViewer.Properties.Resources.calendar_16;
			resources.ApplyResources(this.btnClearMessages, "btnClearMessages");
			this.btnClearMessages.Name = "btnClearMessages";
			this.btnClearMessages.Click += new System.EventHandler(this.btnClearMessages_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
			// 
			// btnCopyToClipboard
			// 
			this.btnCopyToClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCopyToClipboard.Image = global::ProcessViewer.Properties.Resources.copy_16;
			resources.ApplyResources(this.btnCopyToClipboard, "btnCopyToClipboard");
			this.btnCopyToClipboard.Name = "btnCopyToClipboard";
			this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
			// 
			// btnShowMessages
			// 
			this.btnShowMessages.Checked = true;
			this.btnShowMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnShowMessages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnShowMessages.Image = global::ProcessViewer.Properties.Resources.documents_16;
			resources.ApplyResources(this.btnShowMessages, "btnShowMessages");
			this.btnShowMessages.Name = "btnShowMessages";
			this.btnShowMessages.Click += new System.EventHandler(this.btnShowMessages_Click);
			// 
			// lblMessages
			// 
			this.lblMessages.BackColor = System.Drawing.Color.SkyBlue;
			resources.ApplyResources(this.lblMessages, "lblMessages");
			this.lblMessages.Image = global::ProcessViewer.Properties.Resources.Close;
			this.lblMessages.Name = "lblMessages";
			this.lblMessages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMessages_MouseUp);
			// 
			// mainMenu
			// 
			resources.ApplyResources(this.mainMenu, "mainMenu");
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.mainMenu.Name = "mainMenu";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.killProcessToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.refresh_16;
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			resources.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// killProcessToolStripMenuItem
			// 
			resources.ApplyResources(this.killProcessToolStripMenuItem, "killProcessToolStripMenuItem");
			this.killProcessToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.cancel_161;
			this.killProcessToolStripMenuItem.Name = "killProcessToolStripMenuItem";
			this.killProcessToolStripMenuItem.Click += new System.EventHandler(this.btnKill_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breakpointsManagerToolStripMenuItem,
            this.toolStripSeparator4,
            this.highlightSelectedToolStripMenuItem,
            this.propertyGridToolStripMenuItem,
            this.toolStripSeparator13,
            this.clearMessageLogToolStripMenuItem,
            this.copyMessagesToClipboardToolStripMenuItem,
            this.showMessagesLogToolStripMenuItem,
            this.toolStripSeparator3,
            this.languageToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
			// 
			// breakpointsManagerToolStripMenuItem
			// 
			this.breakpointsManagerToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.help_16;
			this.breakpointsManagerToolStripMenuItem.Name = "breakpointsManagerToolStripMenuItem";
			resources.ApplyResources(this.breakpointsManagerToolStripMenuItem, "breakpointsManagerToolStripMenuItem");
			this.breakpointsManagerToolStripMenuItem.Click += new System.EventHandler(this.breakpointsManagerToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// highlightSelectedToolStripMenuItem
			// 
			this.highlightSelectedToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.pictures_16;
			this.highlightSelectedToolStripMenuItem.Name = "highlightSelectedToolStripMenuItem";
			resources.ApplyResources(this.highlightSelectedToolStripMenuItem, "highlightSelectedToolStripMenuItem");
			this.highlightSelectedToolStripMenuItem.Click += new System.EventHandler(this.highlightSelectedToolStripMenuItem_Click);
			// 
			// propertyGridToolStripMenuItem
			// 
			this.propertyGridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDescriptionsToolStripMenuItem,
            this.showVerbsToolStripMenuItem1});
			this.propertyGridToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.PropertyGrid;
			this.propertyGridToolStripMenuItem.Name = "propertyGridToolStripMenuItem";
			resources.ApplyResources(this.propertyGridToolStripMenuItem, "propertyGridToolStripMenuItem");
			this.propertyGridToolStripMenuItem.DropDownOpening += new System.EventHandler(this.propertyGridToolStripMenuItem_DropDownOpening);
			// 
			// showDescriptionsToolStripMenuItem
			// 
			this.showDescriptionsToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.info_16;
			this.showDescriptionsToolStripMenuItem.Name = "showDescriptionsToolStripMenuItem";
			resources.ApplyResources(this.showDescriptionsToolStripMenuItem, "showDescriptionsToolStripMenuItem");
			this.showDescriptionsToolStripMenuItem.Click += new System.EventHandler(this.showDescriptionsToolStripMenuItem_Click);
			// 
			// showVerbsToolStripMenuItem1
			// 
			this.showVerbsToolStripMenuItem1.Image = global::ProcessViewer.Properties.Resources.favorites_add_16;
			this.showVerbsToolStripMenuItem1.Name = "showVerbsToolStripMenuItem1";
			resources.ApplyResources(this.showVerbsToolStripMenuItem1, "showVerbsToolStripMenuItem1");
			this.showVerbsToolStripMenuItem1.Click += new System.EventHandler(this.showVerbsToolStripMenuItem1_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
			// 
			// clearMessageLogToolStripMenuItem
			// 
			this.clearMessageLogToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.calendar_16;
			this.clearMessageLogToolStripMenuItem.Name = "clearMessageLogToolStripMenuItem";
			resources.ApplyResources(this.clearMessageLogToolStripMenuItem, "clearMessageLogToolStripMenuItem");
			this.clearMessageLogToolStripMenuItem.Click += new System.EventHandler(this.btnClearMessages_Click);
			// 
			// copyMessagesToClipboardToolStripMenuItem
			// 
			this.copyMessagesToClipboardToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.copy_16;
			this.copyMessagesToClipboardToolStripMenuItem.Name = "copyMessagesToClipboardToolStripMenuItem";
			resources.ApplyResources(this.copyMessagesToClipboardToolStripMenuItem, "copyMessagesToClipboardToolStripMenuItem");
			this.copyMessagesToClipboardToolStripMenuItem.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
			// 
			// showMessagesLogToolStripMenuItem
			// 
			this.showMessagesLogToolStripMenuItem.Checked = true;
			this.showMessagesLogToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showMessagesLogToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.documents_16;
			this.showMessagesLogToolStripMenuItem.Name = "showMessagesLogToolStripMenuItem";
			resources.ApplyResources(this.showMessagesLogToolStripMenuItem, "showMessagesLogToolStripMenuItem");
			this.showMessagesLogToolStripMenuItem.Click += new System.EventHandler(this.btnShowMessages_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			// 
			// languageToolStripMenuItem
			// 
			this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.turkishToolStripMenuItem});
			this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
			resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
			this.languageToolStripMenuItem.DropDownOpening += new System.EventHandler(this.languageToolStripMenuItem_DropDownOpening);
			// 
			// englishToolStripMenuItem
			// 
			this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
			this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
			// 
			// turkishToolStripMenuItem
			// 
			this.turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
			resources.ApplyResources(this.turkishToolStripMenuItem, "turkishToolStripMenuItem");
			this.turkishToolStripMenuItem.Click += new System.EventHandler(this.turkishToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.applications_16;
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoOzcansWebSiteToolStripMenuItem,
            this.gotoProcessViewerPageToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutProcessViewerToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			// 
			// gotoOzcansWebSiteToolStripMenuItem
			// 
			this.gotoOzcansWebSiteToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.web_16;
			this.gotoOzcansWebSiteToolStripMenuItem.Name = "gotoOzcansWebSiteToolStripMenuItem";
			resources.ApplyResources(this.gotoOzcansWebSiteToolStripMenuItem, "gotoOzcansWebSiteToolStripMenuItem");
			this.gotoOzcansWebSiteToolStripMenuItem.Click += new System.EventHandler(this.gotoOzcansWebSiteToolStripMenuItem_Click);
			// 
			// gotoProcessViewerPageToolStripMenuItem
			// 
			this.gotoProcessViewerPageToolStripMenuItem.Image = global::ProcessViewer.Properties.Resources.web_16;
			this.gotoProcessViewerPageToolStripMenuItem.Name = "gotoProcessViewerPageToolStripMenuItem";
			resources.ApplyResources(this.gotoProcessViewerPageToolStripMenuItem, "gotoProcessViewerPageToolStripMenuItem");
			this.gotoProcessViewerPageToolStripMenuItem.Click += new System.EventHandler(this.gotoProcessViewerPageToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// aboutProcessViewerToolStripMenuItem
			// 
			resources.ApplyResources(this.aboutProcessViewerToolStripMenuItem, "aboutProcessViewerToolStripMenuItem");
			this.aboutProcessViewerToolStripMenuItem.Name = "aboutProcessViewerToolStripMenuItem";
			this.aboutProcessViewerToolStripMenuItem.Click += new System.EventHandler(this.aboutProcessViewerToolStripMenuItem_Click);
			// 
			// standartToolBar
			// 
			resources.ApplyResources(this.standartToolBar, "standartToolBar");
			this.standartToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFindWindow,
            this.toolStripSeparator12,
            this.toolStripButton1,
            this.btnKill,
            this.toolStripButton2});
			this.standartToolBar.Name = "standartToolBar";
			// 
			// btnFindWindow
			// 
			resources.ApplyResources(this.btnFindWindow, "btnFindWindow");
			this.btnFindWindow.Image = global::ProcessViewer.Properties.Resources.navigate_16x16;
			this.btnFindWindow.Name = "btnFindWindow";
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::ProcessViewer.Properties.Resources.refresh_16;
			resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// btnKill
			// 
			this.btnKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.btnKill, "btnKill");
			this.btnKill.Image = global::ProcessViewer.Properties.Resources.cancel_161;
			this.btnKill.Name = "btnKill";
			this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::ProcessViewer.Properties.Resources.applications_16;
			resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// tlbWindow
			// 
			resources.ApplyResources(this.tlbWindow, "tlbWindow");
			this.tlbWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnShowButton,
            this.btnHideWindow,
            this.btnEnableWindow,
            this.btnDisableWindow,
            this.btnHighlight,
            this.toolStripSeparator6,
            this.btnListenMessages});
			this.tlbWindow.Name = "tlbWindow";
			// 
			// btnShowButton
			// 
			resources.ApplyResources(this.btnShowButton, "btnShowButton");
			this.btnShowButton.Image = global::ProcessViewer.Properties.Resources.open_in_browser_16x16;
			this.btnShowButton.Name = "btnShowButton";
			this.btnShowButton.Click += new System.EventHandler(this.btnShowButton_Click);
			// 
			// btnHideWindow
			// 
			resources.ApplyResources(this.btnHideWindow, "btnHideWindow");
			this.btnHideWindow.Image = global::ProcessViewer.Properties.Resources.tab_16x16;
			this.btnHideWindow.Name = "btnHideWindow";
			this.btnHideWindow.Click += new System.EventHandler(this.btnHideWindow_Click);
			// 
			// btnEnableWindow
			// 
			resources.ApplyResources(this.btnEnableWindow, "btnEnableWindow");
			this.btnEnableWindow.Image = global::ProcessViewer.Properties.Resources.horizontal_16x16;
			this.btnEnableWindow.Name = "btnEnableWindow";
			this.btnEnableWindow.Click += new System.EventHandler(this.btnEnableWindow_Click);
			// 
			// btnDisableWindow
			// 
			resources.ApplyResources(this.btnDisableWindow, "btnDisableWindow");
			this.btnDisableWindow.Image = global::ProcessViewer.Properties.Resources.horizontal2_16x16;
			this.btnDisableWindow.Name = "btnDisableWindow";
			this.btnDisableWindow.Click += new System.EventHandler(this.btnDisableWindow_Click);
			// 
			// btnHighlight
			// 
			resources.ApplyResources(this.btnHighlight, "btnHighlight");
			this.btnHighlight.Image = global::ProcessViewer.Properties.Resources.pictures_16;
			this.btnHighlight.Name = "btnHighlight";
			this.btnHighlight.Click += new System.EventHandler(this.highlightSelectedToolStripMenuItem_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			// 
			// btnListenMessages
			// 
			resources.ApplyResources(this.btnListenMessages, "btnListenMessages");
			this.btnListenMessages.Image = global::ProcessViewer.Properties.Resources.zoom_in_16;
			this.btnListenMessages.Name = "btnListenMessages";
			this.btnListenMessages.Click += new System.EventHandler(this.btnListenMessages_Click);
			// 
			// FormMain
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.toolStripContainer1);
			this.Name = "FormMain";
			this.TopMost = true;
			this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.splitContainerMain.Panel1.ResumeLayout(false);
			this.splitContainerMain.Panel2.ResumeLayout(false);
			this.splitContainerMain.ResumeLayout(false);
			this.treeContext.ResumeLayout(false);
			this.pnlMessages.ResumeLayout(false);
			this.pnlMessages.PerformLayout();
			this.toolStripWatch.ResumeLayout(false);
			this.toolStripWatch.PerformLayout();
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.standartToolBar.ResumeLayout(false);
			this.standartToolBar.PerformLayout();
			this.tlbWindow.ResumeLayout(false);
			this.tlbWindow.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.ToolStripStatusLabel statusBarMessage;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStrip standartToolBar;
		private System.Windows.Forms.SplitContainer splitContainerMain;
		private System.Windows.Forms.TreeView trvResult;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gotoOzcansWebSiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem aboutProcessViewerToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem highlightSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton btnKill;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStrip tlbWindow;
		private System.Windows.Forms.ToolStripButton btnShowButton;
		private System.Windows.Forms.ToolStripButton btnHideWindow;
		private System.Windows.Forms.ToolStripButton btnEnableWindow;
		private System.Windows.Forms.ToolStripButton btnDisableWindow;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.Panel pnlMessages;
		private System.Windows.Forms.Label lblMessages;
		private System.Windows.Forms.TextBox txtMessages;
		private System.Windows.Forms.Splitter splitterBottom;
		private System.Windows.Forms.ToolStrip toolStripWatch;
		private System.Windows.Forms.ToolStripButton btnWatcherOn;
		private System.Windows.Forms.ToolStripButton btnWatcherOff;
		private System.Windows.Forms.ToolStripButton btnEditWatchs;
		private System.Windows.Forms.ToolStripStatusLabel sbarWatcher;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton btnListenMessages;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton btnClearMessages;
		private System.Windows.Forms.ToolStripButton btnHighlight;
		private System.Windows.Forms.ToolStripButton btnCopyToClipboard;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem killProcessToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip treeContext;
		private System.Windows.Forms.ToolStripMenuItem highlightToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripMenuItem listenMessagesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem killProcessToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem propertyGridToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showDescriptionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showVerbsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripButton btnShowMessages;
		private System.Windows.Forms.ToolStripButton btnFindWindow;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem clearMessageLogToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyMessagesToClipboardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showMessagesLogToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripStatusLabel btnTopMost;
		private System.Windows.Forms.ToolStripMenuItem breakpointsManagerToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem gotoProcessViewerPageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem turkishToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem btnRefreshList;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;

	}
}

