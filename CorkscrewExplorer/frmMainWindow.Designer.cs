namespace Corkscrew.Explorer
{
    partial class frmMainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainWindow));
            this.imgLstFarmViewIconsLarge = new System.Windows.Forms.ImageList(this.components);
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.menuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemNewSite = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemNewUserGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewUser = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewWorkflowDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganizeCopyTo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganizeMoveTo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganizeRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganizeDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemOrganizeProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemOrganizeSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganizeSelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrganizeSelectInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemOrganizeAssociateWorkflow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemViewSort = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewSortAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewSortDescending = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemViewShowCheckboxes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewShowFilenameExtensions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewShowHiddenItems = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemViewRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemViewWorkflowInstances = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.lvExplorerView = new System.Windows.Forms.ListView();
            this.imgLstFarmViewIconsSmall = new System.Windows.Forms.ImageList(this.components);
            this.sfdSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgLstFarmViewIconsLarge
            // 
            this.imgLstFarmViewIconsLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstFarmViewIconsLarge.ImageStream")));
            this.imgLstFarmViewIconsLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(0, "Up_Large.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(1, "User.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(2, "Folder_Sites.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(3, "Folder_UserGroups.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(4, "Folder_Users.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(5, "Folder_Workflows.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(6, "Folder_Empty.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(7, "Workflow.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(8, "Usergroup.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(9, "Site.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(10, "GenericFile_Large.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(11, "3g2.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(12, "3gp.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(13, "ai.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(14, "air.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(15, "asf.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(16, "avi.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(17, "bib.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(18, "cls.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(19, "csv.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(20, "deb.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(21, "djvu.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(22, "dmg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(23, "doc.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(24, "docx.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(25, "dwf.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(26, "dwg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(27, "eps.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(28, "epub.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(29, "exe.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(30, "f.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(31, "f77.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(32, "f90.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(33, "flac.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(34, "flv.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(35, "gif.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(36, "gz.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(37, "ico.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(38, "indd.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(39, "iso.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(40, "jpeg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(41, "jpg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(42, "log.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(43, "m4a.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(44, "m4v.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(45, "midi.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(46, "mkv.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(47, "mov.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(48, "mp3.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(49, "mp4.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(50, "mpeg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(51, "mpg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(52, "msi.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(53, "odp.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(54, "ods.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(55, "odt.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(56, "oga.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(57, "ogg.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(58, "ogv.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(59, "pdf.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(60, "png.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(61, "pps.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(62, "ppsx.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(63, "ppt.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(64, "pptx.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(65, "psd.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(66, "pub.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(67, "py.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(68, "qt.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(69, "ra.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(70, "ram.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(71, "rar.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(72, "rm.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(73, "rpm.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(74, "rtf.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(75, "rv.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(76, "skp.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(77, "spx.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(78, "sql.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(79, "sty.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(80, "tar.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(81, "tex.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(82, "tgz.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(83, "tiff.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(84, "ttf.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(85, "txt.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(86, "vob.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(87, "wav.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(88, "wmv.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(89, "xls.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(90, "xlsx.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(91, "xml.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(92, "xpi.png");
            this.imgLstFarmViewIconsLarge.Images.SetKeyName(93, "zip.png");
            // 
            // menuBar
            // 
            this.menuBar.Dock = System.Windows.Forms.DockStyle.None;
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNew,
            this.menuItemOrganize,
            this.menuItemView,
            this.menuItemHelp});
            this.menuBar.Location = new System.Drawing.Point(589, 96);
            this.menuBar.Name = "menuBar";
            this.menuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuBar.ShowItemToolTips = true;
            this.menuBar.Size = new System.Drawing.Size(205, 24);
            this.menuBar.TabIndex = 1;
            // 
            // menuItemNew
            // 
            this.menuItemNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripMenuItem9,
            this.menuItemExit});
            this.menuItemNew.Name = "menuItemNew";
            this.menuItemNew.Size = new System.Drawing.Size(43, 20);
            this.menuItemNew.Text = "&New";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNewFolder,
            this.menuItemNewFile,
            this.toolStripMenuItem10,
            this.menuItemNewSite,
            this.menuitemNewUserGroup,
            this.menuItemNewUser,
            this.menuItemNewWorkflowDefinition});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // menuItemNewFolder
            // 
            this.menuItemNewFolder.Name = "menuItemNewFolder";
            this.menuItemNewFolder.Size = new System.Drawing.Size(207, 22);
            this.menuItemNewFolder.Text = "&Folder";
            this.menuItemNewFolder.Click += new System.EventHandler(this.menuItemNewFolder_Click);
            // 
            // menuItemNewFile
            // 
            this.menuItemNewFile.Name = "menuItemNewFile";
            this.menuItemNewFile.Size = new System.Drawing.Size(207, 22);
            this.menuItemNewFile.Text = "Fil&e";
            this.menuItemNewFile.Click += new System.EventHandler(this.menuItemNewFile_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(204, 6);
            // 
            // menuItemNewSite
            // 
            this.menuItemNewSite.Name = "menuItemNewSite";
            this.menuItemNewSite.Size = new System.Drawing.Size(207, 22);
            this.menuItemNewSite.Text = "New &Site";
            this.menuItemNewSite.Click += new System.EventHandler(this.menuItemNewSite_Click);
            // 
            // menuitemNewUserGroup
            // 
            this.menuitemNewUserGroup.Name = "menuitemNewUserGroup";
            this.menuitemNewUserGroup.Size = new System.Drawing.Size(207, 22);
            this.menuitemNewUserGroup.Text = "New User &Group";
            this.menuitemNewUserGroup.Click += new System.EventHandler(this.menuitemNewUserGroup_Click);
            // 
            // menuItemNewUser
            // 
            this.menuItemNewUser.Name = "menuItemNewUser";
            this.menuItemNewUser.Size = new System.Drawing.Size(207, 22);
            this.menuItemNewUser.Text = "New &User";
            this.menuItemNewUser.Click += new System.EventHandler(this.menuItemNewUser_Click);
            // 
            // menuItemNewWorkflowDefinition
            // 
            this.menuItemNewWorkflowDefinition.Name = "menuItemNewWorkflowDefinition";
            this.menuItemNewWorkflowDefinition.Size = new System.Drawing.Size(207, 22);
            this.menuItemNewWorkflowDefinition.Text = "New &Workflow Definition";
            this.menuItemNewWorkflowDefinition.Click += new System.EventHandler(this.menuItemNewWorkflowDefinition_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(95, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(98, 22);
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemOrganize
            // 
            this.menuItemOrganize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOrganizeCopyTo,
            this.menuItemOrganizeMoveTo,
            this.menuItemOrganizeRename,
            this.menuItemOrganizeDelete,
            this.toolStripMenuItem2,
            this.menuItemOrganizeProperties,
            this.toolStripMenuItem3,
            this.menuItemOrganizeSelectAll,
            this.menuItemOrganizeSelectNone,
            this.menuItemOrganizeSelectInvert,
            this.toolStripMenuItem6,
            this.menuItemOrganizeAssociateWorkflow});
            this.menuItemOrganize.Name = "menuItemOrganize";
            this.menuItemOrganize.Size = new System.Drawing.Size(66, 20);
            this.menuItemOrganize.Text = "&Organize";
            // 
            // menuItemOrganizeCopyTo
            // 
            this.menuItemOrganizeCopyTo.Name = "menuItemOrganizeCopyTo";
            this.menuItemOrganizeCopyTo.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeCopyTo.Text = "&Copy to...";
            this.menuItemOrganizeCopyTo.Click += new System.EventHandler(this.menuItemOrganizeCopyTo_Click);
            // 
            // menuItemOrganizeMoveTo
            // 
            this.menuItemOrganizeMoveTo.Name = "menuItemOrganizeMoveTo";
            this.menuItemOrganizeMoveTo.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeMoveTo.Text = "&Move to...";
            this.menuItemOrganizeMoveTo.Click += new System.EventHandler(this.menuItemOrganizeMoveTo_Click);
            // 
            // menuItemOrganizeRename
            // 
            this.menuItemOrganizeRename.Name = "menuItemOrganizeRename";
            this.menuItemOrganizeRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuItemOrganizeRename.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeRename.Text = "&Rename...";
            this.menuItemOrganizeRename.Click += new System.EventHandler(this.menuItemOrganizeRename_Click);
            // 
            // menuItemOrganizeDelete
            // 
            this.menuItemOrganizeDelete.Name = "menuItemOrganizeDelete";
            this.menuItemOrganizeDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.menuItemOrganizeDelete.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeDelete.Text = "&Delete";
            this.menuItemOrganizeDelete.Click += new System.EventHandler(this.menuItemOrganizeDelete_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemOrganizeProperties
            // 
            this.menuItemOrganizeProperties.Name = "menuItemOrganizeProperties";
            this.menuItemOrganizeProperties.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.menuItemOrganizeProperties.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeProperties.Text = "&Properties";
            this.menuItemOrganizeProperties.Click += new System.EventHandler(this.menuItemOrganizeProperties_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemOrganizeSelectAll
            // 
            this.menuItemOrganizeSelectAll.Name = "menuItemOrganizeSelectAll";
            this.menuItemOrganizeSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuItemOrganizeSelectAll.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeSelectAll.Text = "&Select all";
            this.menuItemOrganizeSelectAll.Click += new System.EventHandler(this.menuItemOrganizeSelectAll_Click);
            // 
            // menuItemOrganizeSelectNone
            // 
            this.menuItemOrganizeSelectNone.Name = "menuItemOrganizeSelectNone";
            this.menuItemOrganizeSelectNone.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeSelectNone.Text = "Select &none";
            this.menuItemOrganizeSelectNone.Click += new System.EventHandler(this.menuItemOrganizeSelectNone_Click);
            // 
            // menuItemOrganizeSelectInvert
            // 
            this.menuItemOrganizeSelectInvert.Name = "menuItemOrganizeSelectInvert";
            this.menuItemOrganizeSelectInvert.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeSelectInvert.Text = "&Invert Selection";
            this.menuItemOrganizeSelectInvert.Click += new System.EventHandler(this.menuItemOrganizeSelectInvert_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemOrganizeAssociateWorkflow
            // 
            this.menuItemOrganizeAssociateWorkflow.Name = "menuItemOrganizeAssociateWorkflow";
            this.menuItemOrganizeAssociateWorkflow.Size = new System.Drawing.Size(178, 22);
            this.menuItemOrganizeAssociateWorkflow.Text = "&Associate Workflow";
            this.menuItemOrganizeAssociateWorkflow.Click += new System.EventHandler(this.menuItemOrganizeAssociateWorkflow_Click);
            // 
            // menuItemView
            // 
            this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewLargeIcons,
            this.menuItemViewSmallIcons,
            this.menuItemViewTiles,
            this.menuItemViewList,
            this.menuItemViewDetails,
            this.toolStripMenuItem4,
            this.menuItemViewSort,
            this.toolStripMenuItem5,
            this.menuItemViewShowCheckboxes,
            this.menuItemViewShowFilenameExtensions,
            this.menuItemViewShowHiddenItems,
            this.toolStripMenuItem7,
            this.menuItemViewRefresh,
            this.toolStripMenuItem8,
            this.menuItemViewWorkflowInstances});
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(44, 20);
            this.menuItemView.Text = "&View";
            // 
            // menuItemViewLargeIcons
            // 
            this.menuItemViewLargeIcons.Checked = true;
            this.menuItemViewLargeIcons.CheckOnClick = true;
            this.menuItemViewLargeIcons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemViewLargeIcons.Name = "menuItemViewLargeIcons";
            this.menuItemViewLargeIcons.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewLargeIcons.Text = "Large Icons";
            this.menuItemViewLargeIcons.Click += new System.EventHandler(this.menuItemViewLargeIcons_Click);
            // 
            // menuItemViewSmallIcons
            // 
            this.menuItemViewSmallIcons.CheckOnClick = true;
            this.menuItemViewSmallIcons.Name = "menuItemViewSmallIcons";
            this.menuItemViewSmallIcons.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewSmallIcons.Text = "Small Icons";
            this.menuItemViewSmallIcons.Click += new System.EventHandler(this.menuItemViewSmallIcons_Click);
            // 
            // menuItemViewTiles
            // 
            this.menuItemViewTiles.CheckOnClick = true;
            this.menuItemViewTiles.Name = "menuItemViewTiles";
            this.menuItemViewTiles.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewTiles.Text = "Tiles";
            this.menuItemViewTiles.Click += new System.EventHandler(this.menuItemViewTiles_Click);
            // 
            // menuItemViewList
            // 
            this.menuItemViewList.CheckOnClick = true;
            this.menuItemViewList.Name = "menuItemViewList";
            this.menuItemViewList.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewList.Text = "List";
            this.menuItemViewList.Click += new System.EventHandler(this.menuItemViewList_Click);
            // 
            // menuItemViewDetails
            // 
            this.menuItemViewDetails.CheckOnClick = true;
            this.menuItemViewDetails.Name = "menuItemViewDetails";
            this.menuItemViewDetails.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewDetails.Text = "Details";
            this.menuItemViewDetails.Click += new System.EventHandler(this.menuItemViewDetails_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemViewSort
            // 
            this.menuItemViewSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewSortAscending,
            this.menuItemViewSortDescending});
            this.menuItemViewSort.Name = "menuItemViewSort";
            this.menuItemViewSort.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewSort.Text = "&Sort";
            // 
            // menuItemViewSortAscending
            // 
            this.menuItemViewSortAscending.Checked = true;
            this.menuItemViewSortAscending.CheckOnClick = true;
            this.menuItemViewSortAscending.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemViewSortAscending.Name = "menuItemViewSortAscending";
            this.menuItemViewSortAscending.Size = new System.Drawing.Size(159, 22);
            this.menuItemViewSortAscending.Text = "Sort ascending";
            this.menuItemViewSortAscending.Click += new System.EventHandler(this.menuItemViewSortAscending_Click);
            // 
            // menuItemViewSortDescending
            // 
            this.menuItemViewSortDescending.CheckOnClick = true;
            this.menuItemViewSortDescending.Name = "menuItemViewSortDescending";
            this.menuItemViewSortDescending.Size = new System.Drawing.Size(159, 22);
            this.menuItemViewSortDescending.Text = "Sort descending";
            this.menuItemViewSortDescending.Click += new System.EventHandler(this.menuItemViewSortDescending_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemViewShowCheckboxes
            // 
            this.menuItemViewShowCheckboxes.CheckOnClick = true;
            this.menuItemViewShowCheckboxes.Name = "menuItemViewShowCheckboxes";
            this.menuItemViewShowCheckboxes.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewShowCheckboxes.Text = "Show Checkboxes";
            this.menuItemViewShowCheckboxes.Click += new System.EventHandler(this.menuItemViewShowCheckboxes_Click);
            // 
            // menuItemViewShowFilenameExtensions
            // 
            this.menuItemViewShowFilenameExtensions.Checked = true;
            this.menuItemViewShowFilenameExtensions.CheckOnClick = true;
            this.menuItemViewShowFilenameExtensions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemViewShowFilenameExtensions.Name = "menuItemViewShowFilenameExtensions";
            this.menuItemViewShowFilenameExtensions.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewShowFilenameExtensions.Text = "Filename extensions";
            this.menuItemViewShowFilenameExtensions.Click += new System.EventHandler(this.menuItemViewShowFilenameExtensions_Click);
            // 
            // menuItemViewShowHiddenItems
            // 
            this.menuItemViewShowHiddenItems.CheckOnClick = true;
            this.menuItemViewShowHiddenItems.Name = "menuItemViewShowHiddenItems";
            this.menuItemViewShowHiddenItems.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewShowHiddenItems.Text = "Hidden items";
            this.menuItemViewShowHiddenItems.Click += new System.EventHandler(this.menuItemViewShowHiddenItems_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemViewRefresh
            // 
            this.menuItemViewRefresh.Name = "menuItemViewRefresh";
            this.menuItemViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuItemViewRefresh.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewRefresh.Text = "Refresh";
            this.menuItemViewRefresh.Click += new System.EventHandler(this.menuItemViewRefresh_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemViewWorkflowInstances
            // 
            this.menuItemViewWorkflowInstances.Name = "menuItemViewWorkflowInstances";
            this.menuItemViewWorkflowInstances.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewWorkflowInstances.Text = "&Workflow Instances";
            this.menuItemViewWorkflowInstances.Click += new System.EventHandler(this.menuItemViewWorkflowInstances_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewHelpAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemViewHelpAbout
            // 
            this.menuItemViewHelpAbout.Name = "menuItemViewHelpAbout";
            this.menuItemViewHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.menuItemViewHelpAbout.Text = "&About";
            this.menuItemViewHelpAbout.Click += new System.EventHandler(this.menuItemViewHelpAbout_Click);
            // 
            // lvExplorerView
            // 
            this.lvExplorerView.AllowDrop = true;
            this.lvExplorerView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvExplorerView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lvExplorerView.ForeColor = System.Drawing.Color.White;
            this.lvExplorerView.LargeImageList = this.imgLstFarmViewIconsLarge;
            this.lvExplorerView.Location = new System.Drawing.Point(0, 123);
            this.lvExplorerView.MultiSelect = false;
            this.lvExplorerView.Name = "lvExplorerView";
            this.lvExplorerView.Size = new System.Drawing.Size(800, 476);
            this.lvExplorerView.SmallImageList = this.imgLstFarmViewIconsSmall;
            this.lvExplorerView.TabIndex = 2;
            this.lvExplorerView.UseCompatibleStateImageBehavior = false;
            this.lvExplorerView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvExplorerView_AfterLabelEdit);
            this.lvExplorerView.ItemActivate += new System.EventHandler(this.lvExplorerView_ItemActivate);
            this.lvExplorerView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvExplorerView_ItemChecked);
            this.lvExplorerView.SelectedIndexChanged += new System.EventHandler(this.lvExplorerView_SelectedIndexChanged);
            this.lvExplorerView.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvExplorerView_DragDrop);
            this.lvExplorerView.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvExplorerView_DragEnter);
            // 
            // imgLstFarmViewIconsSmall
            // 
            this.imgLstFarmViewIconsSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstFarmViewIconsSmall.ImageStream")));
            this.imgLstFarmViewIconsSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(0, "Up_Small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(1, "User.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(2, "Folder_Sites.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(3, "Folder_UserGroups.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(4, "Folder_Users.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(5, "Folder_Workflows.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(6, "Folder_Empty.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(7, "Workflow.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(8, "Usergroup.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(9, "Site.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(10, "GenericFile_Small.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(11, "3g2.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(12, "3gp.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(13, "ai.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(14, "air.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(15, "asf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(16, "avi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(17, "bib.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(18, "cls.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(19, "csv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(20, "deb.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(21, "djvu.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(22, "dmg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(23, "doc.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(24, "docx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(25, "dwf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(26, "dwg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(27, "eps.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(28, "epub.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(29, "exe.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(30, "f.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(31, "f77.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(32, "f90.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(33, "flac.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(34, "flv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(35, "gif.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(36, "gz.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(37, "ico.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(38, "indd.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(39, "iso.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(40, "jpeg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(41, "jpg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(42, "log.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(43, "m4a.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(44, "m4v.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(45, "midi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(46, "mkv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(47, "mov.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(48, "mp3.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(49, "mp4.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(50, "mpeg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(51, "mpg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(52, "msi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(53, "odp.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(54, "ods.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(55, "odt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(56, "oga.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(57, "ogg.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(58, "ogv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(59, "pdf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(60, "png.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(61, "pps.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(62, "ppsx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(63, "ppt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(64, "pptx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(65, "psd.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(66, "pub.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(67, "py.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(68, "qt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(69, "ra.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(70, "ram.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(71, "rar.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(72, "rm.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(73, "rpm.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(74, "rtf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(75, "rv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(76, "skp.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(77, "spx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(78, "sql.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(79, "sty.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(80, "tar.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(81, "tex.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(82, "tgz.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(83, "tiff.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(84, "ttf.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(85, "txt.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(86, "vob.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(87, "wav.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(88, "wmv.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(89, "xls.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(90, "xlsx.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(91, "xml.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(92, "xpi.png");
            this.imgLstFarmViewIconsSmall.Images.SetKeyName(93, "zip.png");
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
            this.btnFormClose.Location = new System.Drawing.Point(740, -2);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 4;
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
            this.lblLogoText.Location = new System.Drawing.Point(-5, -2);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(739, 107);
            this.lblLogoText.TabIndex = 3;
            this.lblLogoText.Text = "Explorer";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // frmMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.ControlBox = false;
            this.Controls.Add(this.menuBar);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.lvExplorerView);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuBar;
            this.Name = "frmMainWindow";
            this.Text = "Corkscrew Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.frmMainWindow_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMainWindow_MouseDown);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imgLstFarmViewIconsLarge;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem menuItemNew;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganize;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeCopyTo;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeMoveTo;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeRename;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeSelectNone;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeSelectInvert;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.ListView lvExplorerView;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewLargeIcons;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewSmallIcons;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewTiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewList;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewSort;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewSortAscending;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewSortDescending;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewShowCheckboxes;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewShowFilenameExtensions;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewShowHiddenItems;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewHelpAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrganizeAssociateWorkflow;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewRefresh;
        private System.Windows.Forms.ImageList imgLstFarmViewIconsSmall;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewWorkflowInstances;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.SaveFileDialog sfdSaveFile;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewFolder;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewFile;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewSite;
        private System.Windows.Forms.ToolStripMenuItem menuitemNewUserGroup;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewUser;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewWorkflowDefinition;
    }
}

