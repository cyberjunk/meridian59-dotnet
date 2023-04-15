namespace Meridian59.BgfEditor
{
    partial class AddFrameSetIndexForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbFramesMax = new System.Windows.Forms.ComboBox();
            this.cbFrames = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.picBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbFramesMax);
            this.groupBox1.Controls.Add(this.cbFrames);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Frame Range";
            // 
            // cbFramesMax
            // 
            this.cbFramesMax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFramesMax.FormattingEnabled = true;
            this.cbFramesMax.Location = new System.Drawing.Point(269, 19);
            this.cbFramesMax.Name = "cbFramesMax";
            this.cbFramesMax.Size = new System.Drawing.Size(248, 21);
            this.cbFramesMax.TabIndex = 1;
            this.cbFramesMax.SelectedIndexChanged += new System.EventHandler(this.OnFramesMaxSelectedIndexChanged);
            // 
            // cbFrames
            // 
            this.cbFrames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFrames.FormattingEnabled = true;
            this.cbFrames.Location = new System.Drawing.Point(6, 19);
            this.cbFrames.Name = "cbFrames";
            this.cbFrames.Size = new System.Drawing.Size(251, 21);
            this.cbFrames.TabIndex = 0;
            this.cbFrames.SelectedIndexChanged += new System.EventHandler(this.OnFramesSelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 137);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview";
            // 
            // picBox
            // 
            this.picBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBox.Location = new System.Drawing.Point(3, 16);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(254, 118);
            this.picBox.TabIndex = 0;
            this.picBox.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(225, 223);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(94, 36);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Add Frames";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnOK_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.picBox2);
            this.groupBox3.Location = new System.Drawing.Point(278, 69);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 137);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Preview";
            // 
            // picBox2
            // 
            this.picBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBox2.Location = new System.Drawing.Point(3, 16);
            this.picBox2.Name = "picBox2";
            this.picBox2.Size = new System.Drawing.Size(254, 118);
            this.picBox2.TabIndex = 0;
            this.picBox2.TabStop = false;
            // 
            // AddFrameSetIndexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 274);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AddFrameSetIndexForm";
            this.Text = "Add Frames to Group";
            this.Load += new System.EventHandler(this.AddFrameSetIndexForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbFrames;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.ComboBox cbFramesMax;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox picBox2;
    }
}