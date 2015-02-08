namespace Meridian59.BgfEditor
{
    partial class ComparePalettesForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPaletteLeft = new Meridian59.BgfEditor.Controls.ComboBoxPalette();
            this.picPaletteLeft = new Meridian59.BgfEditor.Controls.InterpolationModePictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPaletteRight = new Meridian59.BgfEditor.Controls.ComboBoxPalette();
            this.picPaletteRight = new Meridian59.BgfEditor.Controls.InterpolationModePictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteRight)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(684, 360);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.cbPaletteLeft);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.picPaletteLeft);
            this.splitContainer2.Size = new System.Drawing.Size(330, 360);
            this.splitContainer2.SplitterDistance = 60;
            this.splitContainer2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Palette";
            // 
            // cbPaletteLeft
            // 
            this.cbPaletteLeft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaletteLeft.FormattingEnabled = true;
            this.cbPaletteLeft.Location = new System.Drawing.Point(58, 24);
            this.cbPaletteLeft.Name = "cbPaletteLeft";
            this.cbPaletteLeft.Size = new System.Drawing.Size(250, 21);
            this.cbPaletteLeft.TabIndex = 0;
            this.cbPaletteLeft.SelectedIndexChanged += new System.EventHandler(this.cbPaletteLeft_SelectedIndexChanged);
            // 
            // picPaletteLeft
            // 
            this.picPaletteLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPaletteLeft.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.picPaletteLeft.Location = new System.Drawing.Point(0, 0);
            this.picPaletteLeft.Name = "picPaletteLeft";
            this.picPaletteLeft.Size = new System.Drawing.Size(330, 296);
            this.picPaletteLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPaletteLeft.TabIndex = 0;
            this.picPaletteLeft.TabStop = false;
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
            this.splitContainer3.Panel1.Controls.Add(this.label2);
            this.splitContainer3.Panel1.Controls.Add(this.cbPaletteRight);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.picPaletteRight);
            this.splitContainer3.Size = new System.Drawing.Size(350, 360);
            this.splitContainer3.SplitterDistance = 60;
            this.splitContainer3.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Palette";
            // 
            // cbPaletteRight
            // 
            this.cbPaletteRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaletteRight.FormattingEnabled = true;
            this.cbPaletteRight.Location = new System.Drawing.Point(72, 24);
            this.cbPaletteRight.Name = "cbPaletteRight";
            this.cbPaletteRight.Size = new System.Drawing.Size(252, 21);
            this.cbPaletteRight.TabIndex = 2;
            this.cbPaletteRight.SelectedIndexChanged += new System.EventHandler(this.cbPaletteRight_SelectedIndexChanged);
            // 
            // picPaletteRight
            // 
            this.picPaletteRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPaletteRight.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.picPaletteRight.Location = new System.Drawing.Point(0, 0);
            this.picPaletteRight.Name = "picPaletteRight";
            this.picPaletteRight.Size = new System.Drawing.Size(350, 296);
            this.picPaletteRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPaletteRight.TabIndex = 0;
            this.picPaletteRight.TabStop = false;
            // 
            // ComparePalettesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 360);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ComparePalettesForm";
            this.Text = "Palettes";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteLeft)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Controls.ComboBoxPalette cbPaletteLeft;
        private Controls.InterpolationModePictureBox picPaletteLeft;
        private Controls.InterpolationModePictureBox picPaletteRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.ComboBoxPalette cbPaletteRight;
    }
}