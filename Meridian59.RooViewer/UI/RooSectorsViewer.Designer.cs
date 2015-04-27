namespace Meridian59.RooViewer.UI
{
    partial class RooSectorsViewer
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
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFloorTexture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCeilingTexture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTextureX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTextureY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFloorHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCeilingHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colFloorTexture,
            this.colCeilingTexture,
            this.colTextureX,
            this.colTextureY,
            this.colFloorHeight,
            this.colCeilingHeight,
            this.colLight,
            this.colFlags,
            this.colSpeed});
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(650, 256);
            this.dataGrid.TabIndex = 0;
            // 
            // colNum
            // 
            this.colNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNum.DataPropertyName = "Num";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Turquoise;
            this.colNum.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNum.HeaderText = "#";
            this.colNum.Name = "colNum";
            this.colNum.Width = 35;
            // 
            // colServerID
            // 
            this.colServerID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colServerID.DataPropertyName = "ServerID";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.colServerID.DefaultCellStyle = dataGridViewCellStyle2;
            this.colServerID.HeaderText = "UID";
            this.colServerID.Name = "colServerID";
            this.colServerID.Width = 35;
            // 
            // colFloorTexture
            // 
            this.colFloorTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFloorTexture.DataPropertyName = "FloorTexture";
            this.colFloorTexture.HeaderText = "FTEX";
            this.colFloorTexture.Name = "colFloorTexture";
            this.colFloorTexture.Width = 40;
            // 
            // colCeilingTexture
            // 
            this.colCeilingTexture.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCeilingTexture.DataPropertyName = "CeilingTexture";
            this.colCeilingTexture.HeaderText = "CTEX";
            this.colCeilingTexture.Name = "colCeilingTexture";
            this.colCeilingTexture.Width = 40;
            // 
            // colTextureX
            // 
            this.colTextureX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTextureX.DataPropertyName = "TextureX";
            this.colTextureX.HeaderText = "XOFF";
            this.colTextureX.Name = "colTextureX";
            this.colTextureX.Width = 40;
            // 
            // colTextureY
            // 
            this.colTextureY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTextureY.DataPropertyName = "TextureY";
            this.colTextureY.HeaderText = "YOFF";
            this.colTextureY.Name = "colTextureY";
            this.colTextureY.Width = 40;
            // 
            // colFloorHeight
            // 
            this.colFloorHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFloorHeight.DataPropertyName = "FloorHeight";
            this.colFloorHeight.HeaderText = "FHEIGHT";
            this.colFloorHeight.Name = "colFloorHeight";
            this.colFloorHeight.Width = 60;
            // 
            // colCeilingHeight
            // 
            this.colCeilingHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCeilingHeight.DataPropertyName = "CeilingHeight";
            this.colCeilingHeight.HeaderText = "CHEIGHT";
            this.colCeilingHeight.Name = "colCeilingHeight";
            this.colCeilingHeight.Width = 60;
            // 
            // colLight
            // 
            this.colLight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLight.DataPropertyName = "Light1";
            this.colLight.HeaderText = "LIGHT";
            this.colLight.Name = "colLight";
            this.colLight.Width = 40;
            // 
            // colFlags
            // 
            this.colFlags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFlags.DataPropertyName = "Flags";
            this.colFlags.HeaderText = "FLAGS";
            this.colFlags.MinimumWidth = 50;
            this.colFlags.Name = "colFlags";
            // 
            // colSpeed
            // 
            this.colSpeed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSpeed.DataPropertyName = "Speed";
            this.colSpeed.HeaderText = "SPEED";
            this.colSpeed.Name = "colSpeed";
            this.colSpeed.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Num";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Turquoise;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn1.HeaderText = "#";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 35;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ServerID";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Silver;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn2.HeaderText = "UID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 35;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "FloorTexture";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Khaki;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn3.HeaderText = "FTEX";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 40;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CeilingTexture";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Khaki;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn4.HeaderText = "CTEX";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 40;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "TextureX";
            this.dataGridViewTextBoxColumn5.HeaderText = "XOFF";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 40;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "TextureY";
            this.dataGridViewTextBoxColumn6.HeaderText = "YOFF";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 40;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "FloorHeight";
            this.dataGridViewTextBoxColumn7.HeaderText = "FHEIGHT";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 60;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CeilingHeight";
            this.dataGridViewTextBoxColumn8.HeaderText = "CHEIGHT";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 60;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Light1";
            this.dataGridViewTextBoxColumn9.HeaderText = "LIGHT";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 40;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Flags";
            this.dataGridViewTextBoxColumn10.HeaderText = "FLAGS";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Speed";
            this.dataGridViewTextBoxColumn11.HeaderText = "SPEED";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 50;
            // 
            // RooSectorsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGrid);
            this.Name = "RooSectorsViewer";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFloorTexture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCeilingTexture;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextureX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextureY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFloorHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCeilingHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpeed;
    }
}
