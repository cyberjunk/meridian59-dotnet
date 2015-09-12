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
            this.gridGuilds = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbYouDeclaredAlly = new System.Windows.Forms.ListBox();
            this.lbYouDeclaredEnemy = new System.Windows.Forms.ListBox();
            this.lbDeclaredYouAlly = new System.Windows.Forms.ListBox();
            this.lbDeclaredYouEnemy = new System.Windows.Forms.ListBox();
            this.lblYouDeclaredAlly = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.lblYouDeclaredAlly);
            this.splitContainer1.Panel1.Controls.Add(this.lbDeclaredYouEnemy);
            this.splitContainer1.Panel1.Controls.Add(this.lbDeclaredYouAlly);
            this.splitContainer1.Panel1.Controls.Add(this.lbYouDeclaredEnemy);
            this.splitContainer1.Panel1.Controls.Add(this.lbYouDeclaredAlly);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridGuilds);
            this.splitContainer1.Size = new System.Drawing.Size(374, 552);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridGuilds
            // 
            this.gridGuilds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGuilds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName});
            this.gridGuilds.Location = new System.Drawing.Point(0, 0);
            this.gridGuilds.Name = "gridGuilds";
            this.gridGuilds.Size = new System.Drawing.Size(374, 373);
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
            // lbYouDeclaredAlly
            // 
            this.lbYouDeclaredAlly.FormattingEnabled = true;
            this.lbYouDeclaredAlly.Location = new System.Drawing.Point(20, 26);
            this.lbYouDeclaredAlly.Name = "lbYouDeclaredAlly";
            this.lbYouDeclaredAlly.Size = new System.Drawing.Size(120, 56);
            this.lbYouDeclaredAlly.TabIndex = 0;
            // 
            // lbYouDeclaredEnemy
            // 
            this.lbYouDeclaredEnemy.FormattingEnabled = true;
            this.lbYouDeclaredEnemy.Location = new System.Drawing.Point(218, 26);
            this.lbYouDeclaredEnemy.Name = "lbYouDeclaredEnemy";
            this.lbYouDeclaredEnemy.Size = new System.Drawing.Size(120, 56);
            this.lbYouDeclaredEnemy.TabIndex = 1;
            // 
            // lbDeclaredYouAlly
            // 
            this.lbDeclaredYouAlly.FormattingEnabled = true;
            this.lbDeclaredYouAlly.Location = new System.Drawing.Point(20, 106);
            this.lbDeclaredYouAlly.Name = "lbDeclaredYouAlly";
            this.lbDeclaredYouAlly.Size = new System.Drawing.Size(120, 56);
            this.lbDeclaredYouAlly.TabIndex = 2;
            // 
            // lbDeclaredYouEnemy
            // 
            this.lbDeclaredYouEnemy.FormattingEnabled = true;
            this.lbDeclaredYouEnemy.Location = new System.Drawing.Point(218, 106);
            this.lbDeclaredYouEnemy.Name = "lbDeclaredYouEnemy";
            this.lbDeclaredYouEnemy.Size = new System.Drawing.Size(120, 56);
            this.lbDeclaredYouEnemy.TabIndex = 3;
            // 
            // lblYouDeclaredAlly
            // 
            this.lblYouDeclaredAlly.AutoSize = true;
            this.lblYouDeclaredAlly.Location = new System.Drawing.Point(17, 10);
            this.lblYouDeclaredAlly.Name = "lblYouDeclaredAlly";
            this.lblYouDeclaredAlly.Size = new System.Drawing.Size(55, 13);
            this.lblYouDeclaredAlly.TabIndex = 4;
            this.lblYouDeclaredAlly.Text = "Your allies";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Allied with you";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Your enemies";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Hostile to you";
            // 
            // DiplomacyInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DiplomacyInfoView";
            this.Size = new System.Drawing.Size(374, 552);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblYouDeclaredAlly;
    }
}
