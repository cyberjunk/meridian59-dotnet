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
            this.progressOverall = new CustomProgress.CustomProgressBar();
            this.infoTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // progressOverall
            // 
            this.progressOverall.Location = new System.Drawing.Point(18, 38);
            this.progressOverall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressOverall.Maximum = 10000;
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(708, 35);
            this.progressOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressOverall.TabIndex = 1;
            this.progressOverall.Fade = 50;
            this.progressOverall.Text = "";
            // 
            // infoTextBox
            // 
            this.infoTextBox.Location = new System.Drawing.Point(18, 81);
            this.infoTextBox.Multiline = true;
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ReadOnly = true;
            this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoTextBox.Size = new System.Drawing.Size(708, 331);
            this.infoTextBox.TabIndex = 9;
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 424);
            this.Controls.Add(this.infoTextBox);
            this.Controls.Add(this.progressOverall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(768, 480);
            this.Name = "DownloadForm";
            this.Text = "Meridian 59 Patcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnDownloadFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomProgress.CustomProgressBar progressOverall;
        private System.Windows.Forms.TextBox infoTextBox;
    }
}

