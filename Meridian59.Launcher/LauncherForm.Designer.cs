namespace Meridian59.Launcher
{
    partial class LauncherForm
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOptions = new System.Windows.Forms.Button();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(246, 314);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(75, 23);
            this.btnOptions.TabIndex = 0;
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // cbServer
            // 
            this.cbServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(200, 226);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(121, 21);
            this.cbServer.TabIndex = 1;
            this.cbServer.SelectedIndexChanged += new System.EventHandler(this.cbServer_SelectedIndexChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.BackColor = System.Drawing.Color.Transparent;
            this.lblServer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.Location = new System.Drawing.Point(116, 229);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(55, 13);
            this.lblServer.TabIndex = 3;
            this.lblServer.Text = "Server:";
            this.lblServer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseDown);
            this.lblServer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseMove);
            this.lblServer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseUp);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(200, 253);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(121, 20);
            this.txtUsername.TabIndex = 4;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(200, 279);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(121, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyUp);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(159, 314);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.BackColor = System.Drawing.Color.Transparent;
            this.lblUsername.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(116, 256);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(77, 13);
            this.lblUsername.TabIndex = 7;
            this.lblUsername.Text = "Username:";
            this.lblUsername.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseDown);
            this.lblUsername.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseMove);
            this.lblUsername.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseUp);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(116, 282);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(73, 13);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password:";
            this.lblPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseDown);
            this.lblPassword.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseMove);
            this.lblPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseUp);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(746, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(31, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Meridian59.Launcher.Properties.Resources.Background_LauncherForm;
            this.ClientSize = new System.Drawing.Size(780, 460);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.cbServer);
            this.Controls.Add(this.btnOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LauncherForm";
            this.Text = "Meridian 59 .NET Launcher";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.ComboBox cbServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnClose;
    }
}

