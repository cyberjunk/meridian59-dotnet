namespace Meridian59.BgfEditor.Forms
{
    partial class RoomTexturesViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomTexturesViewer));
            this.gridTextures = new System.Windows.Forms.DataGridView();
            this.colFrame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Shrink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBgfWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBgfHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBgfRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPngWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPngHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPngRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnBgf = new System.Windows.Forms.Button();
            this.txtBgf = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPng = new System.Windows.Forms.Button();
            this.txtPng = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.picBGF = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.picPNG = new System.Windows.Forms.PictureBox();
            this.fbBgf = new System.Windows.Forms.FolderBrowserDialog();
            this.fbPng = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridTextures)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBGF)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPNG)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTextures
            // 
            this.gridTextures.AllowUserToAddRows = false;
            this.gridTextures.AllowUserToDeleteRows = false;
            this.gridTextures.AllowUserToResizeRows = false;
            this.gridTextures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTextures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFrame,
            this.colState,
            this.Shrink,
            this.colBgfWidth,
            this.colBgfHeight,
            this.colBgfRatio,
            this.colPngWidth,
            this.colPngHeight,
            this.colPngRatio,
            this.colScale});
            this.gridTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTextures.Location = new System.Drawing.Point(3, 16);
            this.gridTextures.Name = "gridTextures";
            this.gridTextures.ReadOnly = true;
            this.gridTextures.RowHeadersVisible = false;
            this.gridTextures.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTextures.Size = new System.Drawing.Size(737, 353);
            this.gridTextures.TabIndex = 0;
            this.gridTextures.SelectionChanged += new System.EventHandler(this.OnGridTexturesSelectionChanged);
            // 
            // colFrame
            // 
            this.colFrame.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFrame.DataPropertyName = "Frame";
            this.colFrame.HeaderText = "Frame";
            this.colFrame.Name = "colFrame";
            this.colFrame.ReadOnly = true;
            // 
            // colState
            // 
            this.colState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colState.DataPropertyName = "State";
            this.colState.HeaderText = "State";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.Width = 60;
            // 
            // Shrink
            // 
            this.Shrink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Shrink.DataPropertyName = "Shrink";
            this.Shrink.HeaderText = "Shrink";
            this.Shrink.Name = "Shrink";
            this.Shrink.ReadOnly = true;
            this.Shrink.Width = 50;
            // 
            // colBgfWidth
            // 
            this.colBgfWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBgfWidth.DataPropertyName = "BgfWidth";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colBgfWidth.DefaultCellStyle = dataGridViewCellStyle7;
            this.colBgfWidth.HeaderText = "BgfWidth";
            this.colBgfWidth.Name = "colBgfWidth";
            this.colBgfWidth.ReadOnly = true;
            this.colBgfWidth.Width = 80;
            // 
            // colBgfHeight
            // 
            this.colBgfHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBgfHeight.DataPropertyName = "BgfHeight";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colBgfHeight.DefaultCellStyle = dataGridViewCellStyle8;
            this.colBgfHeight.HeaderText = "BgfHeight";
            this.colBgfHeight.Name = "colBgfHeight";
            this.colBgfHeight.ReadOnly = true;
            this.colBgfHeight.Width = 80;
            // 
            // colBgfRatio
            // 
            this.colBgfRatio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBgfRatio.DataPropertyName = "BgfRatio";
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.colBgfRatio.DefaultCellStyle = dataGridViewCellStyle9;
            this.colBgfRatio.HeaderText = "BgfRatio";
            this.colBgfRatio.Name = "colBgfRatio";
            this.colBgfRatio.ReadOnly = true;
            this.colBgfRatio.Width = 80;
            // 
            // colPngWidth
            // 
            this.colPngWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPngWidth.DataPropertyName = "PngWidth";
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Bisque;
            this.colPngWidth.DefaultCellStyle = dataGridViewCellStyle10;
            this.colPngWidth.HeaderText = "PngWidth";
            this.colPngWidth.Name = "colPngWidth";
            this.colPngWidth.ReadOnly = true;
            this.colPngWidth.Width = 80;
            // 
            // colPngHeight
            // 
            this.colPngHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPngHeight.DataPropertyName = "PngHeight";
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Bisque;
            this.colPngHeight.DefaultCellStyle = dataGridViewCellStyle11;
            this.colPngHeight.HeaderText = "PngHeight";
            this.colPngHeight.Name = "colPngHeight";
            this.colPngHeight.ReadOnly = true;
            this.colPngHeight.Width = 80;
            // 
            // colPngRatio
            // 
            this.colPngRatio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPngRatio.DataPropertyName = "PngRatio";
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.Bisque;
            this.colPngRatio.DefaultCellStyle = dataGridViewCellStyle12;
            this.colPngRatio.HeaderText = "PngRatio";
            this.colPngRatio.Name = "colPngRatio";
            this.colPngRatio.ReadOnly = true;
            this.colPngRatio.Width = 80;
            // 
            // colScale
            // 
            this.colScale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colScale.DataPropertyName = "Scale";
            this.colScale.HeaderText = "Scale";
            this.colScale.Name = "colScale";
            this.colScale.ReadOnly = true;
            this.colScale.Width = 50;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridTextures);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 372);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Textures";
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1064, 457);
            this.splitContainer1.SplitterDistance = 81;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLoad);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1064, 81);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Folders";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(558, 36);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.OnBtnLoadClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnBgf);
            this.groupBox3.Controls.Add(this.txtBgf);
            this.groupBox3.Location = new System.Drawing.Point(12, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 53);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BGF Folder";
            // 
            // btnBgf
            // 
            this.btnBgf.Location = new System.Drawing.Point(179, 17);
            this.btnBgf.Name = "btnBgf";
            this.btnBgf.Size = new System.Drawing.Size(75, 23);
            this.btnBgf.TabIndex = 3;
            this.btnBgf.Text = "Select";
            this.btnBgf.UseVisualStyleBackColor = true;
            this.btnBgf.Click += new System.EventHandler(this.OnBtnBgfClick);
            // 
            // txtBgf
            // 
            this.txtBgf.Location = new System.Drawing.Point(6, 19);
            this.txtBgf.Name = "txtBgf";
            this.txtBgf.Size = new System.Drawing.Size(167, 20);
            this.txtBgf.TabIndex = 2;
            this.txtBgf.Text = "../../../../Resources/bgftextures";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPng);
            this.groupBox4.Controls.Add(this.txtPng);
            this.groupBox4.Location = new System.Drawing.Point(278, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(260, 55);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PNG Folder";
            // 
            // btnPng
            // 
            this.btnPng.Location = new System.Drawing.Point(179, 17);
            this.btnPng.Name = "btnPng";
            this.btnPng.Size = new System.Drawing.Size(75, 23);
            this.btnPng.TabIndex = 1;
            this.btnPng.Text = "Select";
            this.btnPng.UseVisualStyleBackColor = true;
            this.btnPng.Click += new System.EventHandler(this.OnBtnPngClick);
            // 
            // txtPng
            // 
            this.txtPng.Location = new System.Drawing.Point(6, 19);
            this.txtPng.Name = "txtPng";
            this.txtPng.Size = new System.Drawing.Size(167, 20);
            this.txtPng.TabIndex = 0;
            this.txtPng.Text = "../../../../Resources/roomtextures";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1064, 372);
            this.splitContainer2.SplitterDistance = 743;
            this.splitContainer2.TabIndex = 2;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox5);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer3.Size = new System.Drawing.Size(317, 372);
            this.splitContainer3.SplitterDistance = 186;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.picBGF);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(317, 186);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "BGF";
            // 
            // picBGF
            // 
            this.picBGF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBGF.Location = new System.Drawing.Point(3, 16);
            this.picBGF.Name = "picBGF";
            this.picBGF.Size = new System.Drawing.Size(311, 167);
            this.picBGF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBGF.TabIndex = 1;
            this.picBGF.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.picPNG);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(317, 182);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "PNG";
            // 
            // picPNG
            // 
            this.picPNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPNG.Location = new System.Drawing.Point(3, 16);
            this.picPNG.Name = "picPNG";
            this.picPNG.Size = new System.Drawing.Size(311, 163);
            this.picPNG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPNG.TabIndex = 0;
            this.picPNG.TabStop = false;
            // 
            // RoomTexturesViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 457);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RoomTexturesViewer";
            this.Text = "RoomTexturesViewer";
            ((System.ComponentModel.ISupportInitialize)(this.gridTextures)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBGF)).EndInit();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPNG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridTextures;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FolderBrowserDialog fbBgf;
        private System.Windows.Forms.FolderBrowserDialog fbPng;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnBgf;
        private System.Windows.Forms.TextBox txtBgf;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnPng;
        private System.Windows.Forms.TextBox txtPng;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrame;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Shrink;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBgfWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBgfHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBgfRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPngWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPngHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPngRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScale;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.PictureBox picPNG;
        private System.Windows.Forms.PictureBox picBGF;
    }
}