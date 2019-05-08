namespace Corkscrew.Explorer
{
    partial class frmCreateEditManifestItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateEditManifestItem));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectUploadFile = new System.Windows.Forms.Button();
            this.lblSelectedFilePath = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFilename = new System.Windows.Forms.TextBox();
            this.tbFilenameExtension = new System.Windows.Forms.TextBox();
            this.cbItemType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbRequiredForRun = new System.Windows.Forms.CheckBox();
            this.tbBuildRelativePath = new System.Windows.Forms.TextBox();
            this.tbRuntimeRelativeFolder = new System.Windows.Forms.TextBox();
            this.lblModifiedBy = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ofdSelectFile = new System.Windows.Forms.OpenFileDialog();
            this.tbId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDownloadFile = new System.Windows.Forms.Button();
            this.sfdDownloadName = new System.Windows.Forms.SaveFileDialog();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Manifest file";
            // 
            // btnSelectUploadFile
            // 
            this.btnSelectUploadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectUploadFile.Location = new System.Drawing.Point(512, 157);
            this.btnSelectUploadFile.Name = "btnSelectUploadFile";
            this.btnSelectUploadFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectUploadFile.TabIndex = 7;
            this.btnSelectUploadFile.Text = "&Upload";
            this.btnSelectUploadFile.UseVisualStyleBackColor = true;
            this.btnSelectUploadFile.Click += new System.EventHandler(this.btnSelectUploadFile_Click);
            // 
            // lblSelectedFilePath
            // 
            this.lblSelectedFilePath.AutoEllipsis = true;
            this.lblSelectedFilePath.Location = new System.Drawing.Point(211, 162);
            this.lblSelectedFilePath.Name = "lblSelectedFilePath";
            this.lblSelectedFilePath.Size = new System.Drawing.Size(300, 18);
            this.lblSelectedFilePath.TabIndex = 6;
            this.lblSelectedFilePath.Text = "(select a file to upload)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Filename";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Filename extension";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Item type";
            // 
            // tbFilename
            // 
            this.tbFilename.Location = new System.Drawing.Point(214, 189);
            this.tbFilename.MaxLength = 255;
            this.tbFilename.Name = "tbFilename";
            this.tbFilename.Size = new System.Drawing.Size(372, 20);
            this.tbFilename.TabIndex = 11;
            // 
            // tbFilenameExtension
            // 
            this.tbFilenameExtension.Location = new System.Drawing.Point(214, 215);
            this.tbFilenameExtension.MaxLength = 255;
            this.tbFilenameExtension.Name = "tbFilenameExtension";
            this.tbFilenameExtension.Size = new System.Drawing.Size(372, 20);
            this.tbFilenameExtension.TabIndex = 12;
            // 
            // cbItemType
            // 
            this.cbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItemType.FormattingEnabled = true;
            this.cbItemType.Items.AddRange(new object[] {
            "PrimaryAssembly",
            "DependencyAssembly",
            "SourceCodeFile",
            "XamlFile",
            "ConfigurationFile",
            "MediaResourceFile",
            "Stylesheet",
            "CustomDataFile",
            "ResourceFile"});
            this.cbItemType.Location = new System.Drawing.Point(214, 241);
            this.cbItemType.Name = "cbItemType";
            this.cbItemType.Size = new System.Drawing.Size(372, 21);
            this.cbItemType.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Build folder relative path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 332);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Runtime folder relative path";
            // 
            // cbRequiredForRun
            // 
            this.cbRequiredForRun.AutoSize = true;
            this.cbRequiredForRun.Location = new System.Drawing.Point(40, 309);
            this.cbRequiredForRun.Name = "cbRequiredForRun";
            this.cbRequiredForRun.Size = new System.Drawing.Size(190, 17);
            this.cbRequiredForRun.TabIndex = 16;
            this.cbRequiredForRun.Text = "Item is required to run the workflow";
            this.cbRequiredForRun.UseVisualStyleBackColor = true;
            this.cbRequiredForRun.CheckedChanged += new System.EventHandler(this.cbRequiredForRun_CheckedChanged);
            // 
            // tbBuildRelativePath
            // 
            this.tbBuildRelativePath.Location = new System.Drawing.Point(215, 269);
            this.tbBuildRelativePath.MaxLength = 1024;
            this.tbBuildRelativePath.Name = "tbBuildRelativePath";
            this.tbBuildRelativePath.Size = new System.Drawing.Size(372, 20);
            this.tbBuildRelativePath.TabIndex = 18;
            // 
            // tbRuntimeRelativeFolder
            // 
            this.tbRuntimeRelativeFolder.Location = new System.Drawing.Point(215, 329);
            this.tbRuntimeRelativeFolder.MaxLength = 1024;
            this.tbRuntimeRelativeFolder.Name = "tbRuntimeRelativeFolder";
            this.tbRuntimeRelativeFolder.Size = new System.Drawing.Size(371, 20);
            this.tbRuntimeRelativeFolder.TabIndex = 19;
            // 
            // lblModifiedBy
            // 
            this.lblModifiedBy.AutoEllipsis = true;
            this.lblModifiedBy.AutoSize = true;
            this.lblModifiedBy.Location = new System.Drawing.Point(111, 416);
            this.lblModifiedBy.Name = "lblModifiedBy";
            this.lblModifiedBy.Size = new System.Drawing.Size(66, 13);
            this.lblModifiedBy.TabIndex = 27;
            this.lblModifiedBy.Text = "(modified by)";
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new System.Drawing.Point(111, 403);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(76, 13);
            this.lblModified.TabIndex = 26;
            this.lblModified.Text = "(modified date)";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoEllipsis = true;
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(111, 390);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(63, 13);
            this.lblCreatedBy.TabIndex = 25;
            this.lblCreatedBy.Text = "(created by)";
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(111, 377);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(73, 13);
            this.lblCreated.TabIndex = 24;
            this.lblCreated.Text = "(created date)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(34, 416);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Modified by";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 403);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Modified";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 377);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Created";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 390);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Created by";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(430, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(511, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "&Save File";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ofdSelectFile
            // 
            this.ofdSelectFile.Filter = "All Files (*.*)|*.*";
            this.ofdSelectFile.ShowReadOnly = true;
            this.ofdSelectFile.SupportMultiDottedExtensions = true;
            this.ofdSelectFile.Title = "Select a file to upload to Corkscrew";
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(215, 129);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(372, 20);
            this.tbId.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(37, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Manifest item Id";
            // 
            // btnDownloadFile
            // 
            this.btnDownloadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadFile.Location = new System.Drawing.Point(429, 406);
            this.btnDownloadFile.Name = "btnDownloadFile";
            this.btnDownloadFile.Size = new System.Drawing.Size(157, 23);
            this.btnDownloadFile.TabIndex = 32;
            this.btnDownloadFile.Text = "&Download File";
            this.btnDownloadFile.UseVisualStyleBackColor = true;
            this.btnDownloadFile.Click += new System.EventHandler(this.btnDownloadFile_Click);
            // 
            // sfdDownloadName
            // 
            this.sfdDownloadName.CreatePrompt = true;
            this.sfdDownloadName.Filter = "All Files (*.*)|*.*";
            this.sfdDownloadName.SupportMultiDottedExtensions = true;
            this.sfdDownloadName.Title = "Select the file download location and name";
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(584, 1);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 67;
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
            this.lblLogoText.Location = new System.Drawing.Point(0, -1);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(587, 107);
            this.lblLogoText.TabIndex = 66;
            this.lblLogoText.Text = "Manifest Item";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // frmCreateEditManifestItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(645, 456);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.btnDownloadFile);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblModifiedBy);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbRuntimeRelativeFolder);
            this.Controls.Add(this.tbBuildRelativePath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbRequiredForRun);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbItemType);
            this.Controls.Add(this.tbFilenameExtension);
            this.Controls.Add(this.tbFilename);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectUploadFile);
            this.Controls.Add(this.lblSelectedFilePath);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateEditManifestItem";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create/Edit Workflow Manifest Item";
            this.Shown += new System.EventHandler(this.frmCreateEditManifestItem_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectUploadFile;
        private System.Windows.Forms.Label lblSelectedFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbFilename;
        private System.Windows.Forms.TextBox tbFilenameExtension;
        private System.Windows.Forms.ComboBox cbItemType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbRequiredForRun;
        private System.Windows.Forms.TextBox tbBuildRelativePath;
        private System.Windows.Forms.TextBox tbRuntimeRelativeFolder;
        private System.Windows.Forms.Label lblModifiedBy;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog ofdSelectFile;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDownloadFile;
        private System.Windows.Forms.SaveFileDialog sfdDownloadName;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}