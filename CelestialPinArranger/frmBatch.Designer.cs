﻿
namespace CelestialPinArranger
{
    partial class frmBatch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatch));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnProcess = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDestDirectory = new System.Windows.Forms.Button();
            this.txtDestinationDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSourceDirectory = new System.Windows.Forms.Button();
            this.txtSourceDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFormatFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnImgsDir = new System.Windows.Forms.Button();
            this.txtImagesDir = new System.Windows.Forms.TextBox();
            this.chkImages = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboJson = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtManufacturerName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblProgress);
            this.panel1.Controls.Add(this.pbProgress);
            this.panel1.Controls.Add(this.btnProcess);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 425);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 52);
            this.panel1.TabIndex = 0;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(237, 17);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(60, 15);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "9999/9999";
            this.lblProgress.Visible = false;
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(14, 12);
            this.pbProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(216, 27);
            this.pbProgress.TabIndex = 1;
            this.pbProgress.Visible = false;
            // 
            // btnProcess
            // 
            this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcess.Location = new System.Drawing.Point(332, 12);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(88, 27);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnDestDirectory);
            this.groupBox1.Controls.Add(this.txtDestinationDir);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSourceDirectory);
            this.groupBox1.Controls.Add(this.txtSourceDir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(406, 123);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Directories";
            // 
            // btnDestDirectory
            // 
            this.btnDestDirectory.Location = new System.Drawing.Point(366, 80);
            this.btnDestDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDestDirectory.Name = "btnDestDirectory";
            this.btnDestDirectory.Size = new System.Drawing.Size(33, 27);
            this.btnDestDirectory.TabIndex = 5;
            this.btnDestDirectory.Text = "...";
            this.btnDestDirectory.UseVisualStyleBackColor = true;
            this.btnDestDirectory.Click += new System.EventHandler(this.btnDestDirectory_Click);
            // 
            // txtDestinationDir
            // 
            this.txtDestinationDir.Location = new System.Drawing.Point(7, 82);
            this.txtDestinationDir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDestinationDir.Name = "txtDestinationDir";
            this.txtDestinationDir.Size = new System.Drawing.Size(352, 23);
            this.txtDestinationDir.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination";
            // 
            // btnSourceDirectory
            // 
            this.btnSourceDirectory.Location = new System.Drawing.Point(366, 35);
            this.btnSourceDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSourceDirectory.Name = "btnSourceDirectory";
            this.btnSourceDirectory.Size = new System.Drawing.Size(33, 27);
            this.btnSourceDirectory.TabIndex = 2;
            this.btnSourceDirectory.Text = "...";
            this.btnSourceDirectory.UseVisualStyleBackColor = true;
            this.btnSourceDirectory.Click += new System.EventHandler(this.btnSourceDirectory_Click);
            // 
            // txtSourceDir
            // 
            this.txtSourceDir.Location = new System.Drawing.Point(7, 37);
            this.txtSourceDir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSourceDir.Name = "txtSourceDir";
            this.txtSourceDir.Size = new System.Drawing.Size(352, 23);
            this.txtSourceDir.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtFormatFolder);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnImgsDir);
            this.groupBox2.Controls.Add(this.txtImagesDir);
            this.groupBox2.Controls.Add(this.chkImages);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cboJson);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(14, 144);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(406, 193);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Processing";
            // 
            // txtFormatFolder
            // 
            this.txtFormatFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormatFolder.Location = new System.Drawing.Point(7, 83);
            this.txtFormatFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtFormatFolder.Name = "txtFormatFolder";
            this.txtFormatFolder.Size = new System.Drawing.Size(391, 23);
            this.txtFormatFolder.TabIndex = 9;
            this.txtFormatFolder.Text = "ARM CORTEX";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 65);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(239, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Library Formatting Destination Folder Name";
            // 
            // btnImgsDir
            // 
            this.btnImgsDir.Location = new System.Drawing.Point(366, 152);
            this.btnImgsDir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnImgsDir.Name = "btnImgsDir";
            this.btnImgsDir.Size = new System.Drawing.Size(33, 27);
            this.btnImgsDir.TabIndex = 7;
            this.btnImgsDir.Text = "...";
            this.btnImgsDir.UseVisualStyleBackColor = true;
            this.btnImgsDir.Click += new System.EventHandler(this.btnImgsDir_Click);
            // 
            // txtImagesDir
            // 
            this.txtImagesDir.Location = new System.Drawing.Point(7, 155);
            this.txtImagesDir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtImagesDir.Name = "txtImagesDir";
            this.txtImagesDir.Size = new System.Drawing.Size(352, 23);
            this.txtImagesDir.TabIndex = 6;
            // 
            // chkImages
            // 
            this.chkImages.AutoSize = true;
            this.chkImages.Checked = true;
            this.chkImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImages.Location = new System.Drawing.Point(7, 113);
            this.chkImages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkImages.Name = "chkImages";
            this.chkImages.Size = new System.Drawing.Size(114, 19);
            this.chkImages.TabIndex = 3;
            this.chkImages.Text = "Generate Images";
            this.chkImages.UseVisualStyleBackColor = true;
            this.chkImages.CheckedChanged += new System.EventHandler(this.chkImages_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 136);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Images Subfolder";
            // 
            // cboJson
            // 
            this.cboJson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboJson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboJson.FormattingEnabled = true;
            this.cboJson.Location = new System.Drawing.Point(7, 37);
            this.cboJson.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboJson.Name = "cboJson";
            this.cboJson.Size = new System.Drawing.Size(391, 23);
            this.cboJson.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mapper JSON";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtManufacturerName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(14, 344);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(406, 74);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Manufacturer";
            // 
            // txtManufacturerName
            // 
            this.txtManufacturerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtManufacturerName.Location = new System.Drawing.Point(7, 37);
            this.txtManufacturerName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtManufacturerName.Name = "txtManufacturerName";
            this.txtManufacturerName.Size = new System.Drawing.Size(391, 23);
            this.txtManufacturerName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Name";
            // 
            // frmBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 477);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatch";
            this.Text = "Batch Processor";
            this.Load += new System.EventHandler(this.frmBatch_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDestDirectory;
        private System.Windows.Forms.TextBox txtDestinationDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSourceDirectory;
        private System.Windows.Forms.TextBox txtSourceDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnImgsDir;
        private System.Windows.Forms.TextBox txtImagesDir;
        private System.Windows.Forms.CheckBox chkImages;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboJson;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtManufacturerName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFormatFolder;
        private System.Windows.Forms.Label label6;
    }
}