namespace Meridian59.RooViewer.UI
{
    partial class RooSubSectorsViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSectorNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMinX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMinY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMaxX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMaxY = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colSectorNum,
            this.colBoxMinX,
            this.colBoxMinY,
            this.colBoxMaxX,
            this.colBoxMaxY});
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(650, 256);
            this.dataGrid.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "SectorDefReference";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Turquoise;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.HeaderText = "SECT";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "X1";
            this.dataGridViewTextBoxColumn2.HeaderText = "BOXMINX";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Y1";
            this.dataGridViewTextBoxColumn3.HeaderText = "BOXMINY";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "X2";
            this.dataGridViewTextBoxColumn4.HeaderText = "BOXMAXX";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Y2";
            this.dataGridViewTextBoxColumn5.HeaderText = "BOXMAXY";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // colSectorNum
            // 
            this.colSectorNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSectorNum.DataPropertyName = "SectorNum";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Turquoise;
            this.colSectorNum.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSectorNum.HeaderText = "SECT";
            this.colSectorNum.Name = "colSectorNum";
            this.colSectorNum.Width = 50;
            // 
            // colBoxMinX
            // 
            this.colBoxMinX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBoxMinX.DataPropertyName = "X1";
            this.colBoxMinX.HeaderText = "BOXMINX";
            this.colBoxMinX.Name = "colBoxMinX";
            // 
            // colBoxMinY
            // 
            this.colBoxMinY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBoxMinY.DataPropertyName = "Y1";
            this.colBoxMinY.HeaderText = "BOXMINY";
            this.colBoxMinY.Name = "colBoxMinY";
            // 
            // colBoxMaxX
            // 
            this.colBoxMaxX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBoxMaxX.DataPropertyName = "X2";
            this.colBoxMaxX.HeaderText = "BOXMAXX";
            this.colBoxMaxX.Name = "colBoxMaxX";
            // 
            // colBoxMaxY
            // 
            this.colBoxMaxY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBoxMaxY.DataPropertyName = "Y2";
            this.colBoxMaxY.HeaderText = "BOXMAXY";
            this.colBoxMaxY.Name = "colBoxMaxY";
            // 
            // RooSubSectorsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGrid);
            this.Name = "RooSubSectorsViewer";
            this.Size = new System.Drawing.Size(650, 256);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSectorNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMinX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMinY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMaxX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMaxY;
    }
}
