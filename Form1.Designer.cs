namespace OpenXStreamLoader
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabsControl = new System.Windows.Forms.TabControl();
            this.tpRecord = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.tbFinalFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbId = new System.Windows.Forms.ComboBox();
            this.btAddToFavorites = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbQuality = new System.Windows.Forms.ComboBox();
            this.cbOnlineCheck = new System.Windows.Forms.CheckBox();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOtherSource = new System.Windows.Forms.CheckBox();
            this.cbSameNameAsId = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btStartRecord = new System.Windows.Forms.Button();
            this.lvTasks = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmTasks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTaskUrlInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showInFileExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTaskToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyURLToInputFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewStreamLinkOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvFavorites = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmFavorites = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.nuWaitingTaskInterval = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.nuFavoritesUpdateInterval = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.nuHttpRequestDelay = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btChooseBrowserPath = new System.Windows.Forms.Button();
            this.tbBrowserPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btChooseDefaultRecordsPath = new System.Windows.Forms.Button();
            this.tbDefaultRecordsPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btChooseStreamlinkExe = new System.Windows.Forms.Button();
            this.tbStreamlinkExePath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openStreamlinkExeDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tmrFavoritesStatusCheck = new System.Windows.Forms.Timer(this.components);
            this.tabsControl.SuspendLayout();
            this.tpRecord.SuspendLayout();
            this.panel1.SuspendLayout();
            this.cmTasks.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.cmFavorites.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuWaitingTaskInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuFavoritesUpdateInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuHttpRequestDelay)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabsControl
            // 
            this.tabsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsControl.Controls.Add(this.tpRecord);
            this.tabsControl.Controls.Add(this.tabPage2);
            this.tabsControl.Controls.Add(this.tabPage3);
            this.tabsControl.Controls.Add(this.tabPage4);
            this.tabsControl.Location = new System.Drawing.Point(0, 0);
            this.tabsControl.Name = "tabsControl";
            this.tabsControl.SelectedIndex = 0;
            this.tabsControl.Size = new System.Drawing.Size(753, 425);
            this.tabsControl.TabIndex = 0;
            // 
            // tpRecord
            // 
            this.tpRecord.Controls.Add(this.panel1);
            this.tpRecord.Controls.Add(this.lvTasks);
            this.tpRecord.Location = new System.Drawing.Point(4, 22);
            this.tpRecord.Name = "tpRecord";
            this.tpRecord.Padding = new System.Windows.Forms.Padding(3);
            this.tpRecord.Size = new System.Drawing.Size(745, 399);
            this.tpRecord.TabIndex = 0;
            this.tpRecord.Text = "Record";
            this.tpRecord.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.tbFinalFileName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbId);
            this.panel1.Controls.Add(this.btAddToFavorites);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbQuality);
            this.panel1.Controls.Add(this.cbOnlineCheck);
            this.panel1.Controls.Add(this.tbFileName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbOtherSource);
            this.panel1.Controls.Add(this.cbSameNameAsId);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btStartRecord);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 102);
            this.panel1.TabIndex = 14;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(5, 81);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(120, 13);
            this.label19.TabIndex = 16;
            this.label19.Text = "Final filename with path:";
            // 
            // tbFinalFileName
            // 
            this.tbFinalFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFinalFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFinalFileName.Location = new System.Drawing.Point(131, 82);
            this.tbFinalFileName.Name = "tbFinalFileName";
            this.tbFinalFileName.ReadOnly = true;
            this.tbFinalFileName.Size = new System.Drawing.Size(603, 13);
            this.tbFinalFileName.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter URL/ID:";
            // 
            // cbId
            // 
            this.cbId.FormattingEnabled = true;
            this.cbId.Location = new System.Drawing.Point(6, 16);
            this.cbId.Name = "cbId";
            this.cbId.Size = new System.Drawing.Size(375, 21);
            this.cbId.TabIndex = 3;
            this.cbId.SelectedIndexChanged += new System.EventHandler(this.cbId_SelectedIndexChanged);
            this.cbId.TextChanged += new System.EventHandler(this.cbId_TextChanged);
            // 
            // btAddToFavorites
            // 
            this.btAddToFavorites.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAddToFavorites.Location = new System.Drawing.Point(469, 14);
            this.btAddToFavorites.Name = "btAddToFavorites";
            this.btAddToFavorites.Size = new System.Drawing.Size(23, 23);
            this.btAddToFavorites.TabIndex = 11;
            this.btAddToFavorites.Text = "♥";
            this.toolTip1.SetToolTip(this.btAddToFavorites, "Add to Favorites");
            this.btAddToFavorites.UseVisualStyleBackColor = true;
            this.btAddToFavorites.Click += new System.EventHandler(this.btAddToFavorites_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(591, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Quality:";
            // 
            // cbQuality
            // 
            this.cbQuality.DisplayMember = "2";
            this.cbQuality.FormattingEnabled = true;
            this.cbQuality.Items.AddRange(new object[] {
            "best",
            "2160p",
            "1440p",
            "1080p",
            "720p",
            "480p",
            "360p",
            "240p",
            "worst"});
            this.cbQuality.Location = new System.Drawing.Point(594, 16);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(121, 21);
            this.cbQuality.TabIndex = 6;
            this.cbQuality.Text = "1080p";
            // 
            // cbOnlineCheck
            // 
            this.cbOnlineCheck.AutoSize = true;
            this.cbOnlineCheck.Checked = true;
            this.cbOnlineCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlineCheck.Location = new System.Drawing.Point(469, 58);
            this.cbOnlineCheck.Name = "cbOnlineCheck";
            this.cbOnlineCheck.Size = new System.Drawing.Size(94, 17);
            this.cbOnlineCheck.TabIndex = 12;
            this.cbOnlineCheck.Text = "Wait for online";
            this.cbOnlineCheck.UseVisualStyleBackColor = true;
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(6, 56);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(375, 20);
            this.tbFileName.TabIndex = 8;
            this.tbFileName.TextChanged += new System.EventHandler(this.tbFileName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(498, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Choose source:";
            // 
            // cbOtherSource
            // 
            this.cbOtherSource.AutoSize = true;
            this.cbOtherSource.Checked = true;
            this.cbOtherSource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOtherSource.Enabled = false;
            this.cbOtherSource.Location = new System.Drawing.Point(501, 18);
            this.cbOtherSource.Name = "cbOtherSource";
            this.cbOtherSource.Size = new System.Drawing.Size(87, 17);
            this.cbOtherSource.TabIndex = 2;
            this.cbOtherSource.Text = "Other source";
            this.cbOtherSource.UseVisualStyleBackColor = true;
            // 
            // cbSameNameAsId
            // 
            this.cbSameNameAsId.AutoSize = true;
            this.cbSameNameAsId.Checked = true;
            this.cbSameNameAsId.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSameNameAsId.Location = new System.Drawing.Point(387, 58);
            this.cbSameNameAsId.Name = "cbSameNameAsId";
            this.cbSameNameAsId.Size = new System.Drawing.Size(81, 17);
            this.cbSameNameAsId.TabIndex = 9;
            this.cbSameNameAsId.Text = "Same as ID";
            this.cbSameNameAsId.UseVisualStyleBackColor = true;
            this.cbSameNameAsId.CheckedChanged += new System.EventHandler(this.cbSameNameAsId_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(286, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Filename/Path (absolute or relative to default from settings):";
            // 
            // btStartRecord
            // 
            this.btStartRecord.Location = new System.Drawing.Point(387, 14);
            this.btStartRecord.Name = "btStartRecord";
            this.btStartRecord.Size = new System.Drawing.Size(75, 23);
            this.btStartRecord.TabIndex = 4;
            this.btStartRecord.Text = "Start";
            this.btStartRecord.UseVisualStyleBackColor = true;
            this.btStartRecord.Click += new System.EventHandler(this.btStartRecord_Click);
            // 
            // lvTasks
            // 
            this.lvTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader9,
            this.columnHeader2,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader5,
            this.columnHeader6});
            this.lvTasks.ContextMenuStrip = this.cmTasks;
            this.lvTasks.FullRowSelect = true;
            this.lvTasks.GridLines = true;
            this.lvTasks.HideSelection = false;
            this.lvTasks.Location = new System.Drawing.Point(3, 111);
            this.lvTasks.MultiSelect = false;
            this.lvTasks.Name = "lvTasks";
            this.lvTasks.Size = new System.Drawing.Size(739, 285);
            this.lvTasks.TabIndex = 10;
            this.lvTasks.UseCompatibleStateImageBehavior = false;
            this.lvTasks.View = System.Windows.Forms.View.Details;
            this.lvTasks.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTasks_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "URL";
            this.columnHeader1.Width = 237;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "WfO";
            this.columnHeader9.Width = 35;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 93;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Quality";
            this.columnHeader7.Width = 65;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Duration";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "File size";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "File";
            this.columnHeader6.Width = 320;
            // 
            // cmTasks
            // 
            this.cmTasks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.openTaskUrlInBrowserToolStripMenuItem,
            this.showInFileExplorerToolStripMenuItem,
            this.addTaskToFavoritesToolStripMenuItem,
            this.copyURLToInputFieldToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.viewStreamLinkOutputToolStripMenuItem});
            this.cmTasks.Name = "cmTasks";
            this.cmTasks.Size = new System.Drawing.Size(202, 158);
            this.cmTasks.Opening += new System.ComponentModel.CancelEventHandler(this.cmTasks_Opening);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openFileToolStripMenuItem.Text = "Open file";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openTaskUrlInBrowserToolStripMenuItem
            // 
            this.openTaskUrlInBrowserToolStripMenuItem.Name = "openTaskUrlInBrowserToolStripMenuItem";
            this.openTaskUrlInBrowserToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openTaskUrlInBrowserToolStripMenuItem.Text = "Open URL in browser";
            this.openTaskUrlInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openTaskUrlInBrowserToolStripMenuItem_Click);
            // 
            // showInFileExplorerToolStripMenuItem
            // 
            this.showInFileExplorerToolStripMenuItem.Name = "showInFileExplorerToolStripMenuItem";
            this.showInFileExplorerToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.showInFileExplorerToolStripMenuItem.Text = "Navigate to file in Explorer";
            this.showInFileExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInFileExplorerToolStripMenuItem_Click);
            // 
            // addTaskToFavoritesToolStripMenuItem
            // 
            this.addTaskToFavoritesToolStripMenuItem.Name = "addTaskToFavoritesToolStripMenuItem";
            this.addTaskToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.addTaskToFavoritesToolStripMenuItem.Text = "Add to favorites";
            this.addTaskToFavoritesToolStripMenuItem.Click += new System.EventHandler(this.addTaskToFavoritesToolStripMenuItem_Click);
            // 
            // copyURLToInputFieldToolStripMenuItem
            // 
            this.copyURLToInputFieldToolStripMenuItem.Name = "copyURLToInputFieldToolStripMenuItem";
            this.copyURLToInputFieldToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.copyURLToInputFieldToolStripMenuItem.Text = "Copy URL to input field";
            this.copyURLToInputFieldToolStripMenuItem.Click += new System.EventHandler(this.copyURLToInputFieldToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.deleteToolStripMenuItem.Text = "Delete task";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // viewStreamLinkOutputToolStripMenuItem
            // 
            this.viewStreamLinkOutputToolStripMenuItem.Name = "viewStreamLinkOutputToolStripMenuItem";
            this.viewStreamLinkOutputToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.viewStreamLinkOutputToolStripMenuItem.Text = "View StreamLink output";
            this.viewStreamLinkOutputToolStripMenuItem.Click += new System.EventHandler(this.viewStreamLinkOutputToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lvFavorites);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(745, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Favorites";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // lvFavorites
            // 
            this.lvFavorites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvFavorites.ContextMenuStrip = this.cmFavorites;
            this.lvFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFavorites.FullRowSelect = true;
            this.lvFavorites.GridLines = true;
            this.lvFavorites.HideSelection = false;
            this.lvFavorites.Location = new System.Drawing.Point(3, 3);
            this.lvFavorites.MultiSelect = false;
            this.lvFavorites.Name = "lvFavorites";
            this.lvFavorites.Size = new System.Drawing.Size(739, 393);
            this.lvFavorites.TabIndex = 11;
            this.lvFavorites.UseCompatibleStateImageBehavior = false;
            this.lvFavorites.View = System.Windows.Forms.View.Details;
            this.lvFavorites.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFavorites_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "URL";
            this.columnHeader3.Width = 400;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Online status";
            this.columnHeader4.Width = 100;
            // 
            // cmFavorites
            // 
            this.cmFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInBrowserToolStripMenuItem,
            this.startRecordToolStripMenuItem,
            this.deleteFavToolStripMenuItem,
            this.updateThisToolStripMenuItem,
            this.updateNowToolStripMenuItem});
            this.cmFavorites.Name = "cmFavorites";
            this.cmFavorites.Size = new System.Drawing.Size(191, 114);
            this.cmFavorites.Opening += new System.ComponentModel.CancelEventHandler(this.cmFavorites_Opening);
            // 
            // openInBrowserToolStripMenuItem
            // 
            this.openInBrowserToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
            this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openInBrowserToolStripMenuItem.Text = "Open URL in browser";
            this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
            // 
            // startRecordToolStripMenuItem
            // 
            this.startRecordToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.startRecordToolStripMenuItem.Name = "startRecordToolStripMenuItem";
            this.startRecordToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.startRecordToolStripMenuItem.Text = "Start record";
            this.startRecordToolStripMenuItem.Click += new System.EventHandler(this.startRecordToolStripMenuItem_Click);
            // 
            // deleteFavToolStripMenuItem
            // 
            this.deleteFavToolStripMenuItem.Name = "deleteFavToolStripMenuItem";
            this.deleteFavToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.deleteFavToolStripMenuItem.Text = "Delete";
            this.deleteFavToolStripMenuItem.Click += new System.EventHandler(this.deleteFavToolStripMenuItem_Click);
            // 
            // updateThisToolStripMenuItem
            // 
            this.updateThisToolStripMenuItem.Name = "updateThisToolStripMenuItem";
            this.updateThisToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.updateThisToolStripMenuItem.Text = "Update this";
            this.updateThisToolStripMenuItem.Click += new System.EventHandler(this.updateThisToolStripMenuItem_Click);
            // 
            // updateNowToolStripMenuItem
            // 
            this.updateNowToolStripMenuItem.Name = "updateNowToolStripMenuItem";
            this.updateNowToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.updateNowToolStripMenuItem.Text = "Update all";
            this.updateNowToolStripMenuItem.Click += new System.EventHandler(this.updateNowToolStripMenuItem_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label21);
            this.tabPage3.Controls.Add(this.nuWaitingTaskInterval);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.nuFavoritesUpdateInterval);
            this.tabPage3.Controls.Add(this.label17);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.nuHttpRequestDelay);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.btChooseBrowserPath);
            this.tabPage3.Controls.Add(this.tbBrowserPath);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.btChooseDefaultRecordsPath);
            this.tabPage3.Controls.Add(this.tbDefaultRecordsPath);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.btChooseStreamlinkExe);
            this.tabPage3.Controls.Add(this.tbStreamlinkExePath);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(745, 399);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(137, 356);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(42, 13);
            this.label21.TabIndex = 23;
            this.label21.Text = "second";
            // 
            // nuWaitingTaskInterval
            // 
            this.nuWaitingTaskInterval.Location = new System.Drawing.Point(11, 349);
            this.nuWaitingTaskInterval.Maximum = new decimal(new int[] {
            360000,
            0,
            0,
            0});
            this.nuWaitingTaskInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuWaitingTaskInterval.Name = "nuWaitingTaskInterval";
            this.nuWaitingTaskInterval.Size = new System.Drawing.Size(120, 20);
            this.nuWaitingTaskInterval.TabIndex = 22;
            this.nuWaitingTaskInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nuWaitingTaskInterval.ValueChanged += new System.EventHandler(this.nuWaitingTaskInterval_ValueChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 332);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(447, 13);
            this.label20.TabIndex = 20;
            this.label20.Text = "Interval between online status checks for waiting task. Takes effect on new tasks" +
    " afterwards.";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(137, 306);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(42, 13);
            this.label18.TabIndex = 19;
            this.label18.Text = "second";
            // 
            // nuFavoritesUpdateInterval
            // 
            this.nuFavoritesUpdateInterval.Location = new System.Drawing.Point(11, 299);
            this.nuFavoritesUpdateInterval.Maximum = new decimal(new int[] {
            360000,
            0,
            0,
            0});
            this.nuFavoritesUpdateInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuFavoritesUpdateInterval.Name = "nuFavoritesUpdateInterval";
            this.nuFavoritesUpdateInterval.Size = new System.Drawing.Size(120, 20);
            this.nuFavoritesUpdateInterval.TabIndex = 18;
            this.nuFavoritesUpdateInterval.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nuFavoritesUpdateInterval.ValueChanged += new System.EventHandler(this.nuFavoritesUpdateInterval_ValueChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 283);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(336, 13);
            this.label17.TabIndex = 17;
            this.label17.Text = "You may want to keep this non zero because of possibility of 429 error";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 268);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(343, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = "Interval between all favorites are requested to update their online status";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(137, 241);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "millisecond";
            // 
            // nuHttpRequestDelay
            // 
            this.nuHttpRequestDelay.Location = new System.Drawing.Point(11, 234);
            this.nuHttpRequestDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.nuHttpRequestDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuHttpRequestDelay.Name = "nuHttpRequestDelay";
            this.nuHttpRequestDelay.Size = new System.Drawing.Size(120, 20);
            this.nuHttpRequestDelay.TabIndex = 14;
            this.nuHttpRequestDelay.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 213);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(473, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "If it is too small you may finally get \"The remote server returned an error: (429" +
    ") Too Many Requests\"";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 196);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(494, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Interval between each individual request to server (during online status check). " +
    "Takes effect on startup.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 148);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(229, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Leave this empty to use default system browser";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(377, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "(e.g. C:\\Homework\\OpenXStreamLoader\\Streamlink_Portable\\Streamlink.exe)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(193, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "e.g. \"C:\\bla bla\\chrome.exe\" -incognito";
            // 
            // btChooseBrowserPath
            // 
            this.btChooseBrowserPath.Location = new System.Drawing.Point(518, 162);
            this.btChooseBrowserPath.Name = "btChooseBrowserPath";
            this.btChooseBrowserPath.Size = new System.Drawing.Size(75, 24);
            this.btChooseBrowserPath.TabIndex = 8;
            this.btChooseBrowserPath.Text = "Select...";
            this.btChooseBrowserPath.UseVisualStyleBackColor = true;
            this.btChooseBrowserPath.Click += new System.EventHandler(this.btChooseBrowserPath_Click);
            // 
            // tbBrowserPath
            // 
            this.tbBrowserPath.Location = new System.Drawing.Point(11, 164);
            this.tbBrowserPath.Name = "tbBrowserPath";
            this.tbBrowserPath.Size = new System.Drawing.Size(501, 20);
            this.tbBrowserPath.TabIndex = 7;
            this.tbBrowserPath.TextChanged += new System.EventHandler(this.tbBrowserPath_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(478, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Web browser command line with arguments (don\'t forget quotes in case there are sp" +
    "aces in the path";
            // 
            // btChooseDefaultRecordsPath
            // 
            this.btChooseDefaultRecordsPath.Location = new System.Drawing.Point(518, 81);
            this.btChooseDefaultRecordsPath.Name = "btChooseDefaultRecordsPath";
            this.btChooseDefaultRecordsPath.Size = new System.Drawing.Size(75, 24);
            this.btChooseDefaultRecordsPath.TabIndex = 5;
            this.btChooseDefaultRecordsPath.Text = "Select...";
            this.btChooseDefaultRecordsPath.UseVisualStyleBackColor = true;
            this.btChooseDefaultRecordsPath.Click += new System.EventHandler(this.btChooseDefaultRecordsPath_Click);
            // 
            // tbDefaultRecordsPath
            // 
            this.tbDefaultRecordsPath.Location = new System.Drawing.Point(11, 83);
            this.tbDefaultRecordsPath.Name = "tbDefaultRecordsPath";
            this.tbDefaultRecordsPath.Size = new System.Drawing.Size(501, 20);
            this.tbDefaultRecordsPath.TabIndex = 4;
            this.tbDefaultRecordsPath.TextChanged += new System.EventHandler(this.tbDefaultRecordsPath_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(364, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Default path to records (can be overriden with absolute path at record start):";
            // 
            // btChooseStreamlinkExe
            // 
            this.btChooseStreamlinkExe.Location = new System.Drawing.Point(518, 33);
            this.btChooseStreamlinkExe.Name = "btChooseStreamlinkExe";
            this.btChooseStreamlinkExe.Size = new System.Drawing.Size(75, 23);
            this.btChooseStreamlinkExe.TabIndex = 2;
            this.btChooseStreamlinkExe.Text = "Select...";
            this.btChooseStreamlinkExe.UseVisualStyleBackColor = true;
            this.btChooseStreamlinkExe.Click += new System.EventHandler(this.btChooseStreamlinkExe_Click);
            // 
            // tbStreamlinkExePath
            // 
            this.tbStreamlinkExePath.Location = new System.Drawing.Point(11, 35);
            this.tbStreamlinkExePath.Name = "tbStreamlinkExePath";
            this.tbStreamlinkExePath.Size = new System.Drawing.Size(501, 20);
            this.tbStreamlinkExePath.TabIndex = 1;
            this.tbStreamlinkExePath.TextChanged += new System.EventHandler(this.tbStreamlinkExePath_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Streamlink.exe path";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.linkLabel1);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.pictureBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(745, 399);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "About";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(148, 64);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(319, 17);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://github.com/voidtemp/OpenXStreamLoader";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(145, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "ver 0.1, 03 Sep 2020";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(142, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(283, 32);
            this.label11.TabIndex = 1;
            this.label11.Text = "OpenXStreamLoader";
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // openStreamlinkExeDialog
            // 
            this.openStreamlinkExeDialog.FileName = "Streamlink.exe";
            this.openStreamlinkExeDialog.Filter = "Streamlink|Streamlink.exe";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(753, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tmrFavoritesStatusCheck
            // 
            this.tmrFavoritesStatusCheck.Interval = 300000;
            this.tmrFavoritesStatusCheck.Tick += new System.EventHandler(this.tmrFavoritesStatusCheck_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabsControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenXStreamLoader v0.1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabsControl.ResumeLayout(false);
            this.tpRecord.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.cmTasks.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.cmFavorites.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuWaitingTaskInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuFavoritesUpdateInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuHttpRequestDelay)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabsControl;
        private System.Windows.Forms.TabPage tpRecord;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox cbOtherSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbId;
        private System.Windows.Forms.CheckBox cbSameNameAsId;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbQuality;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btStartRecord;
        private System.Windows.Forms.ListView lvTasks;
        private System.Windows.Forms.Button btAddToFavorites;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btChooseStreamlinkExe;
        private System.Windows.Forms.TextBox tbStreamlinkExePath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openStreamlinkExeDialog;
        private System.Windows.Forms.Button btChooseDefaultRecordsPath;
        private System.Windows.Forms.TextBox tbDefaultRecordsPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbOnlineCheck;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView lvFavorites;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip cmFavorites;
        private System.Windows.Forms.ToolStripMenuItem updateNowToolStripMenuItem;
        private System.Windows.Forms.TextBox tbFinalFileName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btChooseBrowserPath;
        private System.Windows.Forms.TextBox tbBrowserPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startRecordToolStripMenuItem;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip cmTasks;
        private System.Windows.Forms.ToolStripMenuItem viewStreamLinkOutputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Timer tmrFavoritesStatusCheck;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nuHttpRequestDelay;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown nuWaitingTaskInterval;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown nuFavoritesUpdateInterval;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ToolStripMenuItem openTaskUrlInBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showInFileExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateThisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTaskToFavoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyURLToInputFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFavToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Label label19;
    }
}

