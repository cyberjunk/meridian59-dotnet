namespace Meridian59.AdminUI.Viewers
{
    partial class BackgroundOverlayView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBackgroundOverlays = new System.Windows.Forms.GroupBox();
            this.gridObjects = new Meridian59.AdminUI.Generic.BaseGridView();
            this.groupAnimation = new System.Windows.Forms.GroupBox();
            this.avAnimation = new Meridian59.AdminUI.AnimationView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverlayRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorTranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnimation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAngle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverlayFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBackgroundOverlays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).BeginInit();
            this.groupAnimation.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBackgroundOverlays);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupAnimation);
            this.splitContainer1.Size = new System.Drawing.Size(803, 102);
            this.splitContainer1.SplitterDistance = 650;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBackgroundOverlays
            // 
            this.groupBackgroundOverlays.Controls.Add(this.gridObjects);
            this.groupBackgroundOverlays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBackgroundOverlays.Location = new System.Drawing.Point(0, 0);
            this.groupBackgroundOverlays.Name = "groupBackgroundOverlays";
            this.groupBackgroundOverlays.Size = new System.Drawing.Size(650, 102);
            this.groupBackgroundOverlays.TabIndex = 1;
            this.groupBackgroundOverlays.TabStop = false;
            this.groupBackgroundOverlays.Text = "BackgroundOverlays";
            // 
            // gridObjects
            // 
            this.gridObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colCount,
            this.colOverlayRID,
            this.colNameRID,
            this.colColorTranslation,
            this.colEffect,
            this.colAnimation,
            this.colAngle,
            this.colHeight,
            this.colName,
            this.colOverlayFile});
            this.gridObjects.Location = new System.Drawing.Point(3, 16);
            this.gridObjects.Name = "gridObjects";
            this.gridObjects.Size = new System.Drawing.Size(644, 83);
            this.gridObjects.TabIndex = 0;
            this.gridObjects.SelectionChanged += new System.EventHandler(this.OnGridObjectsSelectionChanged);
            // 
            // groupAnimation
            // 
            this.groupAnimation.Controls.Add(this.avAnimation);
            this.groupAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnimation.Location = new System.Drawing.Point(0, 0);
            this.groupAnimation.Name = "groupAnimation";
            this.groupAnimation.Size = new System.Drawing.Size(149, 102);
            this.groupAnimation.TabIndex = 1;
            this.groupAnimation.TabStop = false;
            this.groupAnimation.Text = "Animation";
            // 
            // avAnimation
            // 
            this.avAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avAnimation.Location = new System.Drawing.Point(3, 16);
            this.avAnimation.Name = "avAnimation";
            this.avAnimation.Size = new System.Drawing.Size(143, 83);
            this.avAnimation.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 60;
            // 
            // colCount
            // 
            this.colCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCount.DataPropertyName = "Count";
            this.colCount.HeaderText = "COUNT";
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.Width = 60;
            // 
            // colOverlayRID
            // 
            this.colOverlayRID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colOverlayRID.DataPropertyName = "OverlayFileRID";
            this.colOverlayRID.HeaderText = "OVRID";
            this.colOverlayRID.Name = "colOverlayRID";
            this.colOverlayRID.ReadOnly = true;
            this.colOverlayRID.Width = 60;
            // 
            // colNameRID
            // 
            this.colNameRID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNameRID.DataPropertyName = "NameRID";
            this.colNameRID.HeaderText = "NRID";
            this.colNameRID.Name = "colNameRID";
            this.colNameRID.ReadOnly = true;
            this.colNameRID.Width = 60;
            // 
            // colColorTranslation
            // 
            this.colColorTranslation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colColorTranslation.DataPropertyName = "ColorTranslation";
            this.colColorTranslation.HeaderText = "CT";
            this.colColorTranslation.Name = "colColorTranslation";
            this.colColorTranslation.ReadOnly = true;
            this.colColorTranslation.Width = 35;
            // 
            // colEffect
            // 
            this.colEffect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEffect.DataPropertyName = "Effect";
            this.colEffect.HeaderText = "E";
            this.colEffect.Name = "colEffect";
            this.colEffect.ReadOnly = true;
            this.colEffect.Width = 35;
            // 
            // colAnimation
            // 
            this.colAnimation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAnimation.DataPropertyName = "Animation";
            this.colAnimation.HeaderText = "ANIM";
            this.colAnimation.Name = "colAnimation";
            this.colAnimation.ReadOnly = true;
            this.colAnimation.Width = 60;
            // 
            // colAngle
            // 
            this.colAngle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAngle.DataPropertyName = "Angle";
            this.colAngle.HeaderText = "ANGLE";
            this.colAngle.Name = "colAngle";
            this.colAngle.ReadOnly = true;
            this.colAngle.Width = 60;
            // 
            // colHeight
            // 
            this.colHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colHeight.DataPropertyName = "Height";
            this.colHeight.HeaderText = "HEIGHT";
            this.colHeight.Name = "colHeight";
            this.colHeight.ReadOnly = true;
            this.colHeight.Width = 60;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Wheat;
            this.colName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colOverlayFile
            // 
            this.colOverlayFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOverlayFile.DataPropertyName = "OverlayFile";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Wheat;
            this.colOverlayFile.DefaultCellStyle = dataGridViewCellStyle2;
            this.colOverlayFile.HeaderText = "OVFILE";
            this.colOverlayFile.Name = "colOverlayFile";
            this.colOverlayFile.ReadOnly = true;
            // 
            // BackgroundOverlayView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BackgroundOverlayView";
            this.Size = new System.Drawing.Size(803, 102);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBackgroundOverlays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).EndInit();
            this.groupAnimation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Meridian59.AdminUI.Generic.BaseGridView gridObjects;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBackgroundOverlays;
        private System.Windows.Forms.GroupBox groupAnimation;
        private AnimationView avAnimation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverlayRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorTranslation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnimation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAngle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverlayFile;
    }
}
