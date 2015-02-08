namespace Meridian59.ExampleClient
{
    partial class MainForm
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
            this.LoginControl = new Meridian59.DebugUI.LoginControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonSelectCharacter = new System.Windows.Forms.Button();
            this.CharacterList = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginControl
            // 
            this.LoginControl.ConnectedState = false;
            this.LoginControl.Hostname = "meridian103.openmeridian.org";
            this.LoginControl.Hostport = ((ushort)(5903));
            this.LoginControl.Location = new System.Drawing.Point(12, 0);
            this.LoginControl.Name = "LoginControl";
            this.LoginControl.Password = "";
            this.LoginControl.Size = new System.Drawing.Size(262, 177);
            this.LoginControl.TabIndex = 0;
            this.LoginControl.TabStop = false;
            this.LoginControl.Text = "Login";
            this.LoginControl.Username = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonSelectCharacter);
            this.groupBox1.Controls.Add(this.CharacterList);
            this.groupBox1.Location = new System.Drawing.Point(12, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 154);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Characters";
            // 
            // ButtonSelectCharacter
            // 
            this.ButtonSelectCharacter.Location = new System.Drawing.Point(65, 120);
            this.ButtonSelectCharacter.Name = "ButtonSelectCharacter";
            this.ButtonSelectCharacter.Size = new System.Drawing.Size(144, 23);
            this.ButtonSelectCharacter.TabIndex = 1;
            this.ButtonSelectCharacter.Text = "Select";
            this.ButtonSelectCharacter.UseVisualStyleBackColor = true;
            // 
            // CharacterList
            // 
            this.CharacterList.DisplayMember = "Name";
            this.CharacterList.FormattingEnabled = true;
            this.CharacterList.Location = new System.Drawing.Point(6, 19);
            this.CharacterList.Name = "CharacterList";
            this.CharacterList.Size = new System.Drawing.Size(203, 95);
            this.CharacterList.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 347);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LoginControl);
            this.Name = "MainForm";
            this.Text = "Example M59 Client";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Meridian59.DebugUI.LoginControl LoginControl;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.ListBox CharacterList;
        public System.Windows.Forms.Button ButtonSelectCharacter;

    }
}

