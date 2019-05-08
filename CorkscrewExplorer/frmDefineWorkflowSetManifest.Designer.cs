namespace Corkscrew.Explorer
{
    partial class frmDefineWorkflowSetManifest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefineWorkflowSetManifest));
            this.tbId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbWorkflowEngine = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tbBuildAssemblyFilename = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbWorkflowClassName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbBuildProductFileVersion = new System.Windows.Forms.TextBox();
            this.tbBuildProductVersion = new System.Windows.Forms.TextBox();
            this.tbBuildProductTrademark = new System.Windows.Forms.TextBox();
            this.tbBuildProductCopyright = new System.Windows.Forms.TextBox();
            this.tbBuildProductCompany = new System.Windows.Forms.TextBox();
            this.tbBuildProductDescription = new System.Windows.Forms.TextBox();
            this.tbBuildProductName = new System.Windows.Forms.TextBox();
            this.tbBuildProductTitle = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbAlwaysCache = new System.Windows.Forms.RadioButton();
            this.rbAlwaysCompile = new System.Windows.Forms.RadioButton();
            this.lblCompiled = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDefineManifestItems = new System.Windows.Forms.Button();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(163, 134);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(358, 20);
            this.tbId.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Definition Id";
            // 
            // cbWorkflowEngine
            // 
            this.cbWorkflowEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkflowEngine.FormattingEnabled = true;
            this.cbWorkflowEngine.Items.AddRange(new object[] {
            "Corkscrew Coded Workflow",
            "Workflow v4.0 Coded Workflow",
            "Workflow v4.0 Xaml Workflow",
            "Workflow v3.0 Coded Workflow",
            "Workflow v3.0 Xaml Workflow"});
            this.cbWorkflowEngine.Location = new System.Drawing.Point(163, 160);
            this.cbWorkflowEngine.Name = "cbWorkflowEngine";
            this.cbWorkflowEngine.Size = new System.Drawing.Size(358, 21);
            this.cbWorkflowEngine.TabIndex = 1;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(34, 160);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(87, 13);
            this.label21.TabIndex = 31;
            this.label21.Text = "Workflow engine";
            // 
            // tbBuildAssemblyFilename
            // 
            this.tbBuildAssemblyFilename.Location = new System.Drawing.Point(163, 187);
            this.tbBuildAssemblyFilename.MaxLength = 255;
            this.tbBuildAssemblyFilename.Name = "tbBuildAssemblyFilename";
            this.tbBuildAssemblyFilename.Size = new System.Drawing.Size(358, 20);
            this.tbBuildAssemblyFilename.TabIndex = 2;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(34, 187);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(81, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Output filename";
            // 
            // tbWorkflowClassName
            // 
            this.tbWorkflowClassName.Location = new System.Drawing.Point(163, 213);
            this.tbWorkflowClassName.MaxLength = 1024;
            this.tbWorkflowClassName.Name = "tbWorkflowClassName";
            this.tbWorkflowClassName.Size = new System.Drawing.Size(358, 20);
            this.tbWorkflowClassName.TabIndex = 3;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(34, 213);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(108, 13);
            this.label20.TabIndex = 35;
            this.label20.Text = "Workflow class name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbBuildProductFileVersion);
            this.groupBox2.Controls.Add(this.tbBuildProductVersion);
            this.groupBox2.Controls.Add(this.tbBuildProductTrademark);
            this.groupBox2.Controls.Add(this.tbBuildProductCopyright);
            this.groupBox2.Controls.Add(this.tbBuildProductCompany);
            this.groupBox2.Controls.Add(this.tbBuildProductDescription);
            this.groupBox2.Controls.Add(this.tbBuildProductName);
            this.groupBox2.Controls.Add(this.tbBuildProductTitle);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(37, 239);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(484, 246);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Build data";
            // 
            // tbBuildProductFileVersion
            // 
            this.tbBuildProductFileVersion.Location = new System.Drawing.Point(143, 210);
            this.tbBuildProductFileVersion.MaxLength = 16;
            this.tbBuildProductFileVersion.Name = "tbBuildProductFileVersion";
            this.tbBuildProductFileVersion.Size = new System.Drawing.Size(98, 20);
            this.tbBuildProductFileVersion.TabIndex = 11;
            this.tbBuildProductFileVersion.Text = "1.0.0.0";
            // 
            // tbBuildProductVersion
            // 
            this.tbBuildProductVersion.Location = new System.Drawing.Point(143, 184);
            this.tbBuildProductVersion.MaxLength = 16;
            this.tbBuildProductVersion.Name = "tbBuildProductVersion";
            this.tbBuildProductVersion.Size = new System.Drawing.Size(98, 20);
            this.tbBuildProductVersion.TabIndex = 10;
            this.tbBuildProductVersion.Text = "1.0.0.0";
            // 
            // tbBuildProductTrademark
            // 
            this.tbBuildProductTrademark.Location = new System.Drawing.Point(143, 158);
            this.tbBuildProductTrademark.MaxLength = 255;
            this.tbBuildProductTrademark.Name = "tbBuildProductTrademark";
            this.tbBuildProductTrademark.Size = new System.Drawing.Size(330, 20);
            this.tbBuildProductTrademark.TabIndex = 9;
            // 
            // tbBuildProductCopyright
            // 
            this.tbBuildProductCopyright.Location = new System.Drawing.Point(143, 132);
            this.tbBuildProductCopyright.MaxLength = 255;
            this.tbBuildProductCopyright.Name = "tbBuildProductCopyright";
            this.tbBuildProductCopyright.Size = new System.Drawing.Size(330, 20);
            this.tbBuildProductCopyright.TabIndex = 8;
            // 
            // tbBuildProductCompany
            // 
            this.tbBuildProductCompany.Location = new System.Drawing.Point(143, 106);
            this.tbBuildProductCompany.MaxLength = 255;
            this.tbBuildProductCompany.Name = "tbBuildProductCompany";
            this.tbBuildProductCompany.Size = new System.Drawing.Size(330, 20);
            this.tbBuildProductCompany.TabIndex = 7;
            // 
            // tbBuildProductDescription
            // 
            this.tbBuildProductDescription.Location = new System.Drawing.Point(143, 80);
            this.tbBuildProductDescription.MaxLength = 255;
            this.tbBuildProductDescription.Name = "tbBuildProductDescription";
            this.tbBuildProductDescription.Size = new System.Drawing.Size(330, 20);
            this.tbBuildProductDescription.TabIndex = 6;
            // 
            // tbBuildProductName
            // 
            this.tbBuildProductName.Location = new System.Drawing.Point(143, 54);
            this.tbBuildProductName.MaxLength = 255;
            this.tbBuildProductName.Name = "tbBuildProductName";
            this.tbBuildProductName.Size = new System.Drawing.Size(330, 20);
            this.tbBuildProductName.TabIndex = 5;
            // 
            // tbBuildProductTitle
            // 
            this.tbBuildProductTitle.Location = new System.Drawing.Point(143, 28);
            this.tbBuildProductTitle.MaxLength = 255;
            this.tbBuildProductTitle.Name = "tbBuildProductTitle";
            this.tbBuildProductTitle.Size = new System.Drawing.Size(330, 20);
            this.tbBuildProductTitle.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 210);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 13);
            this.label16.TabIndex = 12;
            this.label16.Text = "Build file-version";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 184);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 16;
            this.label15.Text = "Build version";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 158);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "Trademark notice";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 132);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Copyright notice";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 106);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Company";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Product Description";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Product Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Product Title";
            // 
            // rbAlwaysCache
            // 
            this.rbAlwaysCache.Location = new System.Drawing.Point(37, 515);
            this.rbAlwaysCache.Name = "rbAlwaysCache";
            this.rbAlwaysCache.Size = new System.Drawing.Size(426, 23);
            this.rbAlwaysCache.TabIndex = 13;
            this.rbAlwaysCache.TabStop = true;
            this.rbAlwaysCache.Text = "Always save build output back to persistence. ";
            this.rbAlwaysCache.UseVisualStyleBackColor = true;
            // 
            // rbAlwaysCompile
            // 
            this.rbAlwaysCompile.Location = new System.Drawing.Point(37, 491);
            this.rbAlwaysCompile.Name = "rbAlwaysCompile";
            this.rbAlwaysCompile.Size = new System.Drawing.Size(446, 29);
            this.rbAlwaysCompile.TabIndex = 12;
            this.rbAlwaysCompile.TabStop = true;
            this.rbAlwaysCompile.Text = "Always compile the workflow binaries";
            this.rbAlwaysCompile.UseVisualStyleBackColor = true;
            // 
            // lblCompiled
            // 
            this.lblCompiled.AutoSize = true;
            this.lblCompiled.Location = new System.Drawing.Point(431, 499);
            this.lblCompiled.Name = "lblCompiled";
            this.lblCompiled.Size = new System.Drawing.Size(90, 13);
            this.lblCompiled.TabIndex = 41;
            this.lblCompiled.Text = "(compilation date)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(319, 499);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 13);
            this.label18.TabIndex = 40;
            this.label18.Text = "Last compiled";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(357, 544);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(438, 544);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDefineManifestItems
            // 
            this.btnDefineManifestItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefineManifestItems.Location = new System.Drawing.Point(37, 544);
            this.btnDefineManifestItems.Name = "btnDefineManifestItems";
            this.btnDefineManifestItems.Size = new System.Drawing.Size(106, 23);
            this.btnDefineManifestItems.TabIndex = 14;
            this.btnDefineManifestItems.Text = "Items...";
            this.btnDefineManifestItems.UseVisualStyleBackColor = true;
            this.btnDefineManifestItems.Click += new System.EventHandler(this.btnDefineManifestItems_Click);
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(519, -1);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 77;
            this.btnFormClose.Text = "X";
            this.btnFormClose.UseVisualStyleBackColor = true;
            this.btnFormClose.Click += new System.EventHandler(this.btnFormClose_Click);
            // 
            // lblLogoText
            // 
            this.lblLogoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogoText.Font = new System.Drawing.Font("Microsoft Himalaya", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoText.Image = ((System.Drawing.Image)(resources.GetObject("lblLogoText.Image")));
            this.lblLogoText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLogoText.Location = new System.Drawing.Point(-1, 0);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(525, 107);
            this.lblLogoText.TabIndex = 76;
            this.lblLogoText.Text = "Manifest";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmDefineWorkflowSetManifest_MouseDown);
            // 
            // frmDefineWorkflowSetManifest
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(578, 588);
            this.ControlBox = false;
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.btnDefineManifestItems);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblCompiled);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.rbAlwaysCache);
            this.Controls.Add(this.rbAlwaysCompile);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tbWorkflowClassName);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.tbBuildAssemblyFilename);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.cbWorkflowEngine);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.label4);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefineWorkflowSetManifest";
            this.ShowInTaskbar = false;
            this.Text = "Define Workflow - Setup Manifest";
            this.Shown += new System.EventHandler(this.frmDefineWorkflowSetManifest_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmDefineWorkflowSetManifest_MouseDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbWorkflowEngine;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbBuildAssemblyFilename;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbWorkflowClassName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbBuildProductFileVersion;
        private System.Windows.Forms.TextBox tbBuildProductVersion;
        private System.Windows.Forms.TextBox tbBuildProductTrademark;
        private System.Windows.Forms.TextBox tbBuildProductCopyright;
        private System.Windows.Forms.TextBox tbBuildProductCompany;
        private System.Windows.Forms.TextBox tbBuildProductDescription;
        private System.Windows.Forms.TextBox tbBuildProductName;
        private System.Windows.Forms.TextBox tbBuildProductTitle;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbAlwaysCache;
        private System.Windows.Forms.RadioButton rbAlwaysCompile;
        private System.Windows.Forms.Label lblCompiled;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDefineManifestItems;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}