namespace Meridian59.AdminUI
{
    partial class OnlinePlayersView
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
            this.gridOnlinePlayers = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameRID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridOnlinePlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // gridOnlinePlayers
            // 
            this.gridOnlinePlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOnlinePlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colCount,
            this.colNameRID,
            this.colFlags,
            this.colName});
            this.gridOnlinePlayers.Location = new System.Drawing.Point(0, 0);
            this.gridOnlinePlayers.Name = "gridOnlinePlayers";
            this.gridOnlinePlayers.Size = new System.Drawing.Size(708, 386);
            this.gridOnlinePlayers.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Width = 60;
            // 
            // colCount
            // 
            this.colCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCount.DataPropertyName = "Count";
            this.colCount.HeaderText = "COUNT";
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.Width = 60;
            // 
            // colNameRID
            // 
            this.colNameRID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNameRID.DataPropertyName = "NameRID";
            this.colNameRID.HeaderText = "NRID";
            this.colNameRID.Name = "colNameRID";
            this.colNameRID.ReadOnly = true;
            this.colNameRID.Width = 60;
            // 
            // colFlags
            // 
            this.colFlags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFlags.DataPropertyName = "Flags";
            this.colFlags.HeaderText = "FLAGS";
            this.colFlags.Name = "colFlags";
            this.colFlags.ReadOnly = true;
            this.colFlags.Width = 50;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "NAME";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // OnlinePlayersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridOnlinePlayers);
            this.Name = "OnlinePlayersView";
            this.Size = new System.Drawing.Size(708, 386);
            ((System.ComponentModel.ISupportInitialize)(this.gridOnlinePlayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Meridian59.AdminUI.Generic.BaseGridView gridOnlinePlayers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameRID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;

    }
}
