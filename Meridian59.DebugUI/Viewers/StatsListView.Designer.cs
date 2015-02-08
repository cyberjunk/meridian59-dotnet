namespace Meridian59.DebugUI.Viewers
{
    partial class StatsListView
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
            this.gridStats = new Meridian59.DebugUI.BaseGridView();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSkillPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIconResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResourceIconName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridStats)).BeginInit();
            this.SuspendLayout();
            // 
            // gridStats
            // 
            this.gridStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colResourceID,
            this.colID,
            this.colSkillPoints,
            this.colIconResourceID,
            this.colResourceName,
            this.colResourceIconName});
            this.gridStats.Location = new System.Drawing.Point(0, 0);
            this.gridStats.Name = "gridStats";
            this.gridStats.Size = new System.Drawing.Size(548, 265);
            this.gridStats.TabIndex = 0;
            // 
            // colNum
            // 
            this.colNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNum.DataPropertyName = "Num";
            this.colNum.HeaderText = "Num";
            this.colNum.Name = "colNum";
            this.colNum.ReadOnly = true;
            this.colNum.Width = 40;
            // 
            // colResourceID
            // 
            this.colResourceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colResourceID.DataPropertyName = "ResourceID";
            this.colResourceID.HeaderText = "RID";
            this.colResourceID.Name = "colResourceID";
            this.colResourceID.ReadOnly = true;
            this.colResourceID.Width = 60;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colID.DataPropertyName = "ObjectID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 60;
            // 
            // colSkillPoints
            // 
            this.colSkillPoints.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSkillPoints.DataPropertyName = "SkillPoints";
            this.colSkillPoints.HeaderText = "%";
            this.colSkillPoints.Name = "colSkillPoints";
            this.colSkillPoints.ReadOnly = true;
            this.colSkillPoints.Width = 40;
            // 
            // colIconResourceID
            // 
            this.colIconResourceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colIconResourceID.DataPropertyName = "ResourceIconID";
            this.colIconResourceID.HeaderText = "IRID";
            this.colIconResourceID.Name = "colIconResourceID";
            this.colIconResourceID.ReadOnly = true;
            this.colIconResourceID.Width = 60;
            // 
            // colResourceName
            // 
            this.colResourceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colResourceName.DataPropertyName = "ResourceName";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Wheat;
            this.colResourceName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colResourceName.HeaderText = "ResourceName";
            this.colResourceName.Name = "colResourceName";
            this.colResourceName.ReadOnly = true;
            // 
            // colResourceIconName
            // 
            this.colResourceIconName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colResourceIconName.DataPropertyName = "ResourceIconName";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Wheat;
            this.colResourceIconName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colResourceIconName.HeaderText = "IResourceName";
            this.colResourceIconName.Name = "colResourceIconName";
            this.colResourceIconName.ReadOnly = true;
            // 
            // StatsListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridStats);
            this.Name = "StatsListView";
            this.Size = new System.Drawing.Size(548, 265);
            ((System.ComponentModel.ISupportInitialize)(this.gridStats)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGridView gridStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSkillPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIconResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceIconName;
    }
}
