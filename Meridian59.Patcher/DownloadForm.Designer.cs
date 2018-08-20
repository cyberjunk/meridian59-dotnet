namespace Meridian59.Patcher
{
    partial class DownloadForm
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusText = new System.Windows.Forms.Label();
            this.progressOverall = new CustomProgress.CustomProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(13, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(750, 450);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.BackColor = System.Drawing.Color.Transparent;
            this.statusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusText.ForeColor = System.Drawing.SystemColors.Control;
            this.statusText.Location = new System.Drawing.Point(12, 474);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(74, 25);
            this.statusText.TabIndex = 3;
            this.statusText.Text = "Status";
            // 
            // progressOverall
            // 
            this.progressOverall.Fade = 50;
            this.progressOverall.Location = new System.Drawing.Point(13, 509);
            this.progressOverall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressOverall.Maximum = 10000;
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(750, 35);
            this.progressOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressOverall.TabIndex = 1;
            this.progressOverall.Text = null;
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(762, 544);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressOverall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(784, 600);
            this.MinimumSize = new System.Drawing.Size(784, 600);
            this.Name = "DownloadForm";
            this.Text = "Meridian 59 Patcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnDownloadFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomProgress.CustomProgressBar progressOverall;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label statusText;
    }
}

