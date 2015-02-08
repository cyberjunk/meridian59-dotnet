namespace Meridian59.DebugUI
{
    partial class MiniMapForm
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
            this.picBox = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtZoom = new System.Windows.Forms.TextBox();
            this.btnSetZoom = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBox
            // 
            this.picBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBox.Location = new System.Drawing.Point(0, 0);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(415, 303);
            this.picBox.TabIndex = 0;
            this.picBox.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnSetZoom);
            this.splitContainer1.Panel1.Controls.Add(this.txtZoom);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.picBox);
            this.splitContainer1.Size = new System.Drawing.Size(415, 346);
            this.splitContainer1.SplitterDistance = 39;
            this.splitContainer1.TabIndex = 2;
            // 
            // txtZoom
            // 
            this.txtZoom.Location = new System.Drawing.Point(126, 13);
            this.txtZoom.Name = "txtZoom";
            this.txtZoom.Size = new System.Drawing.Size(73, 20);
            this.txtZoom.TabIndex = 0;
            this.txtZoom.Text = "1.0";
            // 
            // btnSetZoom
            // 
            this.btnSetZoom.Location = new System.Drawing.Point(205, 10);
            this.btnSetZoom.Name = "btnSetZoom";
            this.btnSetZoom.Size = new System.Drawing.Size(75, 23);
            this.btnSetZoom.TabIndex = 1;
            this.btnSetZoom.Text = "Zoom";
            this.btnSetZoom.UseVisualStyleBackColor = true;
            this.btnSetZoom.Click += new System.EventHandler(this.btnSetZoom_Click);
            // 
            // MiniMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 346);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MiniMapForm";
            this.Text = "MiniMapForm";
            this.Load += new System.EventHandler(this.MiniMapForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MiniMapForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnSetZoom;
        private System.Windows.Forms.TextBox txtZoom;
    }
}