namespace Meridian59.RooViewer.UI
{
    partial class RooPartitionLinesViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.colWall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRightChild = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftChild = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMinX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMinY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMaxX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxMaxY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colWall,
            this.colA,
            this.colB,
            this.colC,
            this.colRightChild,
            this.colLeftChild,
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
            // colWall
            // 
            this.colWall.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colWall.DataPropertyName = "WallReference";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Thistle;
            this.colWall.DefaultCellStyle = dataGridViewCellStyle1;
            this.colWall.HeaderText = "WALL";
            this.colWall.Name = "colWall";
            this.colWall.Width = 50;
            // 
            // colA
            // 
            this.colA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colA.DataPropertyName = "A";
            this.colA.HeaderText = "A";
            this.colA.Name = "colA";
            this.colA.Width = 60;
            // 
            // colB
            // 
            this.colB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colB.DataPropertyName = "B";
            this.colB.HeaderText = "B";
            this.colB.Name = "colB";
            this.colB.Width = 60;
            // 
            // colC
            // 
            this.colC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colC.DataPropertyName = "C";
            this.colC.HeaderText = "C";
            this.colC.Name = "colC";
            this.colC.Width = 60;
            // 
            // colRightChild
            // 
            this.colRightChild.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRightChild.DataPropertyName = "Right";
            this.colRightChild.HeaderText = "RCHILD";
            this.colRightChild.Name = "colRightChild";
            this.colRightChild.Width = 50;
            // 
            // colLeftChild
            // 
            this.colLeftChild.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLeftChild.DataPropertyName = "Left";
            this.colLeftChild.HeaderText = "LCHILD";
            this.colLeftChild.Name = "colLeftChild";
            this.colLeftChild.Width = 50;
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
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "A";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Thistle;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.HeaderText = "A";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "B";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Wheat;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "B";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "C";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Wheat;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn3.HeaderText = "C";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Right";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Wheat;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn4.HeaderText = "RCHILD";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Left";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightGreen;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn5.HeaderText = "LCHILD";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "WallReference";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGreen;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn6.HeaderText = "WALL";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 50;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "X1";
            this.dataGridViewTextBoxColumn7.HeaderText = "BOXMINX";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Y1";
            this.dataGridViewTextBoxColumn8.HeaderText = "BOXMINY";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn9.DataPropertyName = "X2";
            this.dataGridViewTextBoxColumn9.HeaderText = "BOXMAXX";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Y2";
            this.dataGridViewTextBoxColumn10.HeaderText = "BOXMAXY";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // RooPartitionLinesViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGrid);
            this.Name = "RooPartitionLinesViewer";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWall;
        private System.Windows.Forms.DataGridViewTextBoxColumn colA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRightChild;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftChild;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMinX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMinY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMaxX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxMaxY;
    }
}
