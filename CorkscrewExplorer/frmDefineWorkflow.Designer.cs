namespace Corkscrew.Explorer
{
    partial class frmDefineWorkflow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefineWorkflow));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.cbAllowStartOnModify = new System.Windows.Forms.CheckBox();
            this.cbAllowStartOnCreate = new System.Windows.Forms.CheckBox();
            this.lblModifiedBy = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbDefaultAssociationData = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbId = new System.Windows.Forms.TextBox();
            this.gbEventList = new System.Windows.Forms.GroupBox();
            this.chkTriggersAllowBubbledEvents = new System.Windows.Forms.CheckBox();
            this.chkTriggerFileDeleted = new System.Windows.Forms.CheckBox();
            this.chkTriggerFileModified = new System.Windows.Forms.CheckBox();
            this.chkTriggerFileCreated = new System.Windows.Forms.CheckBox();
            this.chkTriggerDirectoryDeleted = new System.Windows.Forms.CheckBox();
            this.chkTriggerDirectoryModified = new System.Windows.Forms.CheckBox();
            this.chkTriggerDirectoryCreated = new System.Windows.Forms.CheckBox();
            this.chkTriggerSiteDeleted = new System.Windows.Forms.CheckBox();
            this.chkTriggerSiteModified = new System.Windows.Forms.CheckBox();
            this.chkTriggerSiteCreated = new System.Windows.Forms.CheckBox();
            this.chkTriggerFarmDeleted = new System.Windows.Forms.CheckBox();
            this.chkTriggerFarmModified = new System.Windows.Forms.CheckBox();
            this.chkTriggerFarmCreated = new System.Windows.Forms.CheckBox();
            this.btnDefineManifest = new System.Windows.Forms.Button();
            this.btnInstallFromManifest = new System.Windows.Forms.Button();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbEventList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Default association data";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbEnabled);
            this.groupBox1.Controls.Add(this.cbAllowStartOnModify);
            this.groupBox1.Controls.Add(this.cbAllowStartOnCreate);
            this.groupBox1.Location = new System.Drawing.Point(30, 322);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 86);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flags";
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Location = new System.Drawing.Point(8, 65);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(122, 17);
            this.cbEnabled.TabIndex = 6;
            this.cbEnabled.Text = "Workflow is enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // cbAllowStartOnModify
            // 
            this.cbAllowStartOnModify.AutoSize = true;
            this.cbAllowStartOnModify.Location = new System.Drawing.Point(8, 42);
            this.cbAllowStartOnModify.Name = "cbAllowStartOnModify";
            this.cbAllowStartOnModify.Size = new System.Drawing.Size(360, 17);
            this.cbAllowStartOnModify.TabIndex = 5;
            this.cbAllowStartOnModify.Text = "Start workflow when a child item is modified in the associated container";
            this.cbAllowStartOnModify.UseVisualStyleBackColor = true;
            // 
            // cbAllowStartOnCreate
            // 
            this.cbAllowStartOnCreate.AutoSize = true;
            this.cbAllowStartOnCreate.Location = new System.Drawing.Point(8, 19);
            this.cbAllowStartOnCreate.Name = "cbAllowStartOnCreate";
            this.cbAllowStartOnCreate.Size = new System.Drawing.Size(357, 17);
            this.cbAllowStartOnCreate.TabIndex = 4;
            this.cbAllowStartOnCreate.Text = "Start workflow when a child item is created in the associated container";
            this.cbAllowStartOnCreate.UseVisualStyleBackColor = true;
            // 
            // lblModifiedBy
            // 
            this.lblModifiedBy.AutoEllipsis = true;
            this.lblModifiedBy.AutoSize = true;
            this.lblModifiedBy.Location = new System.Drawing.Point(97, 458);
            this.lblModifiedBy.Name = "lblModifiedBy";
            this.lblModifiedBy.Size = new System.Drawing.Size(66, 13);
            this.lblModifiedBy.TabIndex = 19;
            this.lblModifiedBy.Text = "(modified by)";
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new System.Drawing.Point(97, 445);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(76, 13);
            this.lblModified.TabIndex = 18;
            this.lblModified.Text = "(modified date)";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoEllipsis = true;
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(97, 432);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(63, 13);
            this.lblCreatedBy.TabIndex = 17;
            this.lblCreatedBy.Text = "(created by)";
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(97, 419);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(73, 13);
            this.lblCreated.TabIndex = 16;
            this.lblCreated.Text = "(created date)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 459);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Modified by";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(32, 446);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Modified";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 420);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Created";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 433);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Created by";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(100, 162);
            this.tbName.MaxLength = 255;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(314, 20);
            this.tbName.TabIndex = 1;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(100, 192);
            this.tbDescription.MaxLength = 1024;
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(314, 54);
            this.tbDescription.TabIndex = 2;
            // 
            // tbDefaultAssociationData
            // 
            this.tbDefaultAssociationData.Location = new System.Drawing.Point(31, 274);
            this.tbDefaultAssociationData.Multiline = true;
            this.tbDefaultAssociationData.Name = "tbDefaultAssociationData";
            this.tbDefaultAssociationData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDefaultAssociationData.Size = new System.Drawing.Size(383, 42);
            this.tbDefaultAssociationData.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(578, 495);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(497, 495);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Definition Id";
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(100, 133);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(314, 20);
            this.tbId.TabIndex = 0;
            // 
            // gbEventList
            // 
            this.gbEventList.Controls.Add(this.chkTriggersAllowBubbledEvents);
            this.gbEventList.Controls.Add(this.chkTriggerFileDeleted);
            this.gbEventList.Controls.Add(this.chkTriggerFileModified);
            this.gbEventList.Controls.Add(this.chkTriggerFileCreated);
            this.gbEventList.Controls.Add(this.chkTriggerDirectoryDeleted);
            this.gbEventList.Controls.Add(this.chkTriggerDirectoryModified);
            this.gbEventList.Controls.Add(this.chkTriggerDirectoryCreated);
            this.gbEventList.Controls.Add(this.chkTriggerSiteDeleted);
            this.gbEventList.Controls.Add(this.chkTriggerSiteModified);
            this.gbEventList.Controls.Add(this.chkTriggerSiteCreated);
            this.gbEventList.Controls.Add(this.chkTriggerFarmDeleted);
            this.gbEventList.Controls.Add(this.chkTriggerFarmModified);
            this.gbEventList.Controls.Add(this.chkTriggerFarmCreated);
            this.gbEventList.Location = new System.Drawing.Point(420, 133);
            this.gbEventList.Name = "gbEventList";
            this.gbEventList.Size = new System.Drawing.Size(241, 339);
            this.gbEventList.TabIndex = 33;
            this.gbEventList.TabStop = false;
            this.gbEventList.Text = "Allow events";
            // 
            // chkTriggersAllowBubbledEvents
            // 
            this.chkTriggersAllowBubbledEvents.AutoSize = true;
            this.chkTriggersAllowBubbledEvents.Location = new System.Drawing.Point(20, 304);
            this.chkTriggersAllowBubbledEvents.Name = "chkTriggersAllowBubbledEvents";
            this.chkTriggersAllowBubbledEvents.Size = new System.Drawing.Size(206, 17);
            this.chkTriggersAllowBubbledEvents.TabIndex = 19;
            this.chkTriggersAllowBubbledEvents.Text = "Allow bubbled events to be processed";
            this.chkTriggersAllowBubbledEvents.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFileDeleted
            // 
            this.chkTriggerFileDeleted.AutoSize = true;
            this.chkTriggerFileDeleted.Location = new System.Drawing.Point(20, 281);
            this.chkTriggerFileDeleted.Name = "chkTriggerFileDeleted";
            this.chkTriggerFileDeleted.Size = new System.Drawing.Size(80, 17);
            this.chkTriggerFileDeleted.TabIndex = 18;
            this.chkTriggerFileDeleted.Text = "File deleted";
            this.chkTriggerFileDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFileModified
            // 
            this.chkTriggerFileModified.AutoSize = true;
            this.chkTriggerFileModified.Location = new System.Drawing.Point(20, 258);
            this.chkTriggerFileModified.Name = "chkTriggerFileModified";
            this.chkTriggerFileModified.Size = new System.Drawing.Size(84, 17);
            this.chkTriggerFileModified.TabIndex = 17;
            this.chkTriggerFileModified.Text = "File modified";
            this.chkTriggerFileModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFileCreated
            // 
            this.chkTriggerFileCreated.AutoSize = true;
            this.chkTriggerFileCreated.Location = new System.Drawing.Point(20, 235);
            this.chkTriggerFileCreated.Name = "chkTriggerFileCreated";
            this.chkTriggerFileCreated.Size = new System.Drawing.Size(81, 17);
            this.chkTriggerFileCreated.TabIndex = 16;
            this.chkTriggerFileCreated.Text = "File created";
            this.chkTriggerFileCreated.UseVisualStyleBackColor = true;
            // 
            // chkTriggerDirectoryDeleted
            // 
            this.chkTriggerDirectoryDeleted.AutoSize = true;
            this.chkTriggerDirectoryDeleted.Location = new System.Drawing.Point(20, 212);
            this.chkTriggerDirectoryDeleted.Name = "chkTriggerDirectoryDeleted";
            this.chkTriggerDirectoryDeleted.Size = new System.Drawing.Size(106, 17);
            this.chkTriggerDirectoryDeleted.TabIndex = 15;
            this.chkTriggerDirectoryDeleted.Text = "Directory deleted";
            this.chkTriggerDirectoryDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerDirectoryModified
            // 
            this.chkTriggerDirectoryModified.AutoSize = true;
            this.chkTriggerDirectoryModified.Location = new System.Drawing.Point(20, 189);
            this.chkTriggerDirectoryModified.Name = "chkTriggerDirectoryModified";
            this.chkTriggerDirectoryModified.Size = new System.Drawing.Size(110, 17);
            this.chkTriggerDirectoryModified.TabIndex = 14;
            this.chkTriggerDirectoryModified.Text = "Directory modified";
            this.chkTriggerDirectoryModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerDirectoryCreated
            // 
            this.chkTriggerDirectoryCreated.AutoSize = true;
            this.chkTriggerDirectoryCreated.Location = new System.Drawing.Point(20, 166);
            this.chkTriggerDirectoryCreated.Name = "chkTriggerDirectoryCreated";
            this.chkTriggerDirectoryCreated.Size = new System.Drawing.Size(107, 17);
            this.chkTriggerDirectoryCreated.TabIndex = 13;
            this.chkTriggerDirectoryCreated.Text = "Directory created";
            this.chkTriggerDirectoryCreated.UseVisualStyleBackColor = true;
            // 
            // chkTriggerSiteDeleted
            // 
            this.chkTriggerSiteDeleted.AutoSize = true;
            this.chkTriggerSiteDeleted.Location = new System.Drawing.Point(20, 143);
            this.chkTriggerSiteDeleted.Name = "chkTriggerSiteDeleted";
            this.chkTriggerSiteDeleted.Size = new System.Drawing.Size(82, 17);
            this.chkTriggerSiteDeleted.TabIndex = 12;
            this.chkTriggerSiteDeleted.Text = "Site deleted";
            this.chkTriggerSiteDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerSiteModified
            // 
            this.chkTriggerSiteModified.AutoSize = true;
            this.chkTriggerSiteModified.Location = new System.Drawing.Point(20, 119);
            this.chkTriggerSiteModified.Name = "chkTriggerSiteModified";
            this.chkTriggerSiteModified.Size = new System.Drawing.Size(86, 17);
            this.chkTriggerSiteModified.TabIndex = 11;
            this.chkTriggerSiteModified.Text = "Site modified";
            this.chkTriggerSiteModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerSiteCreated
            // 
            this.chkTriggerSiteCreated.AutoSize = true;
            this.chkTriggerSiteCreated.Location = new System.Drawing.Point(20, 96);
            this.chkTriggerSiteCreated.Name = "chkTriggerSiteCreated";
            this.chkTriggerSiteCreated.Size = new System.Drawing.Size(83, 17);
            this.chkTriggerSiteCreated.TabIndex = 10;
            this.chkTriggerSiteCreated.Text = "Site created";
            this.chkTriggerSiteCreated.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFarmDeleted
            // 
            this.chkTriggerFarmDeleted.AutoSize = true;
            this.chkTriggerFarmDeleted.Location = new System.Drawing.Point(20, 71);
            this.chkTriggerFarmDeleted.Name = "chkTriggerFarmDeleted";
            this.chkTriggerFarmDeleted.Size = new System.Drawing.Size(87, 17);
            this.chkTriggerFarmDeleted.TabIndex = 9;
            this.chkTriggerFarmDeleted.Text = "Farm deleted";
            this.chkTriggerFarmDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFarmModified
            // 
            this.chkTriggerFarmModified.AutoSize = true;
            this.chkTriggerFarmModified.Location = new System.Drawing.Point(20, 48);
            this.chkTriggerFarmModified.Name = "chkTriggerFarmModified";
            this.chkTriggerFarmModified.Size = new System.Drawing.Size(91, 17);
            this.chkTriggerFarmModified.TabIndex = 8;
            this.chkTriggerFarmModified.Text = "Farm modified";
            this.chkTriggerFarmModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFarmCreated
            // 
            this.chkTriggerFarmCreated.AutoSize = true;
            this.chkTriggerFarmCreated.Location = new System.Drawing.Point(20, 25);
            this.chkTriggerFarmCreated.Name = "chkTriggerFarmCreated";
            this.chkTriggerFarmCreated.Size = new System.Drawing.Size(88, 17);
            this.chkTriggerFarmCreated.TabIndex = 7;
            this.chkTriggerFarmCreated.Text = "Farm created";
            this.chkTriggerFarmCreated.UseVisualStyleBackColor = true;
            // 
            // btnDefineManifest
            // 
            this.btnDefineManifest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefineManifest.Location = new System.Drawing.Point(308, 414);
            this.btnDefineManifest.Name = "btnDefineManifest";
            this.btnDefineManifest.Size = new System.Drawing.Size(106, 23);
            this.btnDefineManifest.TabIndex = 20;
            this.btnDefineManifest.Text = "Manifest...";
            this.btnDefineManifest.UseVisualStyleBackColor = true;
            this.btnDefineManifest.Click += new System.EventHandler(this.btnDefineManifest_Click);
            // 
            // btnInstallFromManifest
            // 
            this.btnInstallFromManifest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstallFromManifest.Location = new System.Drawing.Point(31, 495);
            this.btnInstallFromManifest.Name = "btnInstallFromManifest";
            this.btnInstallFromManifest.Size = new System.Drawing.Size(226, 23);
            this.btnInstallFromManifest.TabIndex = 34;
            this.btnInstallFromManifest.Text = "Import from Workflow Install Manifest...";
            this.btnInstallFromManifest.UseVisualStyleBackColor = true;
            this.btnInstallFromManifest.Click += new System.EventHandler(this.btnInstallFromManifest_Click);
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(618, -1);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 73;
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
            this.lblLogoText.Location = new System.Drawing.Point(0, 0);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(620, 107);
            this.lblLogoText.TabIndex = 72;
            this.lblLogoText.Text = "Workflow";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // frmDefineWorkflow
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(676, 535);
            this.ControlBox = false;
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.btnInstallFromManifest);
            this.Controls.Add(this.btnDefineManifest);
            this.Controls.Add(this.gbEventList);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.lblModifiedBy);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.tbDefaultAssociationData);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefineWorkflow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.frmDefineWorkflow_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbEventList.ResumeLayout(false);
            this.gbEventList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbDefaultAssociationData;
        private System.Windows.Forms.CheckBox cbAllowStartOnModify;
        private System.Windows.Forms.CheckBox cbAllowStartOnCreate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Label lblModifiedBy;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox gbEventList;
        private System.Windows.Forms.CheckBox chkTriggerFarmCreated;
        private System.Windows.Forms.CheckBox chkTriggerFarmDeleted;
        private System.Windows.Forms.CheckBox chkTriggerFarmModified;
        private System.Windows.Forms.CheckBox chkTriggerSiteDeleted;
        private System.Windows.Forms.CheckBox chkTriggerSiteModified;
        private System.Windows.Forms.CheckBox chkTriggerSiteCreated;
        private System.Windows.Forms.CheckBox chkTriggerDirectoryDeleted;
        private System.Windows.Forms.CheckBox chkTriggerDirectoryModified;
        private System.Windows.Forms.CheckBox chkTriggerDirectoryCreated;
        private System.Windows.Forms.CheckBox chkTriggerFileDeleted;
        private System.Windows.Forms.CheckBox chkTriggerFileModified;
        private System.Windows.Forms.CheckBox chkTriggerFileCreated;
        private System.Windows.Forms.CheckBox chkTriggersAllowBubbledEvents;
        private System.Windows.Forms.Button btnDefineManifest;
        private System.Windows.Forms.Button btnInstallFromManifest;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}