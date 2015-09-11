namespace Meridian59.AdminUI
{
    partial class SubOverlayGrid
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
            this.gridSubOverlays = new Meridian59.AdminUI.BaseGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHotspot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorTranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnimation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubOverlays)).BeginInit();
            this.SuspendLayout();
            // 
            // gridSubOverlays
            // 
            this.gridSubOverlays.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSubOverlays.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRID,
            this.colHotspot,
            this.colColorTranslation,
            this.colEffect,
            this.colAnimation,
            this.colRName});
            this.gridSubOverlays.Location = new System.Drawing.Point(0, 0);
            this.gridSubOverlays.Name = "gridSubOverlays";
            this.gridSubOverlays.Size = new System.Drawing.Size(371, 165);
            this.gridSubOverlays.TabIndex = 0;
            this.gridSubOverlays.SelectionChanged += new System.EventHandler(this.gridSubOverlays_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.HeaderText = "RID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "HS";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "CT";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.HeaderText = "E";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.HeaderText = "Anim";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "RName";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // colRID
            // 
            this.colRID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRID.DataPropertyName = "ResourceID";
            this.colRID.HeaderText = "RID";
            this.colRID.Name = "colRID";
            this.colRID.ReadOnly = true;
            this.colRID.Width = 50;
            // 
            // colHotspot
            // 
            this.colHotspot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colHotspot.DataPropertyName = "HotSpot";
            this.colHotspot.HeaderText = "HS";
            this.colHotspot.Name = "colHotspot";
            this.colHotspot.ReadOnly = true;
            this.colHotspot.Width = 50;
            // 
            // colColorTranslation
            // 
            this.colColorTranslation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colColorTranslation.DataPropertyName = "ColorTranslation";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.colColorTranslation.DefaultCellStyle = dataGridViewCellStyle1;
            this.colColorTranslation.HeaderText = "CT";
            this.colColorTranslation.Name = "colColorTranslation";
            this.colColorTranslation.ReadOnly = true;
            this.colColorTranslation.Width = 50;
            // 
            // colEffect
            // 
            this.colEffect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEffect.DataPropertyName = "Effect";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.colEffect.DefaultCellStyle = dataGridViewCellStyle2;
            this.colEffect.HeaderText = "E";
            this.colEffect.Name = "colEffect";
            this.colEffect.ReadOnly = true;
            this.colEffect.Width = 50;
            // 
            // colAnimation
            // 
            this.colAnimation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAnimation.DataPropertyName = "Animation";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            this.colAnimation.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAnimation.HeaderText = "ANIM";
            this.colAnimation.Name = "colAnimation";
            this.colAnimation.ReadOnly = true;
            this.colAnimation.Width = 50;
            // 
            // colRName
            // 
            this.colRName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRName.DataPropertyName = "Name";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Wheat;
            this.colRName.DefaultCellStyle = dataGridViewCellStyle4;
            this.colRName.HeaderText = "OVFILE";
            this.colRName.Name = "colRName";
            this.colRName.ReadOnly = true;
            // 
            // SubOverlayGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridSubOverlays);
            this.Name = "SubOverlayGrid";
            this.Size = new System.Drawing.Size(371, 165);
            ((System.ComponentModel.ISupportInitialize)(this.gridSubOverlays)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private BaseGridView gridSubOverlays;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHotspot;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorTranslation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnimation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRName;
    }
}
