namespace Meridian59.AdminUI.Viewers
{
    partial class GuildInfoView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridMembers = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblGuildNameDesc = new System.Windows.Forms.Label();
            this.lblGuildName = new System.Windows.Forms.Label();
            this.lblGuildID = new System.Windows.Forms.Label();
            this.lblGuildIDDesc = new System.Windows.Forms.Label();
            this.lblSupportedMember = new System.Windows.Forms.Label();
            this.lblSupportedMemberDesc = new System.Windows.Forms.Label();
            this.lblFlagsDesc = new System.Windows.Forms.Label();
            this.lblFlags = new System.Windows.Forms.Label();
            this.lblMaleRanks = new System.Windows.Forms.Label();
            this.lblFemaleRanks = new System.Windows.Forms.Label();
            this.lblMaleRank1 = new System.Windows.Forms.Label();
            this.lblMaleRank2 = new System.Windows.Forms.Label();
            this.lblMaleRank3 = new System.Windows.Forms.Label();
            this.lblMaleRank4 = new System.Windows.Forms.Label();
            this.lblMaleRank5 = new System.Windows.Forms.Label();
            this.lblFemaleRank5 = new System.Windows.Forms.Label();
            this.lblFemaleRank4 = new System.Windows.Forms.Label();
            this.lblFemaleRank3 = new System.Windows.Forms.Label();
            this.lblFemaleRank2 = new System.Windows.Forms.Label();
            this.lblFemaleRank1 = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPasswordDesc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMembers)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.lblPassword);
            this.splitContainer1.Panel1.Controls.Add(this.lblPasswordDesc);
            this.splitContainer1.Panel1.Controls.Add(this.lblFemaleRank5);
            this.splitContainer1.Panel1.Controls.Add(this.lblFemaleRank4);
            this.splitContainer1.Panel1.Controls.Add(this.lblFemaleRank3);
            this.splitContainer1.Panel1.Controls.Add(this.lblFemaleRank2);
            this.splitContainer1.Panel1.Controls.Add(this.lblFemaleRank1);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaleRank5);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaleRank4);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaleRank3);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaleRank2);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaleRank1);
            this.splitContainer1.Panel1.Controls.Add(this.lblFemaleRanks);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaleRanks);
            this.splitContainer1.Panel1.Controls.Add(this.lblFlags);
            this.splitContainer1.Panel1.Controls.Add(this.lblFlagsDesc);
            this.splitContainer1.Panel1.Controls.Add(this.lblSupportedMember);
            this.splitContainer1.Panel1.Controls.Add(this.lblSupportedMemberDesc);
            this.splitContainer1.Panel1.Controls.Add(this.lblGuildID);
            this.splitContainer1.Panel1.Controls.Add(this.lblGuildIDDesc);
            this.splitContainer1.Panel1.Controls.Add(this.lblGuildName);
            this.splitContainer1.Panel1.Controls.Add(this.lblGuildNameDesc);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridMembers);
            this.splitContainer1.Size = new System.Drawing.Size(374, 552);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridMembers
            // 
            this.gridMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMembers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName,
            this.colRank,
            this.colGender});
            this.gridMembers.Location = new System.Drawing.Point(0, 0);
            this.gridMembers.Name = "gridMembers";
            this.gridMembers.Size = new System.Drawing.Size(374, 373);
            this.gridMembers.TabIndex = 0;
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
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colRank
            // 
            this.colRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRank.DataPropertyName = "Rank";
            this.colRank.HeaderText = "RANK";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            this.colRank.Width = 70;
            // 
            // colGender
            // 
            this.colGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colGender.DataPropertyName = "Gender";
            this.colGender.HeaderText = "GENDER";
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            this.colGender.Width = 70;
            // 
            // lblGuildNameDesc
            // 
            this.lblGuildNameDesc.AutoSize = true;
            this.lblGuildNameDesc.Location = new System.Drawing.Point(23, 46);
            this.lblGuildNameDesc.Name = "lblGuildNameDesc";
            this.lblGuildNameDesc.Size = new System.Drawing.Size(59, 13);
            this.lblGuildNameDesc.TabIndex = 0;
            this.lblGuildNameDesc.Text = "GuildName";
            // 
            // lblGuildName
            // 
            this.lblGuildName.AutoSize = true;
            this.lblGuildName.Location = new System.Drawing.Point(88, 46);
            this.lblGuildName.Name = "lblGuildName";
            this.lblGuildName.Size = new System.Drawing.Size(10, 13);
            this.lblGuildName.TabIndex = 1;
            this.lblGuildName.Text = "-";
            // 
            // lblGuildID
            // 
            this.lblGuildID.AutoSize = true;
            this.lblGuildID.Location = new System.Drawing.Point(88, 69);
            this.lblGuildID.Name = "lblGuildID";
            this.lblGuildID.Size = new System.Drawing.Size(10, 13);
            this.lblGuildID.TabIndex = 3;
            this.lblGuildID.Text = "-";
            // 
            // lblGuildIDDesc
            // 
            this.lblGuildIDDesc.AutoSize = true;
            this.lblGuildIDDesc.Location = new System.Drawing.Point(23, 69);
            this.lblGuildIDDesc.Name = "lblGuildIDDesc";
            this.lblGuildIDDesc.Size = new System.Drawing.Size(42, 13);
            this.lblGuildIDDesc.TabIndex = 2;
            this.lblGuildIDDesc.Text = "GuildID";
            // 
            // lblSupportedMember
            // 
            this.lblSupportedMember.AutoSize = true;
            this.lblSupportedMember.Location = new System.Drawing.Point(88, 91);
            this.lblSupportedMember.Name = "lblSupportedMember";
            this.lblSupportedMember.Size = new System.Drawing.Size(10, 13);
            this.lblSupportedMember.TabIndex = 5;
            this.lblSupportedMember.Text = "-";
            // 
            // lblSupportedMemberDesc
            // 
            this.lblSupportedMemberDesc.AutoSize = true;
            this.lblSupportedMemberDesc.Location = new System.Drawing.Point(23, 91);
            this.lblSupportedMemberDesc.Name = "lblSupportedMemberDesc";
            this.lblSupportedMemberDesc.Size = new System.Drawing.Size(58, 13);
            this.lblSupportedMemberDesc.TabIndex = 4;
            this.lblSupportedMemberDesc.Text = "Supporting";
            // 
            // lblFlagsDesc
            // 
            this.lblFlagsDesc.AutoSize = true;
            this.lblFlagsDesc.Location = new System.Drawing.Point(23, 113);
            this.lblFlagsDesc.Name = "lblFlagsDesc";
            this.lblFlagsDesc.Size = new System.Drawing.Size(32, 13);
            this.lblFlagsDesc.TabIndex = 6;
            this.lblFlagsDesc.Text = "Flags";
            // 
            // lblFlags
            // 
            this.lblFlags.AutoSize = true;
            this.lblFlags.Location = new System.Drawing.Point(88, 113);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(10, 13);
            this.lblFlags.TabIndex = 7;
            this.lblFlags.Text = "-";
            // 
            // lblMaleRanks
            // 
            this.lblMaleRanks.AutoSize = true;
            this.lblMaleRanks.Location = new System.Drawing.Point(198, 23);
            this.lblMaleRanks.Name = "lblMaleRanks";
            this.lblMaleRanks.Size = new System.Drawing.Size(64, 13);
            this.lblMaleRanks.TabIndex = 8;
            this.lblMaleRanks.Text = "Male Ranks";
            // 
            // lblFemaleRanks
            // 
            this.lblFemaleRanks.AutoSize = true;
            this.lblFemaleRanks.Location = new System.Drawing.Point(290, 23);
            this.lblFemaleRanks.Name = "lblFemaleRanks";
            this.lblFemaleRanks.Size = new System.Drawing.Size(75, 13);
            this.lblFemaleRanks.TabIndex = 9;
            this.lblFemaleRanks.Text = "Female Ranks";
            // 
            // lblMaleRank1
            // 
            this.lblMaleRank1.AutoSize = true;
            this.lblMaleRank1.Location = new System.Drawing.Point(198, 46);
            this.lblMaleRank1.Name = "lblMaleRank1";
            this.lblMaleRank1.Size = new System.Drawing.Size(10, 13);
            this.lblMaleRank1.TabIndex = 10;
            this.lblMaleRank1.Text = "-";
            // 
            // lblMaleRank2
            // 
            this.lblMaleRank2.AutoSize = true;
            this.lblMaleRank2.Location = new System.Drawing.Point(198, 68);
            this.lblMaleRank2.Name = "lblMaleRank2";
            this.lblMaleRank2.Size = new System.Drawing.Size(10, 13);
            this.lblMaleRank2.TabIndex = 11;
            this.lblMaleRank2.Text = "-";
            // 
            // lblMaleRank3
            // 
            this.lblMaleRank3.AutoSize = true;
            this.lblMaleRank3.Location = new System.Drawing.Point(198, 90);
            this.lblMaleRank3.Name = "lblMaleRank3";
            this.lblMaleRank3.Size = new System.Drawing.Size(10, 13);
            this.lblMaleRank3.TabIndex = 12;
            this.lblMaleRank3.Text = "-";
            // 
            // lblMaleRank4
            // 
            this.lblMaleRank4.AutoSize = true;
            this.lblMaleRank4.Location = new System.Drawing.Point(198, 112);
            this.lblMaleRank4.Name = "lblMaleRank4";
            this.lblMaleRank4.Size = new System.Drawing.Size(10, 13);
            this.lblMaleRank4.TabIndex = 13;
            this.lblMaleRank4.Text = "-";
            // 
            // lblMaleRank5
            // 
            this.lblMaleRank5.AutoSize = true;
            this.lblMaleRank5.Location = new System.Drawing.Point(198, 134);
            this.lblMaleRank5.Name = "lblMaleRank5";
            this.lblMaleRank5.Size = new System.Drawing.Size(10, 13);
            this.lblMaleRank5.TabIndex = 14;
            this.lblMaleRank5.Text = "-";
            // 
            // lblFemaleRank5
            // 
            this.lblFemaleRank5.AutoSize = true;
            this.lblFemaleRank5.Location = new System.Drawing.Point(290, 134);
            this.lblFemaleRank5.Name = "lblFemaleRank5";
            this.lblFemaleRank5.Size = new System.Drawing.Size(10, 13);
            this.lblFemaleRank5.TabIndex = 19;
            this.lblFemaleRank5.Text = "-";
            // 
            // lblFemaleRank4
            // 
            this.lblFemaleRank4.AutoSize = true;
            this.lblFemaleRank4.Location = new System.Drawing.Point(290, 112);
            this.lblFemaleRank4.Name = "lblFemaleRank4";
            this.lblFemaleRank4.Size = new System.Drawing.Size(10, 13);
            this.lblFemaleRank4.TabIndex = 18;
            this.lblFemaleRank4.Text = "-";
            // 
            // lblFemaleRank3
            // 
            this.lblFemaleRank3.AutoSize = true;
            this.lblFemaleRank3.Location = new System.Drawing.Point(290, 90);
            this.lblFemaleRank3.Name = "lblFemaleRank3";
            this.lblFemaleRank3.Size = new System.Drawing.Size(10, 13);
            this.lblFemaleRank3.TabIndex = 17;
            this.lblFemaleRank3.Text = "-";
            // 
            // lblFemaleRank2
            // 
            this.lblFemaleRank2.AutoSize = true;
            this.lblFemaleRank2.Location = new System.Drawing.Point(290, 68);
            this.lblFemaleRank2.Name = "lblFemaleRank2";
            this.lblFemaleRank2.Size = new System.Drawing.Size(10, 13);
            this.lblFemaleRank2.TabIndex = 16;
            this.lblFemaleRank2.Text = "-";
            // 
            // lblFemaleRank1
            // 
            this.lblFemaleRank1.AutoSize = true;
            this.lblFemaleRank1.Location = new System.Drawing.Point(290, 46);
            this.lblFemaleRank1.Name = "lblFemaleRank1";
            this.lblFemaleRank1.Size = new System.Drawing.Size(10, 13);
            this.lblFemaleRank1.TabIndex = 15;
            this.lblFemaleRank1.Text = "-";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(88, 134);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(10, 13);
            this.lblPassword.TabIndex = 21;
            this.lblPassword.Text = "-";
            // 
            // lblPasswordDesc
            // 
            this.lblPasswordDesc.AutoSize = true;
            this.lblPasswordDesc.Location = new System.Drawing.Point(23, 134);
            this.lblPasswordDesc.Name = "lblPasswordDesc";
            this.lblPasswordDesc.Size = new System.Drawing.Size(53, 13);
            this.lblPasswordDesc.TabIndex = 20;
            this.lblPasswordDesc.Text = "Password";
            // 
            // GuildInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GuildInfoView";
            this.Size = new System.Drawing.Size(374, 552);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMembers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Generic.BaseGridView gridMembers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.Label lblGuildName;
        private System.Windows.Forms.Label lblGuildNameDesc;
        private System.Windows.Forms.Label lblGuildID;
        private System.Windows.Forms.Label lblGuildIDDesc;
        private System.Windows.Forms.Label lblSupportedMember;
        private System.Windows.Forms.Label lblSupportedMemberDesc;
        private System.Windows.Forms.Label lblFlags;
        private System.Windows.Forms.Label lblFlagsDesc;
        private System.Windows.Forms.Label lblMaleRank1;
        private System.Windows.Forms.Label lblFemaleRanks;
        private System.Windows.Forms.Label lblMaleRanks;
        private System.Windows.Forms.Label lblMaleRank4;
        private System.Windows.Forms.Label lblMaleRank3;
        private System.Windows.Forms.Label lblMaleRank2;
        private System.Windows.Forms.Label lblFemaleRank5;
        private System.Windows.Forms.Label lblFemaleRank4;
        private System.Windows.Forms.Label lblFemaleRank3;
        private System.Windows.Forms.Label lblFemaleRank2;
        private System.Windows.Forms.Label lblFemaleRank1;
        private System.Windows.Forms.Label lblMaleRank5;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblPasswordDesc;
    }
}
