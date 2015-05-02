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
            this.chkVertexMismatches = new System.Windows.Forms.CheckBox();
            this.chkUseEditorWalls = new System.Windows.Forms.CheckBox();
            this.chkVertHortLines = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblMouseCoordinates
            // 
            this.lblMouseCoordinates.AutoSize = true;
            this.lblMouseCoordinates.BackColor = System.Drawing.Color.Transparent;
            this.lblMouseCoordinates.ForeColor = System.Drawing.Color.Gold;
            this.lblMouseCoordinates.Location = new System.Drawing.Point(3, 0);
            this.lblMouseCoordinates.Name = "lblMouseCoordinates";
            this.lblMouseCoordinates.Size = new System.Drawing.Size(28, 13);
            this.lblMouseCoordinates.TabIndex = 0;
            this.lblMouseCoordinates.Text = "(0,0)";
            // 
            // chkVertexMismatches
            // 
            this.chkVertexMismatches.AutoSize = true;
            this.chkVertexMismatches.BackColor = System.Drawing.Color.Transparent;
            this.chkVertexMismatches.ForeColor = System.Drawing.Color.Gold;
            this.chkVertexMismatches.Location = new System.Drawing.Point(101, -1);
            this.chkVertexMismatches.Name = "chkVertexMismatches";
            this.chkVertexMismatches.Size = new System.Drawing.Size(114, 17);
            this.chkVertexMismatches.TabIndex = 1;
            this.chkVertexMismatches.Text = "Vertex mismatches";
            this.chkVertexMismatches.UseVisualStyleBackColor = false;
            this.chkVertexMismatches.CheckedChanged += new System.EventHandler(this.OnVertexMismatchesCheckedChanged);
            // 
            // chkUseEditorWalls
            // 
            this.chkUseEditorWalls.AutoSize = true;
            this.chkUseEditorWalls.BackColor = System.Drawing.Color.Transparent;
            this.chkUseEditorWalls.ForeColor = System.Drawing.Color.Gold;
            this.chkUseEditorWalls.Location = new System.Drawing.Point(389, 0);
            this.chkUseEditorWalls.Name = "chkUseEditorWalls";
            this.chkUseEditorWalls.Size = new System.Drawing.Size(100, 17);
            this.chkUseEditorWalls.TabIndex = 2;
            this.chkUseEditorWalls.Text = "Use editor walls";
            this.chkUseEditorWalls.UseVisualStyleBackColor = false;
            this.chkUseEditorWalls.CheckedChanged += new System.EventHandler(this.OnUseEditorWallsCheckedChanged);
            // 
            // chkVertHortLines
            // 
            this.chkVertHortLines.AutoSize = true;
            this.chkVertHortLines.BackColor = System.Drawing.Color.Transparent;
            this.chkVertHortLines.ForeColor = System.Drawing.Color.Gold;
            this.chkVertHortLines.Location = new System.Drawing.Point(239, -1);
            this.chkVertHortLines.Name = "chkVertHortLines";
            this.chkVertHortLines.Size = new System.Drawing.Size(144, 17);
            this.chkVertHortLines.TabIndex = 3;
            this.chkVertHortLines.Text = "Almost vertical/horizontal";
            this.chkVertHortLines.UseVisualStyleBackColor = false;
            this.chkVertHortLines.CheckedChanged += new System.EventHandler(this.OnVertHortLinesCheckedChanged);
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkVertHortLines);
            this.Controls.Add(this.chkUseEditorWalls);
            this.Controls.Add(this.chkVertexMismatches);
            this.Controls.Add(this.lblMouseCoordinates);
            this.Name = "Map";
            this.Size = new System.Drawing.Size(478, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMouseCoordinates;
        private System.Windows.Forms.CheckBox chkVertexMismatches;
        private System.Windows.Forms.CheckBox chkUseEditorWalls;
        private System.Windows.Forms.CheckBox chkVertHortLines;
    }
}
