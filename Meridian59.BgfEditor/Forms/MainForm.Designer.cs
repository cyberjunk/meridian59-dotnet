namespace Meridian59.BgfEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetShrink = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExportAllBGFToXML = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDecompressAllBGF = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConvertAllToV10 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConvertAllToV9 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuComparePalettes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabFrames = new System.Windows.Forms.TabPage();
            this.splitFramesMain = new System.Windows.Forms.SplitContainer();
            this.splitFramesLeftInner = new System.Windows.Forms.SplitContainer();
            this.groupFrameActions = new System.Windows.Forms.GroupBox();
            this.btnFrameAdd = new System.Windows.Forms.Button();
            this.btnFrameRemove = new System.Windows.Forms.Button();
            this.btnFrameUp = new System.Windows.Forms.Button();
            this.btnFrameDown = new System.Windows.Forms.Button();
            this.dgFrames = new System.Windows.Forms.DataGridView();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitFramesSub1 = new System.Windows.Forms.SplitContainer();
            this.groupFrameImage = new System.Windows.Forms.GroupBox();
            this.picFrameImage = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupHotspotActions = new System.Windows.Forms.GroupBox();
            this.btnHotspotDown = new System.Windows.Forms.Button();
            this.btnHotspotUp = new System.Windows.Forms.Button();
            this.btnHotspotAdd = new System.Windows.Forms.Button();
            this.btnHotspotRemove = new System.Windows.Forms.Button();
            this.dgHotspots = new System.Windows.Forms.DataGridView();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabFrameSets = new System.Windows.Forms.TabPage();
            this.splitFrameSetsMain = new System.Windows.Forms.SplitContainer();
            this.splitFrameSetsLeftInner = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFrameSetAdd = new System.Windows.Forms.Button();
            this.btnFrameSetRemove = new System.Windows.Forms.Button();
            this.btnFrameSetUp = new System.Windows.Forms.Button();
            this.btnFrameSetDown = new System.Windows.Forms.Button();
            this.listFrameSets = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flowLayoutFrameSet = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnFrameIndexAdd = new System.Windows.Forms.Button();
            this.btnFrameIndexRemove = new System.Windows.Forms.Button();
            this.btnFrameIndexUp = new System.Windows.Forms.Button();
            this.btnFrameIndexDown = new System.Windows.Forms.Button();
            this.listFrameNums = new System.Windows.Forms.ListBox();
            this.tabAnimation = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbFinal = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackAngle = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbHigh = new System.Windows.Forms.ComboBox();
            this.cbLow = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.picAnimation = new System.Windows.Forms.PictureBox();
            this.fdAddFrame = new System.Windows.Forms.OpenFileDialog();
            this.fdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.fdSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.cbPalette = new Meridian59.BgfEditor.Controls.ComboBoxPalette();
            this.menuStrip.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabFrames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitFramesMain)).BeginInit();
            this.splitFramesMain.Panel1.SuspendLayout();
            this.splitFramesMain.Panel2.SuspendLayout();
            this.splitFramesMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitFramesLeftInner)).BeginInit();
            this.splitFramesLeftInner.Panel1.SuspendLayout();
            this.splitFramesLeftInner.Panel2.SuspendLayout();
            this.splitFramesLeftInner.SuspendLayout();
            this.groupFrameActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitFramesSub1)).BeginInit();
            this.splitFramesSub1.Panel1.SuspendLayout();
            this.splitFramesSub1.Panel2.SuspendLayout();
            this.splitFramesSub1.SuspendLayout();
            this.groupFrameImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFrameImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupHotspotActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgHotspots)).BeginInit();
            this.tabFrameSets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitFrameSetsMain)).BeginInit();
            this.splitFrameSetsMain.Panel1.SuspendLayout();
            this.splitFrameSetsMain.Panel2.SuspendLayout();
            this.splitFrameSetsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitFrameSetsLeftInner)).BeginInit();
            this.splitFrameSetsLeftInner.Panel1.SuspendLayout();
            this.splitFrameSetsLeftInner.Panel2.SuspendLayout();
            this.splitFrameSetsLeftInner.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabAnimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuTasks,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(984, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menFile";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuSaveAs,
            this.menuSetShrink});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            // 
            // menuNew
            // 
            this.menuNew.Name = "menuNew";
            this.menuNew.Size = new System.Drawing.Size(116, 22);
            this.menuNew.Text = "New";
            this.menuNew.Click += new System.EventHandler(this.OnMenuNewClick);
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Size = new System.Drawing.Size(116, 22);
            this.menuOpen.Text = "Open";
            this.menuOpen.Click += new System.EventHandler(this.OnMenuOpenClick);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(116, 22);
            this.menuSaveAs.Text = "Save";
            this.menuSaveAs.Click += new System.EventHandler(this.OnMenuSaveAsClick);
            // 
            // menuSetShrink
            // 
            this.menuSetShrink.Name = "menuSetShrink";
            this.menuSetShrink.Size = new System.Drawing.Size(116, 22);
            this.menuSetShrink.Text = "Settings";
            this.menuSetShrink.Click += new System.EventHandler(this.OnMenuSetShrinkClick);
            // 
            // menuTasks
            // 
            this.menuTasks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExportAllBGFToXML,
            this.menuDecompressAllBGF,
            this.menuConvertAllToV10,
            this.menuConvertAllToV9,
            this.menuComparePalettes});
            this.menuTasks.Name = "menuTasks";
            this.menuTasks.Size = new System.Drawing.Size(48, 20);
            this.menuTasks.Text = "Tools";
            // 
            // menuExportAllBGFToXML
            // 
            this.menuExportAllBGFToXML.Name = "menuExportAllBGFToXML";
            this.menuExportAllBGFToXML.Size = new System.Drawing.Size(217, 22);
            this.menuExportAllBGFToXML.Text = "Export all BGF to XML/BMP";
            this.menuExportAllBGFToXML.Click += new System.EventHandler(this.OnMenuExportAllBGFToXMLClick);
            // 
            // menuDecompressAllBGF
            // 
            this.menuDecompressAllBGF.Name = "menuDecompressAllBGF";
            this.menuDecompressAllBGF.Size = new System.Drawing.Size(217, 22);
            this.menuDecompressAllBGF.Text = "Decompress all BGF";
            this.menuDecompressAllBGF.Click += new System.EventHandler(this.OnMenuDecompressAllBGFClick);
            // 
            // menuConvertAllToV10
            // 
            this.menuConvertAllToV10.Name = "menuConvertAllToV10";
            this.menuConvertAllToV10.Size = new System.Drawing.Size(217, 22);
            this.menuConvertAllToV10.Text = "Convert all to V10 (zlib)";
            this.menuConvertAllToV10.Click += new System.EventHandler(this.OnMenuConvertAllToV10Click);
            // 
            // menuConvertAllToV9
            // 
            this.menuConvertAllToV9.Name = "menuConvertAllToV9";
            this.menuConvertAllToV9.Size = new System.Drawing.Size(217, 22);
            this.menuConvertAllToV9.Text = "Convert all to V9 (crush32)";
            this.menuConvertAllToV9.Click += new System.EventHandler(this.OnMenuConvertAllToV9Click);
            // 
            // menuComparePalettes
            // 
            this.menuComparePalettes.Name = "menuComparePalettes";
            this.menuComparePalettes.Size = new System.Drawing.Size(217, 22);
            this.menuComparePalettes.Text = "Compare palettes";
            this.menuComparePalettes.Click += new System.EventHandler(this.OnMenuComparePalettesClick);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(107, 22);
            this.menuAbout.Text = "About";
            this.menuAbout.Click += new System.EventHandler(this.OnMenuAboutClick);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabFrames);
            this.tabMain.Controls.Add(this.tabFrameSets);
            this.tabMain.Controls.Add(this.tabAnimation);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 24);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(984, 433);
            this.tabMain.TabIndex = 1;
            // 
            // tabFrames
            // 
            this.tabFrames.Controls.Add(this.splitFramesMain);
            this.tabFrames.Location = new System.Drawing.Point(4, 22);
            this.tabFrames.Name = "tabFrames";
            this.tabFrames.Padding = new System.Windows.Forms.Padding(3);
            this.tabFrames.Size = new System.Drawing.Size(976, 407);
            this.tabFrames.TabIndex = 0;
            this.tabFrames.Text = "Frames";
            this.tabFrames.UseVisualStyleBackColor = true;
            // 
            // splitFramesMain
            // 
            this.splitFramesMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFramesMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitFramesMain.Location = new System.Drawing.Point(3, 3);
            this.splitFramesMain.Name = "splitFramesMain";
            // 
            // splitFramesMain.Panel1
            // 
            this.splitFramesMain.Panel1.Controls.Add(this.splitFramesLeftInner);
            // 
            // splitFramesMain.Panel2
            // 
            this.splitFramesMain.Panel2.Controls.Add(this.splitFramesSub1);
            this.splitFramesMain.Size = new System.Drawing.Size(970, 401);
            this.splitFramesMain.SplitterDistance = 382;
            this.splitFramesMain.TabIndex = 0;
            // 
            // splitFramesLeftInner
            // 
            this.splitFramesLeftInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFramesLeftInner.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitFramesLeftInner.IsSplitterFixed = true;
            this.splitFramesLeftInner.Location = new System.Drawing.Point(0, 0);
            this.splitFramesLeftInner.Name = "splitFramesLeftInner";
            this.splitFramesLeftInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitFramesLeftInner.Panel1
            // 
            this.splitFramesLeftInner.Panel1.Controls.Add(this.groupFrameActions);
            // 
            // splitFramesLeftInner.Panel2
            // 
            this.splitFramesLeftInner.Panel2.Controls.Add(this.dgFrames);
            this.splitFramesLeftInner.Size = new System.Drawing.Size(382, 401);
            this.splitFramesLeftInner.SplitterDistance = 42;
            this.splitFramesLeftInner.TabIndex = 1;
            // 
            // groupFrameActions
            // 
            this.groupFrameActions.Controls.Add(this.btnFrameAdd);
            this.groupFrameActions.Controls.Add(this.btnFrameRemove);
            this.groupFrameActions.Controls.Add(this.btnFrameUp);
            this.groupFrameActions.Controls.Add(this.btnFrameDown);
            this.groupFrameActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupFrameActions.Location = new System.Drawing.Point(0, 0);
            this.groupFrameActions.Name = "groupFrameActions";
            this.groupFrameActions.Size = new System.Drawing.Size(382, 42);
            this.groupFrameActions.TabIndex = 4;
            this.groupFrameActions.TabStop = false;
            this.groupFrameActions.Text = "Frames";
            // 
            // btnFrameAdd
            // 
            this.btnFrameAdd.Image = global::Meridian59.BgfEditor.Properties.Resources.Add;
            this.btnFrameAdd.Location = new System.Drawing.Point(28, 16);
            this.btnFrameAdd.Name = "btnFrameAdd";
            this.btnFrameAdd.Size = new System.Drawing.Size(75, 23);
            this.btnFrameAdd.TabIndex = 0;
            this.btnFrameAdd.UseVisualStyleBackColor = true;
            this.btnFrameAdd.Click += new System.EventHandler(this.OnFrameAddClick);
            // 
            // btnFrameRemove
            // 
            this.btnFrameRemove.Image = global::Meridian59.BgfEditor.Properties.Resources.Delete;
            this.btnFrameRemove.Location = new System.Drawing.Point(109, 16);
            this.btnFrameRemove.Name = "btnFrameRemove";
            this.btnFrameRemove.Size = new System.Drawing.Size(75, 23);
            this.btnFrameRemove.TabIndex = 1;
            this.btnFrameRemove.UseVisualStyleBackColor = true;
            this.btnFrameRemove.Click += new System.EventHandler(this.OnFrameRemoveClick);
            // 
            // btnFrameUp
            // 
            this.btnFrameUp.Image = global::Meridian59.BgfEditor.Properties.Resources.Up;
            this.btnFrameUp.Location = new System.Drawing.Point(190, 16);
            this.btnFrameUp.Name = "btnFrameUp";
            this.btnFrameUp.Size = new System.Drawing.Size(75, 23);
            this.btnFrameUp.TabIndex = 2;
            this.btnFrameUp.UseVisualStyleBackColor = true;
            this.btnFrameUp.Click += new System.EventHandler(this.OnFrameUpClick);
            // 
            // btnFrameDown
            // 
            this.btnFrameDown.Image = global::Meridian59.BgfEditor.Properties.Resources.Down;
            this.btnFrameDown.Location = new System.Drawing.Point(271, 16);
            this.btnFrameDown.Name = "btnFrameDown";
            this.btnFrameDown.Size = new System.Drawing.Size(75, 23);
            this.btnFrameDown.TabIndex = 3;
            this.btnFrameDown.UseVisualStyleBackColor = true;
            this.btnFrameDown.Click += new System.EventHandler(this.OnFrameDownClick);
            // 
            // dgFrames
            // 
            this.dgFrames.AllowUserToAddRows = false;
            this.dgFrames.AllowUserToDeleteRows = false;
            this.dgFrames.AllowUserToResizeRows = false;
            this.dgFrames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFrames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colWidth,
            this.colHeight,
            this.colXOffset,
            this.colYOffset});
            this.dgFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgFrames.Location = new System.Drawing.Point(0, 0);
            this.dgFrames.MultiSelect = false;
            this.dgFrames.Name = "dgFrames";
            this.dgFrames.RowHeadersVisible = false;
            this.dgFrames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgFrames.Size = new System.Drawing.Size(382, 355);
            this.dgFrames.TabIndex = 1;
            this.dgFrames.SelectionChanged += new System.EventHandler(this.OnFramesSelectionChanged);
            // 
            // colNum
            // 
            this.colNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNum.DataPropertyName = "Num";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Wheat;
            this.colNum.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNum.HeaderText = "Num";
            this.colNum.Name = "colNum";
            this.colNum.ReadOnly = true;
            this.colNum.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colNum.Width = 50;
            // 
            // colWidth
            // 
            this.colWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colWidth.DataPropertyName = "Width";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Wheat;
            this.colWidth.DefaultCellStyle = dataGridViewCellStyle2;
            this.colWidth.HeaderText = "Width";
            this.colWidth.Name = "colWidth";
            this.colWidth.ReadOnly = true;
            // 
            // colHeight
            // 
            this.colHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHeight.DataPropertyName = "Height";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Wheat;
            this.colHeight.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHeight.HeaderText = "Height";
            this.colHeight.Name = "colHeight";
            this.colHeight.ReadOnly = true;
            // 
            // colXOffset
            // 
            this.colXOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colXOffset.DataPropertyName = "XOffset";
            this.colXOffset.HeaderText = "XOffset";
            this.colXOffset.Name = "colXOffset";
            // 
            // colYOffset
            // 
            this.colYOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colYOffset.DataPropertyName = "YOffset";
            this.colYOffset.HeaderText = "YOffset";
            this.colYOffset.Name = "colYOffset";
            // 
            // splitFramesSub1
            // 
            this.splitFramesSub1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFramesSub1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitFramesSub1.Location = new System.Drawing.Point(0, 0);
            this.splitFramesSub1.Name = "splitFramesSub1";
            // 
            // splitFramesSub1.Panel1
            // 
            this.splitFramesSub1.Panel1.Controls.Add(this.groupFrameImage);
            // 
            // splitFramesSub1.Panel2
            // 
            this.splitFramesSub1.Panel2.Controls.Add(this.splitContainer1);
            this.splitFramesSub1.Size = new System.Drawing.Size(584, 401);
            this.splitFramesSub1.SplitterDistance = 346;
            this.splitFramesSub1.TabIndex = 0;
            // 
            // groupFrameImage
            // 
            this.groupFrameImage.Controls.Add(this.picFrameImage);
            this.groupFrameImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupFrameImage.Location = new System.Drawing.Point(0, 0);
            this.groupFrameImage.Name = "groupFrameImage";
            this.groupFrameImage.Size = new System.Drawing.Size(346, 401);
            this.groupFrameImage.TabIndex = 1;
            this.groupFrameImage.TabStop = false;
            this.groupFrameImage.Text = "Image";
            // 
            // picFrameImage
            // 
            this.picFrameImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picFrameImage.Location = new System.Drawing.Point(3, 16);
            this.picFrameImage.Name = "picFrameImage";
            this.picFrameImage.Size = new System.Drawing.Size(340, 382);
            this.picFrameImage.TabIndex = 0;
            this.picFrameImage.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupHotspotActions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgHotspots);
            this.splitContainer1.Size = new System.Drawing.Size(234, 401);
            this.splitContainer1.SplitterDistance = 44;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupHotspotActions
            // 
            this.groupHotspotActions.Controls.Add(this.btnHotspotDown);
            this.groupHotspotActions.Controls.Add(this.btnHotspotUp);
            this.groupHotspotActions.Controls.Add(this.btnHotspotAdd);
            this.groupHotspotActions.Controls.Add(this.btnHotspotRemove);
            this.groupHotspotActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupHotspotActions.Location = new System.Drawing.Point(0, 0);
            this.groupHotspotActions.Name = "groupHotspotActions";
            this.groupHotspotActions.Size = new System.Drawing.Size(234, 44);
            this.groupHotspotActions.TabIndex = 0;
            this.groupHotspotActions.TabStop = false;
            this.groupHotspotActions.Text = "Hotspots";
            // 
            // btnHotspotDown
            // 
            this.btnHotspotDown.Image = global::Meridian59.BgfEditor.Properties.Resources.Down;
            this.btnHotspotDown.Location = new System.Drawing.Point(178, 16);
            this.btnHotspotDown.Name = "btnHotspotDown";
            this.btnHotspotDown.Size = new System.Drawing.Size(45, 23);
            this.btnHotspotDown.TabIndex = 5;
            this.btnHotspotDown.UseVisualStyleBackColor = true;
            this.btnHotspotDown.Click += new System.EventHandler(this.OnHotspotDownClick);
            // 
            // btnHotspotUp
            // 
            this.btnHotspotUp.Image = global::Meridian59.BgfEditor.Properties.Resources.Up;
            this.btnHotspotUp.Location = new System.Drawing.Point(127, 16);
            this.btnHotspotUp.Name = "btnHotspotUp";
            this.btnHotspotUp.Size = new System.Drawing.Size(45, 23);
            this.btnHotspotUp.TabIndex = 4;
            this.btnHotspotUp.UseVisualStyleBackColor = true;
            this.btnHotspotUp.Click += new System.EventHandler(this.OnHotspotUpClick);
            // 
            // btnHotspotAdd
            // 
            this.btnHotspotAdd.Image = global::Meridian59.BgfEditor.Properties.Resources.Add;
            this.btnHotspotAdd.Location = new System.Drawing.Point(25, 16);
            this.btnHotspotAdd.Name = "btnHotspotAdd";
            this.btnHotspotAdd.Size = new System.Drawing.Size(45, 23);
            this.btnHotspotAdd.TabIndex = 2;
            this.btnHotspotAdd.UseVisualStyleBackColor = true;
            this.btnHotspotAdd.Click += new System.EventHandler(this.OnHotspotAddClick);
            // 
            // btnHotspotRemove
            // 
            this.btnHotspotRemove.Image = global::Meridian59.BgfEditor.Properties.Resources.Delete;
            this.btnHotspotRemove.Location = new System.Drawing.Point(76, 16);
            this.btnHotspotRemove.Name = "btnHotspotRemove";
            this.btnHotspotRemove.Size = new System.Drawing.Size(45, 23);
            this.btnHotspotRemove.TabIndex = 3;
            this.btnHotspotRemove.UseVisualStyleBackColor = true;
            this.btnHotspotRemove.Click += new System.EventHandler(this.OnHotspotRemoveClick);
            // 
            // dgHotspots
            // 
            this.dgHotspots.AllowUserToAddRows = false;
            this.dgHotspots.AllowUserToDeleteRows = false;
            this.dgHotspots.AllowUserToResizeRows = false;
            this.dgHotspots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgHotspots.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndex,
            this.colX,
            this.colY});
            this.dgHotspots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgHotspots.Location = new System.Drawing.Point(0, 0);
            this.dgHotspots.MultiSelect = false;
            this.dgHotspots.Name = "dgHotspots";
            this.dgHotspots.RowHeadersVisible = false;
            this.dgHotspots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgHotspots.Size = new System.Drawing.Size(234, 353);
            this.dgHotspots.TabIndex = 1;
            // 
            // colIndex
            // 
            this.colIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIndex.DataPropertyName = "Index";
            this.colIndex.FillWeight = 91.37056F;
            this.colIndex.HeaderText = "Index";
            this.colIndex.Name = "colIndex";
            // 
            // colX
            // 
            this.colX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colX.DataPropertyName = "X";
            this.colX.FillWeight = 100.9751F;
            this.colX.HeaderText = "X";
            this.colX.Name = "colX";
            // 
            // colY
            // 
            this.colY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colY.DataPropertyName = "Y";
            this.colY.FillWeight = 107.6544F;
            this.colY.HeaderText = "Y";
            this.colY.Name = "colY";
            // 
            // tabFrameSets
            // 
            this.tabFrameSets.Controls.Add(this.splitFrameSetsMain);
            this.tabFrameSets.Location = new System.Drawing.Point(4, 22);
            this.tabFrameSets.Name = "tabFrameSets";
            this.tabFrameSets.Padding = new System.Windows.Forms.Padding(3);
            this.tabFrameSets.Size = new System.Drawing.Size(976, 407);
            this.tabFrameSets.TabIndex = 1;
            this.tabFrameSets.Text = "Groups";
            this.tabFrameSets.UseVisualStyleBackColor = true;
            // 
            // splitFrameSetsMain
            // 
            this.splitFrameSetsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFrameSetsMain.Location = new System.Drawing.Point(3, 3);
            this.splitFrameSetsMain.Name = "splitFrameSetsMain";
            // 
            // splitFrameSetsMain.Panel1
            // 
            this.splitFrameSetsMain.Panel1.Controls.Add(this.splitFrameSetsLeftInner);
            // 
            // splitFrameSetsMain.Panel2
            // 
            this.splitFrameSetsMain.Panel2.Controls.Add(this.splitContainer3);
            this.splitFrameSetsMain.Size = new System.Drawing.Size(970, 401);
            this.splitFrameSetsMain.SplitterDistance = 231;
            this.splitFrameSetsMain.TabIndex = 0;
            // 
            // splitFrameSetsLeftInner
            // 
            this.splitFrameSetsLeftInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFrameSetsLeftInner.Location = new System.Drawing.Point(0, 0);
            this.splitFrameSetsLeftInner.Name = "splitFrameSetsLeftInner";
            this.splitFrameSetsLeftInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitFrameSetsLeftInner.Panel1
            // 
            this.splitFrameSetsLeftInner.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitFrameSetsLeftInner.Panel2
            // 
            this.splitFrameSetsLeftInner.Panel2.Controls.Add(this.listFrameSets);
            this.splitFrameSetsLeftInner.Size = new System.Drawing.Size(231, 401);
            this.splitFrameSetsLeftInner.SplitterDistance = 51;
            this.splitFrameSetsLeftInner.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFrameSetAdd);
            this.groupBox2.Controls.Add(this.btnFrameSetRemove);
            this.groupBox2.Controls.Add(this.btnFrameSetUp);
            this.groupBox2.Controls.Add(this.btnFrameSetDown);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 51);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Groups";
            // 
            // btnFrameSetAdd
            // 
            this.btnFrameSetAdd.Image = global::Meridian59.BgfEditor.Properties.Resources.Add;
            this.btnFrameSetAdd.Location = new System.Drawing.Point(15, 19);
            this.btnFrameSetAdd.Name = "btnFrameSetAdd";
            this.btnFrameSetAdd.Size = new System.Drawing.Size(45, 23);
            this.btnFrameSetAdd.TabIndex = 4;
            this.btnFrameSetAdd.UseVisualStyleBackColor = true;
            this.btnFrameSetAdd.Click += new System.EventHandler(this.OnFrameSetAddClick);
            // 
            // btnFrameSetRemove
            // 
            this.btnFrameSetRemove.Image = global::Meridian59.BgfEditor.Properties.Resources.Delete;
            this.btnFrameSetRemove.Location = new System.Drawing.Point(66, 19);
            this.btnFrameSetRemove.Name = "btnFrameSetRemove";
            this.btnFrameSetRemove.Size = new System.Drawing.Size(45, 23);
            this.btnFrameSetRemove.TabIndex = 5;
            this.btnFrameSetRemove.UseVisualStyleBackColor = true;
            this.btnFrameSetRemove.Click += new System.EventHandler(this.OnFrameSetRemoveClick);
            // 
            // btnFrameSetUp
            // 
            this.btnFrameSetUp.Image = global::Meridian59.BgfEditor.Properties.Resources.Up;
            this.btnFrameSetUp.Location = new System.Drawing.Point(117, 19);
            this.btnFrameSetUp.Name = "btnFrameSetUp";
            this.btnFrameSetUp.Size = new System.Drawing.Size(45, 23);
            this.btnFrameSetUp.TabIndex = 6;
            this.btnFrameSetUp.UseVisualStyleBackColor = true;
            this.btnFrameSetUp.Click += new System.EventHandler(this.OnFrameSetUpClick);
            // 
            // btnFrameSetDown
            // 
            this.btnFrameSetDown.Image = global::Meridian59.BgfEditor.Properties.Resources.Down;
            this.btnFrameSetDown.Location = new System.Drawing.Point(168, 19);
            this.btnFrameSetDown.Name = "btnFrameSetDown";
            this.btnFrameSetDown.Size = new System.Drawing.Size(45, 23);
            this.btnFrameSetDown.TabIndex = 7;
            this.btnFrameSetDown.UseVisualStyleBackColor = true;
            this.btnFrameSetDown.Click += new System.EventHandler(this.OnFrameSetDownClick);
            // 
            // listFrameSets
            // 
            this.listFrameSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFrameSets.FormattingEnabled = true;
            this.listFrameSets.Location = new System.Drawing.Point(0, 0);
            this.listFrameSets.Name = "listFrameSets";
            this.listFrameSets.Size = new System.Drawing.Size(231, 346);
            this.listFrameSets.TabIndex = 0;
            this.listFrameSets.SelectedIndexChanged += new System.EventHandler(this.OnFrameSetsSelectedIndexChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(735, 401);
            this.splitContainer3.SplitterDistance = 511;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.flowLayoutFrameSet);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(511, 401);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Frames";
            // 
            // flowLayoutFrameSet
            // 
            this.flowLayoutFrameSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutFrameSet.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutFrameSet.Name = "flowLayoutFrameSet";
            this.flowLayoutFrameSet.Size = new System.Drawing.Size(505, 382);
            this.flowLayoutFrameSet.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.listFrameNums);
            this.splitContainer4.Size = new System.Drawing.Size(220, 401);
            this.splitContainer4.SplitterDistance = 47;
            this.splitContainer4.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnFrameIndexAdd);
            this.groupBox3.Controls.Add(this.btnFrameIndexRemove);
            this.groupBox3.Controls.Add(this.btnFrameIndexUp);
            this.groupBox3.Controls.Add(this.btnFrameIndexDown);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 47);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Frames";
            // 
            // btnFrameIndexAdd
            // 
            this.btnFrameIndexAdd.Image = global::Meridian59.BgfEditor.Properties.Resources.Add;
            this.btnFrameIndexAdd.Location = new System.Drawing.Point(7, 18);
            this.btnFrameIndexAdd.Name = "btnFrameIndexAdd";
            this.btnFrameIndexAdd.Size = new System.Drawing.Size(45, 23);
            this.btnFrameIndexAdd.TabIndex = 8;
            this.btnFrameIndexAdd.UseVisualStyleBackColor = true;
            this.btnFrameIndexAdd.Click += new System.EventHandler(this.OnFrameIndexAddClick);
            // 
            // btnFrameIndexRemove
            // 
            this.btnFrameIndexRemove.Image = global::Meridian59.BgfEditor.Properties.Resources.Delete;
            this.btnFrameIndexRemove.Location = new System.Drawing.Point(58, 18);
            this.btnFrameIndexRemove.Name = "btnFrameIndexRemove";
            this.btnFrameIndexRemove.Size = new System.Drawing.Size(45, 23);
            this.btnFrameIndexRemove.TabIndex = 9;
            this.btnFrameIndexRemove.UseVisualStyleBackColor = true;
            this.btnFrameIndexRemove.Click += new System.EventHandler(this.OnFrameIndexRemoveClick);
            // 
            // btnFrameIndexUp
            // 
            this.btnFrameIndexUp.Image = global::Meridian59.BgfEditor.Properties.Resources.Up;
            this.btnFrameIndexUp.Location = new System.Drawing.Point(109, 18);
            this.btnFrameIndexUp.Name = "btnFrameIndexUp";
            this.btnFrameIndexUp.Size = new System.Drawing.Size(45, 23);
            this.btnFrameIndexUp.TabIndex = 10;
            this.btnFrameIndexUp.UseVisualStyleBackColor = true;
            this.btnFrameIndexUp.Click += new System.EventHandler(this.OnFrameIndexUpClick);
            // 
            // btnFrameIndexDown
            // 
            this.btnFrameIndexDown.Image = global::Meridian59.BgfEditor.Properties.Resources.Down;
            this.btnFrameIndexDown.Location = new System.Drawing.Point(160, 18);
            this.btnFrameIndexDown.Name = "btnFrameIndexDown";
            this.btnFrameIndexDown.Size = new System.Drawing.Size(45, 23);
            this.btnFrameIndexDown.TabIndex = 11;
            this.btnFrameIndexDown.UseVisualStyleBackColor = true;
            this.btnFrameIndexDown.Click += new System.EventHandler(this.OnFrameIndexDownClick);
            // 
            // listFrameNums
            // 
            this.listFrameNums.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFrameNums.FormattingEnabled = true;
            this.listFrameNums.Location = new System.Drawing.Point(0, 0);
            this.listFrameNums.Name = "listFrameNums";
            this.listFrameNums.Size = new System.Drawing.Size(220, 350);
            this.listFrameNums.TabIndex = 0;
            // 
            // tabAnimation
            // 
            this.tabAnimation.Controls.Add(this.splitContainer5);
            this.tabAnimation.Location = new System.Drawing.Point(4, 22);
            this.tabAnimation.Name = "tabAnimation";
            this.tabAnimation.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnimation.Size = new System.Drawing.Size(976, 407);
            this.tabAnimation.TabIndex = 2;
            this.tabAnimation.Text = "Animation Test";
            this.tabAnimation.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(3, 3);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.groupBox5);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer5.Size = new System.Drawing.Size(970, 401);
            this.splitContainer5.SplitterDistance = 213;
            this.splitContainer5.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.cbType);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.cbFinal);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.cbGroup);
            this.groupBox5.Controls.Add(this.cbPalette);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.trackAngle);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.btnPlay);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.numInterval);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.cbHigh);
            this.groupBox5.Controls.Add(this.cbLow);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(213, 401);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Settings";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Type:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "None",
            "Cycle",
            "Once"});
            this.cbType.Location = new System.Drawing.Point(123, 28);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(63, 21);
            this.cbType.TabIndex = 23;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.OnTypeSelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "GroupFinal:";
            // 
            // cbFinal
            // 
            this.cbFinal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFinal.FormattingEnabled = true;
            this.cbFinal.Location = new System.Drawing.Point(123, 139);
            this.cbFinal.Name = "cbFinal";
            this.cbFinal.Size = new System.Drawing.Size(63, 21);
            this.cbFinal.TabIndex = 21;
            this.cbFinal.SelectedIndexChanged += new System.EventHandler(this.OnFinalSelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Group:";
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(123, 58);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(63, 21);
            this.cbGroup.TabIndex = 19;
            this.cbGroup.SelectedIndexChanged += new System.EventHandler(this.OnGroupSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Palette:";
            // 
            // trackAngle
            // 
            this.trackAngle.AutoSize = false;
            this.trackAngle.Location = new System.Drawing.Point(123, 199);
            this.trackAngle.Maximum = 4096;
            this.trackAngle.Name = "trackAngle";
            this.trackAngle.Size = new System.Drawing.Size(63, 20);
            this.trackAngle.TabIndex = 16;
            this.trackAngle.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Angle:";
            // 
            // btnPlay
            // 
            this.btnPlay.Image = global::Meridian59.BgfEditor.Properties.Resources.Play;
            this.btnPlay.Location = new System.Drawing.Point(72, 310);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(63, 23);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.OnPlayClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Interval:";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(123, 168);
            this.numInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(63, 20);
            this.numInterval.TabIndex = 4;
            this.numInterval.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "GroupHigh:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "GroupLow:";
            // 
            // cbHigh
            // 
            this.cbHigh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHigh.FormattingEnabled = true;
            this.cbHigh.Location = new System.Drawing.Point(123, 112);
            this.cbHigh.Name = "cbHigh";
            this.cbHigh.Size = new System.Drawing.Size(63, 21);
            this.cbHigh.TabIndex = 1;
            this.cbHigh.SelectedIndexChanged += new System.EventHandler(this.OnHighSelectedIndexChanged);
            // 
            // cbLow
            // 
            this.cbLow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLow.FormattingEnabled = true;
            this.cbLow.Location = new System.Drawing.Point(123, 85);
            this.cbLow.Name = "cbLow";
            this.cbLow.Size = new System.Drawing.Size(63, 21);
            this.cbLow.TabIndex = 0;
            this.cbLow.SelectedIndexChanged += new System.EventHandler(this.OnLowSelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.picAnimation);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(753, 401);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Animation";
            // 
            // picAnimation
            // 
            this.picAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picAnimation.Location = new System.Drawing.Point(3, 16);
            this.picAnimation.Name = "picAnimation";
            this.picAnimation.Size = new System.Drawing.Size(747, 382);
            this.picAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picAnimation.TabIndex = 0;
            this.picAnimation.TabStop = false;
            // 
            // fdAddFrame
            // 
            this.fdAddFrame.Filter = "8-Bit Bitmap |*.bmp";
            this.fdAddFrame.FileOk += new System.ComponentModel.CancelEventHandler(this.OnFileDialogAddFrameFileOk);
            // 
            // fdOpenFile
            // 
            this.fdOpenFile.Filter = "M59 Images |*.bgf|M59 Images|*.xml";
            this.fdOpenFile.FileOk += new System.ComponentModel.CancelEventHandler(this.OnFileDialogOpenFileOk);
            // 
            // fdSaveFile
            // 
            this.fdSaveFile.Filter = "BGF-File|*.bgf|XML-File|*.xml";
            this.fdSaveFile.FileOk += new System.ComponentModel.CancelEventHandler(this.OnFileDialogSaveFileOk);
            // 
            // cbPalette
            // 
            this.cbPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPalette.FormattingEnabled = true;            
            this.cbPalette.Location = new System.Drawing.Point(22, 244);
            this.cbPalette.Name = "cbPalette";
            this.cbPalette.Size = new System.Drawing.Size(164, 21);
            this.cbPalette.TabIndex = 18;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 457);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Meridian 59 BGF Editor";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResizeEnd += new System.EventHandler(this.OnResizeEnd);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabFrames.ResumeLayout(false);
            this.splitFramesMain.Panel1.ResumeLayout(false);
            this.splitFramesMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFramesMain)).EndInit();
            this.splitFramesMain.ResumeLayout(false);
            this.splitFramesLeftInner.Panel1.ResumeLayout(false);
            this.splitFramesLeftInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFramesLeftInner)).EndInit();
            this.splitFramesLeftInner.ResumeLayout(false);
            this.groupFrameActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFrames)).EndInit();
            this.splitFramesSub1.Panel1.ResumeLayout(false);
            this.splitFramesSub1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFramesSub1)).EndInit();
            this.splitFramesSub1.ResumeLayout(false);
            this.groupFrameImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFrameImage)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupHotspotActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgHotspots)).EndInit();
            this.tabFrameSets.ResumeLayout(false);
            this.splitFrameSetsMain.Panel1.ResumeLayout(false);
            this.splitFrameSetsMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFrameSetsMain)).EndInit();
            this.splitFrameSetsMain.ResumeLayout(false);
            this.splitFrameSetsLeftInner.Panel1.ResumeLayout(false);
            this.splitFrameSetsLeftInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFrameSetsLeftInner)).EndInit();
            this.splitFrameSetsLeftInner.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabAnimation.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAnimation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabFrames;
        private System.Windows.Forms.TabPage tabFrameSets;
        private System.Windows.Forms.ToolStripMenuItem menuTasks;
        private System.Windows.Forms.SplitContainer splitFramesMain;
        private System.Windows.Forms.SplitContainer splitFramesSub1;
        private System.Windows.Forms.SplitContainer splitFramesLeftInner;
        private System.Windows.Forms.Button btnFrameAdd;
        private System.Windows.Forms.Button btnFrameRemove;
        private System.Windows.Forms.Button btnFrameDown;
        private System.Windows.Forms.Button btnFrameUp;
        private System.Windows.Forms.GroupBox groupFrameActions;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupHotspotActions;
        private System.Windows.Forms.Button btnHotspotAdd;
        private System.Windows.Forms.Button btnHotspotRemove;
        private System.Windows.Forms.OpenFileDialog fdAddFrame;
        private System.Windows.Forms.OpenFileDialog fdOpenFile;
        private System.Windows.Forms.SaveFileDialog fdSaveFile;
        private System.Windows.Forms.DataGridView dgFrames;
        private System.Windows.Forms.DataGridView dgHotspots;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
        private System.Windows.Forms.Button btnHotspotUp;
        private System.Windows.Forms.Button btnHotspotDown;
        private System.Windows.Forms.SplitContainer splitFrameSetsMain;
        private System.Windows.Forms.SplitContainer splitFrameSetsLeftInner;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFrameSetAdd;
        private System.Windows.Forms.Button btnFrameSetRemove;
        private System.Windows.Forms.Button btnFrameSetUp;
        private System.Windows.Forms.Button btnFrameSetDown;
        private System.Windows.Forms.ListBox listFrameSets;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListBox listFrameNums;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnFrameIndexAdd;
        private System.Windows.Forms.Button btnFrameIndexRemove;
        private System.Windows.Forms.Button btnFrameIndexUp;
        private System.Windows.Forms.Button btnFrameIndexDown;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutFrameSet;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuExportAllBGFToXML;
        private System.Windows.Forms.ToolStripMenuItem menuDecompressAllBGF;
        private System.Windows.Forms.TabPage tabAnimation;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbHigh;
        private System.Windows.Forms.ComboBox cbLow;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem menuSetShrink;
        private System.Windows.Forms.GroupBox groupFrameImage;
        private System.Windows.Forms.ToolStripMenuItem menuConvertAllToV10;
        private System.Windows.Forms.ToolStripMenuItem menuConvertAllToV9;
        private System.Windows.Forms.TrackBar trackAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYOffset;
        private System.Windows.Forms.ToolStripMenuItem menuComparePalettes;
        private Controls.ComboBoxPalette cbPalette;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbFinal;
        public System.Windows.Forms.PictureBox picFrameImage;
        public System.Windows.Forms.PictureBox picAnimation;
        public System.Windows.Forms.Button btnPlay;
    }
}

