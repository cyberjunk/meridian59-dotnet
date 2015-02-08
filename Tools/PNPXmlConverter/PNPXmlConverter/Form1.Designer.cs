namespace PNPXmlConverter
{
    partial class Form1
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
            this.btnSelectGrass = new System.Windows.Forms.Button();
            this.openFileDialogGrass = new System.Windows.Forms.OpenFileDialog();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkGrassShadows = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectTrees = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.chkTreeShadows = new System.Windows.Forms.CheckBox();
            this.txtTreeFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGrassFilename = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFileDialogTrees = new System.Windows.Forms.OpenFileDialog();
            this.txtWorldSize = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectGrass
            // 
            this.btnSelectGrass.Location = new System.Drawing.Point(419, 13);
            this.btnSelectGrass.Name = "btnSelectGrass";
            this.btnSelectGrass.Size = new System.Drawing.Size(75, 23);
            this.btnSelectGrass.TabIndex = 0;
            this.btnSelectGrass.Text = "Select";
            this.btnSelectGrass.UseVisualStyleBackColor = true;
            this.btnSelectGrass.Click += new System.EventHandler(this.btnSelectGrass_Click);
            // 
            // openFileDialogGrass
            // 
            this.openFileDialogGrass.FileName = "grass.xml";
            this.openFileDialogGrass.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogGrass_FileOk);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(37, 195);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(506, 203);
            this.txtLog.TabIndex = 2;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(338, 24);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 3;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(65, 26);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(65, 20);
            this.txtScale.TabIndex = 4;
            this.txtScale.Text = "1.0";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(191, 26);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(52, 20);
            this.txtX.TabIndex = 5;
            this.txtX.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Scale";
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(269, 26);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(57, 20);
            this.txtZ.TabIndex = 8;
            this.txtZ.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Z";
            // 
            // chkGrassShadows
            // 
            this.chkGrassShadows.AutoSize = true;
            this.chkGrassShadows.Location = new System.Drawing.Point(343, 22);
            this.chkGrassShadows.Name = "chkGrassShadows";
            this.chkGrassShadows.Size = new System.Drawing.Size(70, 17);
            this.chkGrassShadows.TabIndex = 11;
            this.chkGrassShadows.Text = "Shadows";
            this.chkGrassShadows.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectTrees);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkTreeShadows);
            this.groupBox1.Controls.Add(this.txtTreeFilename);
            this.groupBox1.Controls.Add(this.chkGrassShadows);
            this.groupBox1.Controls.Add(this.btnSelectGrass);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtGrassFilename);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 70);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input files";
            // 
            // btnSelectTrees
            // 
            this.btnSelectTrees.Location = new System.Drawing.Point(419, 43);
            this.btnSelectTrees.Name = "btnSelectTrees";
            this.btnSelectTrees.Size = new System.Drawing.Size(75, 23);
            this.btnSelectTrees.TabIndex = 14;
            this.btnSelectTrees.Text = "Select";
            this.btnSelectTrees.UseVisualStyleBackColor = true;
            this.btnSelectTrees.Click += new System.EventHandler(this.btnSelectTrees_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Trees";
            // 
            // chkTreeShadows
            // 
            this.chkTreeShadows.AutoSize = true;
            this.chkTreeShadows.Location = new System.Drawing.Point(343, 45);
            this.chkTreeShadows.Name = "chkTreeShadows";
            this.chkTreeShadows.Size = new System.Drawing.Size(70, 17);
            this.chkTreeShadows.TabIndex = 12;
            this.chkTreeShadows.Text = "Shadows";
            this.chkTreeShadows.UseVisualStyleBackColor = true;
            // 
            // txtTreeFilename
            // 
            this.txtTreeFilename.Location = new System.Drawing.Point(65, 45);
            this.txtTreeFilename.Name = "txtTreeFilename";
            this.txtTreeFilename.Size = new System.Drawing.Size(272, 20);
            this.txtTreeFilename.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Grass";
            // 
            // txtGrassFilename
            // 
            this.txtGrassFilename.Location = new System.Drawing.Point(65, 19);
            this.txtGrassFilename.Name = "txtGrassFilename";
            this.txtGrassFilename.Size = new System.Drawing.Size(272, 20);
            this.txtGrassFilename.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtWorldSize);
            this.groupBox2.Controls.Add(this.txtScale);
            this.groupBox2.Controls.Add(this.txtX);
            this.groupBox2.Controls.Add(this.btnConvert);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtZ);
            this.groupBox2.Location = new System.Drawing.Point(12, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(531, 88);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transformation";
            // 
            // openFileDialogTrees
            // 
            this.openFileDialogTrees.FileName = "grass.xml";
            this.openFileDialogTrees.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogTrees_FileOk);
            // 
            // txtWorldSize
            // 
            this.txtWorldSize.Location = new System.Drawing.Point(65, 53);
            this.txtWorldSize.Name = "txtWorldSize";
            this.txtWorldSize.Size = new System.Drawing.Size(65, 20);
            this.txtWorldSize.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "WorldSize";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 432);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLog);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectGrass;
        private System.Windows.Forms.OpenFileDialog openFileDialogGrass;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkGrassShadows;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkTreeShadows;
        private System.Windows.Forms.TextBox txtTreeFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGrassFilename;
        private System.Windows.Forms.Button btnSelectTrees;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialogTrees;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWorldSize;
    }
}

