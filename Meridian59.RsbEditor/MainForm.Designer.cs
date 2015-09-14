namespace Meridian59.RsbEditor
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbFilterLanguage = new System.Windows.Forms.GroupBox();
            this.cbFilterLanguage = new System.Windows.Forms.ComboBox();
            this.chkFilterLanguage = new System.Windows.Forms.CheckBox();
            this.gbStats = new System.Windows.Forms.GroupBox();
            this.lblEntriesDesc = new System.Windows.Forms.Label();
            this.lblEntries = new System.Windows.Forms.Label();
            this.lblShownDesc = new System.Windows.Forms.Label();
            this.lblShown = new System.Windows.Forms.Label();
            this.gbFilterID = new System.Windows.Forms.GroupBox();
            this.chkFilterID = new System.Windows.Forms.CheckBox();
            this.numFilterID = new System.Windows.Forms.NumericUpDown();
            this.gbVersion = new System.Windows.Forms.GroupBox();
            this.numVersion = new System.Windows.Forms.NumericUpDown();
            this.gbFilterText = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chkFilterText = new System.Windows.Forms.CheckBox();
            this.txtFilterText = new System.Windows.Forms.TextBox();
            this.gbStrings = new System.Windows.Forms.GroupBox();
            this.gridStrings = new System.Windows.Forms.DataGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbFilterLanguage.SuspendLayout();
            this.gbStats.SuspendLayout();
            this.gbFilterID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterID)).BeginInit();
            this.gbVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVersion)).BeginInit();
            this.gbFilterText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbStrings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStrings)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(786, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OnMenuOpenClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.OnMenuSaveAsClick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "M59 RSB Files|*.rsb";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbStrings);
            this.splitContainer1.Size = new System.Drawing.Size(786, 482);
            this.splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 201F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Controls.Add(this.gbFilterLanguage, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbStats, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbFilterID, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbVersion, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbFilterText, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(786, 50);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // gbFilterLanguage
            // 
            this.gbFilterLanguage.Controls.Add(this.cbFilterLanguage);
            this.gbFilterLanguage.Controls.Add(this.chkFilterLanguage);
            this.gbFilterLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilterLanguage.Location = new System.Drawing.Point(563, 3);
            this.gbFilterLanguage.Name = "gbFilterLanguage";
            this.gbFilterLanguage.Size = new System.Drawing.Size(139, 44);
            this.gbFilterLanguage.TabIndex = 6;
            this.gbFilterLanguage.TabStop = false;
            this.gbFilterLanguage.Text = "Filter by Language";
            // 
            // cbFilterLanguage
            // 
            this.cbFilterLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilterLanguage.Enabled = false;
            this.cbFilterLanguage.FormattingEnabled = true;
            this.cbFilterLanguage.Location = new System.Drawing.Point(27, 16);
            this.cbFilterLanguage.Name = "cbFilterLanguage";
            this.cbFilterLanguage.Size = new System.Drawing.Size(98, 21);
            this.cbFilterLanguage.TabIndex = 12;
            this.cbFilterLanguage.SelectedIndexChanged += new System.EventHandler(this.OnFilterLanguageSelectedIndexChanged);
            // 
            // chkFilterLanguage
            // 
            this.chkFilterLanguage.AutoSize = true;
            this.chkFilterLanguage.Location = new System.Drawing.Point(6, 18);
            this.chkFilterLanguage.Name = "chkFilterLanguage";
            this.chkFilterLanguage.Size = new System.Drawing.Size(15, 14);
            this.chkFilterLanguage.TabIndex = 11;
            this.chkFilterLanguage.UseVisualStyleBackColor = true;
            this.chkFilterLanguage.CheckedChanged += new System.EventHandler(this.OnFilterCheckedChanged);
            // 
            // gbStats
            // 
            this.gbStats.Controls.Add(this.lblEntriesDesc);
            this.gbStats.Controls.Add(this.lblEntries);
            this.gbStats.Controls.Add(this.lblShownDesc);
            this.gbStats.Controls.Add(this.lblShown);
            this.gbStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbStats.Location = new System.Drawing.Point(3, 3);
            this.gbStats.Name = "gbStats";
            this.gbStats.Size = new System.Drawing.Size(195, 44);
            this.gbStats.TabIndex = 0;
            this.gbStats.TabStop = false;
            this.gbStats.Text = "Stats";
            // 
            // lblEntriesDesc
            // 
            this.lblEntriesDesc.AutoSize = true;
            this.lblEntriesDesc.Location = new System.Drawing.Point(9, 21);
            this.lblEntriesDesc.Name = "lblEntriesDesc";
            this.lblEntriesDesc.Size = new System.Drawing.Size(42, 13);
            this.lblEntriesDesc.TabIndex = 2;
            this.lblEntriesDesc.Text = "Entries:";
            // 
            // lblEntries
            // 
            this.lblEntries.AutoSize = true;
            this.lblEntries.Location = new System.Drawing.Point(50, 21);
            this.lblEntries.Name = "lblEntries";
            this.lblEntries.Size = new System.Drawing.Size(10, 13);
            this.lblEntries.TabIndex = 1;
            this.lblEntries.Text = "-";
            // 
            // lblShownDesc
            // 
            this.lblShownDesc.AutoSize = true;
            this.lblShownDesc.Location = new System.Drawing.Point(98, 21);
            this.lblShownDesc.Name = "lblShownDesc";
            this.lblShownDesc.Size = new System.Drawing.Size(43, 13);
            this.lblShownDesc.TabIndex = 5;
            this.lblShownDesc.Text = "Shown:";
            // 
            // lblShown
            // 
            this.lblShown.AutoSize = true;
            this.lblShown.Location = new System.Drawing.Point(140, 21);
            this.lblShown.Name = "lblShown";
            this.lblShown.Size = new System.Drawing.Size(10, 13);
            this.lblShown.TabIndex = 4;
            this.lblShown.Text = "-";
            // 
            // gbFilterID
            // 
            this.gbFilterID.Controls.Add(this.chkFilterID);
            this.gbFilterID.Controls.Add(this.numFilterID);
            this.gbFilterID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilterID.Location = new System.Drawing.Point(424, 3);
            this.gbFilterID.Name = "gbFilterID";
            this.gbFilterID.Size = new System.Drawing.Size(133, 44);
            this.gbFilterID.TabIndex = 1;
            this.gbFilterID.TabStop = false;
            this.gbFilterID.Text = "Filter by ID";
            // 
            // chkFilterID
            // 
            this.chkFilterID.AutoSize = true;
            this.chkFilterID.Location = new System.Drawing.Point(6, 18);
            this.chkFilterID.Name = "chkFilterID";
            this.chkFilterID.Size = new System.Drawing.Size(15, 14);
            this.chkFilterID.TabIndex = 11;
            this.chkFilterID.UseVisualStyleBackColor = true;
            this.chkFilterID.CheckedChanged += new System.EventHandler(this.OnFilterCheckedChanged);
            // 
            // numFilterID
            // 
            this.numFilterID.Enabled = false;
            this.numFilterID.Location = new System.Drawing.Point(27, 16);
            this.numFilterID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numFilterID.Name = "numFilterID";
            this.numFilterID.Size = new System.Drawing.Size(94, 20);
            this.numFilterID.TabIndex = 9;
            this.numFilterID.Value = new decimal(new int[] {
            20001,
            0,
            0,
            0});
            this.numFilterID.ValueChanged += new System.EventHandler(this.OnFilterIDValueChanged);
            // 
            // gbVersion
            // 
            this.gbVersion.Controls.Add(this.numVersion);
            this.gbVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVersion.Location = new System.Drawing.Point(708, 3);
            this.gbVersion.Name = "gbVersion";
            this.gbVersion.Size = new System.Drawing.Size(75, 44);
            this.gbVersion.TabIndex = 4;
            this.gbVersion.TabStop = false;
            this.gbVersion.Text = "Version";
            // 
            // numVersion
            // 
            this.numVersion.Location = new System.Drawing.Point(15, 17);
            this.numVersion.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numVersion.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numVersion.Name = "numVersion";
            this.numVersion.Size = new System.Drawing.Size(42, 20);
            this.numVersion.TabIndex = 6;
            this.numVersion.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numVersion.ValueChanged += new System.EventHandler(this.OnVersionValueChanged);
            // 
            // gbFilterText
            // 
            this.gbFilterText.Controls.Add(this.splitContainer2);
            this.gbFilterText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilterText.Location = new System.Drawing.Point(204, 3);
            this.gbFilterText.Name = "gbFilterText";
            this.gbFilterText.Size = new System.Drawing.Size(214, 44);
            this.gbFilterText.TabIndex = 5;
            this.gbFilterText.TabStop = false;
            this.gbFilterText.Text = "Filter by Text";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chkFilterText);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtFilterText);
            this.splitContainer2.Size = new System.Drawing.Size(208, 25);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 11;
            // 
            // chkFilterText
            // 
            this.chkFilterText.AutoSize = true;
            this.chkFilterText.Location = new System.Drawing.Point(7, 2);
            this.chkFilterText.Name = "chkFilterText";
            this.chkFilterText.Size = new System.Drawing.Size(15, 14);
            this.chkFilterText.TabIndex = 10;
            this.chkFilterText.UseVisualStyleBackColor = true;
            this.chkFilterText.CheckedChanged += new System.EventHandler(this.OnFilterCheckedChanged);
            // 
            // txtFilterText
            // 
            this.txtFilterText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilterText.Enabled = false;
            this.txtFilterText.Location = new System.Drawing.Point(0, 0);
            this.txtFilterText.Name = "txtFilterText";
            this.txtFilterText.Size = new System.Drawing.Size(179, 20);
            this.txtFilterText.TabIndex = 7;
            this.txtFilterText.TextChanged += new System.EventHandler(this.OnFilterTextChanged);
            // 
            // gbStrings
            // 
            this.gbStrings.Controls.Add(this.gridStrings);
            this.gbStrings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbStrings.Location = new System.Drawing.Point(0, 0);
            this.gbStrings.Name = "gbStrings";
            this.gbStrings.Size = new System.Drawing.Size(786, 428);
            this.gbStrings.TabIndex = 0;
            this.gbStrings.TabStop = false;
            this.gbStrings.Text = "Strings";
            // 
            // gridStrings
            // 
            this.gridStrings.AllowUserToAddRows = false;
            this.gridStrings.AllowUserToDeleteRows = false;
            this.gridStrings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStrings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colLanguage,
            this.colText});
            this.gridStrings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStrings.Location = new System.Drawing.Point(3, 16);
            this.gridStrings.Name = "gridStrings";
            this.gridStrings.RowHeadersVisible = false;
            this.gridStrings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStrings.Size = new System.Drawing.Size(780, 409);
            this.gridStrings.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 80;
            // 
            // colLanguage
            // 
            this.colLanguage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLanguage.DataPropertyName = "Language";
            this.colLanguage.HeaderText = "LANG";
            this.colLanguage.Name = "colLanguage";
            this.colLanguage.ReadOnly = true;
            this.colLanguage.Width = 80;
            // 
            // colText
            // 
            this.colText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colText.DataPropertyName = "Text";
            this.colText.HeaderText = "TEXT";
            this.colText.MaxInputLength = 100000;
            this.colText.Name = "colText";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "M59 RSB Files|*.rsb";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 506);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Meridian 59 RSB Editor";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbFilterLanguage.ResumeLayout(false);
            this.gbFilterLanguage.PerformLayout();
            this.gbStats.ResumeLayout(false);
            this.gbStats.PerformLayout();
            this.gbFilterID.ResumeLayout(false);
            this.gbFilterID.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterID)).EndInit();
            this.gbVersion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numVersion)).EndInit();
            this.gbFilterText.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.gbStrings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStrings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbStrings;
        private System.Windows.Forms.DataGridView gridStrings;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label lblEntries;
        private System.Windows.Forms.Label lblEntriesDesc;
        private System.Windows.Forms.Label lblShownDesc;
        private System.Windows.Forms.Label lblShown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtFilterText;
        private System.Windows.Forms.GroupBox gbStats;
        private System.Windows.Forms.GroupBox gbFilterID;
        private System.Windows.Forms.CheckBox chkFilterText;
        private System.Windows.Forms.NumericUpDown numFilterID;
        private System.Windows.Forms.GroupBox gbVersion;
        private System.Windows.Forms.NumericUpDown numVersion;
        private System.Windows.Forms.GroupBox gbFilterText;
        private System.Windows.Forms.CheckBox chkFilterID;
        private System.Windows.Forms.GroupBox gbFilterLanguage;
        private System.Windows.Forms.ComboBox cbFilterLanguage;
        private System.Windows.Forms.CheckBox chkFilterLanguage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
    }
}

