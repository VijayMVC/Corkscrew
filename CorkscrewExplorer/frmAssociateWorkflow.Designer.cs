namespace Corkscrew.Explorer
{
    partial class frmAssociateWorkflow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAssociateWorkflow));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbEventList = new System.Windows.Forms.GroupBox();
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
            this.chkAssociationIsEnabled = new System.Windows.Forms.CheckBox();
            this.tbId = new System.Windows.Forms.TextBox();
            this.tbAssociationScopeUrl = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbWorkflowDefinition = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbAssociationName = new System.Windows.Forms.TextBox();
            this.lblModifiedBy = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.gbEventList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Association Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Workflow";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Association scope";
            // 
            // gbEventList
            // 
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
            this.gbEventList.Location = new System.Drawing.Point(36, 273);
            this.gbEventList.Name = "gbEventList";
            this.gbEventList.Size = new System.Drawing.Size(388, 138);
            this.gbEventList.TabIndex = 34;
            this.gbEventList.TabStop = false;
            this.gbEventList.Text = "Allow events";
            // 
            // chkTriggerFileDeleted
            // 
            this.chkTriggerFileDeleted.AutoSize = true;
            this.chkTriggerFileDeleted.Location = new System.Drawing.Point(272, 105);
            this.chkTriggerFileDeleted.Name = "chkTriggerFileDeleted";
            this.chkTriggerFileDeleted.Size = new System.Drawing.Size(80, 17);
            this.chkTriggerFileDeleted.TabIndex = 11;
            this.chkTriggerFileDeleted.Text = "File deleted";
            this.chkTriggerFileDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFileModified
            // 
            this.chkTriggerFileModified.AutoSize = true;
            this.chkTriggerFileModified.Location = new System.Drawing.Point(140, 105);
            this.chkTriggerFileModified.Name = "chkTriggerFileModified";
            this.chkTriggerFileModified.Size = new System.Drawing.Size(84, 17);
            this.chkTriggerFileModified.TabIndex = 10;
            this.chkTriggerFileModified.Text = "File modified";
            this.chkTriggerFileModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFileCreated
            // 
            this.chkTriggerFileCreated.AutoSize = true;
            this.chkTriggerFileCreated.Location = new System.Drawing.Point(27, 105);
            this.chkTriggerFileCreated.Name = "chkTriggerFileCreated";
            this.chkTriggerFileCreated.Size = new System.Drawing.Size(81, 17);
            this.chkTriggerFileCreated.TabIndex = 9;
            this.chkTriggerFileCreated.Text = "File created";
            this.chkTriggerFileCreated.UseVisualStyleBackColor = true;
            // 
            // chkTriggerDirectoryDeleted
            // 
            this.chkTriggerDirectoryDeleted.AutoSize = true;
            this.chkTriggerDirectoryDeleted.Location = new System.Drawing.Point(272, 82);
            this.chkTriggerDirectoryDeleted.Name = "chkTriggerDirectoryDeleted";
            this.chkTriggerDirectoryDeleted.Size = new System.Drawing.Size(106, 17);
            this.chkTriggerDirectoryDeleted.TabIndex = 8;
            this.chkTriggerDirectoryDeleted.Text = "Directory deleted";
            this.chkTriggerDirectoryDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerDirectoryModified
            // 
            this.chkTriggerDirectoryModified.AutoSize = true;
            this.chkTriggerDirectoryModified.Location = new System.Drawing.Point(140, 82);
            this.chkTriggerDirectoryModified.Name = "chkTriggerDirectoryModified";
            this.chkTriggerDirectoryModified.Size = new System.Drawing.Size(110, 17);
            this.chkTriggerDirectoryModified.TabIndex = 7;
            this.chkTriggerDirectoryModified.Text = "Directory modified";
            this.chkTriggerDirectoryModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerDirectoryCreated
            // 
            this.chkTriggerDirectoryCreated.AutoSize = true;
            this.chkTriggerDirectoryCreated.Location = new System.Drawing.Point(27, 82);
            this.chkTriggerDirectoryCreated.Name = "chkTriggerDirectoryCreated";
            this.chkTriggerDirectoryCreated.Size = new System.Drawing.Size(107, 17);
            this.chkTriggerDirectoryCreated.TabIndex = 6;
            this.chkTriggerDirectoryCreated.Text = "Directory created";
            this.chkTriggerDirectoryCreated.UseVisualStyleBackColor = true;
            // 
            // chkTriggerSiteDeleted
            // 
            this.chkTriggerSiteDeleted.AutoSize = true;
            this.chkTriggerSiteDeleted.Location = new System.Drawing.Point(272, 59);
            this.chkTriggerSiteDeleted.Name = "chkTriggerSiteDeleted";
            this.chkTriggerSiteDeleted.Size = new System.Drawing.Size(82, 17);
            this.chkTriggerSiteDeleted.TabIndex = 5;
            this.chkTriggerSiteDeleted.Text = "Site deleted";
            this.chkTriggerSiteDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerSiteModified
            // 
            this.chkTriggerSiteModified.AutoSize = true;
            this.chkTriggerSiteModified.Location = new System.Drawing.Point(140, 59);
            this.chkTriggerSiteModified.Name = "chkTriggerSiteModified";
            this.chkTriggerSiteModified.Size = new System.Drawing.Size(86, 17);
            this.chkTriggerSiteModified.TabIndex = 4;
            this.chkTriggerSiteModified.Text = "Site modified";
            this.chkTriggerSiteModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerSiteCreated
            // 
            this.chkTriggerSiteCreated.AutoSize = true;
            this.chkTriggerSiteCreated.Location = new System.Drawing.Point(27, 59);
            this.chkTriggerSiteCreated.Name = "chkTriggerSiteCreated";
            this.chkTriggerSiteCreated.Size = new System.Drawing.Size(83, 17);
            this.chkTriggerSiteCreated.TabIndex = 3;
            this.chkTriggerSiteCreated.Text = "Site created";
            this.chkTriggerSiteCreated.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFarmDeleted
            // 
            this.chkTriggerFarmDeleted.AutoSize = true;
            this.chkTriggerFarmDeleted.Location = new System.Drawing.Point(272, 36);
            this.chkTriggerFarmDeleted.Name = "chkTriggerFarmDeleted";
            this.chkTriggerFarmDeleted.Size = new System.Drawing.Size(87, 17);
            this.chkTriggerFarmDeleted.TabIndex = 2;
            this.chkTriggerFarmDeleted.Text = "Farm deleted";
            this.chkTriggerFarmDeleted.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFarmModified
            // 
            this.chkTriggerFarmModified.AutoSize = true;
            this.chkTriggerFarmModified.Location = new System.Drawing.Point(140, 36);
            this.chkTriggerFarmModified.Name = "chkTriggerFarmModified";
            this.chkTriggerFarmModified.Size = new System.Drawing.Size(91, 17);
            this.chkTriggerFarmModified.TabIndex = 1;
            this.chkTriggerFarmModified.Text = "Farm modified";
            this.chkTriggerFarmModified.UseVisualStyleBackColor = true;
            // 
            // chkTriggerFarmCreated
            // 
            this.chkTriggerFarmCreated.AutoSize = true;
            this.chkTriggerFarmCreated.Location = new System.Drawing.Point(27, 36);
            this.chkTriggerFarmCreated.Name = "chkTriggerFarmCreated";
            this.chkTriggerFarmCreated.Size = new System.Drawing.Size(88, 17);
            this.chkTriggerFarmCreated.TabIndex = 0;
            this.chkTriggerFarmCreated.Text = "Farm created";
            this.chkTriggerFarmCreated.UseVisualStyleBackColor = true;
            // 
            // chkAssociationIsEnabled
            // 
            this.chkAssociationIsEnabled.AutoSize = true;
            this.chkAssociationIsEnabled.Checked = true;
            this.chkAssociationIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAssociationIsEnabled.Location = new System.Drawing.Point(511, 140);
            this.chkAssociationIsEnabled.Name = "chkAssociationIsEnabled";
            this.chkAssociationIsEnabled.Size = new System.Drawing.Size(132, 17);
            this.chkAssociationIsEnabled.TabIndex = 43;
            this.chkAssociationIsEnabled.Text = "Association is Enabled";
            this.chkAssociationIsEnabled.UseVisualStyleBackColor = true;
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(176, 137);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(313, 20);
            this.tbId.TabIndex = 35;
            // 
            // tbAssociationScopeUrl
            // 
            this.tbAssociationScopeUrl.Location = new System.Drawing.Point(176, 201);
            this.tbAssociationScopeUrl.Name = "tbAssociationScopeUrl";
            this.tbAssociationScopeUrl.ReadOnly = true;
            this.tbAssociationScopeUrl.Size = new System.Drawing.Size(469, 20);
            this.tbAssociationScopeUrl.TabIndex = 37;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(529, 388);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 23);
            this.btnSave.TabIndex = 38;
            this.btnSave.Text = "Save Association";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(448, 388);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 39;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbWorkflowDefinition
            // 
            this.cmbWorkflowDefinition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkflowDefinition.FormattingEnabled = true;
            this.cmbWorkflowDefinition.Location = new System.Drawing.Point(176, 169);
            this.cmbWorkflowDefinition.Name = "cmbWorkflowDefinition";
            this.cmbWorkflowDefinition.Size = new System.Drawing.Size(467, 21);
            this.cmbWorkflowDefinition.TabIndex = 40;
            this.cmbWorkflowDefinition.SelectedIndexChanged += new System.EventHandler(this.cmbWorkflowDefinition_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 233);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Association name";
            // 
            // tbAssociationName
            // 
            this.tbAssociationName.Location = new System.Drawing.Point(176, 233);
            this.tbAssociationName.MaxLength = 255;
            this.tbAssociationName.Name = "tbAssociationName";
            this.tbAssociationName.Size = new System.Drawing.Size(467, 20);
            this.tbAssociationName.TabIndex = 42;
            // 
            // lblModifiedBy
            // 
            this.lblModifiedBy.AutoEllipsis = true;
            this.lblModifiedBy.AutoSize = true;
            this.lblModifiedBy.Location = new System.Drawing.Point(532, 323);
            this.lblModifiedBy.Name = "lblModifiedBy";
            this.lblModifiedBy.Size = new System.Drawing.Size(66, 13);
            this.lblModifiedBy.TabIndex = 51;
            this.lblModifiedBy.Text = "(modified by)";
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new System.Drawing.Point(532, 310);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(76, 13);
            this.lblModified.TabIndex = 50;
            this.lblModified.Text = "(modified date)";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoEllipsis = true;
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(532, 297);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(63, 13);
            this.lblCreatedBy.TabIndex = 49;
            this.lblCreatedBy.Text = "(created by)";
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(532, 284);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(73, 13);
            this.lblCreated.TabIndex = 48;
            this.lblCreated.Text = "(created date)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(445, 323);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 47;
            this.label10.Text = "Modified by";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(445, 310);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "Modified";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(445, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Created";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(445, 297);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "Created by";
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(666, 0);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 53;
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
            this.lblLogoText.Size = new System.Drawing.Size(666, 107);
            this.lblLogoText.TabIndex = 52;
            this.lblLogoText.Text = "Associate Workflow";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseMove);
            // 
            // frmAssociateWorkflow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(725, 440);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.lblModifiedBy);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chkAssociationIsEnabled);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbAssociationName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbWorkflowDefinition);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbAssociationScopeUrl);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.gbEventList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAssociateWorkflow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Associate workflow";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Shown += new System.EventHandler(this.frmAssociateWorkflow_Shown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseMove);
            this.gbEventList.ResumeLayout(false);
            this.gbEventList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbEventList;
        private System.Windows.Forms.CheckBox chkTriggerFileDeleted;
        private System.Windows.Forms.CheckBox chkTriggerFileModified;
        private System.Windows.Forms.CheckBox chkTriggerFileCreated;
        private System.Windows.Forms.CheckBox chkTriggerDirectoryDeleted;
        private System.Windows.Forms.CheckBox chkTriggerDirectoryModified;
        private System.Windows.Forms.CheckBox chkTriggerDirectoryCreated;
        private System.Windows.Forms.CheckBox chkTriggerSiteDeleted;
        private System.Windows.Forms.CheckBox chkTriggerSiteModified;
        private System.Windows.Forms.CheckBox chkTriggerSiteCreated;
        private System.Windows.Forms.CheckBox chkTriggerFarmDeleted;
        private System.Windows.Forms.CheckBox chkTriggerFarmModified;
        private System.Windows.Forms.CheckBox chkTriggerFarmCreated;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.TextBox tbAssociationScopeUrl;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbWorkflowDefinition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbAssociationName;
        private System.Windows.Forms.CheckBox chkAssociationIsEnabled;
        private System.Windows.Forms.Label lblModifiedBy;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}