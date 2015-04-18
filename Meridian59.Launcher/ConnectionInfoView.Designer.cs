namespace Meridian59.Launcher.Controls
{
    partial class ConnectionInfoView
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
            this.txtHost = new System.Windows.Forms.TextBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblHostDesc = new System.Windows.Forms.Label();
            this.lblPortDesc = new System.Windows.Forms.Label();
            this.lblNameDesc = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbStringDictionary = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkUseIPv6 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(92, 41);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(151, 20);
            this.txtHost.TabIndex = 0;
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(92, 68);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(55, 20);
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] {
            5959,
            0,
            0,
            0});
            // 
            // lblHostDesc
            // 
            this.lblHostDesc.AutoSize = true;
            this.lblHostDesc.Location = new System.Drawing.Point(42, 44);
            this.lblHostDesc.Name = "lblHostDesc";
            this.lblHostDesc.Size = new System.Drawing.Size(32, 13);
            this.lblHostDesc.TabIndex = 2;
            this.lblHostDesc.Text = "Host:";
            // 
            // lblPortDesc
            // 
            this.lblPortDesc.AutoSize = true;
            this.lblPortDesc.Location = new System.Drawing.Point(45, 70);
            this.lblPortDesc.Name = "lblPortDesc";
            this.lblPortDesc.Size = new System.Drawing.Size(29, 13);
            this.lblPortDesc.TabIndex = 3;
            this.lblPortDesc.Text = "Port:";
            // 
            // lblNameDesc
            // 
            this.lblNameDesc.AutoSize = true;
            this.lblNameDesc.Location = new System.Drawing.Point(36, 15);
            this.lblNameDesc.Name = "lblNameDesc";
            this.lblNameDesc.Size = new System.Drawing.Size(38, 13);
            this.lblNameDesc.TabIndex = 4;
            this.lblNameDesc.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(92, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(151, 20);
            this.txtName.TabIndex = 5;
            // 
            // cbStringDictionary
            // 
            this.cbStringDictionary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStringDictionary.FormattingEnabled = true;
            this.cbStringDictionary.Location = new System.Drawing.Point(92, 94);
            this.cbStringDictionary.Name = "cbStringDictionary";
            this.cbStringDictionary.Size = new System.Drawing.Size(151, 21);
            this.cbStringDictionary.TabIndex = 6;
            this.cbStringDictionary.SelectionChangeCommitted += new System.EventHandler(this.cbStringDictionary_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Dictionary:";
            // 
            // chkUseIPv6
            // 
            this.chkUseIPv6.AutoSize = true;
            this.chkUseIPv6.Location = new System.Drawing.Point(173, 69);
            this.chkUseIPv6.Name = "chkUseIPv6";
            this.chkUseIPv6.Size = new System.Drawing.Size(70, 17);
            this.chkUseIPv6.TabIndex = 8;
            this.chkUseIPv6.Text = "Use IPv6";
            this.chkUseIPv6.UseVisualStyleBackColor = true;
            // 
            // ConnectionInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkUseIPv6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbStringDictionary);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblNameDesc);
            this.Controls.Add(this.lblPortDesc);
            this.Controls.Add(this.lblHostDesc);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.txtHost);
            this.Name = "ConnectionInfoView";
            this.Size = new System.Drawing.Size(267, 136);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblHostDesc;
        private System.Windows.Forms.Label lblPortDesc;
        private System.Windows.Forms.Label lblNameDesc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cbStringDictionary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUseIPv6;
    }
}
