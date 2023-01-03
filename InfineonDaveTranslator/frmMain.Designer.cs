namespace InfineonDaveTranslator
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstMcus = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDaveDir = new System.Windows.Forms.TextBox();
            this.bntDaveDir = new System.Windows.Forms.Button();
            this.btnDataRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExport = new System.Windows.Forms.TextBox();
            this.btnBrowseExport = new System.Windows.Forms.Button();
            this.chkQfn = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkTssop = new System.Windows.Forms.CheckBox();
            this.chkLQFP = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 458);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.chkLQFP);
            this.tabPage1.Controls.Add(this.chkTssop);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.chkQfn);
            this.tabPage1.Controls.Add(this.lstMcus);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 430);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Microcontrollers";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnBrowseExport);
            this.tabPage2.Controls.Add(this.txtExport);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.btnDataRefresh);
            this.tabPage2.Controls.Add(this.bntDaveDir);
            this.tabPage2.Controls.Add(this.txtDaveDir);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 430);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data Source";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstMcus
            // 
            this.lstMcus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstMcus.FormattingEnabled = true;
            this.lstMcus.ItemHeight = 15;
            this.lstMcus.Location = new System.Drawing.Point(8, 71);
            this.lstMcus.Name = "lstMcus";
            this.lstMcus.Size = new System.Drawing.Size(172, 349);
            this.lstMcus.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "DAVE IDE Directory";
            // 
            // txtDaveDir
            // 
            this.txtDaveDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDaveDir.Location = new System.Drawing.Point(8, 21);
            this.txtDaveDir.Name = "txtDaveDir";
            this.txtDaveDir.Size = new System.Drawing.Size(661, 23);
            this.txtDaveDir.TabIndex = 1;
            this.txtDaveDir.Text = "C:\\Infineon\\Tools\\DAVE IDE\\";
            // 
            // bntDaveDir
            // 
            this.bntDaveDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bntDaveDir.Location = new System.Drawing.Point(675, 21);
            this.bntDaveDir.Name = "bntDaveDir";
            this.bntDaveDir.Size = new System.Drawing.Size(28, 23);
            this.bntDaveDir.TabIndex = 2;
            this.bntDaveDir.Text = "...";
            this.bntDaveDir.UseVisualStyleBackColor = true;
            this.bntDaveDir.Click += new System.EventHandler(this.bntDaveDir_Click);
            // 
            // btnDataRefresh
            // 
            this.btnDataRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataRefresh.Location = new System.Drawing.Point(709, 21);
            this.btnDataRefresh.Name = "btnDataRefresh";
            this.btnDataRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnDataRefresh.TabIndex = 3;
            this.btnDataRefresh.Text = "Refresh";
            this.btnDataRefresh.UseVisualStyleBackColor = true;
            this.btnDataRefresh.Click += new System.EventHandler(this.btnDataRefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Export Directory";
            // 
            // txtExport
            // 
            this.txtExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExport.Location = new System.Drawing.Point(8, 65);
            this.txtExport.Name = "txtExport";
            this.txtExport.Size = new System.Drawing.Size(742, 23);
            this.txtExport.TabIndex = 5;
            this.txtExport.Text = "C:\\temp\\common";
            // 
            // btnBrowseExport
            // 
            this.btnBrowseExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseExport.Location = new System.Drawing.Point(756, 65);
            this.btnBrowseExport.Name = "btnBrowseExport";
            this.btnBrowseExport.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseExport.TabIndex = 6;
            this.btnBrowseExport.Text = "...";
            this.btnBrowseExport.UseVisualStyleBackColor = true;
            this.btnBrowseExport.Click += new System.EventHandler(this.btnBrowseExport_Click);
            // 
            // chkQfn
            // 
            this.chkQfn.AutoSize = true;
            this.chkQfn.Checked = true;
            this.chkQfn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQfn.Location = new System.Drawing.Point(8, 21);
            this.chkQfn.Name = "chkQfn";
            this.chkQfn.Size = new System.Drawing.Size(57, 19);
            this.chkQfn.TabIndex = 1;
            this.chkQfn.Text = "VQFN";
            this.chkQfn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(8, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Show";
            // 
            // chkTssop
            // 
            this.chkTssop.AutoSize = true;
            this.chkTssop.Checked = true;
            this.chkTssop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTssop.Location = new System.Drawing.Point(8, 46);
            this.chkTssop.Name = "chkTssop";
            this.chkTssop.Size = new System.Drawing.Size(60, 19);
            this.chkTssop.TabIndex = 3;
            this.chkTssop.Text = "TSSOP";
            this.chkTssop.UseVisualStyleBackColor = true;
            // 
            // chkLQFP
            // 
            this.chkLQFP.AutoSize = true;
            this.chkLQFP.Checked = true;
            this.chkLQFP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLQFP.Location = new System.Drawing.Point(126, 21);
            this.chkLQFP.Name = "chkLQFP";
            this.chkLQFP.Size = new System.Drawing.Size(54, 19);
            this.chkLQFP.TabIndex = 4;
            this.chkLQFP.Text = "LQFP";
            this.chkLQFP.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(186, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(598, 414);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MCU Data";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 458);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.Text = "Infineon DAVE Translator";
            this.Load += new System.EventHandler(this.btnDataRefresh_Click);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListBox lstMcus;
        private Button btnBrowseExport;
        private TextBox txtExport;
        private Label label2;
        private Button btnDataRefresh;
        private Button bntDaveDir;
        private TextBox txtDaveDir;
        private Label label1;
        private CheckBox chkLQFP;
        private CheckBox chkTssop;
        private Label label3;
        private CheckBox chkQfn;
        private GroupBox groupBox1;
    }
}