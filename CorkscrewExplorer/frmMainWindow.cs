using Corkscrew.SDK.constants;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.providers.database;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmMainWindow : Form
    {

        // these are flags set by clicking on various menu options
        private bool _showFilenameExtension = true;                     // show filename extensions or hide them
        private bool _showHiddenItems = false;                          // show filesystem items with Hidden attribute set


        private bool _workflowsEnabled = false;                         // determined if workflows are enabled for this farm. If disabled, related items are not shown

        // references to items as we load and use them
        private CSFarm _currentFarm = null;
        private CSSite _currentSite = null;
        private CSFileSystemEntry _currentFileSystemEntry = null;

        private bool _isCreatingNewDirectory = false;                   // set to true when File > New Directory is activated, and unset when action is completed.

        // delegate to fire when loading a special folder
        private Action _loadSpecialFolderFunction = null;

        // index markers in our ImageList controls
        private const int ICON_INDEX_UP = 0;
        private const int ICON_INDEX_SINGLE_USER = 1;
        private const int ICON_INDEX_FOLDER_SITES = 2;
        private const int ICON_INDEX_FOLDER_USERGROUPS = 3;
        private const int ICON_INDEX_FOLDER_USERS = 4;
        private const int ICON_INDEX_FOLDER_WORKFLOWS = 5;
        private const int ICON_INDEX_FOLDER_FOLDER = 6;
        private const int ICON_INDEX_ITEM_WORKFLOW = 7;
        private const int ICON_INDEX_ITEM_USERGROUP = 8;
        private const int ICON_INDEX_ITEM_SITE = 9;
        private const int ICON_INDEX_FILE_GENERIC = 10;
        private const int ICON_INDEX_FILE_TYPES_START = 11;

        #region Form Events
        public frmMainWindow()
        {
            InitializeComponent();
        }

        private void frmMainWindow_Shown(object sender, EventArgs e)
        {
            bool _isLoggedIn = false;

            bool farmIsProvisioned = true;
            if ((ConfigurationManager.ConnectionStrings != null) && (ConfigurationManager.ConnectionStrings["configdb"] != null))
            {
                ICSDatabaseProvider provider = CSDatabaseProviderFactory.GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["configdb"].ConnectionString, true);
                farmIsProvisioned = (provider != null);
                provider = null;
            }
            else
            {
                farmIsProvisioned = false;
            }

            if (!farmIsProvisioned)
            {
                using (frmConnectToFarm frm = new frmConnectToFarm())
                {
                    frm.ShowDialog();
                }
            }

            frmLogin login = new frmLogin();
            if (login.ShowDialog() != DialogResult.Cancel)
            {
                if (login.LoginResult)
                {
                    _isLoggedIn = true;
                    _currentFarm = login.Farm;
                    _workflowsEnabled = _currentFarm.IsWorkflowEnabled;

                    LoadListView(CSPath.CmsPathPrefix);     // load the farm
                }
            }

            login.Close();

            if (!_isLoggedIn)
            {
                Application.Exit();
            }
        }

        private void frmMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Quit();
        }
        #endregion

        #region Methods

        private void RefreshCurrentView()
        {
            lvExplorerView.Items.Clear();

            if (_loadSpecialFolderFunction != null)     // if a delegate is set, run it
            {
                _loadSpecialFolderFunction();
            }
            else
            {
                if (_currentFileSystemEntry != null)
                {
                    LoadListView(_currentFileSystemEntry.FullPath);
                }
                else
                {
                    LoadListView(CSPath.CmsPathPrefix);
                }
            }
        }

        private void LoadListView(string resourceUri)
        {
            PATH_INFO resourcePathInfo = CSPath.GetPathInfo(resourceUri);
            if (!resourcePathInfo.IsValid)
            {
                return;     // we have no idea what we were expected to load. Probably a special folder and we were called by accident.
            }

            bool uiNeedsAclChange = false;

            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;

            _loadSpecialFolderFunction = null;
            lvExplorerView.Items.Clear();

            if (lvExplorerView.Columns.Count == 0)
            {
                lvExplorerView.Columns.Add("Name", 280);
                lvExplorerView.Columns.Add("Date modified", 120);
                lvExplorerView.Columns.Add("Type", 120);
                lvExplorerView.Columns.Add("Size", 80);
            }

            switch (resourcePathInfo.ResourceUriScope)
            {
                case ScopeEnum.Farm:
                    _currentSite = null;
                    _currentFileSystemEntry = null;
                    uiNeedsAclChange = true;
                    LoadFarm();
                    break;

                case ScopeEnum.Site:
                case ScopeEnum.FileOrDirectory:
                    _currentSite = _currentFarm.AllSites.Find(resourcePathInfo.SiteId);
                    CSFileSystemEntry fsItem = CSFileSystemEntry.GetItemInfo(_currentSite, CSPath.GetFullPath(_currentSite, resourcePathInfo.ResourceURI));
                    if (fsItem == null)
                    {
                        MessageBox.Show("Error! Item cannot be retrieved.");
                        return;
                    }

                    if (fsItem.IsFolder)
                    {
                        _currentFileSystemEntry = fsItem;
                        uiNeedsAclChange = true;
                        LoadDirectory();
                    }
                    else
                    {
                        // a file was double-clicked. Download it to a temp location and open it
                        string localFileName = Path.Combine(Path.GetTempPath(), fsItem.FilenameWithExtension);
                        using (CSFileSystemEntryFile file = new CSFileSystemEntryFile(fsItem))
                        {
                            if (file.Open(FileAccess.Read))
                            {
                                byte[] buffer = new byte[file.Size];
                                int bytes = file.Read(buffer, 0, file.Size);
                                if (bytes > 0)
                                {
                                    Stream stream = File.OpenWrite(localFileName);
                                    stream.Write(buffer, 0, bytes);
                                    stream.Close();

                                    // cheat so that we know if the file was edited by the user
                                    File.SetCreationTime(localFileName, file.Created);
                                    File.SetLastWriteTime(localFileName, file.Modified);

                                    if (Process.Start("rundll32.exe", "shell32.dll,ShellExec_RunDLL \"" + localFileName + "\"") == null)
                                    {
                                        // could not open the file
                                        /*
                                         *  On a modern Windows OS, we should never actually get here because RunDLL32 will find a handler or show the 
                                         *  "Pick a program to open..." dialog. 
                                         */ 

                                        if (sfdSaveFile.ShowDialog(this) == DialogResult.Cancel)
                                        {
                                            File.Delete(localFileName);
                                        }
                                        else
                                        {
                                            File.Copy(localFileName, sfdSaveFile.FileName);

                                            // cheat so that we know if the file was edited by the user
                                            File.SetCreationTime(sfdSaveFile.FileName, file.Created);
                                            File.SetLastWriteTime(sfdSaveFile.FileName, file.Modified);
                                        }
                                    }
                                }

                                file.Close();
                            }
                        }

                        LoadDirectory();
                    }
                    break;
            }

            if (uiNeedsAclChange)
            {
                SetUIBasedOnACL(CSPermission.TestAccess(_currentSite, _currentFileSystemEntry, _currentFarm.AuthenticatedUser));
            }

            this.UseWaitCursor = false;
            this.Cursor = Cursors.Default;
        }

        private void LoadFarm()
        {
            bool usingWaitCursor = this.UseWaitCursor;
            Cursor previousCursor = this.Cursor;
            if (!usingWaitCursor)
            {
                previousCursor = this.Cursor;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.Default;
            }

            // three "folders": Users, Workflow Definitions, Workflow Associations
            lvExplorerView.Items.Add(new ListViewItem("Users", ICON_INDEX_FOLDER_USERS));
            lvExplorerView.Items.Add(new ListViewItem("User Groups", ICON_INDEX_FOLDER_USERGROUPS));

            if (_workflowsEnabled)
            {
                lvExplorerView.Items.Add(new ListViewItem("Workflow Definitions", ICON_INDEX_FOLDER_WORKFLOWS));
                lvExplorerView.Items.Add(new ListViewItem("Workflow Associations", ICON_INDEX_FOLDER_WORKFLOWS));
            }

            foreach (CSSite site in _currentFarm.AllSites)
            {
                ListViewItem item = new ListViewItem(site.Name);
                item.SubItems.Add(site.Modified.ToString("MMM dd, yyyy HH:mm:ss"));
                item.SubItems.Add("Site");
                item.SubItems.Add(site.RootFolder.FolderSizeBytes.ToString());          // on a large site this may take a while? need to check!
                item.ImageIndex = ICON_INDEX_ITEM_SITE;
                item.Tag = site.RootFolder.FullPath;

                lvExplorerView.Items.Add(item);
                Application.DoEvents();
            }

            if (!usingWaitCursor)
            {
                this.UseWaitCursor = false;
                this.Cursor = previousCursor;
            }
        }

        private void LoadDirectory()
        {
            if (!_currentFileSystemEntry.IsFolder)
            {
                return;
            }

            bool usingWaitCursor = this.UseWaitCursor;
            Cursor previousCursor = this.Cursor;
            if (!usingWaitCursor)
            {
                previousCursor = this.Cursor;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.Default;
            }

            lvExplorerView.Items.Add(
                    new ListViewItem
                    (
                        new string[]
                        {
                            "..",                   // name
                            "",                     // modified
                            "Parent Level",         // type
                            ""                      // size
                        },
                        ICON_INDEX_UP               // icon
                    )
                    {
                        Tag = ((_currentFileSystemEntry.ParentDirectory == null) ? CSPath.CmsPathPrefix : _currentFileSystemEntry.ParentDirectoryPath)
                    }
                );

            if (_workflowsEnabled)
            {
                lvExplorerView.Items.Add(new ListViewItem("Workflow Associations", ICON_INDEX_FOLDER_WORKFLOWS));
            }

            CSFileSystemEntryDirectory folder = new CSFileSystemEntryDirectory(_currentFileSystemEntry);
            foreach (CSFileSystemEntry entry in folder.Items)
            {
                if (entry.IsHidden && (!_showHiddenItems))
                {
                    continue;
                }

                CSFileSystemEntryFile file = ((entry.IsFolder) ? null : new CSFileSystemEntryFile(entry));

                ListViewItem item = new ListViewItem((_showFilenameExtension ? entry.FilenameWithExtension : entry.Filename));
                item.SubItems.Add(entry.Modified.ToString("MMM dd, yyyy HH:mm:ss"));
                item.SubItems.Add((entry.IsFolder ? "Folder" : file.MimeType));
                item.SubItems.Add((entry.IsFolder ? HumanSize((new CSFileSystemEntryDirectory(entry)).FolderSizeBytes) : file.SizeHuman));
                item.Tag = entry.FullPath;

                int imgIndex = ICON_INDEX_FOLDER_FOLDER;   // folder icon

                if (!entry.IsFolder)
                {
                    imgIndex = ICON_INDEX_FILE_GENERIC;   // default for files

                    string fileExtensionAsPngName = entry.FilenameExtension.SafeString(removeAtStart: ".");
                    for (int index = ICON_INDEX_FILE_TYPES_START; index < imgLstFarmViewIconsLarge.Images.Count; index++)
                    {
                        string imgKeyFilename = Path.GetFileNameWithoutExtension(imgLstFarmViewIconsLarge.Images.Keys[index]);
                        if (imgKeyFilename == fileExtensionAsPngName)
                        {
                            imgIndex = index;
                            break;
                        }
                    }
                }

                item.ImageIndex = imgIndex;

                lvExplorerView.Items.Add(item);
                Application.DoEvents();
            }

            if (!usingWaitCursor)
            {
                this.UseWaitCursor = false;
                this.Cursor = previousCursor;
            }
        }

        private void LoadUsers()
        {
            bool usingWaitCursor = this.UseWaitCursor;
            Cursor previousCursor = this.Cursor;
            if (!usingWaitCursor)
            {
                previousCursor = this.Cursor;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.Default;
            }

            _loadSpecialFolderFunction = LoadUsers;

            lvExplorerView.Items.Add(
                new ListViewItem
                    (
                        new string[]
                        {
                            "..",                   // name
                            "",                     // modified
                            "Parent Level",         // type
                            ""                      // size
                        },
                        ICON_INDEX_UP               // icon
                    )
                {
                    Tag = CSPath.CmsPathPrefix
                }
            );

            foreach (CSUser user in _currentFarm.AllUsers)
            {
                ListViewItem item = new ListViewItem(user.LongformDisplayName);
                item.SubItems.Add("");
                item.SubItems.Add("User");
                item.SubItems.Add("");      // size.. user does not have one!
                item.ImageIndex = ICON_INDEX_SINGLE_USER;
                item.Tag = string.Format("user://{0}", user.Id);

                lvExplorerView.Items.Add(item);
                Application.DoEvents();
            }

            if (! usingWaitCursor)
            {
                this.UseWaitCursor = false;
                this.Cursor = previousCursor;
            }
        }

        private void LoadUserGroups()
        {
            bool usingWaitCursor = this.UseWaitCursor;
            Cursor previousCursor = this.Cursor;
            if (!usingWaitCursor)
            {
                previousCursor = this.Cursor;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.Default;
            }

            _loadSpecialFolderFunction = LoadUserGroups;

            lvExplorerView.Items.Add(
                new ListViewItem
                    (
                        new string[]
                        {
                            "..",                   // name
                            "",                     // modified
                            "Parent Level",         // type
                            ""                      // size
                        },
                        ICON_INDEX_UP               // icon
                    )
                {
                    Tag = CSPath.CmsPathPrefix
                }
            );

            foreach (CSUserGroup group in _currentFarm.AllUserGroups)
            {
                ListViewItem item = new ListViewItem(group.LongformDisplayName);
                item.SubItems.Add("");
                item.SubItems.Add("User group");
                item.SubItems.Add("");      // size.. user does not have one!
                item.ImageIndex = ICON_INDEX_ITEM_USERGROUP;
                item.Tag = string.Format("group://{0}", group.Id);

                lvExplorerView.Items.Add(item);
                Application.DoEvents();
            }

            if (!usingWaitCursor)
            {
                this.UseWaitCursor = false;
                this.Cursor = previousCursor;
            }
        }

        private void LoadWorkflowDefinitions()
        {
            bool usingWaitCursor = this.UseWaitCursor;
            Cursor previousCursor = this.Cursor;
            if (!usingWaitCursor)
            {
                previousCursor = this.Cursor;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.Default;
            }

            _loadSpecialFolderFunction = LoadWorkflowDefinitions;

            lvExplorerView.Items.Add(
                new ListViewItem
                    (
                        new string[]
                        {
                            "..",                   // name
                            "",                     // modified
                            "Parent Level",         // type
                            ""                      // size
                        },
                        ICON_INDEX_UP               // icon
                    )
                {
                    Tag = CSPath.CmsPathPrefix
                }
            );

            foreach (CSWorkflowDefinition definition in _currentFarm.AllWorkflowDefinitions)
            {
                ListViewItem item = new ListViewItem(definition.Name);
                item.SubItems.Add(definition.Modified.ToString("MMM dd, yyyy HH:mm:ss"));
                item.SubItems.Add("Workflow definition");
                item.SubItems.Add("");      // size.. wf-def does not have one!
                item.ImageIndex = ICON_INDEX_ITEM_WORKFLOW;
                item.Tag = string.Format("wfdef://{0}", definition.Id);

                lvExplorerView.Items.Add(item);
                Application.DoEvents();
            }

            if (! usingWaitCursor)
            {
                this.UseWaitCursor = false;
                this.Cursor = previousCursor;
            }
        }

        private void LoadWorkflowAssociations()
        {
            bool usingWaitCursor = this.UseWaitCursor;
            Cursor previousCursor = this.Cursor;
            if (!usingWaitCursor)
            {
                previousCursor = this.Cursor;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.Default;
            }

            _loadSpecialFolderFunction = LoadWorkflowAssociations;

            lvExplorerView.Items.Add(
                new ListViewItem
                    (
                        new string[]
                        {
                            "..",                   // name
                            "",                     // modified
                            "Parent Level",         // type
                            ""                      // size
                        },
                        ICON_INDEX_UP               // icon
                    )
                {
                    Tag = ((_currentSite != null) ? _currentSite.RootFolder.FullPath : CSPath.CmsPathPrefix)
                }
            );

            CSWorkflowAssociationCollection associations = null;
            if ((_currentFileSystemEntry != null) && (_currentFileSystemEntry.IsFolder))
            {
                associations = new CSWorkflowAssociationCollection(new CSFileSystemEntryDirectory(_currentFileSystemEntry), _currentSite.IsAuthenticatedUserSiteAdministrator);
            }
            else if (_currentSite != null)
            {
                associations = new CSWorkflowAssociationCollection(_currentSite, _currentSite.IsAuthenticatedUserSiteAdministrator);
            }
            else
            {
                associations = new CSWorkflowAssociationCollection(_currentFarm, _currentFarm.IsAuthenticatedUserFarmAdministrator);
            }

            if (associations != null)
            {
                foreach (CSWorkflowAssociation assoc in associations)
                {
                    ListViewItem item = new ListViewItem(assoc.Name);
                    item.SubItems.Add(assoc.Modified.ToString("MMM dd, yyyy HH:mm:ss"));
                    item.SubItems.Add("Workflow association");
                    item.SubItems.Add("");      // size.. wf-assn does not have one!
                    item.ImageIndex = ICON_INDEX_ITEM_WORKFLOW;
                    item.Tag = string.Format("wfassoc://{0}", assoc.Id);

                    lvExplorerView.Items.Add(item);
                    Application.DoEvents();
                }
            }

            if (! usingWaitCursor)
            {
                this.UseWaitCursor = false;
                this.Cursor = previousCursor;
            }
        }

        private void UnsetViewOptions()
        {
            switch (lvExplorerView.View)
            {
                case View.LargeIcon:
                    menuItemViewSmallIcons.Checked = false;
                    menuItemViewTiles.Checked = false;
                    menuItemViewList.Checked = false;
                    menuItemViewDetails.Checked = false;
                    break;

                case View.SmallIcon:
                    menuItemViewLargeIcons.Checked = false;
                    menuItemViewTiles.Checked = false;
                    menuItemViewList.Checked = false;
                    menuItemViewDetails.Checked = false;
                    break;

                case View.Tile:
                    menuItemViewSmallIcons.Checked = false;
                    menuItemViewLargeIcons.Checked = false;
                    menuItemViewList.Checked = false;
                    menuItemViewDetails.Checked = false;
                    break;

                case View.List:
                    menuItemViewSmallIcons.Checked = false;
                    menuItemViewTiles.Checked = false;
                    menuItemViewLargeIcons.Checked = false;
                    menuItemViewDetails.Checked = false;
                    break;

                case View.Details:
                    menuItemViewSmallIcons.Checked = false;
                    menuItemViewTiles.Checked = false;
                    menuItemViewList.Checked = false;
                    menuItemViewLargeIcons.Checked = false;
                    break;
            }

            if ((!menuItemViewLargeIcons.Checked) && (!menuItemViewSmallIcons.Checked) && (!menuItemViewTiles.Checked) && (!menuItemViewList.Checked) && (!menuItemViewDetails.Checked))
            {
                // nothing is checked, go default
                menuItemViewLargeIcons.Checked = true;
                lvExplorerView.View = View.LargeIcon;
            }
        }

        private void SetUIBasedOnACL(CSPermission acl)
        {

            bool enableEditControls = (acl.CanContribute || acl.CanFullControl);

            // disable everything 
            //foreach (ToolStripItem item in menuItemNew.DropDownItems)
            //{
            //    if (item is ToolStripMenuItem)
            //    {
            //        item.Enabled = false;
            //    }
            //}
            //menuItemNew.Enabled = enableEditControls;

            foreach (ToolStripItem item in menuItemOrganize.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    item.Enabled = false;
                }
            }
            menuItemOrganize.Enabled = enableEditControls;

            menuItemViewWorkflowInstances.Enabled = _workflowsEnabled;

            // always keep enabled few things...
            menuItemExit.Enabled = true;
            menuItemOrganizeProperties.Enabled = true;

            // enable a few items regardless of context based on acl alone
            if (enableEditControls)
            {
                menuItemOrganizeAssociateWorkflow.Enabled = true && _workflowsEnabled;
                menuItemViewWorkflowInstances.Enabled = true && _workflowsEnabled;
            }

            // now enable items based on where we are...
            if (_currentFileSystemEntry != null)
            {
                if (_currentFileSystemEntry.IsFolder)
                {
                    // we are in a folder. Enable New Folder and New File items
                    menuItemNewFile.Enabled = enableEditControls;
                    menuItemNewFolder.Enabled = enableEditControls;
                }

                // enable copy/move/rename
                menuItemOrganizeCopyTo.Enabled = enableEditControls;
                menuItemOrganizeMoveTo.Enabled = enableEditControls;
                menuItemOrganizeRename.Enabled = enableEditControls;

                menuItemOrganizeAssociateWorkflow.Enabled = enableEditControls && _workflowsEnabled;
            }
            else if (_currentSite == null)
            {
                // farm level
                menuItemNewSite.Enabled = enableEditControls;
                menuItemNewUser.Enabled = enableEditControls;

                menuItemNewWorkflowDefinition.Enabled = enableEditControls && _workflowsEnabled;
            }

        }

        private void ShowItemPropertyWindow(string tagName)
        {
            if (tagName.StartsWith("user://"))
            {
                Guid userId = Utility.SafeConvertToGuid(tagName.Replace("user://", ""));
                CSUser user = CSUser.GetById(userId);

                using (frmCreateEditUser frm = new frmCreateEditUser())
                {
                    frm.User = user;
                    frm.Farm = _currentFarm;
                    frm.ShowDialog(this);
                }

                lvExplorerView.Items.Clear();
                LoadUsers();
            }

            if (tagName.StartsWith("group://"))
            {
                Guid userId = Utility.SafeConvertToGuid(tagName.Replace("group://", ""));
                CSUserGroup group = CSUserGroup.GetById(userId);

                using (frmCreateEditUserGroup frm = new frmCreateEditUserGroup())
                {
                    frm.UserGroup = group;
                    frm.Farm = _currentFarm;
                    frm.ShowDialog(this);
                }

                lvExplorerView.Items.Clear();
                LoadUserGroups();
            }

            if (tagName.StartsWith("wfdef://"))
            {
                Guid wfDefId = Utility.SafeConvertToGuid(tagName.Replace("wfdef://", ""));
                CSWorkflowDefinition def = CSWorkflowDefinition.Get(_currentFarm, wfDefId);

                using (frmDefineWorkflow frm = new frmDefineWorkflow())
                {
                    frm.Farm = _currentFarm;
                    frm.WorkflowDefinition = def;
                    frm.ShowDialog(this);
                }

                lvExplorerView.Items.Clear();
                LoadWorkflowDefinitions();
            }

            if (tagName.StartsWith("wfassoc://"))
            {
                Guid wfAssId = Utility.SafeConvertToGuid(tagName.Replace("wfassoc://", ""));

                if ((_currentFileSystemEntry != null) && (_currentFileSystemEntry.IsFolder))
                {
                    CSFileSystemEntryDirectory dir = new CSFileSystemEntryDirectory(_currentFileSystemEntry);
                    foreach (CSWorkflowAssociation assoc in dir.AllWorkflows)
                    {
                        if (assoc.Id.Equals(wfAssId))
                        {
                            using (frmAssociateWorkflow frm = new frmAssociateWorkflow())
                            {
                                frm.Farm = _currentFarm;
                                frm.WorkflowDefinition = assoc.WorkflowDefinition;
                                frm.WorkflowAssociation = assoc;
                                frm.WorkflowScope = ScopeEnum.Directory;
                                frm.WorkflowTargetObject = dir;

                                frm.ShowDialog(this);
                            }

                            break;
                        }
                    }
                }
                else if (_currentSite != null)
                {
                    foreach (CSWorkflowAssociation assoc in _currentSite.AllWorkflowAssociations)
                    {
                        if (assoc.Id.Equals(wfAssId))
                        {
                            using (frmAssociateWorkflow frm = new frmAssociateWorkflow())
                            {
                                frm.Farm = _currentFarm;
                                frm.WorkflowDefinition = assoc.WorkflowDefinition;
                                frm.WorkflowAssociation = assoc;
                                frm.WorkflowScope = ScopeEnum.Site;
                                frm.WorkflowTargetObject = _currentSite;

                                frm.ShowDialog(this);
                            }

                            break;
                        }
                    }
                }
                else
                {
                    foreach (CSWorkflowAssociation assoc in _currentFarm.AllWorkflowAssociations)
                    {
                        if (assoc.Id.Equals(wfAssId))
                        {
                            using (frmAssociateWorkflow frm = new frmAssociateWorkflow())
                            {
                                frm.Farm = _currentFarm;
                                frm.WorkflowDefinition = assoc.WorkflowDefinition;
                                frm.WorkflowAssociation = assoc;
                                frm.WorkflowScope = ScopeEnum.Farm;
                                frm.WorkflowTargetObject = _currentFarm;

                                frm.ShowDialog(this);
                            }

                            break;
                        }
                    }
                }

                lvExplorerView.Items.Clear();
                LoadWorkflowAssociations();
            }

            if (tagName.StartsWith(CSPath.CmsPathPrefix))
            {
                PATH_INFO pathInfo = CSPath.GetPathInfo(tagName);
                if (!pathInfo.IsValid)
                {
                    return;
                }

                switch (pathInfo.ResourceUriScope)
                {
                    case ScopeEnum.Farm:
                        using (frmFarmConfig frm = new frmFarmConfig())
                        {
                            frm.Farm = _currentFarm;
                            frm.ShowDialog(this);
                        }
                        break;

                    case ScopeEnum.Site:
                        using (frmCreateEditSite frm = new frmCreateEditSite())
                        {
                            frm.Farm = _currentFarm;
                            frm.CurrentSite = _currentFarm.AllSites.Find(pathInfo.SiteId);
                            frm.ShowDialog(this);
                        }

                        lvExplorerView.Items.Clear();
                        LoadFarm();

                        break;

                    case ScopeEnum.FileOrDirectory:
                        using (frmProperties frm = new frmProperties())
                        {
                            frm.SelectedSite = _currentSite;
                            frm.SelectedFilesystemEntry = _currentSite.GetFileSystemItem(tagName);

                            frm.ShowDialog(this);
                        }

                        lvExplorerView.Items.Clear();
                        LoadDirectory();

                        break;
                }
            }
        }

        private string GetTargetFolder()
        {
            string result = null;

            using (frmCSFolderBrowserDialog frm = new frmCSFolderBrowserDialog())
            {
                frm.BrowseSite = _currentSite;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    result = frm.SelectedPath;
                }
            }

            return result;
        }

        private string HumanSize(long bytes)
        {
            const int ONE_KILOBYTE = 1024;
            const int ONE_MEGABYTE = 1024 * 1024;
            const int ONE_GIGABYTE = 1024 * 1024 * 1024;
            string _human = "0 bytes";

            if (bytes < 1024)
            {
                _human = string.Format("{0} bytes", bytes);
            }
            else if ((bytes >= ONE_KILOBYTE) && (bytes < ONE_MEGABYTE))
            {
                _human = string.Format("{0:F2} KB", (bytes / ONE_KILOBYTE));
            }
            else if ((bytes >= ONE_MEGABYTE) && (bytes < ONE_GIGABYTE))
            {
                _human = string.Format("{0:F2} MB", (bytes / ONE_MEGABYTE));
            }
            else
            {
                _human = string.Format("{0:F2} GB", (bytes / ONE_GIGABYTE));
            }

            return _human;
        }

        #endregion

        #region Menu item events
        private void menuItemNewSite_Click(object sender, EventArgs e)
        {
            using (frmCreateEditSite frm = new frmCreateEditSite())
            {
                frm.Farm = _currentFarm;
                frm.CurrentSite = null;

                frm.ShowDialog(this);
            }

            RefreshCurrentView();
        }

        private void menuItemNewFolder_Click(object sender, EventArgs e)
        {
            ListViewItem newFolderItem = new ListViewItem("New Folder", 0)
            {
                Tag = CSPath.Combine(_currentFileSystemEntry.FullPath, "New Folder")
            };

            lvExplorerView.Items.Add(newFolderItem);

            _isCreatingNewDirectory = true;
            lvExplorerView.LabelEdit = true;
            newFolderItem.BeginEdit();          // run continues in lvExplorer_AfterLabelEdit()
        }

        private void menuItemNewFile_Click(object sender, EventArgs e)
        {
            using (frmUploadFile frm = new frmUploadFile())
            {
                frm.ContainingFolder = new CSFileSystemEntryDirectory(_currentFileSystemEntry);
                frm.ShowDialog(this);
            }

            RefreshCurrentView();
        }

        private void menuItemNewUser_Click(object sender, EventArgs e)
        {
            using (frmCreateEditUser frm = new frmCreateEditUser())
            {
                frm.Farm = _currentFarm;
                frm.User = null;
                frm.ShowDialog(this);
            }

            RefreshCurrentView();
        }

        private void menuitemNewUserGroup_Click(object sender, EventArgs e)
        {
            using (frmCreateEditUserGroup frm = new frmCreateEditUserGroup())
            {
                frm.Farm = _currentFarm;
                frm.UserGroup = null;
                frm.ShowDialog(this);
            }

            RefreshCurrentView();
        }

        private void menuItemNewWorkflowDefinition_Click(object sender, EventArgs e)
        {
            using (frmDefineWorkflow frm = new frmDefineWorkflow())
            {
                frm.WorkflowDefinition = null;
                frm.Farm = _currentFarm;
                frm.ShowDialog(this);
            }

            RefreshCurrentView();
        }

        private void menuItemOrganizeProperties_Click(object sender, EventArgs e)
        {
            string tagString = null;
            if ((lvExplorerView.SelectedItems.Count == 1) && (lvExplorerView.SelectedItems[0].Tag != null))
            {
                tagString = lvExplorerView.SelectedItems[0].Tag.ToString();
            }
            else if (lvExplorerView.SelectedItems.Count == 0)
            {
                if (_currentFileSystemEntry != null)
                {
                    tagString = _currentFileSystemEntry.FullPath;
                }
                else if (_currentSite != null)
                {
                    tagString = _currentSite.RootFolder.FullPath;
                }
                else
                {
                    tagString = CSPath.CmsPathPrefix;
                }
            }

            ShowItemPropertyWindow(tagString);
        }

        private void menuItemOrganizeCopyTo_Click(object sender, EventArgs e)
        {
            string targetFolder = GetTargetFolder();
            if (targetFolder == null)
            {
                return;
            }

            bool overwrite = false;
            if (UI.ShowMessage(this, "If the item already exists at target location, do you wish to overwrite it?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                overwrite = true;
            }

            string tag = ((lvExplorerView.SelectedItems[0].Tag != null) ? lvExplorerView.SelectedItems[0].Tag.ToString() : null);
            if (tag != null)
            {
                PATH_INFO pathInfo = CSPath.GetPathInfo(tag);
                if ((pathInfo.IsValid) && (pathInfo.ResourceUriScope == ScopeEnum.FileOrDirectory))
                {
                    CSFileSystemEntry entry = CSFileSystemEntry.GetItemInfo(_currentSite, CSPath.GetFullPath(_currentSite, pathInfo.ResourceURI));
                    entry.CopyTo(targetFolder, overwrite);
                    MessageBox.Show("File copied successfully.");
                }
            }
        }

        private void menuItemOrganizeMoveTo_Click(object sender, EventArgs e)
        {
            string targetFolder = GetTargetFolder();
            if (targetFolder == null)
            {
                return;
            }

            bool overwrite = false;
            if (UI.ShowMessage(this, "If the item already exists at target location, do you wish to overwrite it?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                overwrite = true;
            }

            string tag = ((lvExplorerView.SelectedItems[0].Tag != null) ? lvExplorerView.SelectedItems[0].Tag.ToString() : null);
            if (tag != null)
            {
                PATH_INFO pathInfo = CSPath.GetPathInfo(tag);
                if ((pathInfo.IsValid) && (pathInfo.ResourceUriScope == ScopeEnum.FileOrDirectory))
                {
                    CSFileSystemEntry entry = CSFileSystemEntry.GetItemInfo(_currentSite, CSPath.GetFullPath(_currentSite, pathInfo.ResourceURI));
                    entry.MoveTo(targetFolder, overwrite);
                    MessageBox.Show("File moved successfully.");
                }
            }

            RefreshCurrentView();
        }

        private void menuItemOrganizeRename_Click(object sender, EventArgs e)
        {
            _isCreatingNewDirectory = false;
            lvExplorerView.LabelEdit = true;
            lvExplorerView.SelectedItems[0].BeginEdit(); // run continues in lvExplorerView_AfterLabelEdit()
        }

        private void menuItemOrganizeDelete_Click(object sender, EventArgs e)
        {
            string tagName = lvExplorerView.SelectedItems[0].Tag.ToString();
            PATH_INFO pathInfo = CSPath.GetPathInfo(tagName);
            if (pathInfo.IsValid)
            {
                switch (pathInfo.ResourceUriScope)
                {
                    case ScopeEnum.Site:
                        _currentFarm.AllSites.Remove(pathInfo.SiteId);
                        break;

                    case ScopeEnum.FileOrDirectory:
                        CSFileSystemEntry entry = CSFileSystemEntry.GetItemInfo(_currentSite, CSPath.GetFullPath(_currentSite, pathInfo.ResourceURI));
                        entry.Delete();
                        break;
                }
            }
            else
            {
                if (tagName.StartsWith("user://"))
                {
                    Guid userId = Utility.SafeConvertToGuid(tagName.Replace("user://", ""));
                    CSUser user = CSUser.GetById(userId);
                    user.Delete();
                }

                if (tagName.StartsWith("wfdef://"))
                {
                    Guid wfDefId = Utility.SafeConvertToGuid(tagName.Replace("wfdef://", ""));
                    CSWorkflowDefinition def = CSWorkflowDefinition.Get(_currentFarm, wfDefId);

                    def.Delete();
                }

                if (tagName.StartsWith("wfassoc://"))
                {
                    Guid wfAssId = Utility.SafeConvertToGuid(tagName.Replace("wfassoc://", ""));

                    if ((_currentFileSystemEntry != null) && (_currentFileSystemEntry.IsFolder))
                    {
                        CSFileSystemEntryDirectory dir = new CSFileSystemEntryDirectory(_currentFileSystemEntry);
                        foreach (CSWorkflowAssociation assoc in dir.AllWorkflows)
                        {
                            if (assoc.Id.Equals(wfAssId))
                            {
                                assoc.Delete();
                                break;
                            }
                        }
                    }
                    else if (_currentSite != null)
                    {
                        foreach (CSWorkflowAssociation assoc in _currentSite.AllWorkflowAssociations)
                        {
                            if (assoc.Id.Equals(wfAssId))
                            {
                                assoc.Delete();
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (CSWorkflowAssociation assoc in _currentFarm.AllWorkflowAssociations)
                        {
                            if (assoc.Id.Equals(wfAssId))
                            {
                                assoc.Delete();
                                break;
                            }
                        }
                    }
                }
            }

            RefreshCurrentView();
        }

        private void menuItemOrganizeAssociateWorkflow_Click(object sender, EventArgs e)
        {
            string tag = (((lvExplorerView.SelectedItems.Count == 1) && (lvExplorerView.SelectedItems[0].Tag != null)) ? lvExplorerView.SelectedItems[0].Tag.ToString() : CSPath.CmsPathPrefix);
            if (tag != null)
            {
                CSSite selectedSite = null;
                CSFileSystemEntry selectedFilesystemEntry = null;

                PATH_INFO pathInfo = CSPath.GetPathInfo(tag);
                switch (pathInfo.ResourceUriScope)
                {
                    case ScopeEnum.Site:
                        selectedSite = _currentFarm.AllSites.Find(pathInfo.SiteId);
                        break;

                    case ScopeEnum.FileOrDirectory:
                        selectedSite = _currentFarm.AllSites.Find(pathInfo.SiteId);
                        selectedFilesystemEntry = CSFileSystemEntry.GetItemInfo(selectedSite, CSPath.GetFullPath(selectedSite, pathInfo.ResourceURI));
                        break;
                }

                using (frmAssociateWorkflow frm = new frmAssociateWorkflow())
                {
                    frm.WorkflowDefinition = null;
                    frm.WorkflowAssociation = null;
                    frm.Farm = _currentFarm;
                    frm.WorkflowScope = ScopeEnum.Invalid;

                    if ((selectedFilesystemEntry != null) && selectedFilesystemEntry.IsFolder)
                    {
                        frm.WorkflowTargetObject = new CSFileSystemEntryDirectory(selectedFilesystemEntry);
                        frm.WorkflowScope = ScopeEnum.Directory;
                    }
                    else
                    {
                        if (selectedSite != null)
                        {
                            frm.WorkflowTargetObject = selectedSite;
                            frm.WorkflowScope = ScopeEnum.Site;
                        }
                        else
                        {
                            frm.WorkflowTargetObject = null;
                            frm.WorkflowScope = ScopeEnum.Farm;
                        }
                    }

                    frm.ShowDialog(this);
                }
            }
        }

        private void menuItemViewWorkflowInstances_Click(object sender, EventArgs e)
        {
            frmWorkflowInstances frm = new frmWorkflowInstances();
            frm.Farm = _currentFarm;
            frm.Show();
        }

        private void menuItemViewRefresh_Click(object sender, EventArgs e)
        {
            RefreshCurrentView();
        }

        #region Trivial menu item events
        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void menuItemViewHelpAbout_Click(object sender, EventArgs e)
        {
            UI.ShowMessage
            (
                this,
                "Corkscrew Explorer" + Environment.NewLine + Environment.NewLine +
                "Version: " + Application.ProductVersion
            );
        }

        private void menuItemViewLargeIcons_Click(object sender, EventArgs e)
        {
            if (menuItemViewLargeIcons.Checked)
            {
                lvExplorerView.View = View.LargeIcon;
                UnsetViewOptions();
            }
        }

        private void menuItemViewSmallIcons_Click(object sender, EventArgs e)
        {
            if (menuItemViewSmallIcons.Checked)
            {
                lvExplorerView.View = View.SmallIcon;
                UnsetViewOptions();
            }
        }

        private void menuItemViewTiles_Click(object sender, EventArgs e)
        {
            if (menuItemViewTiles.Checked)
            {
                lvExplorerView.View = View.Tile;
                UnsetViewOptions();
            }
        }

        private void menuItemViewList_Click(object sender, EventArgs e)
        {
            if (menuItemViewList.Checked)
            {
                lvExplorerView.View = View.List;
                UnsetViewOptions();
            }
        }

        private void menuItemViewDetails_Click(object sender, EventArgs e)
        {
            if (menuItemViewDetails.Checked)
            {
                lvExplorerView.View = View.Details;
                UnsetViewOptions();
            }
        }

        private void menuItemViewSortAscending_Click(object sender, EventArgs e)
        {
            if (menuItemViewSortAscending.Checked)
            {
                lvExplorerView.Sorting = SortOrder.Ascending;
                menuItemViewSortDescending.Checked = false;
            }
        }

        private void menuItemViewSortDescending_Click(object sender, EventArgs e)
        {
            if (menuItemViewSortDescending.Checked)
            {
                lvExplorerView.Sorting = SortOrder.Descending;
                menuItemViewSortAscending.Checked = false;
            }
        }

        private void menuItemViewShowCheckboxes_Click(object sender, EventArgs e)
        {
            lvExplorerView.CheckBoxes = menuItemViewShowCheckboxes.Checked;
        }

        private void menuItemViewShowFilenameExtensions_Click(object sender, EventArgs e)
        {
            _showFilenameExtension = menuItemViewShowFilenameExtensions.Checked;

            // this option applies only in this one case
            if (_currentFileSystemEntry != null)
            {
                lvExplorerView.Items.Clear();
                LoadDirectory();
            }
        }

        private void menuItemViewShowHiddenItems_Click(object sender, EventArgs e)
        {
            _showHiddenItems = menuItemViewShowHiddenItems.Checked;

            // this option applies only in this one case
            if (_currentFileSystemEntry != null)
            {
                lvExplorerView.Items.Clear();
                LoadDirectory();
            }
        }

        private void menuItemOrganizeSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvExplorerView.Items)
            {
                if ((item.Tag != null) && (item.Text != ".."))
                {
                    if (lvExplorerView.CheckBoxes)
                    {
                        item.Checked = true;
                    }

                    item.Selected = true;
                }
            }
        }

        private void menuItemOrganizeSelectNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvExplorerView.Items)
            {
                if ((item.Tag != null) && (item.Text != ".."))
                {
                    if (lvExplorerView.CheckBoxes)
                    {
                        item.Checked = false;
                    }

                    item.Selected = false;
                }
            }
        }

        private void menuItemOrganizeSelectInvert_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvExplorerView.Items)
            {
                if ((item.Tag != null) && (item.Text != ".."))
                {
                    if (lvExplorerView.CheckBoxes)
                    {
                        item.Checked = (!item.Checked);
                    }

                    item.Selected = (!item.Selected);
                }
            }
        }
        #endregion

        #endregion

        #region Explorer Listview events

        private void lvExplorerView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            e.Item.Selected = e.Item.Checked;
        }

        private void lvExplorerView_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lvExplorerView.SelectedItems.Count != 1)    // view is set to single-select
            {
                return;
            }

            string tag = ((lvExplorerView.SelectedItems[0].Tag != null) ? lvExplorerView.SelectedItems[0].Tag.ToString() : null);
            CSPermission acl = CSPermission.TestAccess(_currentSite, _currentFileSystemEntry, _currentFarm.AuthenticatedUser);

            bool specialFolder = false;
            bool fsItem = false;


            switch (lvExplorerView.SelectedItems[0].Text)
            {
                case "Users":
                case "Workflow Definitions":
                case "Workflow Associations":
                    specialFolder = true;
                    break;
            }

            if (!specialFolder)
            {
                PATH_INFO pathInfo = CSPath.GetPathInfo(tag);
                if ((pathInfo.IsValid) && (pathInfo.ResourceUriScope == ScopeEnum.FileOrDirectory))
                {
                    fsItem = true;
                    CSFileSystemEntry item = _currentSite.GetFileSystemItem(tag);
                    acl = CSPermission.TestAccess(item.Site, item, item.AuthenticatedUser);
                }
            }

            menuItemOrganize.Enabled = true;

            if (!specialFolder)
            {
                if (fsItem)
                {
                    menuItemOrganizeCopyTo.Enabled = acl.CanRead;
                    menuItemOrganizeMoveTo.Enabled = acl.CanContribute;
                    menuItemOrganizeRename.Enabled = acl.CanContribute;
                }

                menuItemOrganizeDelete.Enabled = acl.CanContribute;
            }

            menuItemOrganizeAssociateWorkflow.Enabled = acl.CanFullControl;
            menuItemOrganizeProperties.Enabled = acl.CanRead;
        }

        private void lvExplorerView_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem item = lvExplorerView.SelectedItems[0];        // view is set to single-select
            lvExplorerView.Items.Clear();

            if (item.Tag == null)
            {
                // special folder
                switch (item.Text)
                {
                    case "Users":
                        LoadUsers();
                        break;

                    case "User Groups":
                        LoadUserGroups();
                        break;

                    case "Workflow Definitions":
                        LoadWorkflowDefinitions();
                        break;

                    case "Workflow Associations":
                        LoadWorkflowAssociations();
                        break;
                }
            }
            else if (!item.Tag.ToString().StartsWith(CSPath.CmsPathPrefix))
            {
                ShowItemPropertyWindow(item.Tag.ToString());
            }
            else
            {
                LoadListView(item.Tag.ToString());
            }
        }

        private void lvExplorerView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            lvExplorerView.LabelEdit = false;

            if (_isCreatingNewDirectory)
            {
                // create directory mode
                CSFileSystemEntryDirectory folder = new CSFileSystemEntryDirectory(_currentFileSystemEntry);
                folder.CreateDirectory(e.Label);
            }
            else
            {
                // rename
                CSFileSystemEntry entry = CSFileSystemEntry.GetItemInfo(_currentSite, lvExplorerView.SelectedItems[0].Tag.ToString());
                if (entry != null)
                {
                    string name = Path.GetFileNameWithoutExtension(e.Label), extension = Path.GetExtension(e.Label);
                    entry.Rename(name, extension);
                }
                entry = null;
            }

            RefreshCurrentView();
        }
        private void lvExplorerView_DragEnter(Object sender, DragEventArgs e)
        {
            if ((_currentFileSystemEntry != null) && (_currentFileSystemEntry.IsFolder))
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void lvExplorerView_DragDrop(Object sender, DragEventArgs e)
        {
            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;

            string[] droppedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            CSFileSystemEntryDirectory currentDirectory = new CSFileSystemEntryDirectory(_currentFileSystemEntry);

            using (frmProgressBar progress = new frmProgressBar())
            {
                progress.Show(this);

                int pbValue = 0;
                int percentile = droppedFilePaths.Length;
                int step = (100 / percentile);
                if (step < 1)
                {
                    step = 10;
                }

                foreach (string droppedFile in droppedFilePaths)
                {
                    pbValue += step;

                    progress.Status = "Copying " + droppedFile;
                    progress.Progress = pbValue;
                    currentDirectory.CreateFile(Path.GetFileNameWithoutExtension(droppedFile), Path.GetExtension(droppedFile), File.ReadAllBytes(droppedFile));
                }

                progress.Close();
            }

            this.UseWaitCursor = false;
            this.Cursor = Cursors.Default;

            RefreshCurrentView();
        }

        #endregion


        private void Quit()
        {
            if (_currentFileSystemEntry != null)
            {
                _currentFileSystemEntry.Dispose();
            }

            if (_currentSite != null)
            {
                _currentSite.Dispose();
            }

            if (_currentFarm != null)
            {
                _currentFarm.Dispose();
            }

            Application.Exit();
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void frmMainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
