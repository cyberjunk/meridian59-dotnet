namespace Meridian59.RooViewer.UI
{
    partial class RooSidesViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLowerTexture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMiddleTexture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpperTexture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colNum,
            this.colServerID,
            this.colLowerTexture,
            this.colMiddleTexture,
            this.colUpperTexture,
            this.colFlags,
            this.colSpeed});
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(650, 256);
            this.dataGrid.TabIndex = 0;
            // 
            // colNum
            // 
            this.colNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNum.DataPropertyName = "Num";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.PaleGreen;
            this.colNum.DefaultCellStyle = dataGridViewCellStyle7;
            this.colNum.HeaderText = "#";
            this.colNum.Name = "colNum";
            this.colNum.ReadOnly = true;
            this.colNum.Width = 35;
            // 
            // colServerID
            // 
            this.colServerID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colServerID.DataPropertyName = "ServerID";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Silver;
            this.colServerID.DefaultCellStyle = dataGridViewCellStyle8;
            this.colServerID.HeaderText = "UID";
            this.colServerID.Name = "colServerID";
            this.colServerID.ReadOnly = true;
            this.colServerID.Width = 35;
            // 
            // colLowerTexture
            // 
            this.colLowerTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLowerTexture.DataPropertyName = "LowerTexture";
            this.colLowerTexture.HeaderText = "LTEX";
            this.colLowerTexture.Name = "colLowerTexture";
            // 
            // colMiddleTexture
            // 
            this.colMiddleTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colMiddleTexture.DataPropertyName = "MiddleTexture";
            this.colMiddleTexture.HeaderText = "MTEX";
            this.colMiddleTexture.Name = "colMiddleTexture";
            // 
            // colUpperTexture
            // 
            this.colUpperTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colUpperTexture.DataPropertyName = "UpperTexture";
            this.colUpperTexture.HeaderText = "UTEX";
            this.colUpperTexture.Name = "colUpperTexture";
            // 
            // colFlags
            // 
            this.colFlags.DataPropertyName = "Flags";
            this.colFlags.HeaderText = "FLAGS";
            this.colFlags.Name = "colFlags";
            // 
            // colSpeed
            // 
            this.colSpeed.DataPropertyName = "Speed";
            this.colSpeed.HeaderText = "SPEED";
            this.colSpeed.Name = "colSpeed";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Num";
            this.dataGridViewTextBoxColumn1.HeaderText = "#";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 35;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ServerID";
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Silver;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn2.HeaderText = "UID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 35;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "LowerTexture";
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Khaki;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn3.HeaderText = "LTEX";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "MiddleTexture";
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Khaki;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn4.HeaderText = "MTEX";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "UpperTexture";
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.Khaki;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn5.HeaderText = "UTEX";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Flags";
            this.dataGridViewTextBoxColumn6.HeaderText = "FLAGS";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Speed";
            this.dataGridViewTextBoxColumn7.HeaderText = "SPEED";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // RooSidesViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGrid);
            this.Name = "RooSidesViewer";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLowerTexture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMiddleTexture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpperTexture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpeed;
    }
}
