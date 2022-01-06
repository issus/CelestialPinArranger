namespace CelestialPinArranger
{
    partial class FormMain
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
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.cmbPinMapper = new System.Windows.Forms.ComboBox();
            this.lblPinMapper = new System.Windows.Forms.Label();
            this.txtPartNumber = new System.Windows.Forms.TextBox();
            this.lblPartNumber = new System.Windows.Forms.Label();
            this.txtManufacturer = new System.Windows.Forms.TextBox();
            this.lblManufacturer = new System.Windows.Forms.Label();
            this.btnLoadClipboard = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnJsonEditor = new System.Windows.Forms.Button();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnBatch = new System.Windows.Forms.Button();
            this.chkOpenAltium = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.gridDataPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataDesignator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataFunction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataElectricalType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstPackages = new System.Windows.Forms.ListBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSymbolNext = new System.Windows.Forms.Button();
            this.btnSymbolPrev = new System.Windows.Forms.Button();
            this.pnlPreview.SuspendLayout();
            this.grpDetails.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.grpPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPreview
            // 
            this.pnlPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(232)))));
            this.pnlPreview.Controls.Add(this.btnSymbolPrev);
            this.pnlPreview.Controls.Add(this.btnSymbolNext);
            this.pnlPreview.Location = new System.Drawing.Point(6, 123);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(307, 328);
            this.pnlPreview.TabIndex = 2;
            this.pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.preview_Paint);
            this.pnlPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlPreview_MouseMove);
            this.pnlPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlPreview_MouseMove);
            this.pnlPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlPreview_MouseMove);
            this.pnlPreview.Resize += new System.EventHandler(this.preview_Resize);
            // 
            // grpDetails
            // 
            this.grpDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDetails.Controls.Add(this.cmbPinMapper);
            this.grpDetails.Controls.Add(this.lblPinMapper);
            this.grpDetails.Controls.Add(this.txtPartNumber);
            this.grpDetails.Controls.Add(this.lblPartNumber);
            this.grpDetails.Controls.Add(this.txtManufacturer);
            this.grpDetails.Controls.Add(this.lblManufacturer);
            this.grpDetails.Location = new System.Drawing.Point(12, 12);
            this.grpDetails.MinimumSize = new System.Drawing.Size(300, 104);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(431, 104);
            this.grpDetails.TabIndex = 1;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Component Details";
            // 
            // cmbPinMapper
            // 
            this.cmbPinMapper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPinMapper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPinMapper.FormattingEnabled = true;
            this.cmbPinMapper.Location = new System.Drawing.Point(100, 72);
            this.cmbPinMapper.Name = "cmbPinMapper";
            this.cmbPinMapper.Size = new System.Drawing.Size(325, 21);
            this.cmbPinMapper.TabIndex = 7;
            // 
            // lblPinMapper
            // 
            this.lblPinMapper.AutoSize = true;
            this.lblPinMapper.Location = new System.Drawing.Point(6, 75);
            this.lblPinMapper.Name = "lblPinMapper";
            this.lblPinMapper.Size = new System.Drawing.Size(61, 13);
            this.lblPinMapper.TabIndex = 6;
            this.lblPinMapper.Text = "Pin Mapper";
            // 
            // txtPartNumber
            // 
            this.txtPartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartNumber.Location = new System.Drawing.Point(100, 46);
            this.txtPartNumber.Name = "txtPartNumber";
            this.txtPartNumber.Size = new System.Drawing.Size(325, 20);
            this.txtPartNumber.TabIndex = 5;
            // 
            // lblPartNumber
            // 
            this.lblPartNumber.AutoSize = true;
            this.lblPartNumber.Location = new System.Drawing.Point(6, 49);
            this.lblPartNumber.Name = "lblPartNumber";
            this.lblPartNumber.Size = new System.Drawing.Size(66, 13);
            this.lblPartNumber.TabIndex = 4;
            this.lblPartNumber.Text = "Part Number";
            // 
            // txtManufacturer
            // 
            this.txtManufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtManufacturer.Location = new System.Drawing.Point(100, 19);
            this.txtManufacturer.Name = "txtManufacturer";
            this.txtManufacturer.Size = new System.Drawing.Size(325, 20);
            this.txtManufacturer.TabIndex = 3;
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.AutoSize = true;
            this.lblManufacturer.Location = new System.Drawing.Point(6, 22);
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(70, 13);
            this.lblManufacturer.TabIndex = 2;
            this.lblManufacturer.Text = "Manufacturer";
            // 
            // btnLoadClipboard
            // 
            this.btnLoadClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadClipboard.Location = new System.Drawing.Point(9, 19);
            this.btnLoadClipboard.Name = "btnLoadClipboard";
            this.btnLoadClipboard.Size = new System.Drawing.Size(116, 23);
            this.btnLoadClipboard.TabIndex = 1;
            this.btnLoadClipboard.Text = "Load from Clipboard";
            this.btnLoadClipboard.UseVisualStyleBackColor = true;
            this.btnLoadClipboard.Click += new System.EventHandler(this.btnLoadClipboard_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(9, 45);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(116, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load from File...";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnJsonEditor);
            this.pnlBottom.Controls.Add(this.btnSaveAll);
            this.pnlBottom.Controls.Add(this.btnBatch);
            this.pnlBottom.Controls.Add(this.chkOpenAltium);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 475);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(921, 43);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnJsonEditor
            // 
            this.btnJsonEditor.Location = new System.Drawing.Point(133, 8);
            this.btnJsonEditor.Name = "btnJsonEditor";
            this.btnJsonEditor.Size = new System.Drawing.Size(131, 23);
            this.btnJsonEditor.TabIndex = 4;
            this.btnJsonEditor.Text = "JSON Mapping Editor";
            this.btnJsonEditor.UseVisualStyleBackColor = true;
            this.btnJsonEditor.Click += new System.EventHandler(this.btnJsonEditor_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAll.Location = new System.Drawing.Point(809, 6);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(100, 25);
            this.btnSaveAll.TabIndex = 3;
            this.btnSaveAll.Text = "Save All";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnBatch
            // 
            this.btnBatch.Location = new System.Drawing.Point(9, 8);
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.Size = new System.Drawing.Size(118, 23);
            this.btnBatch.TabIndex = 2;
            this.btnBatch.Text = "Batch Processing";
            this.btnBatch.UseVisualStyleBackColor = true;
            this.btnBatch.Click += new System.EventHandler(this.btnBatch_Click);
            // 
            // chkOpenAltium
            // 
            this.chkOpenAltium.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkOpenAltium.AutoSize = true;
            this.chkOpenAltium.Checked = true;
            this.chkOpenAltium.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenAltium.Location = new System.Drawing.Point(602, 11);
            this.chkOpenAltium.Name = "chkOpenAltium";
            this.chkOpenAltium.Size = new System.Drawing.Size(95, 17);
            this.chkOpenAltium.TabIndex = 1;
            this.chkOpenAltium.Text = "Open In Altium";
            this.chkOpenAltium.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(703, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer.Panel1.Controls.Add(this.gridData);
            this.splitContainer.Panel1.Controls.Add(this.grpDetails);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpPreview);
            this.splitContainer.Size = new System.Drawing.Size(921, 475);
            this.splitContainer.SplitterDistance = 583;
            this.splitContainer.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnLoadClipboard);
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Location = new System.Drawing.Point(449, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(131, 104);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pin Data";
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.AllowUserToResizeRows = false;
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridDataPackage,
            this.gridDataDesignator,
            this.gridDataName,
            this.gridDataFunction,
            this.gridDataPosition,
            this.gridDataElectricalType});
            this.gridData.Location = new System.Drawing.Point(12, 122);
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.RowHeadersVisible = false;
            this.gridData.Size = new System.Drawing.Size(568, 341);
            this.gridData.TabIndex = 2;
            // 
            // gridDataPackage
            // 
            this.gridDataPackage.HeaderText = "Package";
            this.gridDataPackage.Name = "gridDataPackage";
            this.gridDataPackage.ReadOnly = true;
            this.gridDataPackage.Width = 75;
            // 
            // gridDataDesignator
            // 
            this.gridDataDesignator.HeaderText = "Designator";
            this.gridDataDesignator.Name = "gridDataDesignator";
            this.gridDataDesignator.ReadOnly = true;
            this.gridDataDesignator.Width = 83;
            // 
            // gridDataName
            // 
            this.gridDataName.HeaderText = "Name";
            this.gridDataName.Name = "gridDataName";
            this.gridDataName.ReadOnly = true;
            this.gridDataName.Width = 60;
            // 
            // gridDataFunction
            // 
            this.gridDataFunction.HeaderText = "Function";
            this.gridDataFunction.Name = "gridDataFunction";
            this.gridDataFunction.ReadOnly = true;
            this.gridDataFunction.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDataFunction.Width = 73;
            // 
            // gridDataPosition
            // 
            this.gridDataPosition.HeaderText = "Position";
            this.gridDataPosition.Name = "gridDataPosition";
            this.gridDataPosition.ReadOnly = true;
            this.gridDataPosition.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDataPosition.Width = 69;
            // 
            // gridDataElectricalType
            // 
            this.gridDataElectricalType.HeaderText = "Type";
            this.gridDataElectricalType.Name = "gridDataElectricalType";
            this.gridDataElectricalType.ReadOnly = true;
            this.gridDataElectricalType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDataElectricalType.Width = 56;
            // 
            // grpPreview
            // 
            this.grpPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPreview.Controls.Add(this.label2);
            this.grpPreview.Controls.Add(this.label1);
            this.grpPreview.Controls.Add(this.lstPackages);
            this.grpPreview.Controls.Add(this.pnlPreview);
            this.grpPreview.Location = new System.Drawing.Point(3, 12);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(319, 457);
            this.grpPreview.TabIndex = 4;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Symbol View";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Packages";
            // 
            // lstPackages
            // 
            this.lstPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPackages.FormattingEnabled = true;
            this.lstPackages.Location = new System.Drawing.Point(6, 35);
            this.lstPackages.Name = "lstPackages";
            this.lstPackages.Size = new System.Drawing.Size(307, 69);
            this.lstPackages.TabIndex = 3;
            this.lstPackages.SelectedIndexChanged += new System.EventHandler(this.lstPackages_SelectedIndexChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Title = "Open Pin Data File";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Location to save SchLibs";
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // btnSymbolNext
            // 
            this.btnSymbolNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSymbolNext.Location = new System.Drawing.Point(281, 302);
            this.btnSymbolNext.Name = "btnSymbolNext";
            this.btnSymbolNext.Size = new System.Drawing.Size(23, 23);
            this.btnSymbolNext.TabIndex = 0;
            this.btnSymbolNext.Text = ">";
            this.btnSymbolNext.UseVisualStyleBackColor = true;
            this.btnSymbolNext.Click += new System.EventHandler(this.btnSymbolNext_Click);
            // 
            // btnSymbolPrev
            // 
            this.btnSymbolPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSymbolPrev.Location = new System.Drawing.Point(251, 302);
            this.btnSymbolPrev.Name = "btnSymbolPrev";
            this.btnSymbolPrev.Size = new System.Drawing.Size(23, 23);
            this.btnSymbolPrev.TabIndex = 1;
            this.btnSymbolPrev.Text = "<";
            this.btnSymbolPrev.UseVisualStyleBackColor = true;
            this.btnSymbolPrev.Click += new System.EventHandler(this.btnSymbolPrev_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 518);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlBottom);
            this.Name = "FormMain";
            this.Text = "Celestial Pin Arranger";
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.pnlPreview.ResumeLayout(false);
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.grpPreview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.ListBox lstPackages;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Button btnLoadClipboard;
        private System.Windows.Forms.TextBox txtPartNumber;
        private System.Windows.Forms.Label lblPartNumber;
        private System.Windows.Forms.TextBox txtManufacturer;
        private System.Windows.Forms.Label lblManufacturer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox cmbPinMapper;
        private System.Windows.Forms.Label lblPinMapper;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDataPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDataDesignator;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDataName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDataFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDataPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridDataElectricalType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.CheckBox chkOpenAltium;
        private System.Windows.Forms.Button btnBatch;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Button btnJsonEditor;
        private System.Windows.Forms.Button btnSymbolPrev;
        private System.Windows.Forms.Button btnSymbolNext;
    }
}

