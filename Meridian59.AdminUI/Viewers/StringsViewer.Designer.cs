namespace Meridian59.AdminUI.Viewers
{
    partial class StringsViewer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridStrings = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtFilterText = new System.Windows.Forms.TextBox();
            this.lblFilterText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStrings)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblFilterText);
            this.splitContainer1.Panel1.Controls.Add(this.txtFilterText);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridStrings);
            this.splitContainer1.Size = new System.Drawing.Size(425, 190);
            this.splitContainer1.SplitterDistance = 59;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridStrings
            // 
            this.gridStrings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStrings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResourceID,
            this.colText});
            this.gridStrings.Location = new System.Drawing.Point(0, 0);
            this.gridStrings.Name = "gridStrings";
            this.gridStrings.Size = new System.Drawing.Size(425, 127);
            this.gridStrings.TabIndex = 0;
            // 
            // colResourceID
            // 
            this.colResourceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colResourceID.DataPropertyName = "Value";
            this.colResourceID.HeaderText = "RESID";
            this.colResourceID.Name = "colResourceID";
            this.colResourceID.ReadOnly = true;
            // 
            // colText
            // 
            this.colText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colText.DataPropertyName = "Name";
            this.colText.HeaderText = "TEXT";
            this.colText.Name = "colText";
            this.colText.ReadOnly = true;
            // 
            // txtFilterText
            // 
            this.txtFilterText.Location = new System.Drawing.Point(179, 25);
            this.txtFilterText.Name = "txtFilterText";
            this.txtFilterText.Size = new System.Drawing.Size(117, 20);
            this.txtFilterText.TabIndex = 0;
            this.txtFilterText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnFilterTextKeyUp);
            // 
            // lblFilterText
            // 
            this.lblFilterText.AutoSize = true;
            this.lblFilterText.Location = new System.Drawing.Point(176, 9);
            this.lblFilterText.Name = "lblFilterText";
            this.lblFilterText.Size = new System.Drawing.Size(53, 13);
            this.lblFilterText.TabIndex = 1;
            this.lblFilterText.Text = "Filter Text";
            // 
            // StringsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "StringsViewer";
            this.Size = new System.Drawing.Size(425, 190);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStrings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Generic.BaseGridView gridStrings;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
        private System.Windows.Forms.Label lblFilterText;
        private System.Windows.Forms.TextBox txtFilterText;
    }
}
