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
            this.gridMessages = new Meridian59.AdminUI.Generic.BaseGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkReceive = new System.Windows.Forms.CheckBox();
            this.chkSend = new System.Windows.Forms.CheckBox();
            this.chkPings = new System.Windows.Forms.CheckBox();
            this.chkLength = new System.Windows.Forms.CheckBox();
            this.chkCRC = new System.Windows.Forms.CheckBox();
            this.chkServerSave = new System.Windows.Forms.CheckBox();
            this.chkPI = new System.Windows.Forms.CheckBox();
            this.chkData = new System.Windows.Forms.CheckBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
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
            this.gridMessages.Location = new System.Drawing.Point(0, 0);
            this.gridMessages.Name = "gridMessages";
            this.gridMessages.Size = new System.Drawing.Size(643, 210);
            this.gridMessages.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
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
            this.splitContainer1.SplitterDistance = 71;
            this.splitContainer1.TabIndex = 1;
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
            // 
            // chkLength
            // 
            this.chkLength.AutoSize = true;
            this.chkLength.Location = new System.Drawing.Point(211, 15);
            this.chkLength.Name = "chkLength";
            this.chkLength.Size = new System.Drawing.Size(47, 17);
            this.chkLength.TabIndex = 3;
            this.chkLength.Text = "LEN";
            this.chkLength.UseVisualStyleBackColor = true;
            // 
            // chkCRC
            // 
            this.chkCRC.AutoSize = true;
            this.chkCRC.Location = new System.Drawing.Point(264, 15);
            this.chkCRC.Name = "chkCRC";
            this.chkCRC.Size = new System.Drawing.Size(48, 17);
            this.chkCRC.TabIndex = 4;
            this.chkCRC.Text = "CRC";
            this.chkCRC.UseVisualStyleBackColor = true;
            // 
            // chkServerSave
            // 
            this.chkServerSave.AutoSize = true;
            this.chkServerSave.Location = new System.Drawing.Point(318, 15);
            this.chkServerSave.Name = "chkServerSave";
            this.chkServerSave.Size = new System.Drawing.Size(40, 17);
            this.chkServerSave.TabIndex = 5;
            this.chkServerSave.Text = "SS";
            this.chkServerSave.UseVisualStyleBackColor = true;
            // 
            // chkPI
            // 
            this.chkPI.AutoSize = true;
            this.chkPI.Location = new System.Drawing.Point(364, 15);
            this.chkPI.Name = "chkPI";
            this.chkPI.Size = new System.Drawing.Size(36, 17);
            this.chkPI.TabIndex = 6;
            this.chkPI.Text = "PI";
            this.chkPI.UseVisualStyleBackColor = true;
            // 
            // chkData
            // 
            this.chkData.AutoSize = true;
            this.chkData.Location = new System.Drawing.Point(406, 15);
            this.chkData.Name = "chkData";
            this.chkData.Size = new System.Drawing.Size(55, 17);
            this.chkData.TabIndex = 7;
            this.chkData.Text = "DATA";
            this.chkData.UseVisualStyleBackColor = true;
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(467, 15);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(98, 17);
            this.chkAutoScroll.TabIndex = 8;
            this.chkAutoScroll.Text = "AUTOSCROLL";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
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
    }
}
