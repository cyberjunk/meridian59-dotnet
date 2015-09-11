namespace Meridian59.AdminUI.Viewers
{
    partial class BackgroundMusicView
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
            this.lblRIDDesc = new System.Windows.Forms.Label();
            this.lblRID = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblFileDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRIDDesc
            // 
            this.lblRIDDesc.AutoSize = true;
            this.lblRIDDesc.Location = new System.Drawing.Point(4, 5);
            this.lblRIDDesc.Name = "lblRIDDesc";
            this.lblRIDDesc.Size = new System.Drawing.Size(29, 13);
            this.lblRIDDesc.TabIndex = 0;
            this.lblRIDDesc.Text = "RID:";
            // 
            // lblRID
            // 
            this.lblRID.AutoSize = true;
            this.lblRID.Location = new System.Drawing.Point(72, 5);
            this.lblRID.Name = "lblRID";
            this.lblRID.Size = new System.Drawing.Size(10, 13);
            this.lblRID.TabIndex = 1;
            this.lblRID.Text = "-";
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(72, 27);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(10, 13);
            this.lblFile.TabIndex = 3;
            this.lblFile.Text = "-";
            // 
            // lblFileDesc
            // 
            this.lblFileDesc.AutoSize = true;
            this.lblFileDesc.Location = new System.Drawing.Point(4, 27);
            this.lblFileDesc.Name = "lblFileDesc";
            this.lblFileDesc.Size = new System.Drawing.Size(26, 13);
            this.lblFileDesc.TabIndex = 2;
            this.lblFileDesc.Text = "File:";
            // 
            // BackgroundMusicView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.lblFileDesc);
            this.Controls.Add(this.lblRID);
            this.Controls.Add(this.lblRIDDesc);
            this.Name = "BackgroundMusicView";
            this.Size = new System.Drawing.Size(176, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRIDDesc;
        private System.Windows.Forms.Label lblRID;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblFileDesc;
    }
}
