namespace Meridian59.Ogre.Patcher
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
            this.progressOverall = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblFilesProcessed = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDataProcessed = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressOverall
            // 
            this.progressOverall.Location = new System.Drawing.Point(12, 25);
            this.progressOverall.Maximum = 10000;
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(472, 23);
            this.progressOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressOverall.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Speed:";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(59, 9);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(25, 13);
            this.lblSpeed.TabIndex = 4;
            this.lblSpeed.Text = "------";
            // 
            // lblFilesProcessed
            // 
            this.lblFilesProcessed.AutoSize = true;
            this.lblFilesProcessed.Location = new System.Drawing.Point(233, 9);
            this.lblFilesProcessed.Name = "lblFilesProcessed";
            this.lblFilesProcessed.Size = new System.Drawing.Size(25, 13);
            this.lblFilesProcessed.TabIndex = 6;
            this.lblFilesProcessed.Text = "------";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Files:";
            // 
            // lblDataProcessed
            // 
            this.lblDataProcessed.AutoSize = true;
            this.lblDataProcessed.Location = new System.Drawing.Point(365, 9);
            this.lblDataProcessed.Name = "lblDataProcessed";
            this.lblDataProcessed.Size = new System.Drawing.Size(103, 13);
            this.lblDataProcessed.TabIndex = 8;
            this.lblDataProcessed.Text = "000.00 / 000.00 MB";
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 57);
            this.Controls.Add(this.lblDataProcessed);
            this.Controls.Add(this.lblFilesProcessed);
            this.Controls.Add(this.progressOverall);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(512, 96);
            this.Name = "DownloadForm";
            this.Text = "Meridian 59 Patcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnDownloadFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressOverall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFilesProcessed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblDataProcessed;
    }
}

