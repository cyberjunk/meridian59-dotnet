namespace Meridian59.AdminUI.Viewers
{
    partial class SpellsView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.groupSpells = new System.Windows.Forms.GroupBox();
            this.gridSpells = new Meridian59.AdminUI.Generic.BaseGridView();
            this.splitMain2 = new System.Windows.Forms.SplitContainer();
            this.groupSubOverlays = new System.Windows.Forms.GroupBox();
            this.gridSubOverlays = new Meridian59.AdminUI.SubOverlayGrid();
            this.splitAnimations = new System.Windows.Forms.SplitContainer();
            this.groupAnimation = new System.Windows.Forms.GroupBox();
            this.avAnimation = new Meridian59.AdminUI.AnimationView();
            this.groupSubOverlayAnimation = new System.Windows.Forms.GroupBox();
            this.avSubOverlayAnimation = new Meridian59.AdminUI.AnimationView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverlayRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLightFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLightIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLightColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorTranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnimation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchoolType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppearanceHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverlayFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.groupSpells.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpells)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain2)).BeginInit();
            this.splitMain2.Panel1.SuspendLayout();
            this.splitMain2.Panel2.SuspendLayout();
            this.splitMain2.SuspendLayout();
            this.groupSubOverlays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitAnimations)).BeginInit();
            this.splitAnimations.Panel1.SuspendLayout();
            this.splitAnimations.Panel2.SuspendLayout();
            this.splitAnimations.SuspendLayout();
            this.groupAnimation.SuspendLayout();
            this.groupSubOverlayAnimation.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.groupSpells);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitMain2);
            this.splitMain.Size = new System.Drawing.Size(815, 371);
            this.splitMain.SplitterDistance = 182;
            this.splitMain.TabIndex = 0;
            // 
            // groupSpells
            // 
            this.groupSpells.Controls.Add(this.gridSpells);
            this.groupSpells.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSpells.Location = new System.Drawing.Point(0, 0);
            this.groupSpells.Name = "groupSpells";
            this.groupSpells.Size = new System.Drawing.Size(815, 182);
            this.groupSpells.TabIndex = 0;
            this.groupSpells.TabStop = false;
            this.groupSpells.Text = "Spells";
            // 
            // gridSpells
            // 
            this.gridSpells.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSpells.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colCount,
            this.colOverlayRID,
            this.colNameRID,
            this.colFlags,
            this.colLightFlags,
            this.colLightIntensity,
            this.colLightColor,
            this.colColorTranslation,
            this.colEffect,
            this.colAnimation,
            this.colTargetsCount,
            this.colSchoolType,
            this.colAppearanceHash,
            this.colName,
            this.colOverlayFile});
            this.gridSpells.Location = new System.Drawing.Point(3, 16);
            this.gridSpells.Name = "gridSpells";
            this.gridSpells.Size = new System.Drawing.Size(809, 163);
            this.gridSpells.TabIndex = 0;
            this.gridSpells.SelectionChanged += new System.EventHandler(this.OnGridSpellsSelectionChanged);
            // 
            // splitMain2
            // 
            this.splitMain2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain2.Location = new System.Drawing.Point(0, 0);
            this.splitMain2.Name = "splitMain2";
            // 
            // splitMain2.Panel1
            // 
            this.splitMain2.Panel1.Controls.Add(this.groupSubOverlays);
            // 
            // splitMain2.Panel2
            // 
            this.splitMain2.Panel2.Controls.Add(this.splitAnimations);
            this.splitMain2.Size = new System.Drawing.Size(815, 185);
            this.splitMain2.SplitterDistance = 405;
            this.splitMain2.TabIndex = 0;
            // 
            // groupSubOverlays
            // 
            this.groupSubOverlays.Controls.Add(this.gridSubOverlays);
            this.groupSubOverlays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSubOverlays.Location = new System.Drawing.Point(0, 0);
            this.groupSubOverlays.Name = "groupSubOverlays";
            this.groupSubOverlays.Size = new System.Drawing.Size(405, 185);
            this.groupSubOverlays.TabIndex = 0;
            this.groupSubOverlays.TabStop = false;
            this.groupSubOverlays.Text = "SubOverlays";
            // 
            // gridSubOverlays
            // 
            this.gridSubOverlays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSubOverlays.Location = new System.Drawing.Point(3, 16);
            this.gridSubOverlays.Name = "gridSubOverlays";
            this.gridSubOverlays.Size = new System.Drawing.Size(399, 166);
            this.gridSubOverlays.TabIndex = 0;
            // 
            // splitAnimations
            // 
            this.splitAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitAnimations.Location = new System.Drawing.Point(0, 0);
            this.splitAnimations.Name = "splitAnimations";
            // 
            // splitAnimations.Panel1
            // 
            this.splitAnimations.Panel1.Controls.Add(this.groupAnimation);
            // 
            // splitAnimations.Panel2
            // 
            this.splitAnimations.Panel2.Controls.Add(this.groupSubOverlayAnimation);
            this.splitAnimations.Size = new System.Drawing.Size(406, 185);
            this.splitAnimations.SplitterDistance = 194;
            this.splitAnimations.TabIndex = 0;
            // 
            // groupAnimation
            // 
            this.groupAnimation.Controls.Add(this.avAnimation);
            this.groupAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnimation.Location = new System.Drawing.Point(0, 0);
            this.groupAnimation.Name = "groupAnimation";
            this.groupAnimation.Size = new System.Drawing.Size(194, 185);
            this.groupAnimation.TabIndex = 0;
            this.groupAnimation.TabStop = false;
            this.groupAnimation.Text = "Animation";
            // 
            // avAnimation
            // 
            this.avAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avAnimation.Location = new System.Drawing.Point(3, 16);
            this.avAnimation.Name = "avAnimation";
            this.avAnimation.Size = new System.Drawing.Size(188, 166);
            this.avAnimation.TabIndex = 0;
            // 
            // groupSubOverlayAnimation
            // 
            this.groupSubOverlayAnimation.Controls.Add(this.avSubOverlayAnimation);
            this.groupSubOverlayAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSubOverlayAnimation.Location = new System.Drawing.Point(0, 0);
            this.groupSubOverlayAnimation.Name = "groupSubOverlayAnimation";
            this.groupSubOverlayAnimation.Size = new System.Drawing.Size(208, 185);
            this.groupSubOverlayAnimation.TabIndex = 0;
            this.groupSubOverlayAnimation.TabStop = false;
            this.groupSubOverlayAnimation.Text = "SubOverlayAnimation";
            // 
            // avSubOverlayAnimation
            // 
            this.avSubOverlayAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avSubOverlayAnimation.Location = new System.Drawing.Point(3, 16);
            this.avSubOverlayAnimation.Name = "avSubOverlayAnimation";
            this.avSubOverlayAnimation.Size = new System.Drawing.Size(202, 166);
            this.avSubOverlayAnimation.TabIndex = 0;
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
            // colFlags
            // 
            this.colFlags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFlags.DataPropertyName = "Flags";
            this.colFlags.HeaderText = "FLAGS";
            this.colFlags.Name = "colFlags";
            this.colFlags.ReadOnly = true;
            this.colFlags.Width = 50;
            // 
            // colLightFlags
            // 
            this.colLightFlags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLightFlags.DataPropertyName = "LightFlags";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colLightFlags.DefaultCellStyle = dataGridViewCellStyle1;
            this.colLightFlags.HeaderText = "LF";
            this.colLightFlags.Name = "colLightFlags";
            this.colLightFlags.ReadOnly = true;
            this.colLightFlags.Width = 35;
            // 
            // colLightIntensity
            // 
            this.colLightIntensity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLightIntensity.DataPropertyName = "LightIntensity";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colLightIntensity.DefaultCellStyle = dataGridViewCellStyle2;
            this.colLightIntensity.HeaderText = "LI";
            this.colLightIntensity.Name = "colLightIntensity";
            this.colLightIntensity.ReadOnly = true;
            this.colLightIntensity.Width = 35;
            // 
            // colLightColor
            // 
            this.colLightColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLightColor.DataPropertyName = "LightColor";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colLightColor.DefaultCellStyle = dataGridViewCellStyle3;
            this.colLightColor.HeaderText = "LC";
            this.colLightColor.Name = "colLightColor";
            this.colLightColor.ReadOnly = true;
            this.colLightColor.Width = 50;
            // 
            // colColorTranslation
            // 
            this.colColorTranslation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colColorTranslation.DataPropertyName = "ColorTranslation";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Thistle;
            this.colColorTranslation.DefaultCellStyle = dataGridViewCellStyle4;
            this.colColorTranslation.HeaderText = "CT";
            this.colColorTranslation.Name = "colColorTranslation";
            this.colColorTranslation.ReadOnly = true;
            this.colColorTranslation.Width = 35;
            // 
            // colEffect
            // 
            this.colEffect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEffect.DataPropertyName = "Effect";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Thistle;
            this.colEffect.DefaultCellStyle = dataGridViewCellStyle5;
            this.colEffect.HeaderText = "E";
            this.colEffect.Name = "colEffect";
            this.colEffect.ReadOnly = true;
            this.colEffect.Width = 35;
            // 
            // colAnimation
            // 
            this.colAnimation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAnimation.DataPropertyName = "Animation";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Thistle;
            this.colAnimation.DefaultCellStyle = dataGridViewCellStyle6;
            this.colAnimation.HeaderText = "ANIM";
            this.colAnimation.Name = "colAnimation";
            this.colAnimation.ReadOnly = true;
            this.colAnimation.Width = 60;
            // 
            // colTargetsCount
            // 
            this.colTargetsCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTargetsCount.DataPropertyName = "TargetsCount";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.SkyBlue;
            this.colTargetsCount.DefaultCellStyle = dataGridViewCellStyle7;
            this.colTargetsCount.HeaderText = "TC";
            this.colTargetsCount.Name = "colTargetsCount";
            this.colTargetsCount.ReadOnly = true;
            this.colTargetsCount.Width = 50;
            // 
            // colSchoolType
            // 
            this.colSchoolType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSchoolType.DataPropertyName = "SchoolType";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.SkyBlue;
            this.colSchoolType.DefaultCellStyle = dataGridViewCellStyle8;
            this.colSchoolType.HeaderText = "ST";
            this.colSchoolType.Name = "colSchoolType";
            this.colSchoolType.ReadOnly = true;
            this.colSchoolType.Width = 60;
            // 
            // colAppearanceHash
            // 
            this.colAppearanceHash.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAppearanceHash.DataPropertyName = "AppearanceHash";
            this.colAppearanceHash.HeaderText = "APPHASH";
            this.colAppearanceHash.Name = "colAppearanceHash";
            this.colAppearanceHash.ReadOnly = true;
            this.colAppearanceHash.Width = 70;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Wheat;
            this.colName.DefaultCellStyle = dataGridViewCellStyle9;
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colOverlayFile
            // 
            this.colOverlayFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOverlayFile.DataPropertyName = "OverlayFile";
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Wheat;
            this.colOverlayFile.DefaultCellStyle = dataGridViewCellStyle10;
            this.colOverlayFile.HeaderText = "OVFILE";
            this.colOverlayFile.Name = "colOverlayFile";
            this.colOverlayFile.ReadOnly = true;
            // 
            // SpellsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Name = "SpellsView";
            this.Size = new System.Drawing.Size(815, 371);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.groupSpells.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSpells)).EndInit();
            this.splitMain2.Panel1.ResumeLayout(false);
            this.splitMain2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain2)).EndInit();
            this.splitMain2.ResumeLayout(false);
            this.groupSubOverlays.ResumeLayout(false);
            this.splitAnimations.Panel1.ResumeLayout(false);
            this.splitAnimations.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitAnimations)).EndInit();
            this.splitAnimations.ResumeLayout(false);
            this.groupAnimation.ResumeLayout(false);
            this.groupSubOverlayAnimation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox groupSpells;
        private System.Windows.Forms.SplitContainer splitMain2;
        private System.Windows.Forms.GroupBox groupSubOverlays;
        private System.Windows.Forms.SplitContainer splitAnimations;
        private SubOverlayGrid gridSubOverlays;
        private System.Windows.Forms.GroupBox groupAnimation;
        private System.Windows.Forms.GroupBox groupSubOverlayAnimation;
        private AnimationView avAnimation;
        private AnimationView avSubOverlayAnimation;
        private Meridian59.AdminUI.Generic.BaseGridView gridSpells;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverlayRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLightFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLightIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLightColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorTranslation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnimation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetsCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchoolType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppearanceHash;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverlayFile;
    }
}
