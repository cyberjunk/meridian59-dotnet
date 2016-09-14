﻿namespace Meridian59.AdminUI
{
    partial class ObjectBaseView
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
            this.groupObjects = new System.Windows.Forms.GroupBox();
            this.gridObjects = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverlayRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLightingInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorTranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnimation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppearanceHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverlayFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitMain2 = new System.Windows.Forms.SplitContainer();
            this.groupSubOverlays = new System.Windows.Forms.GroupBox();
            this.gridSubOverlays = new Meridian59.AdminUI.SubOverlayGrid();
            this.splitAnimationsPic = new System.Windows.Forms.SplitContainer();
            this.splitAnimations = new System.Windows.Forms.SplitContainer();
            this.groupAnimation = new System.Windows.Forms.GroupBox();
            this.avAnimation = new Meridian59.AdminUI.AnimationView();
            this.groupBoxSubOverlayAnimation = new System.Windows.Forms.GroupBox();
            this.avSubOverlayAnimation = new Meridian59.AdminUI.AnimationView();
            this.groupImage = new System.Windows.Forms.GroupBox();
            this.pictureBox = new Meridian59.AdminUI.Generic.PictureBoxObjectBase();
            this.groupObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain2)).BeginInit();
            this.splitMain2.Panel1.SuspendLayout();
            this.splitMain2.Panel2.SuspendLayout();
            this.splitMain2.SuspendLayout();
            this.groupSubOverlays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitAnimationsPic)).BeginInit();
            this.splitAnimationsPic.Panel1.SuspendLayout();
            this.splitAnimationsPic.Panel2.SuspendLayout();
            this.splitAnimationsPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitAnimations)).BeginInit();
            this.splitAnimations.Panel1.SuspendLayout();
            this.splitAnimations.Panel2.SuspendLayout();
            this.splitAnimations.SuspendLayout();
            this.groupAnimation.SuspendLayout();
            this.groupBoxSubOverlayAnimation.SuspendLayout();
            this.groupImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupObjects
            // 
            this.groupObjects.Controls.Add(this.gridObjects);
            this.groupObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupObjects.Location = new System.Drawing.Point(0, 0);
            this.groupObjects.Name = "groupObjects";
            this.groupObjects.Size = new System.Drawing.Size(797, 218);
            this.groupObjects.TabIndex = 0;
            this.groupObjects.TabStop = false;
            this.groupObjects.Text = "Objects";
            // 
            // gridObjects
            // 
            this.gridObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colCount,
            this.colOverlayRID,
            this.colNameRID,
            this.colFlags,
            this.colLightingInfo,
            this.colColorTranslation,
            this.colEffect,
            this.colAnimation,
            this.colAppearanceHash,
            this.colName,
            this.colOverlayFile});
            this.gridObjects.Location = new System.Drawing.Point(3, 16);
            this.gridObjects.Name = "gridObjects";
            this.gridObjects.Size = new System.Drawing.Size(791, 199);
            this.gridObjects.TabIndex = 0;
            this.gridObjects.SelectionChanged += new System.EventHandler(this.OnGridObjectsSelectionChanged);
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
            // colLightingInfo
            // 
            this.colLightingInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLightingInfo.DataPropertyName = "LightingInfo";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colLightingInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colLightingInfo.HeaderText = "LI";
            this.colLightingInfo.Name = "colLightingInfo";
            this.colLightingInfo.ReadOnly = true;
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
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Wheat;
            this.colName.DefaultCellStyle = dataGridViewCellStyle7;
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colOverlayFile
            // 
            this.colOverlayFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOverlayFile.DataPropertyName = "OverlayFile";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Wheat;
            this.colOverlayFile.DefaultCellStyle = dataGridViewCellStyle8;
            this.colOverlayFile.HeaderText = "OVFILE";
            this.colOverlayFile.Name = "colOverlayFile";
            this.colOverlayFile.ReadOnly = true;
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
            this.splitMain.Panel1.Controls.Add(this.groupObjects);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitMain2);
            this.splitMain.Size = new System.Drawing.Size(797, 400);
            this.splitMain.SplitterDistance = 218;
            this.splitMain.TabIndex = 1;
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
            this.splitMain2.Panel2.Controls.Add(this.splitAnimationsPic);
            this.splitMain2.Size = new System.Drawing.Size(797, 178);
            this.splitMain2.SplitterDistance = 352;
            this.splitMain2.TabIndex = 0;
            // 
            // groupSubOverlays
            // 
            this.groupSubOverlays.Controls.Add(this.gridSubOverlays);
            this.groupSubOverlays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSubOverlays.Location = new System.Drawing.Point(0, 0);
            this.groupSubOverlays.Name = "groupSubOverlays";
            this.groupSubOverlays.Size = new System.Drawing.Size(352, 178);
            this.groupSubOverlays.TabIndex = 0;
            this.groupSubOverlays.TabStop = false;
            this.groupSubOverlays.Text = "SubOverlays";
            // 
            // gridSubOverlays
            // 
            this.gridSubOverlays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSubOverlays.Location = new System.Drawing.Point(3, 16);
            this.gridSubOverlays.Name = "gridSubOverlays";
            this.gridSubOverlays.Size = new System.Drawing.Size(346, 159);
            this.gridSubOverlays.TabIndex = 0;
            this.gridSubOverlays.SelectionChanged += new System.EventHandler(this.OnGridSubOverlaysSelectionChanged);
            // 
            // splitAnimationsPic
            // 
            this.splitAnimationsPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitAnimationsPic.Location = new System.Drawing.Point(0, 0);
            this.splitAnimationsPic.Name = "splitAnimationsPic";
            // 
            // splitAnimationsPic.Panel1
            // 
            this.splitAnimationsPic.Panel1.Controls.Add(this.splitAnimations);
            // 
            // splitAnimationsPic.Panel2
            // 
            this.splitAnimationsPic.Panel2.Controls.Add(this.groupImage);
            this.splitAnimationsPic.Size = new System.Drawing.Size(441, 178);
            this.splitAnimationsPic.SplitterDistance = 159;
            this.splitAnimationsPic.TabIndex = 0;
            // 
            // splitAnimations
            // 
            this.splitAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitAnimations.Location = new System.Drawing.Point(0, 0);
            this.splitAnimations.Name = "splitAnimations";
            this.splitAnimations.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitAnimations.Panel1
            // 
            this.splitAnimations.Panel1.Controls.Add(this.groupAnimation);
            // 
            // splitAnimations.Panel2
            // 
            this.splitAnimations.Panel2.Controls.Add(this.groupBoxSubOverlayAnimation);
            this.splitAnimations.Size = new System.Drawing.Size(159, 178);
            this.splitAnimations.SplitterDistance = 89;
            this.splitAnimations.TabIndex = 1;
            // 
            // groupAnimation
            // 
            this.groupAnimation.Controls.Add(this.avAnimation);
            this.groupAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnimation.Location = new System.Drawing.Point(0, 0);
            this.groupAnimation.Name = "groupAnimation";
            this.groupAnimation.Size = new System.Drawing.Size(159, 89);
            this.groupAnimation.TabIndex = 0;
            this.groupAnimation.TabStop = false;
            this.groupAnimation.Text = "Animaton";
            // 
            // avAnimation
            // 
            this.avAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avAnimation.Location = new System.Drawing.Point(3, 16);
            this.avAnimation.Name = "avAnimation";
            this.avAnimation.Size = new System.Drawing.Size(153, 70);
            this.avAnimation.TabIndex = 0;
            // 
            // groupBoxSubOverlayAnimation
            // 
            this.groupBoxSubOverlayAnimation.Controls.Add(this.avSubOverlayAnimation);
            this.groupBoxSubOverlayAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSubOverlayAnimation.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSubOverlayAnimation.Name = "groupBoxSubOverlayAnimation";
            this.groupBoxSubOverlayAnimation.Size = new System.Drawing.Size(159, 85);
            this.groupBoxSubOverlayAnimation.TabIndex = 1;
            this.groupBoxSubOverlayAnimation.TabStop = false;
            this.groupBoxSubOverlayAnimation.Text = "SubOverlayAnimaton";
            // 
            // avSubOverlayAnimation
            // 
            this.avSubOverlayAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avSubOverlayAnimation.Location = new System.Drawing.Point(3, 16);
            this.avSubOverlayAnimation.Name = "avSubOverlayAnimation";
            this.avSubOverlayAnimation.Size = new System.Drawing.Size(153, 66);
            this.avSubOverlayAnimation.TabIndex = 0;
            // 
            // groupImage
            // 
            this.groupImage.Controls.Add(this.pictureBox);
            this.groupImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupImage.Location = new System.Drawing.Point(0, 0);
            this.groupImage.Name = "groupImage";
            this.groupImage.Size = new System.Drawing.Size(278, 178);
            this.groupImage.TabIndex = 0;
            this.groupImage.TabStop = false;
            this.groupImage.Text = "Image";
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 16);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(272, 159);
            this.pictureBox.TabIndex = 0;
            // 
            // ObjectBaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Name = "ObjectBaseView";
            this.Size = new System.Drawing.Size(797, 400);
            this.groupObjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitMain2.Panel1.ResumeLayout(false);
            this.splitMain2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain2)).EndInit();
            this.splitMain2.ResumeLayout(false);
            this.groupSubOverlays.ResumeLayout(false);
            this.splitAnimationsPic.Panel1.ResumeLayout(false);
            this.splitAnimationsPic.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitAnimationsPic)).EndInit();
            this.splitAnimationsPic.ResumeLayout(false);
            this.splitAnimations.Panel1.ResumeLayout(false);
            this.splitAnimations.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitAnimations)).EndInit();
            this.splitAnimations.ResumeLayout(false);
            this.groupAnimation.ResumeLayout(false);
            this.groupBoxSubOverlayAnimation.ResumeLayout(false);
            this.groupImage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupObjects;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.SplitContainer splitMain2;
        private System.Windows.Forms.SplitContainer splitAnimationsPic;
        private System.Windows.Forms.GroupBox groupSubOverlays;
        private SubOverlayGrid gridSubOverlays;
        private AnimationView avAnimation;
        private System.Windows.Forms.GroupBox groupAnimation;
        private System.Windows.Forms.GroupBox groupBoxSubOverlayAnimation;
        private AnimationView avSubOverlayAnimation;
        private Meridian59.AdminUI.Generic.BaseGridView gridObjects;
        private System.Windows.Forms.SplitContainer splitAnimations;
        private System.Windows.Forms.GroupBox groupImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverlayRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLightingInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorTranslation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnimation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppearanceHash;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverlayFile;
        private Generic.PictureBoxObjectBase pictureBox;
    }
}
