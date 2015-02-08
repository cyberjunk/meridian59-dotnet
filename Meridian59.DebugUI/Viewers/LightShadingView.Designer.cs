namespace Meridian59.DebugUI.Viewers
{
    partial class LightShadingView
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
            this.lblLightIntensityDesc = new System.Windows.Forms.Label();
            this.lblAngleDesc = new System.Windows.Forms.Label();
            this.lblHeightDesc = new System.Windows.Forms.Label();
            this.lblLightIntensity = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblLightIntensityDesc
            // 
            this.lblLightIntensityDesc.AutoSize = true;
            this.lblLightIntensityDesc.Location = new System.Drawing.Point(3, 5);
            this.lblLightIntensityDesc.Name = "lblLightIntensityDesc";
            this.lblLightIntensityDesc.Size = new System.Drawing.Size(72, 13);
            this.lblLightIntensityDesc.TabIndex = 0;
            this.lblLightIntensityDesc.Text = "LightIntensity:";
            // 
            // lblAngleDesc
            // 
            this.lblAngleDesc.AutoSize = true;
            this.lblAngleDesc.Location = new System.Drawing.Point(3, 27);
            this.lblAngleDesc.Name = "lblAngleDesc";
            this.lblAngleDesc.Size = new System.Drawing.Size(37, 13);
            this.lblAngleDesc.TabIndex = 1;
            this.lblAngleDesc.Text = "Angle:";
            // 
            // lblHeightDesc
            // 
            this.lblHeightDesc.AutoSize = true;
            this.lblHeightDesc.Location = new System.Drawing.Point(3, 49);
            this.lblHeightDesc.Name = "lblHeightDesc";
            this.lblHeightDesc.Size = new System.Drawing.Size(41, 13);
            this.lblHeightDesc.TabIndex = 2;
            this.lblHeightDesc.Text = "Height:";
            // 
            // lblLightIntensity
            // 
            this.lblLightIntensity.AutoSize = true;
            this.lblLightIntensity.Location = new System.Drawing.Point(93, 5);
            this.lblLightIntensity.Name = "lblLightIntensity";
            this.lblLightIntensity.Size = new System.Drawing.Size(10, 13);
            this.lblLightIntensity.TabIndex = 3;
            this.lblLightIntensity.Text = "-";
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(93, 27);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(10, 13);
            this.lblAngle.TabIndex = 4;
            this.lblAngle.Text = "-";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(93, 49);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(10, 13);
            this.lblHeight.TabIndex = 5;
            this.lblHeight.Text = "-";
            // 
            // LightShadingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.lblLightIntensity);
            this.Controls.Add(this.lblHeightDesc);
            this.Controls.Add(this.lblAngleDesc);
            this.Controls.Add(this.lblLightIntensityDesc);
            this.Name = "LightShadingView";
            this.Size = new System.Drawing.Size(150, 76);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLightIntensityDesc;
        private System.Windows.Forms.Label lblAngleDesc;
        private System.Windows.Forms.Label lblHeightDesc;
        private System.Windows.Forms.Label lblLightIntensity;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.Label lblHeight;
    }
}
