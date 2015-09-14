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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.gbSendCommand = new System.Windows.Forms.GroupBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.gbObjects = new System.Windows.Forms.GroupBox();
            this.tabObjects = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.gbSendCommand.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.gbOutput.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbObjects);
            this.splitContainer1.Size = new System.Drawing.Size(755, 337);
            this.splitContainer1.SplitterDistance = 363;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gbOutput);
            this.splitContainer2.Size = new System.Drawing.Size(363, 337);
            this.splitContainer2.SplitterDistance = 40;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.gbSendCommand);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.gbActions);
            this.splitContainer3.Size = new System.Drawing.Size(363, 40);
            this.splitContainer3.SplitterDistance = 281;
            this.splitContainer3.TabIndex = 5;
            // 
            // gbSendCommand
            // 
            this.gbSendCommand.Controls.Add(this.txtCommand);
            this.gbSendCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSendCommand.Location = new System.Drawing.Point(0, 0);
            this.gbSendCommand.Name = "gbSendCommand";
            this.gbSendCommand.Size = new System.Drawing.Size(281, 40);
            this.gbSendCommand.TabIndex = 0;
            this.gbSendCommand.TabStop = false;
            this.gbSendCommand.Text = "Send";
            // 
            // txtCommand
            // 
            this.txtCommand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtCommand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommand.Location = new System.Drawing.Point(3, 16);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(275, 20);
            this.txtCommand.TabIndex = 0;
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCommandKeyDown);
            this.txtCommand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnCommandKeyPress);
            this.txtCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnCommandKeyUp);
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnClearLog);
            this.gbActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbActions.Location = new System.Drawing.Point(0, 0);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(78, 40);
            this.gbActions.TabIndex = 0;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearLog.Location = new System.Drawing.Point(3, 16);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(72, 21);
            this.btnClearLog.TabIndex = 0;
            this.btnClearLog.Text = "Clear log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.OnClearLogClick);
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.txtConsole);
            this.gbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOutput.Location = new System.Drawing.Point(0, 0);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(363, 293);
            this.gbOutput.TabIndex = 1;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = "Output";
            // 
            // txtConsole
            // 
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.txtConsole.Location = new System.Drawing.Point(3, 16);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.Size = new System.Drawing.Size(357, 274);
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
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.gbSendCommand.ResumeLayout(false);
            this.gbSendCommand.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.gbOutput.ResumeLayout(false);
            this.gbObjects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.GroupBox gbObjects;
        private System.Windows.Forms.TabControl tabObjects;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox gbSendCommand;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.GroupBox gbOutput;
        private System.Windows.Forms.RichTextBox txtConsole;
    }
}
