namespace Corkscrew.Explorer
{
    partial class frmCreateEditSite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateEditSite));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbSiteName = new System.Windows.Forms.TextBox();
            this.tbSiteDescription = new System.Windows.Forms.TextBox();
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.lblModifiedBy = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.lblQuotaUsed = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudSiteQuotaValue = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDnsNames = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.cbDoNotDeploySiteDB = new System.Windows.Forms.CheckBox();
            this.tbId = new System.Windows.Forms.TextBox();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSiteQuotaValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Site Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Description";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 249);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Database name";
            // 
            // tbSiteName
            // 
            this.tbSiteName.Location = new System.Drawing.Point(215, 156);
            this.tbSiteName.MaxLength = 255;
            this.tbSiteName.Name = "tbSiteName";
            this.tbSiteName.Size = new System.Drawing.Size(272, 20);
            this.tbSiteName.TabIndex = 6;
            // 
            // tbSiteDescription
            // 
            this.tbSiteDescription.Location = new System.Drawing.Point(215, 184);
            this.tbSiteDescription.MaxLength = 512;
            this.tbSiteDescription.Multiline = true;
            this.tbSiteDescription.Name = "tbSiteDescription";
            this.tbSiteDescription.Size = new System.Drawing.Size(272, 56);
            this.tbSiteDescription.TabIndex = 7;
            // 
            // tbDbName
            // 
            this.tbDbName.Location = new System.Drawing.Point(215, 246);
            this.tbDbName.MaxLength = 255;
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.ReadOnly = true;
            this.tbDbName.Size = new System.Drawing.Size(272, 20);
            this.tbDbName.TabIndex = 9;
            // 
            // lblModifiedBy
            // 
            this.lblModifiedBy.AutoEllipsis = true;
            this.lblModifiedBy.AutoSize = true;
            this.lblModifiedBy.Location = new System.Drawing.Point(116, 434);
            this.lblModifiedBy.Name = "lblModifiedBy";
            this.lblModifiedBy.Size = new System.Drawing.Size(66, 13);
            this.lblModifiedBy.TabIndex = 11;
            this.lblModifiedBy.Text = "(modified by)";
            // 
            // lblModified
            // 
            this.lblModified.AutoSize = true;
            this.lblModified.Location = new System.Drawing.Point(116, 421);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(76, 13);
            this.lblModified.TabIndex = 10;
            this.lblModified.Text = "(modified date)";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoEllipsis = true;
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(116, 408);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(63, 13);
            this.lblCreatedBy.TabIndex = 9;
            this.lblCreatedBy.Text = "(created by)";
            // 
            // lblCreated
            // 
            this.lblCreated.AutoSize = true;
            this.lblCreated.Location = new System.Drawing.Point(116, 395);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(73, 13);
            this.lblCreated.TabIndex = 8;
            this.lblCreated.Text = "(created date)";
            // 
            // lblQuotaUsed
            // 
            this.lblQuotaUsed.AutoSize = true;
            this.lblQuotaUsed.Location = new System.Drawing.Point(421, 309);
            this.lblQuotaUsed.Name = "lblQuotaUsed";
            this.lblQuotaUsed.Size = new System.Drawing.Size(66, 13);
            this.lblQuotaUsed.TabIndex = 6;
            this.lblQuotaUsed.Text = "(quota used)";
            this.lblQuotaUsed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 434);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Modified by";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 421);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Modified";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 408);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Created by";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 395);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Created";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(339, 309);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Quota Used:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Site quota";
            // 
            // nudSiteQuotaValue
            // 
            this.nudSiteQuotaValue.Increment = new decimal(new int[] {
            10240,
            0,
            0,
            0});
            this.nudSiteQuotaValue.Location = new System.Drawing.Point(215, 302);
            this.nudSiteQuotaValue.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudSiteQuotaValue.Name = "nudSiteQuotaValue";
            this.nudSiteQuotaValue.Size = new System.Drawing.Size(70, 20);
            this.nudSiteQuotaValue.TabIndex = 12;
            this.nudSiteQuotaValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudSiteQuotaValue.Value = new decimal(new int[] {
            1024000,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 333);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "DNS names";
            // 
            // tbDnsNames
            // 
            this.tbDnsNames.Location = new System.Drawing.Point(215, 333);
            this.tbDnsNames.Multiline = true;
            this.tbDnsNames.Name = "tbDnsNames";
            this.tbDnsNames.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDnsNames.Size = new System.Drawing.Size(272, 56);
            this.tbDnsNames.TabIndex = 15;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(22, 346);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "(only if web-hosted)";
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(412, 429);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(326, 429);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(291, 309);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "bytes";
            // 
            // cbDoNotDeploySiteDB
            // 
            this.cbDoNotDeploySiteDB.AutoSize = true;
            this.cbDoNotDeploySiteDB.Location = new System.Drawing.Point(215, 272);
            this.cbDoNotDeploySiteDB.Name = "cbDoNotDeploySiteDB";
            this.cbDoNotDeploySiteDB.Size = new System.Drawing.Size(191, 17);
            this.cbDoNotDeploySiteDB.TabIndex = 20;
            this.cbDoNotDeploySiteDB.Text = "Do not create a separate database";
            this.cbDoNotDeploySiteDB.UseVisualStyleBackColor = true;
            this.cbDoNotDeploySiteDB.Visible = false;
            this.cbDoNotDeploySiteDB.CheckedChanged += new System.EventHandler(this.cbDoNotDeploySiteDB_CheckedChanged);
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(215, 131);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(272, 20);
            this.tbId.TabIndex = 21;
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(449, 0);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 69;
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
            this.lblLogoText.Location = new System.Drawing.Point(-4, 0);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(454, 107);
            this.lblLogoText.TabIndex = 68;
            this.lblLogoText.Text = "Site";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // frmCreateEditSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(506, 481);
            this.Controls.Add(this.lblModifiedBy);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbDoNotDeploySiteDB);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblQuotaUsed);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbDnsNames);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.nudSiteQuotaValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDbName);
            this.Controls.Add(this.tbSiteDescription);
            this.Controls.Add(this.tbSiteName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateEditSite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create/Edit Site";
            this.Shown += new System.EventHandler(this.frmCreateEditSite_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudSiteQuotaValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSiteName;
        private System.Windows.Forms.TextBox tbSiteDescription;
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudSiteQuotaValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbDnsNames;
        private System.Windows.Forms.Label lblQuotaUsed;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblModifiedBy;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox cbDoNotDeploySiteDB;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}