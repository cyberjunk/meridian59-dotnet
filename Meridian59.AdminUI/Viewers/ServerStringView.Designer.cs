namespace Meridian59.AdminUI.Viewers
{
    partial class ServerStringView
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
            this.gridChatMessages = new Meridian59.AdminUI.Generic.BaseGridView();
            this.colResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridChatMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // gridChatMessages
            // 
            this.gridChatMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridChatMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResourceID,
            this.colResource,
            this.colFullString});
            this.gridChatMessages.Location = new System.Drawing.Point(0, 0);
            this.gridChatMessages.Name = "gridChatMessages";
            this.gridChatMessages.Size = new System.Drawing.Size(474, 206);
            this.gridChatMessages.TabIndex = 0;
            // 
            // colResourceID
            // 
            this.colResourceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colResourceID.DataPropertyName = "ResourceID";
            this.colResourceID.HeaderText = "RID";
            this.colResourceID.Name = "colResourceID";
            this.colResourceID.ReadOnly = true;
            // 
            // colResource
            // 
            this.colResource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colResource.DataPropertyName = "ResourceName";
            this.colResource.HeaderText = "ROOT";
            this.colResource.Name = "colResource";
            this.colResource.ReadOnly = true;
            // 
            // colFullString
            // 
            this.colFullString.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFullString.DataPropertyName = "FullString";
            this.colFullString.HeaderText = "FULL";
            this.colFullString.Name = "colFullString";
            this.colFullString.ReadOnly = true;
            // 
            // ChatMessageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridChatMessages);
            this.Name = "ChatMessageView";
            this.Size = new System.Drawing.Size(474, 206);
            ((System.ComponentModel.ISupportInitialize)(this.gridChatMessages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Generic.BaseGridView gridChatMessages;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullString;
    }
}
