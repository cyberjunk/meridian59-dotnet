namespace Meridian59.RooViewer.UI
{
    partial class RooWallsEditorViewer
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
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.colSideDef1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSideDef2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRightSector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftSector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRightXOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRightYOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftXOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftYOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSideDef1,
            this.colSideDef2,
            this.colRightSector,
            this.colLeftSector,
            this.colX0,
            this.colY0,
            this.colX1,
            this.colY1,
            this.colRightXOffset,
            this.colRightYOffset,
            this.colLeftXOffset,
            this.colLeftYOffset});
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(650, 256);
            this.dataGrid.TabIndex = 0;
            // 
            // colSideDef1
            // 
            this.colSideDef1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSideDef1.DataPropertyName = "FileSideDef1";
            this.colSideDef1.HeaderText = "RSIDE";
            this.colSideDef1.Name = "colSideDef1";
            this.colSideDef1.Width = 50;
            // 
            // colSideDef2
            // 
            this.colSideDef2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSideDef2.DataPropertyName = "FileSideDef2";
            this.colSideDef2.HeaderText = "LSIDE";
            this.colSideDef2.Name = "colSideDef2";
            this.colSideDef2.Width = 50;
            // 
            // colRightSector
            // 
            this.colRightSector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRightSector.DataPropertyName = "Side1Sector";
            this.colRightSector.HeaderText = "RSECT";
            this.colRightSector.Name = "colRightSector";
            this.colRightSector.Width = 50;
            // 
            // colLeftSector
            // 
            this.colLeftSector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLeftSector.DataPropertyName = "Side2Sector";
            this.colLeftSector.HeaderText = "LSECT";
            this.colLeftSector.Name = "colLeftSector";
            this.colLeftSector.Width = 50;
            // 
            // colX0
            // 
            this.colX0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colX0.DataPropertyName = "X0";
            this.colX0.HeaderText = "X0";
            this.colX0.Name = "colX0";
            // 
            // colY0
            // 
            this.colY0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colY0.DataPropertyName = "Y0";
            this.colY0.HeaderText = "Y0";
            this.colY0.Name = "colY0";
            // 
            // colX1
            // 
            this.colX1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colX1.DataPropertyName = "X1";
            this.colX1.HeaderText = "X1";
            this.colX1.Name = "colX1";
            // 
            // colY1
            // 
            this.colY1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colY1.DataPropertyName = "Y1";
            this.colY1.HeaderText = "Y1";
            this.colY1.Name = "colY1";
            // 
            // colRightXOffset
            // 
            this.colRightXOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRightXOffset.DataPropertyName = "Side1XOffset";
            this.colRightXOffset.HeaderText = "RXO";
            this.colRightXOffset.Name = "colRightXOffset";
            this.colRightXOffset.Width = 50;
            // 
            // colRightYOffset
            // 
            this.colRightYOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRightYOffset.DataPropertyName = "Side1YOffset";
            this.colRightYOffset.HeaderText = "RYO";
            this.colRightYOffset.Name = "colRightYOffset";
            this.colRightYOffset.Width = 50;
            // 
            // colLeftXOffset
            // 
            this.colLeftXOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLeftXOffset.DataPropertyName = "Side2XOffset";
            this.colLeftXOffset.HeaderText = "LXO";
            this.colLeftXOffset.Name = "colLeftXOffset";
            this.colLeftXOffset.Width = 50;
            // 
            // colLeftYOffset
            // 
            this.colLeftYOffset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLeftYOffset.DataPropertyName = "Side2YOffset";
            this.colLeftYOffset.HeaderText = "LYO";
            this.colLeftYOffset.Name = "colLeftYOffset";
            this.colLeftYOffset.Width = 50;
            // 
            // RooWallsEditorViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGrid);
            this.Name = "RooWallsEditorViewer";
            this.Size = new System.Drawing.Size(650, 256);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSideDef1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSideDef2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRightSector;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftSector;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX0;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY0;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRightXOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRightYOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftXOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftYOffset;
    }
}
