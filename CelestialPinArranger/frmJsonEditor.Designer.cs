
namespace CelestialPinArranger
{
    partial class frmJsonEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLoadJson = new System.Windows.Forms.Button();
            this.btnNewMapper = new System.Windows.Forms.Button();
            this.btnDefaultMapper = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabMapperDef = new System.Windows.Forms.TabPage();
            this.tabTestResult = new System.Windows.Forms.TabPage();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTestFile = new System.Windows.Forms.TextBox();
            this.btnTestFilePath = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveMapper = new System.Windows.Forms.Button();
            this.dgFunctions = new System.Windows.Forms.DataGridView();
            this.lnkRegexRef = new System.Windows.Forms.LinkLabel();
            this.txtUnmappedPins = new System.Windows.Forms.TextBox();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPinClass = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cRegex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSide = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cAlignment = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cGroupSpacing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSuccessResults = new System.Windows.Forms.DataGridView();
            this.cMappedFunction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPinName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPortBit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabMapperDef.SuspendLayout();
            this.tabTestResult.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFunctions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSuccessResults)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveMapper);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.btnValidate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 564);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 47);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLoadJson);
            this.panel2.Controls.Add(this.btnNewMapper);
            this.panel2.Controls.Add(this.btnDefaultMapper);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(984, 50);
            this.panel2.TabIndex = 3;
            // 
            // btnLoadJson
            // 
            this.btnLoadJson.Location = new System.Drawing.Point(264, 12);
            this.btnLoadJson.Name = "btnLoadJson";
            this.btnLoadJson.Size = new System.Drawing.Size(120, 23);
            this.btnLoadJson.TabIndex = 5;
            this.btnLoadJson.Text = "Load JSON";
            this.btnLoadJson.UseVisualStyleBackColor = true;
            this.btnLoadJson.Click += new System.EventHandler(this.btnLoadJson_Click);
            // 
            // btnNewMapper
            // 
            this.btnNewMapper.Location = new System.Drawing.Point(12, 12);
            this.btnNewMapper.Name = "btnNewMapper";
            this.btnNewMapper.Size = new System.Drawing.Size(120, 23);
            this.btnNewMapper.TabIndex = 4;
            this.btnNewMapper.Text = "New Mapper";
            this.btnNewMapper.UseVisualStyleBackColor = true;
            this.btnNewMapper.Click += new System.EventHandler(this.btnNewMapper_Click);
            // 
            // btnDefaultMapper
            // 
            this.btnDefaultMapper.Location = new System.Drawing.Point(138, 12);
            this.btnDefaultMapper.Name = "btnDefaultMapper";
            this.btnDefaultMapper.Size = new System.Drawing.Size(120, 23);
            this.btnDefaultMapper.TabIndex = 3;
            this.btnDefaultMapper.Text = "Default Mapper";
            this.btnDefaultMapper.UseVisualStyleBackColor = true;
            this.btnDefaultMapper.Click += new System.EventHandler(this.btnDefaultMapper_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMapperDef);
            this.tabControl.Controls.Add(this.tabTestResult);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 50);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(984, 514);
            this.tabControl.TabIndex = 4;
            // 
            // tabMapperDef
            // 
            this.tabMapperDef.Controls.Add(this.lnkRegexRef);
            this.tabMapperDef.Controls.Add(this.dgFunctions);
            this.tabMapperDef.Location = new System.Drawing.Point(4, 22);
            this.tabMapperDef.Name = "tabMapperDef";
            this.tabMapperDef.Padding = new System.Windows.Forms.Padding(3);
            this.tabMapperDef.Size = new System.Drawing.Size(976, 488);
            this.tabMapperDef.TabIndex = 0;
            this.tabMapperDef.Text = "Mapper Definition";
            this.tabMapperDef.UseVisualStyleBackColor = true;
            // 
            // tabTestResult
            // 
            this.tabTestResult.Controls.Add(this.splitContainer1);
            this.tabTestResult.Controls.Add(this.groupBox1);
            this.tabTestResult.Location = new System.Drawing.Point(4, 22);
            this.tabTestResult.Name = "tabTestResult";
            this.tabTestResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabTestResult.Size = new System.Drawing.Size(976, 488);
            this.tabTestResult.TabIndex = 1;
            this.tabTestResult.Text = "Test Result";
            this.tabTestResult.UseVisualStyleBackColor = true;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(12, 6);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(120, 23);
            this.btnValidate.TabIndex = 0;
            this.btnValidate.Text = "Validate RegEx";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(138, 6);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(120, 23);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test Mapper";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnTestFilePath);
            this.groupBox1.Controls.Add(this.txtTestFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(960, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test File";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Location";
            // 
            // txtTestFile
            // 
            this.txtTestFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestFile.Location = new System.Drawing.Point(79, 19);
            this.txtTestFile.Name = "txtTestFile";
            this.txtTestFile.Size = new System.Drawing.Size(833, 20);
            this.txtTestFile.TabIndex = 1;
            // 
            // btnTestFilePath
            // 
            this.btnTestFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestFilePath.Location = new System.Drawing.Point(918, 17);
            this.btnTestFilePath.Name = "btnTestFilePath";
            this.btnTestFilePath.Size = new System.Drawing.Size(36, 23);
            this.btnTestFilePath.TabIndex = 2;
            this.btnTestFilePath.Text = "...";
            this.btnTestFilePath.UseVisualStyleBackColor = true;
            this.btnTestFilePath.Click += new System.EventHandler(this.btnTestFilePath_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(8, 67);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgSuccessResults);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtUnmappedPins);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(962, 415);
            this.splitContainer1.SplitterDistance = 666;
            this.splitContainer1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Unmapped Pins";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mapped Pins";
            // 
            // btnSaveMapper
            // 
            this.btnSaveMapper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMapper.Location = new System.Drawing.Point(852, 6);
            this.btnSaveMapper.Name = "btnSaveMapper";
            this.btnSaveMapper.Size = new System.Drawing.Size(120, 23);
            this.btnSaveMapper.TabIndex = 2;
            this.btnSaveMapper.Text = "Save Mapper";
            this.btnSaveMapper.UseVisualStyleBackColor = true;
            this.btnSaveMapper.Click += new System.EventHandler(this.btnSaveMapper_Click);
            // 
            // dgFunctions
            // 
            this.dgFunctions.AllowDrop = true;
            this.dgFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgFunctions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFunctions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cName,
            this.cPinClass,
            this.cRegex,
            this.cSide,
            this.cAlignment,
            this.cPriority,
            this.cGroupSpacing});
            this.dgFunctions.Location = new System.Drawing.Point(6, 6);
            this.dgFunctions.Name = "dgFunctions";
            this.dgFunctions.Size = new System.Drawing.Size(962, 463);
            this.dgFunctions.TabIndex = 1;
            this.dgFunctions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgFunctions_CellValueChanged);
            this.dgFunctions.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgFunctions_DragDrop);
            this.dgFunctions.DragOver += new System.Windows.Forms.DragEventHandler(this.dgFunctions_DragOver);
            this.dgFunctions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgFunctions_MouseDown);
            this.dgFunctions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgFunctions_MouseMove);
            // 
            // lnkRegexRef
            // 
            this.lnkRegexRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkRegexRef.AutoSize = true;
            this.lnkRegexRef.Location = new System.Drawing.Point(8, 472);
            this.lnkRegexRef.Name = "lnkRegexRef";
            this.lnkRegexRef.Size = new System.Drawing.Size(92, 13);
            this.lnkRegexRef.TabIndex = 2;
            this.lnkRegexRef.TabStop = true;
            this.lnkRegexRef.Text = "RegEx Reference";
            this.lnkRegexRef.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegexRef_LinkClicked);
            // 
            // txtUnmappedPins
            // 
            this.txtUnmappedPins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUnmappedPins.Location = new System.Drawing.Point(6, 16);
            this.txtUnmappedPins.Multiline = true;
            this.txtUnmappedPins.Name = "txtUnmappedPins";
            this.txtUnmappedPins.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUnmappedPins.Size = new System.Drawing.Size(278, 396);
            this.txtUnmappedPins.TabIndex = 1;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cName.ToolTipText = "Name of the mapping function";
            // 
            // cPinClass
            // 
            this.cPinClass.HeaderText = "Pin Class";
            this.cPinClass.Items.AddRange(new object[] {
            "Generic",
            "PowerSupply",
            "ChipConfiguration",
            "IOPort"});
            this.cPinClass.Name = "cPinClass";
            this.cPinClass.ToolTipText = "Pin class for arrangement use only. This determines priority for breaking pins of" +
    "f into new schematic blocks.";
            // 
            // cRegex
            // 
            this.cRegex.HeaderText = "RegEx";
            this.cRegex.Name = "cRegex";
            this.cRegex.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cRegex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cRegex.ToolTipText = "Regular expression to match on the pin name";
            this.cRegex.Width = 350;
            // 
            // cSide
            // 
            this.cSide.HeaderText = "Side";
            this.cSide.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.cSide.Name = "cSide";
            this.cSide.ToolTipText = "Which side of the symbol the pins should be placed";
            // 
            // cAlignment
            // 
            this.cAlignment.HeaderText = "Alignment";
            this.cAlignment.Items.AddRange(new object[] {
            "Upper",
            "Middle",
            "Lower"});
            this.cAlignment.Name = "cAlignment";
            this.cAlignment.ToolTipText = "Vertical alignment on the side of the rectangular symbol";
            // 
            // cPriority
            // 
            this.cPriority.HeaderText = "Priority";
            this.cPriority.Name = "cPriority";
            this.cPriority.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPriority.ToolTipText = "Order priority when arranging pins within the same position";
            this.cPriority.Width = 75;
            // 
            // cGroupSpacing
            // 
            this.cGroupSpacing.HeaderText = "Group Spacing";
            this.cGroupSpacing.Name = "cGroupSpacing";
            this.cGroupSpacing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cGroupSpacing.ToolTipText = "Number of spaces to add between groups. This should almost always be set to 1";
            this.cGroupSpacing.Width = 75;
            // 
            // dgSuccessResults
            // 
            this.dgSuccessResults.AllowUserToAddRows = false;
            this.dgSuccessResults.AllowUserToDeleteRows = false;
            this.dgSuccessResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSuccessResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSuccessResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cMappedFunction,
            this.cPinName,
            this.cPort,
            this.cPortBit});
            this.dgSuccessResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgSuccessResults.Location = new System.Drawing.Point(9, 16);
            this.dgSuccessResults.Name = "dgSuccessResults";
            this.dgSuccessResults.ReadOnly = true;
            this.dgSuccessResults.Size = new System.Drawing.Size(654, 396);
            this.dgSuccessResults.TabIndex = 1;
            // 
            // cMappedFunction
            // 
            this.cMappedFunction.HeaderText = "Function";
            this.cMappedFunction.Name = "cMappedFunction";
            this.cMappedFunction.ReadOnly = true;
            // 
            // cPinName
            // 
            this.cPinName.HeaderText = "Pin";
            this.cPinName.Name = "cPinName";
            this.cPinName.ReadOnly = true;
            // 
            // cPort
            // 
            this.cPort.HeaderText = "Port";
            this.cPort.Name = "cPort";
            this.cPort.ReadOnly = true;
            // 
            // cPortBit
            // 
            this.cPortBit.HeaderText = "Port Bit";
            this.cPortBit.Name = "cPortBit";
            this.cPortBit.ReadOnly = true;
            // 
            // frmJsonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmJsonEditor";
            this.Text = "JSON Editor";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabMapperDef.ResumeLayout(false);
            this.tabMapperDef.PerformLayout();
            this.tabTestResult.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFunctions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSuccessResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveMapper;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLoadJson;
        private System.Windows.Forms.Button btnNewMapper;
        private System.Windows.Forms.Button btnDefaultMapper;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabMapperDef;
        private System.Windows.Forms.TabPage tabTestResult;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTestFilePath;
        private System.Windows.Forms.TextBox txtTestFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgFunctions;
        private System.Windows.Forms.LinkLabel lnkRegexRef;
        private System.Windows.Forms.TextBox txtUnmappedPins;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewComboBoxColumn cPinClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRegex;
        private System.Windows.Forms.DataGridViewComboBoxColumn cSide;
        private System.Windows.Forms.DataGridViewComboBoxColumn cAlignment;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn cGroupSpacing;
        private System.Windows.Forms.DataGridView dgSuccessResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMappedFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPinName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPortBit;
    }
}