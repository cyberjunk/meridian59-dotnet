using System.Drawing;
namespace Meridian59.AdminUI.Viewers
{
    partial class AdminInfoView
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.gbObjects = new System.Windows.Forms.GroupBox();
            this.tabObjects = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbObjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbObjects);
            this.splitContainer1.Size = new System.Drawing.Size(755, 337);
            this.splitContainer1.SplitterDistance = 363;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 337);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtCommand);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtConsole);
            this.splitContainer2.Size = new System.Drawing.Size(357, 318);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 0;
            // 
            // txtCommand
            // 
            this.txtCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommand.Location = new System.Drawing.Point(0, 0);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(357, 20);
            this.txtCommand.TabIndex = 0;
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCommandKeyDown);
            this.txtCommand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnCommandKeyPress);
            this.txtCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnCommandKeyUp);
            // 
            // txtConsole
            // 
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.txtConsole.Location = new System.Drawing.Point(0, 0);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.Size = new System.Drawing.Size(357, 289);
            this.txtConsole.TabIndex = 0;
            this.txtConsole.Text = "";
            // 
            // gbObjects
            // 
            this.gbObjects.Controls.Add(this.tabObjects);
            this.gbObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbObjects.Location = new System.Drawing.Point(0, 0);
            this.gbObjects.Name = "gbObjects";
            this.gbObjects.Size = new System.Drawing.Size(388, 337);
            this.gbObjects.TabIndex = 1;
            this.gbObjects.TabStop = false;
            this.gbObjects.Text = "Objects";
            // 
            // tabObjects
            // 
            this.tabObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabObjects.Location = new System.Drawing.Point(3, 16);
            this.tabObjects.Name = "tabObjects";
            this.tabObjects.SelectedIndex = 0;
            this.tabObjects.Size = new System.Drawing.Size(382, 318);
            this.tabObjects.TabIndex = 0;
            // 
            // AdminInfoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "AdminInfoView";
            this.Size = new System.Drawing.Size(755, 337);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.gbObjects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.GroupBox gbObjects;
        private System.Windows.Forms.TabControl tabObjects;
    }
}
