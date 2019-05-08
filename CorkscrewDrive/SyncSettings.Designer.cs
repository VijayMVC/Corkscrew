namespace Corkscrew.Drive
{
    partial class SyncSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncSettings));
            this.btnCancel = new System.Windows.Forms.Button();
            this.fbdBrowseForFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnBrowseLocalFolder = new System.Windows.Forms.Button();
            this.tbSyncRootFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbTargetSite = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbExclusionPatterns = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbIdleDurationUnit = new System.Windows.Forms.ComboBox();
            this.nudIdleDurationValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.chkOptionRunOnlyOnIdle = new System.Windows.Forms.CheckBox();
            this.chkOptionDeleteSyncedFilesOnDisconnect = new System.Windows.Forms.CheckBox();
            this.chkOptionDeleteLocalOnRemoteDelete = new System.Windows.Forms.CheckBox();
            this.chkOptionDeleteRemoteOnLocalDelete = new System.Windows.Forms.CheckBox();
            this.chkOptionDownloadToLocalIfNotPresent = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCorkscrewTargetFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseCorkscrewFolder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudIdleDurationValue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(20, 489);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fbdBrowseForFolder
            // 
            this.fbdBrowseForFolder.Description = "Select the folder you wish to add for syncing";
            this.fbdBrowseForFolder.ShowNewFolderButton = false;
            // 
            // btnBrowseLocalFolder
            // 
            this.btnBrowseLocalFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseLocalFolder.Location = new System.Drawing.Point(506, 227);
            this.btnBrowseLocalFolder.Name = "btnBrowseLocalFolder";
            this.btnBrowseLocalFolder.Size = new System.Drawing.Size(72, 23);
            this.btnBrowseLocalFolder.TabIndex = 7;
            this.btnBrowseLocalFolder.Text = "B&rowse";
            this.btnBrowseLocalFolder.UseVisualStyleBackColor = true;
            this.btnBrowseLocalFolder.Click += new System.EventHandler(this.btnBrowseLocalFolder_Click);
            // 
            // tbSyncRootFolder
            // 
            this.tbSyncRootFolder.Location = new System.Drawing.Point(118, 227);
            this.tbSyncRootFolder.Name = "tbSyncRootFolder";
            this.tbSyncRootFolder.Size = new System.Drawing.Size(379, 20);
            this.tbSyncRootFolder.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Local folder:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Target folder:";
            // 
            // cbTargetSite
            // 
            this.cbTargetSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetSite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTargetSite.FormattingEnabled = true;
            this.cbTargetSite.Location = new System.Drawing.Point(118, 173);
            this.cbTargetSite.Name = "cbTargetSite";
            this.cbTargetSite.Size = new System.Drawing.Size(460, 21);
            this.cbTargetSite.TabIndex = 3;
            this.cbTargetSite.SelectedIndexChanged += new System.EventHandler(this.cbTargetSite_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Select the site:";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.Location = new System.Drawing.Point(199, 137);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "&Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Location = new System.Drawing.Point(118, 137);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "C&onnect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Location = new System.Drawing.Point(119, 121);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(78, 13);
            this.lblConnectionStatus.TabIndex = 0;
            this.lblConnectionStatus.Text = "Not connected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "You are currently : ";
            // 
            // tbExclusionPatterns
            // 
            this.tbExclusionPatterns.AcceptsReturn = true;
            this.tbExclusionPatterns.Location = new System.Drawing.Point(118, 259);
            this.tbExclusionPatterns.Multiline = true;
            this.tbExclusionPatterns.Name = "tbExclusionPatterns";
            this.tbExclusionPatterns.Size = new System.Drawing.Size(460, 53);
            this.tbExclusionPatterns.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 259);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 64);
            this.label3.TabIndex = 5;
            this.label3.Text = "Exclusion patterns (can be RegEx):";
            // 
            // cbIdleDurationUnit
            // 
            this.cbIdleDurationUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIdleDurationUnit.Enabled = false;
            this.cbIdleDurationUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbIdleDurationUnit.FormattingEnabled = true;
            this.cbIdleDurationUnit.Items.AddRange(new object[] {
            "milliseconds",
            "seconds",
            "minutes",
            "hours"});
            this.cbIdleDurationUnit.Location = new System.Drawing.Point(326, 461);
            this.cbIdleDurationUnit.Name = "cbIdleDurationUnit";
            this.cbIdleDurationUnit.Size = new System.Drawing.Size(154, 21);
            this.cbIdleDurationUnit.TabIndex = 15;
            // 
            // nudIdleDurationValue
            // 
            this.nudIdleDurationValue.Enabled = false;
            this.nudIdleDurationValue.Location = new System.Drawing.Point(215, 461);
            this.nudIdleDurationValue.Maximum = new decimal(new int[] {
            8640000,
            0,
            0,
            0});
            this.nudIdleDurationValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudIdleDurationValue.Name = "nudIdleDurationValue";
            this.nudIdleDurationValue.Size = new System.Drawing.Size(105, 20);
            this.nudIdleDurationValue.TabIndex = 14;
            this.nudIdleDurationValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudIdleDurationValue.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 463);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Idle duration: ";
            // 
            // chkOptionRunOnlyOnIdle
            // 
            this.chkOptionRunOnlyOnIdle.AutoSize = true;
            this.chkOptionRunOnlyOnIdle.Location = new System.Drawing.Point(118, 438);
            this.chkOptionRunOnlyOnIdle.Name = "chkOptionRunOnlyOnIdle";
            this.chkOptionRunOnlyOnIdle.Size = new System.Drawing.Size(173, 17);
            this.chkOptionRunOnlyOnIdle.TabIndex = 13;
            this.chkOptionRunOnlyOnIdle.Text = "Run only when computer is idle";
            this.chkOptionRunOnlyOnIdle.UseVisualStyleBackColor = true;
            this.chkOptionRunOnlyOnIdle.CheckedChanged += new System.EventHandler(this.chkOptionRunOnlyOnIdle_CheckedChanged);
            // 
            // chkOptionDeleteSyncedFilesOnDisconnect
            // 
            this.chkOptionDeleteSyncedFilesOnDisconnect.Location = new System.Drawing.Point(118, 399);
            this.chkOptionDeleteSyncedFilesOnDisconnect.Name = "chkOptionDeleteSyncedFilesOnDisconnect";
            this.chkOptionDeleteSyncedFilesOnDisconnect.Size = new System.Drawing.Size(463, 33);
            this.chkOptionDeleteSyncedFilesOnDisconnect.TabIndex = 12;
            this.chkOptionDeleteSyncedFilesOnDisconnect.Text = "When I disconnect this account on this computer, delete all the synchronised file" +
    "s from this computer.";
            this.chkOptionDeleteSyncedFilesOnDisconnect.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkOptionDeleteSyncedFilesOnDisconnect.UseVisualStyleBackColor = true;
            // 
            // chkOptionDeleteLocalOnRemoteDelete
            // 
            this.chkOptionDeleteLocalOnRemoteDelete.AutoSize = true;
            this.chkOptionDeleteLocalOnRemoteDelete.Location = new System.Drawing.Point(118, 376);
            this.chkOptionDeleteLocalOnRemoteDelete.Name = "chkOptionDeleteLocalOnRemoteDelete";
            this.chkOptionDeleteLocalOnRemoteDelete.Size = new System.Drawing.Size(374, 17);
            this.chkOptionDeleteLocalOnRemoteDelete.TabIndex = 11;
            this.chkOptionDeleteLocalOnRemoteDelete.Text = "When I delete a file in the Corkscrew Drive, also delete it on my computer.";
            this.chkOptionDeleteLocalOnRemoteDelete.UseVisualStyleBackColor = true;
            // 
            // chkOptionDeleteRemoteOnLocalDelete
            // 
            this.chkOptionDeleteRemoteOnLocalDelete.AutoSize = true;
            this.chkOptionDeleteRemoteOnLocalDelete.Location = new System.Drawing.Point(118, 351);
            this.chkOptionDeleteRemoteOnLocalDelete.Name = "chkOptionDeleteRemoteOnLocalDelete";
            this.chkOptionDeleteRemoteOnLocalDelete.Size = new System.Drawing.Size(374, 17);
            this.chkOptionDeleteRemoteOnLocalDelete.TabIndex = 10;
            this.chkOptionDeleteRemoteOnLocalDelete.Text = "When I delete a file on my computer, also delete it in the Corkscrew Drive.";
            this.chkOptionDeleteRemoteOnLocalDelete.UseVisualStyleBackColor = true;
            // 
            // chkOptionDownloadToLocalIfNotPresent
            // 
            this.chkOptionDownloadToLocalIfNotPresent.AutoSize = true;
            this.chkOptionDownloadToLocalIfNotPresent.Location = new System.Drawing.Point(118, 328);
            this.chkOptionDownloadToLocalIfNotPresent.Name = "chkOptionDownloadToLocalIfNotPresent";
            this.chkOptionDownloadToLocalIfNotPresent.Size = new System.Drawing.Size(436, 17);
            this.chkOptionDownloadToLocalIfNotPresent.TabIndex = 9;
            this.chkOptionDownloadToLocalIfNotPresent.Text = "Download files that are not present in my computer, but present in the Corkscrew " +
    "Drive.";
            this.chkOptionDownloadToLocalIfNotPresent.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Location = new System.Drawing.Point(503, 489);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 16;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblLogoText
            // 
            this.lblLogoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogoText.Font = new System.Drawing.Font("Microsoft Himalaya", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoText.Image = ((System.Drawing.Image)(resources.GetObject("lblLogoText.Image")));
            this.lblLogoText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLogoText.Location = new System.Drawing.Point(-5, -1);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(610, 107);
            this.lblLogoText.TabIndex = 80;
            this.lblLogoText.Text = "Drive - Settings";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Options";
            // 
            // tbCorkscrewTargetFolder
            // 
            this.tbCorkscrewTargetFolder.Location = new System.Drawing.Point(118, 201);
            this.tbCorkscrewTargetFolder.Name = "tbCorkscrewTargetFolder";
            this.tbCorkscrewTargetFolder.Size = new System.Drawing.Size(379, 20);
            this.tbCorkscrewTargetFolder.TabIndex = 4;
            // 
            // btnBrowseCorkscrewFolder
            // 
            this.btnBrowseCorkscrewFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseCorkscrewFolder.Location = new System.Drawing.Point(506, 201);
            this.btnBrowseCorkscrewFolder.Name = "btnBrowseCorkscrewFolder";
            this.btnBrowseCorkscrewFolder.Size = new System.Drawing.Size(72, 23);
            this.btnBrowseCorkscrewFolder.TabIndex = 5;
            this.btnBrowseCorkscrewFolder.Text = "B&rowse";
            this.btnBrowseCorkscrewFolder.UseVisualStyleBackColor = true;
            this.btnBrowseCorkscrewFolder.Click += new System.EventHandler(this.btnBrowseCorkscrewFolder_Click);
            // 
            // SyncSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(608, 532);
            this.ControlBox = false;
            this.Controls.Add(this.btnBrowseCorkscrewFolder);
            this.Controls.Add(this.tbCorkscrewTargetFolder);
            this.Controls.Add(this.cbIdleDurationUnit);
            this.Controls.Add(this.tbExclusionPatterns);
            this.Controls.Add(this.nudIdleDurationValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBrowseLocalFolder);
            this.Controls.Add(this.chkOptionRunOnlyOnIdle);
            this.Controls.Add(this.tbSyncRootFolder);
            this.Controls.Add(this.chkOptionDeleteSyncedFilesOnDisconnect);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.chkOptionDeleteLocalOnRemoteDelete);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkOptionDeleteRemoteOnLocalDelete);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.chkOptionDownloadToLocalIfNotPresent);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbTargetSite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SyncSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Corkscrew Drive";
            this.Shown += new System.EventHandler(this.SyncSettings_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudIdleDurationValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog fbdBrowseForFolder;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbExclusionPatterns;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox chkOptionDeleteLocalOnRemoteDelete;
        private System.Windows.Forms.CheckBox chkOptionDeleteRemoteOnLocalDelete;
        private System.Windows.Forms.CheckBox chkOptionDownloadToLocalIfNotPresent;
        private System.Windows.Forms.CheckBox chkOptionDeleteSyncedFilesOnDisconnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkOptionRunOnlyOnIdle;
        private System.Windows.Forms.ComboBox cbIdleDurationUnit;
        private System.Windows.Forms.NumericUpDown nudIdleDurationValue;
        private System.Windows.Forms.ComboBox cbTargetSite;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBrowseLocalFolder;
        private System.Windows.Forms.TextBox tbSyncRootFolder;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCorkscrewTargetFolder;
        private System.Windows.Forms.Button btnBrowseCorkscrewFolder;
    }
}