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
            this.colTrafficDirection = new Meridian59.AdminUI.CustomDataGridColumns.InOutColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLEN1 = new Meridian59.AdminUI.CustomDataGridColumns.HexColumn();
            this.colCRC = new Meridian59.AdminUI.CustomDataGridColumns.HexColumn();
            this.colLEN2 = new Meridian59.AdminUI.CustomDataGridColumns.HexColumn();
            this.colSS = new Meridian59.AdminUI.CustomDataGridColumns.HexColumn();
            this.colPI = new Meridian59.AdminUI.CustomDataGridColumns.HexColumn();
            this.colData = new Meridian59.AdminUI.CustomDataGridColumns.HexColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblMessageBody = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessageBody = new Meridian59.AdminUI.Generic.HexTextBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.chkData = new System.Windows.Forms.CheckBox();
            this.chkPI = new System.Windows.Forms.CheckBox();
            this.chkServerSave = new System.Windows.Forms.CheckBox();
            this.chkCRC = new System.Windows.Forms.CheckBox();
            this.chkLength = new System.Windows.Forms.CheckBox();
            this.chkPings = new System.Windows.Forms.CheckBox();
            this.chkSend = new System.Windows.Forms.CheckBox();
            this.chkReceive = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.gridMessages.Location = new System.Drawing.Point(0, 0);
            this.gridMessages.Name = "gridMessages";
            this.gridMessages.Size = new System.Drawing.Size(643, 211);
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
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnClear);
            this.splitContainer1.Panel1.Controls.Add(this.lblMessageBody);
            this.splitContainer1.Panel1.Controls.Add(this.btnSend);
            this.splitContainer1.Panel1.Controls.Add(this.txtMessageBody);
            this.splitContainer1.Panel1.Controls.Add(this.chkAutoScroll);
            this.splitContainer1.Panel1.Controls.Add(this.chkData);
            this.splitContainer1.Panel1.Controls.Add(this.chkPI);
            this.splitContainer1.Panel1.Controls.Add(this.chkServerSave);
            this.splitContainer1.Panel1.Controls.Add(this.chkCRC);
            this.splitContainer1.Panel1.Controls.Add(this.chkLength);
            this.splitContainer1.Panel1.Controls.Add(this.chkPings);
            this.splitContainer1.Panel1.Controls.Add(this.chkSend);
            this.splitContainer1.Panel1.Controls.Add(this.chkReceive);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridMessages);
            this.splitContainer1.Size = new System.Drawing.Size(643, 285);
            this.splitContainer1.SplitterDistance = 70;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 37);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(63, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear log";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.OnClearClick);
            // 
            // lblMessageBody
            // 
            this.lblMessageBody.AutoSize = true;
            this.lblMessageBody.Location = new System.Drawing.Point(139, 42);
            this.lblMessageBody.Name = "lblMessageBody";
            this.lblMessageBody.Size = new System.Drawing.Size(98, 13);
            this.lblMessageBody.TabIndex = 11;
            this.lblMessageBody.Text = "Send custom body:";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(565, 37);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(63, 23);
            this.btnSend.TabIndex = 10;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.OnSendClick);
            // 
            // txtMessageBody
            // 
            this.txtMessageBody.Location = new System.Drawing.Point(243, 39);
            this.txtMessageBody.Name = "txtMessageBody";
            this.txtMessageBody.Size = new System.Drawing.Size(316, 20);
            this.txtMessageBody.TabIndex = 9;
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(542, 15);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(98, 17);
            this.chkAutoScroll.TabIndex = 8;
            this.chkAutoScroll.Text = "AUTOSCROLL";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // chkData
            // 
            this.chkData.AutoSize = true;
            this.chkData.Checked = true;
            this.chkData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkData.Location = new System.Drawing.Point(438, 15);
            this.chkData.Name = "chkData";
            this.chkData.Size = new System.Drawing.Size(55, 17);
            this.chkData.TabIndex = 7;
            this.chkData.Text = "DATA";
            this.chkData.UseVisualStyleBackColor = true;
            this.chkData.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkPI
            // 
            this.chkPI.AutoSize = true;
            this.chkPI.Checked = true;
            this.chkPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPI.Location = new System.Drawing.Point(396, 15);
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
            this.chkServerSave.Location = new System.Drawing.Point(350, 15);
            this.chkServerSave.Name = "chkServerSave";
            this.chkServerSave.Size = new System.Drawing.Size(40, 17);
            this.chkServerSave.TabIndex = 5;
            this.chkServerSave.Text = "SS";
            this.chkServerSave.UseVisualStyleBackColor = true;
            this.chkServerSave.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkCRC
            // 
            this.chkCRC.AutoSize = true;
            this.chkCRC.Checked = true;
            this.chkCRC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCRC.Location = new System.Drawing.Point(296, 15);
            this.chkCRC.Name = "chkCRC";
            this.chkCRC.Size = new System.Drawing.Size(48, 17);
            this.chkCRC.TabIndex = 4;
            this.chkCRC.Text = "CRC";
            this.chkCRC.UseVisualStyleBackColor = true;
            this.chkCRC.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkLength
            // 
            this.chkLength.AutoSize = true;
            this.chkLength.Checked = true;
            this.chkLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLength.Location = new System.Drawing.Point(243, 15);
            this.chkLength.Name = "chkLength";
            this.chkLength.Size = new System.Drawing.Size(47, 17);
            this.chkLength.TabIndex = 3;
            this.chkLength.Text = "LEN";
            this.chkLength.UseVisualStyleBackColor = true;
            this.chkLength.CheckedChanged += new System.EventHandler(this.OnColumnCheckBoxCheckedChanged);
            // 
            // chkPings
            // 
            this.chkPings.AutoSize = true;
            this.chkPings.Location = new System.Drawing.Point(135, 15);
            this.chkPings.Name = "chkPings";
            this.chkPings.Size = new System.Drawing.Size(59, 17);
            this.chkPings.TabIndex = 2;
            this.chkPings.Text = "PINGS";
            this.chkPings.UseVisualStyleBackColor = true;
            this.chkPings.CheckedChanged += new System.EventHandler(this.OnMessageFilterCheckedChanged);
            // 
            // chkSend
            // 
            this.chkSend.AutoSize = true;
            this.chkSend.Location = new System.Drawing.Point(73, 15);
            this.chkSend.Name = "chkSend";
            this.chkSend.Size = new System.Drawing.Size(56, 17);
            this.chkSend.TabIndex = 1;
            this.chkSend.Text = "SEND";
            this.chkSend.UseVisualStyleBackColor = true;
            this.chkSend.CheckedChanged += new System.EventHandler(this.OnMessageFilterCheckedChanged);
            // 
            // chkReceive
            // 
            this.chkReceive.AutoSize = true;
            this.chkReceive.Location = new System.Drawing.Point(12, 15);
            this.chkReceive.Name = "chkReceive";
            this.chkReceive.Size = new System.Drawing.Size(55, 17);
            this.chkReceive.TabIndex = 0;
            this.chkReceive.Text = "RECV";
            this.chkReceive.UseVisualStyleBackColor = true;
            this.chkReceive.CheckedChanged += new System.EventHandler(this.OnMessageFilterCheckedChanged);
            // 
            // GameMessageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GameMessageView";
            this.Size = new System.Drawing.Size(643, 285);
            ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblMessageBody;
        private System.Windows.Forms.Button btnSend;
        private Generic.HexTextBox txtMessageBody;
        private CustomDataGridColumns.InOutColumn colTrafficDirection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private CustomDataGridColumns.HexColumn colLEN1;
        private CustomDataGridColumns.HexColumn colCRC;
        private CustomDataGridColumns.HexColumn colLEN2;
        private CustomDataGridColumns.HexColumn colSS;
        private CustomDataGridColumns.HexColumn colPI;
        private CustomDataGridColumns.HexColumn colData;
        private System.Windows.Forms.Button btnClear;
    }
}
