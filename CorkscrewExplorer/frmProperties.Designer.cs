namespace Corkscrew.Explorer
{
    partial class frmProperties
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProperties));
            this.tbItemGuid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbItemName = new System.Windows.Forms.TextBox();
            this.pbItemIcon = new System.Windows.Forms.PictureBox();
            this.clbPermissionsForPrincipal = new System.Windows.Forms.CheckedListBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnPermissionsEdit = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.lbUsersGroupsNames = new System.Windows.Forms.ListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnViewVersion = new System.Windows.Forms.Button();
            this.btnRestoreVersion = new System.Windows.Forms.Button();
            this.lvItemHistory = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label17 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.imgLstFarmViewIconsSmall = new System.Windows.Forms.ImageList(this.components);
            this.sfdSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblItemType = new System.Windows.Forms.Label();
            this.lblItemLocation = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblContains = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.Label();
            this.lblAccessed = new System.Windows.Forms.Label();
            this.chkReadonly = new System.Windows.Forms.CheckBox();
            this.chkHidden = new System.Windows.Forms.CheckBox();
            this.chkSystem = new System.Windows.Forms.CheckBox();
            this.chkArchive = new System.Windows.Forms.CheckBox();
            this.btnDownloadFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbItemIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // tbItemGuid
            // 
            this.tbItemGuid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbItemGuid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbItemGuid.ForeColor = System.Drawing.Color.DarkGray;
            this.tbItemGuid.Location = new System.Drawing.Point(85, 122);
            this.tbItemGuid.Name = "tbItemGuid";
            this.tbItemGuid.ReadOnly = true;
            this.tbItemGuid.Size = new System.Drawing.Size(382, 13);
            this.tbItemGuid.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(30, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 2);
            this.label1.TabIndex = 2;
            // 
            // tbItemName
            // 
            this.tbItemName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbItemName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbItemName.ForeColor = System.Drawing.Color.DarkGray;
            this.tbItemName.Location = new System.Drawing.Point(85, 157);
            this.tbItemName.Name = "tbItemName";
            this.tbItemName.Size = new System.Drawing.Size(251, 13);
            this.tbItemName.TabIndex = 1;
            // 
            // pbItemIcon
            // 
            this.pbItemIcon.Location = new System.Drawing.Point(29, 120);
            this.pbItemIcon.Name = "pbItemIcon";
            this.pbItemIcon.Size = new System.Drawing.Size(50, 50);
            this.pbItemIcon.TabIndex = 0;
            this.pbItemIcon.TabStop = false;
            // 
            // clbPermissionsForPrincipal
            // 
            this.clbPermissionsForPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.clbPermissionsForPrincipal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbPermissionsForPrincipal.Enabled = false;
            this.clbPermissionsForPrincipal.ForeColor = System.Drawing.Color.White;
            this.clbPermissionsForPrincipal.FormattingEnabled = true;
            this.clbPermissionsForPrincipal.Items.AddRange(new object[] {
            "Full control",
            "Contribute",
            "Read"});
            this.clbPermissionsForPrincipal.Location = new System.Drawing.Point(34, 550);
            this.clbPermissionsForPrincipal.MultiColumn = true;
            this.clbPermissionsForPrincipal.Name = "clbPermissionsForPrincipal";
            this.clbPermissionsForPrincipal.Size = new System.Drawing.Size(329, 15);
            this.clbPermissionsForPrincipal.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(31, 534);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Permissions for (user)";
            // 
            // btnPermissionsEdit
            // 
            this.btnPermissionsEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPermissionsEdit.Location = new System.Drawing.Point(288, 509);
            this.btnPermissionsEdit.Name = "btnPermissionsEdit";
            this.btnPermissionsEdit.Size = new System.Drawing.Size(75, 23);
            this.btnPermissionsEdit.TabIndex = 5;
            this.btnPermissionsEdit.Text = "E&dit...";
            this.btnPermissionsEdit.UseVisualStyleBackColor = true;
            this.btnPermissionsEdit.Click += new System.EventHandler(this.btnPermissionsEdit_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(30, 509);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(168, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "To change permissions, click Edit.";
            // 
            // lbUsersGroupsNames
            // 
            this.lbUsersGroupsNames.FormattingEnabled = true;
            this.lbUsersGroupsNames.Location = new System.Drawing.Point(30, 421);
            this.lbUsersGroupsNames.Name = "lbUsersGroupsNames";
            this.lbUsersGroupsNames.Size = new System.Drawing.Size(333, 82);
            this.lbUsersGroupsNames.TabIndex = 3;
            this.lbUsersGroupsNames.SelectedIndexChanged += new System.EventHandler(this.lbUsersGroupsNames_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(29, 405);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(108, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Group or user names:";
            // 
            // btnViewVersion
            // 
            this.btnViewVersion.Enabled = false;
            this.btnViewVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewVersion.Location = new System.Drawing.Point(494, 509);
            this.btnViewVersion.Name = "btnViewVersion";
            this.btnViewVersion.Size = new System.Drawing.Size(75, 23);
            this.btnViewVersion.TabIndex = 3;
            this.btnViewVersion.Text = "&View";
            this.btnViewVersion.UseVisualStyleBackColor = true;
            this.btnViewVersion.Click += new System.EventHandler(this.btnViewVersion_Click);
            // 
            // btnRestoreVersion
            // 
            this.btnRestoreVersion.Enabled = false;
            this.btnRestoreVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreVersion.Location = new System.Drawing.Point(575, 509);
            this.btnRestoreVersion.Name = "btnRestoreVersion";
            this.btnRestoreVersion.Size = new System.Drawing.Size(75, 23);
            this.btnRestoreVersion.TabIndex = 2;
            this.btnRestoreVersion.Text = "&Restore";
            this.btnRestoreVersion.UseVisualStyleBackColor = true;
            this.btnRestoreVersion.Click += new System.EventHandler(this.btnRestoreVersion_Click);
            // 
            // lvItemHistory
            // 
            this.lvItemHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvItemHistory.Location = new System.Drawing.Point(393, 132);
            this.lvItemHistory.MultiSelect = false;
            this.lvItemHistory.Name = "lvItemHistory";
            this.lvItemHistory.Size = new System.Drawing.Size(257, 371);
            this.lvItemHistory.TabIndex = 1;
            this.lvItemHistory.UseCompatibleStateImageBehavior = false;
            this.lvItemHistory.View = System.Windows.Forms.View.Details;
            this.lvItemHistory.SelectedIndexChanged += new System.EventHandler(this.lvItemHistory_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date modified";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 0;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(390, 116);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(252, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Previous versions are listed from item change history";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(413, 568);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(494, 568);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Location = new System.Drawing.Point(575, 568);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "A&pply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // imgLstFarmViewIconsSmall
            // 
            this.imgLstFarmViewIconsSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstFarmViewIconsSmall.ImageStream")));
            this.imgLstFarmViewIconsSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(0, "web_oobe_siteicon.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(1, "FolderTools_Small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(2, "Folder_Small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(3, "user_small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(4, "Up_Small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(5, "GenericFile_Small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(6, "3g2.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(7, "3gp.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(8, "ai.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(9, "air.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(10, "asf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(11, "avi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(12, "bib.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(13, "cls.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(14, "csv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(15, "deb.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(16, "djvu.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(17, "dmg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(18, "doc.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(19, "docx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(20, "dwf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(21, "dwg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(22, "eps.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(23, "epub.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(24, "exe.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(25, "f.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(26, "f77.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(27, "f90.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(28, "flac.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(29, "flv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(30, "gif.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(31, "gz.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(32, "ico.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(33, "indd.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(34, "iso.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(35, "jpeg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(36, "jpg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(37, "log.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(38, "m4a.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(39, "m4v.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(40, "midi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(41, "mkv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(42, "mov.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(43, "mp3.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(44, "mp4.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(45, "mpeg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(46, "mpg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(47, "msi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(48, "odp.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(49, "ods.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(50, "odt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(51, "oga.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(52, "ogg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(53, "ogv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(54, "pdf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(55, "png.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(56, "pps.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(57, "ppsx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(58, "ppt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(59, "pptx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(60, "psd.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(61, "pub.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(62, "py.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(63, "qt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(64, "ra.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(65, "ram.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(66, "rar.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(67, "rm.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(68, "rpm.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(69, "rtf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(70, "rv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(71, "skp.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(72, "spx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(73, "sql.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(74, "sty.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(75, "tar.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(76, "tex.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(77, "tgz.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(78, "tiff.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(79, "ttf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(80, "txt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(81, "vob.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(82, "wav.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(83, "wmv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(84, "xls.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(85, "xlsx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(86, "xml.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(87, "xpi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(88, "zip.png");
            // 
            // sfdSaveFile
            // 
            this.sfdSaveFile.Filter = "All Files (*.*)|*.*";
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(615, 0);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 81;
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
            this.lblLogoText.Size = new System.Drawing.Size(623, 107);
            this.lblLogoText.TabIndex = 80;
            this.lblLogoText.Text = "Item Properties";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(27, 293);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(330, 2);
            this.label2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(27, 366);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(330, 2);
            this.label3.TabIndex = 4;
            // 
            // lblItemType
            // 
            this.lblItemType.Location = new System.Drawing.Point(103, 192);
            this.lblItemType.Name = "lblItemType";
            this.lblItemType.Size = new System.Drawing.Size(251, 22);
            this.lblItemType.TabIndex = 14;
            // 
            // lblItemLocation
            // 
            this.lblItemLocation.Location = new System.Drawing.Point(103, 216);
            this.lblItemLocation.Name = "lblItemLocation";
            this.lblItemLocation.Size = new System.Drawing.Size(251, 22);
            this.lblItemLocation.TabIndex = 15;
            // 
            // lblSize
            // 
            this.lblSize.Location = new System.Drawing.Point(103, 240);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(251, 22);
            this.lblSize.TabIndex = 16;
            // 
            // lblContains
            // 
            this.lblContains.Location = new System.Drawing.Point(103, 262);
            this.lblContains.Name = "lblContains";
            this.lblContains.Size = new System.Drawing.Size(170, 22);
            this.lblContains.TabIndex = 18;
            // 
            // lblCreated
            // 
            this.lblCreated.Location = new System.Drawing.Point(103, 297);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(251, 22);
            this.lblCreated.TabIndex = 19;
            // 
            // lblModified
            // 
            this.lblModified.Location = new System.Drawing.Point(103, 321);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(251, 22);
            this.lblModified.TabIndex = 20;
            // 
            // lblAccessed
            // 
            this.lblAccessed.Location = new System.Drawing.Point(103, 344);
            this.lblAccessed.Name = "lblAccessed";
            this.lblAccessed.Size = new System.Drawing.Size(251, 22);
            this.lblAccessed.TabIndex = 21;
            // 
            // chkReadonly
            // 
            this.chkReadonly.AutoSize = true;
            this.chkReadonly.Location = new System.Drawing.Point(89, 376);
            this.chkReadonly.Name = "chkReadonly";
            this.chkReadonly.Size = new System.Drawing.Size(74, 17);
            this.chkReadonly.TabIndex = 22;
            this.chkReadonly.Text = "&Read-only";
            this.chkReadonly.UseVisualStyleBackColor = true;
            // 
            // chkHidden
            // 
            this.chkHidden.AutoSize = true;
            this.chkHidden.Location = new System.Drawing.Point(169, 375);
            this.chkHidden.Name = "chkHidden";
            this.chkHidden.Size = new System.Drawing.Size(60, 17);
            this.chkHidden.TabIndex = 23;
            this.chkHidden.Text = "&Hidden";
            this.chkHidden.UseVisualStyleBackColor = true;
            // 
            // chkSystem
            // 
            this.chkSystem.AutoSize = true;
            this.chkSystem.Enabled = false;
            this.chkSystem.Location = new System.Drawing.Point(303, 375);
            this.chkSystem.Name = "chkSystem";
            this.chkSystem.Size = new System.Drawing.Size(60, 17);
            this.chkSystem.TabIndex = 24;
            this.chkSystem.Text = "&System";
            this.chkSystem.UseVisualStyleBackColor = true;
            // 
            // chkArchive
            // 
            this.chkArchive.AutoSize = true;
            this.chkArchive.Enabled = false;
            this.chkArchive.Location = new System.Drawing.Point(235, 375);
            this.chkArchive.Name = "chkArchive";
            this.chkArchive.Size = new System.Drawing.Size(62, 17);
            this.chkArchive.TabIndex = 25;
            this.chkArchive.Text = "&Archive";
            this.chkArchive.UseVisualStyleBackColor = true;
            // 
            // btnDownloadFile
            // 
            this.btnDownloadFile.Enabled = false;
            this.btnDownloadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadFile.Location = new System.Drawing.Point(279, 262);
            this.btnDownloadFile.Name = "btnDownloadFile";
            this.btnDownloadFile.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadFile.TabIndex = 27;
            this.btnDownloadFile.Text = "&Download";
            this.btnDownloadFile.UseVisualStyleBackColor = true;
            this.btnDownloadFile.Click += new System.EventHandler(this.btnDownloadFile_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(28, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 22);
            this.label4.TabIndex = 82;
            this.label4.Text = "Type";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(28, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 22);
            this.label5.TabIndex = 82;
            this.label5.Text = "Location";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(28, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 22);
            this.label6.TabIndex = 82;
            this.label6.Text = "Size";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(28, 262);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 22);
            this.label7.TabIndex = 82;
            this.label7.Text = "Contains";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(26, 299);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 22);
            this.label8.TabIndex = 82;
            this.label8.Text = "Created";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(26, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 22);
            this.label9.TabIndex = 82;
            this.label9.Text = "Modified";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(24, 343);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 22);
            this.label10.TabIndex = 82;
            this.label10.Text = "Accessed";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(25, 376);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 22);
            this.label11.TabIndex = 82;
            this.label11.Text = "Attributes";
            // 
            // frmProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(673, 613);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnViewVersion);
            this.Controls.Add(this.clbPermissionsForPrincipal);
            this.Controls.Add(this.btnRestoreVersion);
            this.Controls.Add(this.btnDownloadFile);
            this.Controls.Add(this.lvItemHistory);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnPermissionsEdit);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.chkArchive);
            this.Controls.Add(this.tbItemGuid);
            this.Controls.Add(this.lbUsersGroupsNames);
            this.Controls.Add(this.chkSystem);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.chkHidden);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.chkReadonly);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblAccessed);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblModified);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.pbItemIcon);
            this.Controls.Add(this.lblContains);
            this.Controls.Add(this.tbItemName);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblItemType);
            this.Controls.Add(this.lblItemLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.Shown += new System.EventHandler(this.frmProperties_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbItemIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbItemIcon;
        private System.Windows.Forms.TextBox tbItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnPermissionsEdit;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ListBox lbUsersGroupsNames;
        private System.Windows.Forms.CheckedListBox clbPermissionsForPrincipal;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ListView lvItemHistory;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnRestoreVersion;
        private System.Windows.Forms.Button btnViewVersion;
        private System.Windows.Forms.ImageList imgLstFarmViewIconsSmall;
        private System.Windows.Forms.TextBox tbItemGuid;
        private System.Windows.Forms.SaveFileDialog sfdSaveFile;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblItemType;
        private System.Windows.Forms.Label lblItemLocation;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblContains;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.Label lblModified;
        private System.Windows.Forms.Label lblAccessed;
        private System.Windows.Forms.CheckBox chkReadonly;
        private System.Windows.Forms.CheckBox chkHidden;
        private System.Windows.Forms.CheckBox chkSystem;
        private System.Windows.Forms.CheckBox chkArchive;
        private System.Windows.Forms.Button btnDownloadFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}