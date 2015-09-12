namespace Meridian59.AdminUI.Viewers
{
    partial class GameMessageView
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridMessages = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colTrafficDirection = new Meridian59.AdminUI.DataGridColumns.InOutColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLEN1 = new Meridian59.AdminUI.DataGridColumns.HexColumn();
            this.colCRC = new Meridian59.AdminUI.DataGridColumns.HexColumn();
            this.colLEN2 = new Meridian59.AdminUI.DataGridColumns.HexColumn();
            this.colSS = new Meridian59.AdminUI.DataGridColumns.HexColumn();
            this.colPI = new Meridian59.AdminUI.DataGridColumns.HexColumn();
            this.colData = new Meridian59.AdminUI.DataGridColumns.HexColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.gbFilters = new System.Windows.Forms.GroupBox();
            this.chkReceive = new System.Windows.Forms.CheckBox();
            this.chkSend = new System.Windows.Forms.CheckBox();
            this.chkPings = new System.Windows.Forms.CheckBox();
            this.gbColumns = new System.Windows.Forms.GroupBox();
            this.chkLength = new System.Windows.Forms.CheckBox();
            this.chkData = new System.Windows.Forms.CheckBox();
            this.chkCRC = new System.Windows.Forms.CheckBox();
            this.chkPI = new System.Windows.Forms.CheckBox();
            this.chkServerSave = new System.Windows.Forms.CheckBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.gbCustomMessage = new System.Windows.Forms.GroupBox();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.txtMessageBody = new Meridian59.AdminUI.Generic.HexTextBox();
            this.btnSendCustom = new System.Windows.Forms.Button();
            this.gbRequests = new System.Windows.Forms.GroupBox();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.cbRequests = new System.Windows.Forms.ComboBox();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.gbOther = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.gbMessages = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.gbFilters.SuspendLayout();
            this.gbColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.gbCustomMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.gbRequests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.gbOther.SuspendLayout();
            this.gbMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridMessages
            // 
            this.gridMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTrafficDirection,
            this.colDescription,
            this.colLEN1,
            this.colCRC,
            this.colLEN2,
            this.colSS,
            this.colPI,
            this.colData});
            this.gridMessages.Location = new System.Drawing.Point(3, 16);
            this.gridMessages.Name = "gridMessages";
            this.gridMessages.Size = new System.Drawing.Size(942, 276);
            this.gridMessages.TabIndex = 0;
            // 
            // colTrafficDirection
            // 
            this.colTrafficDirection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTrafficDirection.DataPropertyName = "TransferDirection";
            this.colTrafficDirection.HeaderText = "";
            this.colTrafficDirection.Name = "colTrafficDirection";
            this.colTrafficDirection.ReadOnly = true;
            this.colTrafficDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTrafficDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colTrafficDirection.Width = 40;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDescription.DataPropertyName = "Description";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Wheat;
            this.colDescription.DefaultCellStyle = dataGridViewCellStyle1;
            this.colDescription.HeaderText = "DESC";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colLEN1
            // 
            this.colLEN1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLEN1.DataPropertyName = "LEN1Bytes";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.PaleGreen;
            this.colLEN1.DefaultCellStyle = dataGridViewCellStyle2;
            this.colLEN1.HeaderText = "LEN1";
            this.colLEN1.Name = "colLEN1";
            this.colLEN1.ReadOnly = true;
            this.colLEN1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLEN1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colLEN1.Width = 40;
            // 
            // colCRC
            // 
            this.colCRC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCRC.DataPropertyName = "CRCBytes";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.PaleGreen;
            this.colCRC.DefaultCellStyle = dataGridViewCellStyle3;
            this.colCRC.HeaderText = "CRC";
            this.colCRC.Name = "colCRC";
            this.colCRC.ReadOnly = true;
            this.colCRC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCRC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCRC.Width = 40;
            // 
            // colLEN2
            // 
            this.colLEN2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLEN2.DataPropertyName = "LEN2Bytes";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.PaleGreen;
            this.colLEN2.DefaultCellStyle = dataGridViewCellStyle4;
            this.colLEN2.HeaderText = "LEN2";
            this.colLEN2.Name = "colLEN2";
            this.colLEN2.ReadOnly = true;
            this.colLEN2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLEN2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colLEN2.Width = 40;
            // 
            // colSS
            // 
            this.colSS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSS.DataPropertyName = "HeaderSS";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.PaleGreen;
            this.colSS.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSS.HeaderText = "SS";
            this.colSS.Name = "colSS";
            this.colSS.ReadOnly = true;
            this.colSS.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSS.Width = 30;
            // 
            // colPI
            // 
            this.colPI.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPI.DataPropertyName = "PI";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Wheat;
            this.colPI.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPI.HeaderText = "PI";
            this.colPI.Name = "colPI";
            this.colPI.ReadOnly = true;
            this.colPI.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPI.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPI.Width = 30;
            // 
            // colData
            // 
            this.colData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colData.DataPropertyName = "DataBytes";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colData.DefaultCellStyle = dataGridViewCellStyle7;
            this.colData.HeaderText = "DATA";
            this.colData.Name = "colData";
            this.colData.ReadOnly = true;
            this.colData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbMessages);
            this.splitContainer1.Size = new System.Drawing.Size(948, 387);
            this.splitContainer1.SplitterDistance = 88;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(948, 88);
            this.splitContainer2.SplitterDistance = 259;
            this.splitContainer2.TabIndex = 13;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.gbFilters);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.gbColumns);
            this.splitContainer3.Size = new System.Drawing.Size(259, 88);
            this.splitContainer3.SplitterDistance = 41;
            this.splitContainer3.TabIndex = 0;
            // 
            // gbFilters
            // 
            this.gbFilters.Controls.Add(this.chkReceive);
            this.gbFilters.Controls.Add(this.chkSend);
            this.gbFilters.Controls.Add(this.chkPings);
            this.gbFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilters.Location = new System.Drawing.Point(0, 0);
            this.gbFilters.Name = "gbFilters";
            this.gbFilters.Size = new System.Drawing.Size(259, 41);
            this.gbFilters.TabIndex = 0;
            this.gbFilters.TabStop = false;
            this.gbFilters.Text = "Show";
            // 
            // chkReceive
            // 
            this.chkReceive.AutoSize = true;
            this.chkReceive.Location = new System.Drawing.Point(6, 19);
            this.chkReceive.Name = "chkReceive";
            this.chkReceive.Size = new System.Drawing.Size(55, 17);
            this.chkReceive.TabIndex = 0;
            this.chkReceive.Text = "RECV";
            this.chkReceive.UseVisualStyleBackColor = true;
            this.chkReceive.CheckedChanged += new System.EventHandler(this.OnMessageFilterCheckedChanged);
            // 
            // chkSend
            // 
            this.chkSend.AutoSize = true;
            this.chkSend.Location = new System.Drawing.Point(67, 19);
            this.chkSend.Name = "chkSend";
            this.chkSend.Size = new System.Drawing.Size(56, 17);
            this.chkSend.TabIndex = 1;
            this.chkSend.Text = "SEND";
            this.chkSend.UseVisualStyleBackColor = true;
            this.chkSend.CheckedChanged += new System.EventHandler(this.OnMessageFilterCheckedChanged);
            // 
            // chkPings
            // 
            this.chkPings.AutoSize = true;
            this.chkPings.Location = new System.Drawing.Point(129, 19);
            this.chkPings.Name = "chkPings";
            this.chkPings.Size = new System.Drawing.Size(59, 17);
            this.chkPings.TabIndex = 2;
            this.chkPings.Text = "PINGS";
            this.chkPings.UseVisualStyleBackColor = true;
            this.chkPings.CheckedChanged += new System.EventHandler(this.OnMessageFilterCheckedChanged);
            // 
            // gbColumns
            // 
            this.gbColumns.Controls.Add(this.chkLength);
            this.gbColumns.Controls.Add(this.chkData);
            this.gbColumns.Controls.Add(this.chkCRC);
            this.gbColumns.Controls.Add(this.chkPI);
            this.gbColumns.Controls.Add(this.chkServerSave);
            this.gbColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbColumns.Location = new System.Drawing.Point(0, 0);
            this.gbColumns.Name = "gbColumns";
            this.gbColumns.Size = new System.Drawing.Size(259, 43);
            this.gbColumns.TabIndex = 0;
            this.gbColumns.TabStop = false;
            this.gbColumns.Text = "Columns";
            // 
            // chkLength
            // 
            this.chkLength.AutoSize = true;
            this.chkLength.Checked = true;
            this.chkLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLength.Location = new System.Drawing.Point(6, 19);
            this.chkLength.Name = "chkLength";
            this.chkLength.Size = new System.Drawing.Size(47, 17);
            this.chkLength.TabIndex = 3;
            this.chkLength.Text = "LEN";
            this.chkLength.UseVisualStyleBackColor = true;
            this.chkLength.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkData
            // 
            this.chkData.AutoSize = true;
            this.chkData.Checked = true;
            this.chkData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkData.Location = new System.Drawing.Point(201, 19);
            this.chkData.Name = "chkData";
            this.chkData.Size = new System.Drawing.Size(55, 17);
            this.chkData.TabIndex = 7;
            this.chkData.Text = "DATA";
            this.chkData.UseVisualStyleBackColor = true;
            this.chkData.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkCRC
            // 
            this.chkCRC.AutoSize = true;
            this.chkCRC.Checked = true;
            this.chkCRC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCRC.Location = new System.Drawing.Point(59, 19);
            this.chkCRC.Name = "chkCRC";
            this.chkCRC.Size = new System.Drawing.Size(48, 17);
            this.chkCRC.TabIndex = 4;
            this.chkCRC.Text = "CRC";
            this.chkCRC.UseVisualStyleBackColor = true;
            this.chkCRC.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkPI
            // 
            this.chkPI.AutoSize = true;
            this.chkPI.Checked = true;
            this.chkPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPI.Location = new System.Drawing.Point(159, 19);
            this.chkPI.Name = "chkPI";
            this.chkPI.Size = new System.Drawing.Size(36, 17);
            this.chkPI.TabIndex = 6;
            this.chkPI.Text = "PI";
            this.chkPI.UseVisualStyleBackColor = true;
            this.chkPI.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkServerSave
            // 
            this.chkServerSave.AutoSize = true;
            this.chkServerSave.Checked = true;
            this.chkServerSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkServerSave.Location = new System.Drawing.Point(113, 19);
            this.chkServerSave.Name = "chkServerSave";
            this.chkServerSave.Size = new System.Drawing.Size(40, 17);
            this.chkServerSave.TabIndex = 5;
            this.chkServerSave.Text = "SS";
            this.chkServerSave.UseVisualStyleBackColor = true;
            this.chkServerSave.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.gbOther);
            this.splitContainer4.Size = new System.Drawing.Size(685, 88);
            this.splitContainer4.SplitterDistance = 500;
            this.splitContainer4.TabIndex = 14;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.gbCustomMessage);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.gbRequests);
            this.splitContainer5.Size = new System.Drawing.Size(500, 88);
            this.splitContainer5.SplitterDistance = 42;
            this.splitContainer5.TabIndex = 0;
            // 
            // gbCustomMessage
            // 
            this.gbCustomMessage.Controls.Add(this.splitContainer6);
            this.gbCustomMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCustomMessage.Location = new System.Drawing.Point(0, 0);
            this.gbCustomMessage.Name = "gbCustomMessage";
            this.gbCustomMessage.Size = new System.Drawing.Size(500, 42);
            this.gbCustomMessage.TabIndex = 0;
            this.gbCustomMessage.TabStop = false;
            this.gbCustomMessage.Text = "Custom Message";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer6.Location = new System.Drawing.Point(3, 16);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.txtMessageBody);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.btnSendCustom);
            this.splitContainer6.Size = new System.Drawing.Size(494, 23);
            this.splitContainer6.SplitterDistance = 391;
            this.splitContainer6.TabIndex = 0;
            // 
            // txtMessageBody
            // 
            this.txtMessageBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageBody.Location = new System.Drawing.Point(0, 0);
            this.txtMessageBody.Name = "txtMessageBody";
            this.txtMessageBody.Size = new System.Drawing.Size(391, 20);
            this.txtMessageBody.TabIndex = 9;
            // 
            // btnSendCustom
            // 
            this.btnSendCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendCustom.Location = new System.Drawing.Point(0, 0);
            this.btnSendCustom.Name = "btnSendCustom";
            this.btnSendCustom.Size = new System.Drawing.Size(99, 23);
            this.btnSendCustom.TabIndex = 10;
            this.btnSendCustom.Text = "Send";
            this.btnSendCustom.UseVisualStyleBackColor = true;
            this.btnSendCustom.Click += new System.EventHandler(this.OnSendCustomClick);
            // 
            // gbRequests
            // 
            this.gbRequests.Controls.Add(this.splitContainer7);
            this.gbRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRequests.Location = new System.Drawing.Point(0, 0);
            this.gbRequests.Name = "gbRequests";
            this.gbRequests.Size = new System.Drawing.Size(500, 42);
            this.gbRequests.TabIndex = 1;
            this.gbRequests.TabStop = false;
            this.gbRequests.Text = "Requests";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer7.IsSplitterFixed = true;
            this.splitContainer7.Location = new System.Drawing.Point(3, 16);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.cbRequests);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.btnSendRequest);
            this.splitContainer7.Size = new System.Drawing.Size(494, 23);
            this.splitContainer7.SplitterDistance = 391;
            this.splitContainer7.TabIndex = 0;
            // 
            // cbRequests
            // 
            this.cbRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRequests.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRequests.FormattingEnabled = true;
            this.cbRequests.Location = new System.Drawing.Point(0, 0);
            this.cbRequests.Name = "cbRequests";
            this.cbRequests.Size = new System.Drawing.Size(391, 21);
            this.cbRequests.TabIndex = 1;
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendRequest.Location = new System.Drawing.Point(0, 0);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(99, 23);
            this.btnSendRequest.TabIndex = 11;
            this.btnSendRequest.Text = "Send";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.OnSendRequestClick);
            // 
            // gbOther
            // 
            this.gbOther.Controls.Add(this.btnClear);
            this.gbOther.Controls.Add(this.chkAutoScroll);
            this.gbOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOther.Location = new System.Drawing.Point(0, 0);
            this.gbOther.Name = "gbOther";
            this.gbOther.Size = new System.Drawing.Size(181, 88);
            this.gbOther.TabIndex = 14;
            this.gbOther.TabStop = false;
            this.gbOther.Text = "Other";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(45, 48);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(98, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear log";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.OnClearClick);
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(45, 20);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(98, 17);
            this.chkAutoScroll.TabIndex = 8;
            this.chkAutoScroll.Text = "AUTOSCROLL";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // gbMessages
            // 
            this.gbMessages.Controls.Add(this.gridMessages);
            this.gbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMessages.Location = new System.Drawing.Point(0, 0);
            this.gbMessages.Name = "gbMessages";
            this.gbMessages.Size = new System.Drawing.Size(948, 295);
            this.gbMessages.TabIndex = 13;
            this.gbMessages.TabStop = false;
            this.gbMessages.Text = "Messages";
            // 
            // GameMessageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GameMessageView";
            this.Size = new System.Drawing.Size(948, 387);
            ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.gbFilters.ResumeLayout(false);
            this.gbFilters.PerformLayout();
            this.gbColumns.ResumeLayout(false);
            this.gbColumns.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.gbCustomMessage.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.gbRequests.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.gbOther.ResumeLayout(false);
            this.gbOther.PerformLayout();
            this.gbMessages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Generic.BaseGridView gridMessages;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private System.Windows.Forms.CheckBox chkData;
        private System.Windows.Forms.CheckBox chkPI;
        private System.Windows.Forms.CheckBox chkServerSave;
        private System.Windows.Forms.CheckBox chkCRC;
        private System.Windows.Forms.CheckBox chkLength;
        private System.Windows.Forms.CheckBox chkPings;
        private System.Windows.Forms.CheckBox chkSend;
        private System.Windows.Forms.CheckBox chkReceive;
        private System.Windows.Forms.Button btnSendCustom;
        private Generic.HexTextBox txtMessageBody;
        private DataGridColumns.InOutColumn colTrafficDirection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private DataGridColumns.HexColumn colLEN1;
        private DataGridColumns.HexColumn colCRC;
        private DataGridColumns.HexColumn colLEN2;
        private DataGridColumns.HexColumn colSS;
        private DataGridColumns.HexColumn colPI;
        private DataGridColumns.HexColumn colData;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox gbMessages;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox gbFilters;
        private System.Windows.Forms.GroupBox gbColumns;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.GroupBox gbCustomMessage;
        private System.Windows.Forms.GroupBox gbRequests;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.GroupBox gbOther;
        private System.Windows.Forms.ComboBox cbRequests;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.Button btnSendRequest;
    }
}
