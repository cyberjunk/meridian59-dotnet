namespace Meridian59.AdminUI.Viewers
{
    partial class DiplomacyInfoView
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
            this.gbDiplomacy = new System.Windows.Forms.GroupBox();
            this.lblYourAllies = new System.Windows.Forms.Label();
            this.lblDeclaredWarOnYou = new System.Windows.Forms.Label();
            this.lbYouDeclaredAlly = new System.Windows.Forms.ListBox();
            this.lblYourEnemies = new System.Windows.Forms.Label();
            this.lbYouDeclaredEnemy = new System.Windows.Forms.ListBox();
            this.lblDeclaredYouAlly = new System.Windows.Forms.Label();
            this.lbDeclaredYouAlly = new System.Windows.Forms.ListBox();
            this.lbDeclaredYouEnemy = new System.Windows.Forms.ListBox();
            this.gbGuilds = new System.Windows.Forms.GroupBox();
            this.gridGuilds = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbDiplomacy.SuspendLayout();
            this.gbGuilds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGuilds)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.gbDiplomacy);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbGuilds);
            this.splitContainer1.Size = new System.Drawing.Size(374, 552);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 0;
            // 
            // gbDiplomacy
            // 
            this.gbDiplomacy.Controls.Add(this.lblYourAllies);
            this.gbDiplomacy.Controls.Add(this.lblDeclaredWarOnYou);
            this.gbDiplomacy.Controls.Add(this.lbYouDeclaredAlly);
            this.gbDiplomacy.Controls.Add(this.lblYourEnemies);
            this.gbDiplomacy.Controls.Add(this.lbYouDeclaredEnemy);
            this.gbDiplomacy.Controls.Add(this.lblDeclaredYouAlly);
            this.gbDiplomacy.Controls.Add(this.lbDeclaredYouAlly);
            this.gbDiplomacy.Controls.Add(this.lbDeclaredYouEnemy);
            this.gbDiplomacy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDiplomacy.Location = new System.Drawing.Point(0, 0);
            this.gbDiplomacy.Name = "gbDiplomacy";
            this.gbDiplomacy.Size = new System.Drawing.Size(374, 175);
            this.gbDiplomacy.TabIndex = 8;
            this.gbDiplomacy.TabStop = false;
            this.gbDiplomacy.Text = "Diplomacy";
            // 
            // lblYourAllies
            // 
            this.lblYourAllies.AutoSize = true;
            this.lblYourAllies.Location = new System.Drawing.Point(10, 24);
            this.lblYourAllies.Name = "lblYourAllies";
            this.lblYourAllies.Size = new System.Drawing.Size(55, 13);
            this.lblYourAllies.TabIndex = 4;
            this.lblYourAllies.Text = "Your allies";
            // 
            // lblDeclaredWarOnYou
            // 
            this.lblDeclaredWarOnYou.AutoSize = true;
            this.lblDeclaredWarOnYou.Location = new System.Drawing.Point(274, 24);
            this.lblDeclaredWarOnYou.Name = "lblDeclaredWarOnYou";
            this.lblDeclaredWarOnYou.Size = new System.Drawing.Size(71, 13);
            this.lblDeclaredWarOnYou.TabIndex = 7;
            this.lblDeclaredWarOnYou.Text = "Hostile to you";
            // 
            // lbYouDeclaredAlly
            // 
            this.lbYouDeclaredAlly.FormattingEnabled = true;
            this.lbYouDeclaredAlly.Location = new System.Drawing.Point(13, 40);
            this.lbYouDeclaredAlly.Name = "lbYouDeclaredAlly";
            this.lbYouDeclaredAlly.Size = new System.Drawing.Size(82, 121);
            this.lbYouDeclaredAlly.TabIndex = 0;
            // 
            // lblYourEnemies
            // 
            this.lblYourEnemies.AutoSize = true;
            this.lblYourEnemies.Location = new System.Drawing.Point(98, 24);
            this.lblYourEnemies.Name = "lblYourEnemies";
            this.lblYourEnemies.Size = new System.Drawing.Size(71, 13);
            this.lblYourEnemies.TabIndex = 6;
            this.lblYourEnemies.Text = "Your enemies";
            // 
            // lbYouDeclaredEnemy
            // 
            this.lbYouDeclaredEnemy.FormattingEnabled = true;
            this.lbYouDeclaredEnemy.Location = new System.Drawing.Point(101, 40);
            this.lbYouDeclaredEnemy.Name = "lbYouDeclaredEnemy";
            this.lbYouDeclaredEnemy.Size = new System.Drawing.Size(82, 121);
            this.lbYouDeclaredEnemy.TabIndex = 1;
            // 
            // lblDeclaredYouAlly
            // 
            this.lblDeclaredYouAlly.AutoSize = true;
            this.lblDeclaredYouAlly.Location = new System.Drawing.Point(186, 24);
            this.lblDeclaredYouAlly.Name = "lblDeclaredYouAlly";
            this.lblDeclaredYouAlly.Size = new System.Drawing.Size(74, 13);
            this.lblDeclaredYouAlly.TabIndex = 5;
            this.lblDeclaredYouAlly.Text = "Allied with you";
            // 
            // lbDeclaredYouAlly
            // 
            this.lbDeclaredYouAlly.FormattingEnabled = true;
            this.lbDeclaredYouAlly.Location = new System.Drawing.Point(189, 40);
            this.lbDeclaredYouAlly.Name = "lbDeclaredYouAlly";
            this.lbDeclaredYouAlly.Size = new System.Drawing.Size(82, 121);
            this.lbDeclaredYouAlly.TabIndex = 2;
            // 
            // lbDeclaredYouEnemy
            // 
            this.lbDeclaredYouEnemy.FormattingEnabled = true;
            this.lbDeclaredYouEnemy.Location = new System.Drawing.Point(277, 40);
            this.lbDeclaredYouEnemy.Name = "lbDeclaredYouEnemy";
            this.lbDeclaredYouEnemy.Size = new System.Drawing.Size(82, 121);
            this.lbDeclaredYouEnemy.TabIndex = 3;
            // 
            // gbGuilds
            // 
            this.gbGuilds.Controls.Add(this.gridGuilds);
            this.gbGuilds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGuilds.Location = new System.Drawing.Point(0, 0);
            this.gbGuilds.Name = "gbGuilds";
            this.gbGuilds.Size = new System.Drawing.Size(374, 373);
            this.gbGuilds.TabIndex = 2;
            this.gbGuilds.TabStop = false;
            this.gbGuilds.Text = "Guilds";
            // 
            // gridGuilds
            // 
            this.gridGuilds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGuilds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName});
            this.gridGuilds.Location = new System.Drawing.Point(3, 16);
            this.gridGuilds.Name = "gridGuilds";
            this.gridGuilds.Size = new System.Drawing.Size(368, 354);
            this.gridGuilds.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // DiplomacyInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DiplomacyInfoView";
            this.Size = new System.Drawing.Size(374, 552);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbDiplomacy.ResumeLayout(false);
            this.gbDiplomacy.PerformLayout();
            this.gbGuilds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridGuilds)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Generic.BaseGridView gridGuilds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.ListBox lbDeclaredYouEnemy;
        private System.Windows.Forms.ListBox lbDeclaredYouAlly;
        private System.Windows.Forms.ListBox lbYouDeclaredEnemy;
        private System.Windows.Forms.ListBox lbYouDeclaredAlly;
        private System.Windows.Forms.Label lblDeclaredWarOnYou;
        private System.Windows.Forms.Label lblYourEnemies;
        private System.Windows.Forms.Label lblDeclaredYouAlly;
        private System.Windows.Forms.Label lblYourAllies;
        private System.Windows.Forms.GroupBox gbDiplomacy;
        private System.Windows.Forms.GroupBox gbGuilds;
    }
}
