namespace Meridian59.AdminUI.Viewers
{
    partial class AdminInfoObjectView
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
            this.gridProperties = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colPropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropertyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbProperties = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.gbProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridProperties
            // 
            this.gridProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPropertyName,
            this.colPropertyType,
            this.colPropertyValue});
            this.gridProperties.Location = new System.Drawing.Point(3, 16);
            this.gridProperties.Name = "gridProperties";
            this.gridProperties.Size = new System.Drawing.Size(313, 337);
            this.gridProperties.TabIndex = 0;
            // 
            // colPropertyName
            // 
            this.colPropertyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPropertyName.DataPropertyName = "PropertyName";
            this.colPropertyName.HeaderText = "PropertyName";
            this.colPropertyName.Name = "colPropertyName";
            this.colPropertyName.ReadOnly = true;
            // 
            // colPropertyType
            // 
            this.colPropertyType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPropertyType.DataPropertyName = "PropertyType";
            this.colPropertyType.HeaderText = "PropertyType";
            this.colPropertyType.Name = "colPropertyType";
            this.colPropertyType.ReadOnly = true;
            // 
            // colPropertyValue
            // 
            this.colPropertyValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPropertyValue.DataPropertyName = "PropertyValue";
            this.colPropertyValue.HeaderText = "PropertyValue";
            this.colPropertyValue.Name = "colPropertyValue";
            this.colPropertyValue.ReadOnly = true;
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
            this.splitContainer1.Panel1.Controls.Add(this.gbActions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbProperties);
            this.splitContainer1.Size = new System.Drawing.Size(319, 410);
            this.splitContainer1.TabIndex = 1;
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnClose);
            this.gbActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbActions.Location = new System.Drawing.Point(0, 0);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(319, 50);
            this.gbActions.TabIndex = 0;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(118, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // gbProperties
            // 
            this.gbProperties.Controls.Add(this.gridProperties);
            this.gbProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbProperties.Location = new System.Drawing.Point(0, 0);
            this.gbProperties.Name = "gbProperties";
            this.gbProperties.Size = new System.Drawing.Size(319, 356);
            this.gbProperties.TabIndex = 1;
            this.gbProperties.TabStop = false;
            this.gbProperties.Text = "Properties";
            // 
            // AdminInfoObjectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "AdminInfoObjectView";
            this.Size = new System.Drawing.Size(319, 410);
            ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.gbProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Generic.BaseGridView gridProperties;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertyValue;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbProperties;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Button btnClose;
    }
}
