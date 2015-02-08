namespace Meridian59.DebugUI.Viewers
{
    partial class StatsNumericView
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
            this.gridStats = new Meridian59.DebugUI.BaseGridView();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueRenderMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueRenderMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueMaximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridStats)).BeginInit();
            this.SuspendLayout();
            // 
            // gridStats
            // 
            this.gridStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colResourceID,
            this.colTag,
            this.colValueCurrent,
            this.colValueRenderMin,
            this.colValueRenderMax,
            this.colValueMaximum,
            this.colResourceName});
            this.gridStats.Location = new System.Drawing.Point(0, 0);
            this.gridStats.Name = "gridStats";
            this.gridStats.Size = new System.Drawing.Size(518, 178);
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
            // colTag
            // 
            this.colTag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTag.DataPropertyName = "Tag";
            this.colTag.HeaderText = "Tag";
            this.colTag.Name = "colTag";
            this.colTag.ReadOnly = true;
            this.colTag.Width = 40;
            // 
            // colValueCurrent
            // 
            this.colValueCurrent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colValueCurrent.DataPropertyName = "ValueCurrent";
            this.colValueCurrent.HeaderText = "VCur";
            this.colValueCurrent.Name = "colValueCurrent";
            this.colValueCurrent.ReadOnly = true;
            this.colValueCurrent.Width = 60;
            // 
            // colValueRenderMin
            // 
            this.colValueRenderMin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colValueRenderMin.DataPropertyName = "ValueRenderMin";
            this.colValueRenderMin.HeaderText = "VRMin";
            this.colValueRenderMin.Name = "colValueRenderMin";
            this.colValueRenderMin.ReadOnly = true;
            this.colValueRenderMin.Width = 60;
            // 
            // colValueRenderMax
            // 
            this.colValueRenderMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colValueRenderMax.DataPropertyName = "ValueRenderMax";
            this.colValueRenderMax.HeaderText = "VRMax";
            this.colValueRenderMax.Name = "colValueRenderMax";
            this.colValueRenderMax.ReadOnly = true;
            this.colValueRenderMax.Width = 60;
            // 
            // colValueMaximum
            // 
            this.colValueMaximum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colValueMaximum.DataPropertyName = "ValueMaximum";
            this.colValueMaximum.HeaderText = "VMax";
            this.colValueMaximum.Name = "colValueMaximum";
            this.colValueMaximum.ReadOnly = true;
            this.colValueMaximum.Width = 60;
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
            // StatsNumericView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridStats);
            this.Name = "StatsNumericView";
            this.Size = new System.Drawing.Size(518, 178);
            ((System.ComponentModel.ISupportInitialize)(this.gridStats)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGridView gridStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueRenderMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueRenderMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueMaximum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceName;
    }
}
