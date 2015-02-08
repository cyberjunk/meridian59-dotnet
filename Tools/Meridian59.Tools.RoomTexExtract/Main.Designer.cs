namespace Meridian59.Tools.RoomTexExtract
{
    partial class Main
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
            this.diagBGFFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectRoom = new System.Windows.Forms.Button();
            this.btnSelectBGFFolder = new System.Windows.Forms.Button();
            this.txtRoomFile = new System.Windows.Forms.TextBox();
            this.lblRoomFile = new System.Windows.Forms.Label();
            this.groupRoom = new System.Windows.Forms.GroupBox();
            this.groupBGFFolder = new System.Windows.Forms.GroupBox();
            this.groupOutputFolder = new System.Windows.Forms.GroupBox();
            this.txtBGFFolder = new System.Windows.Forms.TextBox();
            this.lblBGFFolder = new System.Windows.Forms.Label();
            this.diagRoom = new System.Windows.Forms.OpenFileDialog();
            this.diagOutputFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.btnSelectOutputFolder = new System.Windows.Forms.Button();
            this.btnGO = new System.Windows.Forms.Button();
            this.groupRoom.SuspendLayout();
            this.groupBGFFolder.SuspendLayout();
            this.groupOutputFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectRoom
            // 
            this.btnSelectRoom.Location = new System.Drawing.Point(229, 20);
            this.btnSelectRoom.Name = "btnSelectRoom";
            this.btnSelectRoom.Size = new System.Drawing.Size(75, 23);
            this.btnSelectRoom.TabIndex = 0;
            this.btnSelectRoom.Text = "Select";
            this.btnSelectRoom.UseVisualStyleBackColor = true;
            this.btnSelectRoom.Click += new System.EventHandler(this.btnSelectRoom_Click);
            // 
            // btnSelectBGFFolder
            // 
            this.btnSelectBGFFolder.Location = new System.Drawing.Point(229, 19);
            this.btnSelectBGFFolder.Name = "btnSelectBGFFolder";
            this.btnSelectBGFFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectBGFFolder.TabIndex = 1;
            this.btnSelectBGFFolder.Text = "Select";
            this.btnSelectBGFFolder.UseVisualStyleBackColor = true;
            this.btnSelectBGFFolder.Click += new System.EventHandler(this.btnSelectBGFFolder_Click);
            // 
            // txtRoomFile
            // 
            this.txtRoomFile.Location = new System.Drawing.Point(55, 20);
            this.txtRoomFile.Name = "txtRoomFile";
            this.txtRoomFile.Size = new System.Drawing.Size(168, 20);
            this.txtRoomFile.TabIndex = 2;
            // 
            // lblRoomFile
            // 
            this.lblRoomFile.AutoSize = true;
            this.lblRoomFile.Location = new System.Drawing.Point(23, 23);
            this.lblRoomFile.Name = "lblRoomFile";
            this.lblRoomFile.Size = new System.Drawing.Size(26, 13);
            this.lblRoomFile.TabIndex = 3;
            this.lblRoomFile.Text = "File:";
            // 
            // groupRoom
            // 
            this.groupRoom.Controls.Add(this.txtRoomFile);
            this.groupRoom.Controls.Add(this.lblRoomFile);
            this.groupRoom.Controls.Add(this.btnSelectRoom);
            this.groupRoom.Location = new System.Drawing.Point(12, 12);
            this.groupRoom.Name = "groupRoom";
            this.groupRoom.Size = new System.Drawing.Size(333, 56);
            this.groupRoom.TabIndex = 4;
            this.groupRoom.TabStop = false;
            this.groupRoom.Text = "1. Select room";
            // 
            // groupBGFFolder
            // 
            this.groupBGFFolder.Controls.Add(this.txtBGFFolder);
            this.groupBGFFolder.Controls.Add(this.lblBGFFolder);
            this.groupBGFFolder.Controls.Add(this.btnSelectBGFFolder);
            this.groupBGFFolder.Enabled = false;
            this.groupBGFFolder.Location = new System.Drawing.Point(12, 74);
            this.groupBGFFolder.Name = "groupBGFFolder";
            this.groupBGFFolder.Size = new System.Drawing.Size(333, 61);
            this.groupBGFFolder.TabIndex = 5;
            this.groupBGFFolder.TabStop = false;
            this.groupBGFFolder.Text = "2. Select BGF folder";
            // 
            // groupOutputFolder
            // 
            this.groupOutputFolder.Controls.Add(this.txtOutputFolder);
            this.groupOutputFolder.Controls.Add(this.lblOutputFolder);
            this.groupOutputFolder.Controls.Add(this.btnSelectOutputFolder);
            this.groupOutputFolder.Enabled = false;
            this.groupOutputFolder.Location = new System.Drawing.Point(12, 141);
            this.groupOutputFolder.Name = "groupOutputFolder";
            this.groupOutputFolder.Size = new System.Drawing.Size(333, 61);
            this.groupOutputFolder.TabIndex = 6;
            this.groupOutputFolder.TabStop = false;
            this.groupOutputFolder.Text = "3. Select Output folder";
            // 
            // txtBGFFolder
            // 
            this.txtBGFFolder.Location = new System.Drawing.Point(55, 21);
            this.txtBGFFolder.Name = "txtBGFFolder";
            this.txtBGFFolder.Size = new System.Drawing.Size(168, 20);
            this.txtBGFFolder.TabIndex = 4;
            // 
            // lblBGFFolder
            // 
            this.lblBGFFolder.AutoSize = true;
            this.lblBGFFolder.Location = new System.Drawing.Point(10, 24);
            this.lblBGFFolder.Name = "lblBGFFolder";
            this.lblBGFFolder.Size = new System.Drawing.Size(39, 13);
            this.lblBGFFolder.TabIndex = 5;
            this.lblBGFFolder.Text = "Folder:";
            // 
            // diagRoom
            // 
            this.diagRoom.FileName = "openFileDialog1";
            this.diagRoom.Filter = "Meridian 59 room|*.ROO";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(55, 22);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(168, 20);
            this.txtOutputFolder.TabIndex = 7;
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(10, 24);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(39, 13);
            this.lblOutputFolder.TabIndex = 8;
            this.lblOutputFolder.Text = "Folder:";
            // 
            // btnSelectOutputFolder
            // 
            this.btnSelectOutputFolder.Location = new System.Drawing.Point(229, 19);
            this.btnSelectOutputFolder.Name = "btnSelectOutputFolder";
            this.btnSelectOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectOutputFolder.TabIndex = 6;
            this.btnSelectOutputFolder.Text = "Select";
            this.btnSelectOutputFolder.UseVisualStyleBackColor = true;
            this.btnSelectOutputFolder.Click += new System.EventHandler(this.btnSelectOutputFolder_Click);
            // 
            // btnGO
            // 
            this.btnGO.Enabled = false;
            this.btnGO.Location = new System.Drawing.Point(133, 208);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(75, 23);
            this.btnGO.TabIndex = 7;
            this.btnGO.Text = "EXTRACT";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 247);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.groupOutputFolder);
            this.Controls.Add(this.groupBGFFolder);
            this.Controls.Add(this.groupRoom);
            this.Name = "Main";
            this.Text = "Room BGF Extractor";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupRoom.ResumeLayout(false);
            this.groupRoom.PerformLayout();
            this.groupBGFFolder.ResumeLayout(false);
            this.groupBGFFolder.PerformLayout();
            this.groupOutputFolder.ResumeLayout(false);
            this.groupOutputFolder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog diagBGFFolder;
        private System.Windows.Forms.Button btnSelectRoom;
        private System.Windows.Forms.Button btnSelectBGFFolder;
        private System.Windows.Forms.TextBox txtRoomFile;
        private System.Windows.Forms.Label lblRoomFile;
        private System.Windows.Forms.GroupBox groupRoom;
        private System.Windows.Forms.GroupBox groupBGFFolder;
        private System.Windows.Forms.TextBox txtBGFFolder;
        private System.Windows.Forms.Label lblBGFFolder;
        private System.Windows.Forms.GroupBox groupOutputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.Button btnSelectOutputFolder;
        private System.Windows.Forms.OpenFileDialog diagRoom;
        private System.Windows.Forms.FolderBrowserDialog diagOutputFolder;
        private System.Windows.Forms.Button btnGO;
    }
}

