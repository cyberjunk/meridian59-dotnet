namespace Meridian59.RooViewer.UI
{
    partial class Map
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
            this.lblMouseCoordinates = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMouseCoordinates
            // 
            this.lblMouseCoordinates.AutoSize = true;
            this.lblMouseCoordinates.BackColor = System.Drawing.Color.Transparent;
            this.lblMouseCoordinates.ForeColor = System.Drawing.Color.Gold;
            this.lblMouseCoordinates.Location = new System.Drawing.Point(24, 16);
            this.lblMouseCoordinates.Name = "lblMouseCoordinates";
            this.lblMouseCoordinates.Size = new System.Drawing.Size(28, 13);
            this.lblMouseCoordinates.TabIndex = 0;
            this.lblMouseCoordinates.Text = "(0,0)";
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMouseCoordinates);
            this.Name = "Map";
            this.Size = new System.Drawing.Size(478, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMouseCoordinates;
    }
}
