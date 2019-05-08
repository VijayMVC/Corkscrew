using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmProperties : Form
    {

        #region Properties

        public CSSite SelectedSite { get; set; }

        public CSFileSystemEntry SelectedFilesystemEntry { get; set; }

        #endregion

        public frmProperties()
        {
            InitializeComponent();
        }

        private void frmProperties_Shown(object sender, EventArgs e)
        {
            ShowProperties();
        }

        private void ShowProperties()
        {
            if ((SelectedSite == null) && (SelectedFilesystemEntry == null))
            {
                // we gotta wait..
                return;
            }

            pbItemIcon.Image = imgLstFarmViewIconsSmall.Images[0];              // CMS icon
            lvItemHistory.Items.Clear();

            btnViewVersion.Enabled = false;
            btnRestoreVersion.Enabled = false;

            if (SelectedFilesystemEntry != null)
            {
                if (SelectedFilesystemEntry.IsFolder)
                {
                    pbItemIcon.Image = imgLstFarmViewIconsSmall.Images[2];      // folder
                    lblItemType.Text = "Directory";

                    CSFileSystemEntryDirectory directory = new CSFileSystemEntryDirectory(SelectedFilesystemEntry);

                    long folderSize = directory.FolderSizeBytes;
                    lblSize.Text = GetShortSize(folderSize) + " (" + folderSize + " bytes)";

                    lblContains.Text = directory.Directories.Count.ToString() + " directories, " + directory.Files.Count.ToString() + " files";
                }
                else
                {
                    int imgIndex = 5;   // generic
                    string fileExtensionAsPngName = SelectedFilesystemEntry.FilenameExtension.SafeString(removeAtStart: ".");

                    for (int index = 6; index < imgLstFarmViewIconsSmall.Images.Count; index++)
                    {
                        string imgKeyFilename = Path.GetFileNameWithoutExtension(imgLstFarmViewIconsSmall.Images.Keys[index]);
                        if (imgKeyFilename == fileExtensionAsPngName)
                        {
                            imgIndex = index;
                            break;
                        }
                    }

                    pbItemIcon.Image = imgLstFarmViewIconsSmall.Images[imgIndex];

                    CSMIMEType fileType = SelectedFilesystemEntry.Farm.AllContentTypes.Find(SelectedFilesystemEntry.FilenameExtension);
                    string mimeType = ((fileType != null) ? fileType.KnownMimeType : SelectedFilesystemEntry.FilenameExtension + " File");
                    lblItemType.Text = mimeType + " (" + SelectedFilesystemEntry.FilenameExtension + ")";

                    CSFileSystemEntryFile file = new CSFileSystemEntryFile(SelectedFilesystemEntry);
                    lblSize.Text = file.SizeHuman + " (" + file.Size + " bytes)";

                    btnDownloadFile.Enabled = true;
                }

                tbItemGuid.Text = SelectedFilesystemEntry.Id.ToString("d");
                tbItemName.Text = SelectedFilesystemEntry.FilenameWithExtension;
                lblItemLocation.Text = SelectedFilesystemEntry.ParentDirectoryPath.Replace(SelectedFilesystemEntry.RootDirectory.FullPath, "").SafeString(onEmpty: "/", expectStart: "/");
                lblContains.Text = "(data)";

                lblCreated.Text = SelectedFilesystemEntry.Created.ToString("dddd, MMMM dd, yyyy, hh:mm:ss tt");
                lblModified.Text = SelectedFilesystemEntry.Modified.ToString("dddd, MMMM dd, yyyy, hh:mm:ss tt");
                lblAccessed.Text = SelectedFilesystemEntry.LastAccessed.ToString("dddd, MMMM dd, yyyy, hh:mm:ss tt");

                chkArchive.Checked = SelectedFilesystemEntry.IsArchive;
                chkHidden.Checked = SelectedFilesystemEntry.IsHidden;
                chkSystem.Checked = SelectedFilesystemEntry.IsSystem;
                chkReadonly.Checked = SelectedFilesystemEntry.IsReadonly;

                // site change history
                foreach(CSSiteHistoryRecord record in SelectedSite.History)
                {
                    if (!string.IsNullOrEmpty(record.Name))
                    {
                        lvItemHistory.Items.Add(
                            new ListViewItem(
                                new string[] 
                                {
                                    record.Name,
                                    record.ChangeTimestamp.ToString("MMM dd, yyyy hh:mm:ss tt"),
                                    record.Id.ToString()
                                }
                            )
                        );
                    }
                }
            }
            else
            {
                tbItemGuid.Text = SelectedSite.Id.ToString("d");
                tbItemName.Text = SelectedSite.Name;

                lblItemLocation.Text = "Farm";
                lblContains.Text = SelectedSite.RootFolder.Directories.Count.ToString() + " directories, " + SelectedSite.RootFolder.Files.Count.ToString() + " files";

                lblCreated.Text = SelectedSite.Created.ToString("dddd, MMMM dd, yyyy, hh:mm:ss tt");
                lblModified.Text = SelectedSite.Modified.ToString("dddd, MMMM dd, yyyy, hh:mm:ss tt");
                lblAccessed.Text = "";

                // attributes dont apply to sites
                chkArchive.Visible = false;
                chkHidden.Visible = false;
                chkSystem.Visible = false;
                chkReadonly.Visible = false;

                // item  change history
                foreach (CSFileSystemHistoryRecord record in SelectedFilesystemEntry.History)
                {
                    if (!string.IsNullOrEmpty(record.FilenameWithExtension))
                    {
                        lvItemHistory.Items.Add(
                            new ListViewItem(
                                new string[]
                                {
                                    record.FilenameWithExtension,
                                    record.ChangeTimestamp.ToString("MMM dd, yyyy hh:mm:ss tt"),
                                    record.Id.ToString()
                                }
                            )
                            {
                                // In the lvItemHistory_SelectedIndexChanged event, this is used to determine whether to show the buttons
                                Tag = (((! SelectedFilesystemEntry.IsFolder) && (record.PreviousDataSize > 0)) ? Guid.Empty : record.Id)
                            }
                        );
                    }
                }
            }

            lbUsersGroupsNames.Items.Clear();
            CSPermissionCollection acls = new CSPermissionCollection(SelectedSite, SelectedFilesystemEntry, null);

            // Always add...
            lbUsersGroupsNames.Items.Add(CSUser.CreateSystemUser().Username);

            if (SelectedSite.IsConfigSite)
            {
                // only if config site, add anon
                lbUsersGroupsNames.Items.Add(CSUser.CreateAnonymousUser().Username);
            }

            foreach(CSPermission acl in acls)
            {
                if ((acl.SecurityPrincipal != null) && (! lbUsersGroupsNames.Items.Contains(acl.SecurityPrincipal.Username)))
                {
                    lbUsersGroupsNames.Items.Add(acl.SecurityPrincipal.Username);
                }
            }

        }

        private void lbUsersGroupsNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbPermissionsForPrincipal.SetItemChecked(0, false);
            clbPermissionsForPrincipal.SetItemChecked(1, false);
            clbPermissionsForPrincipal.SetItemChecked(2, false);

            if (lbUsersGroupsNames.SelectedItem != null)
            {
                string userName = lbUsersGroupsNames.SelectedItem.ToString();
                CSPermission acl = CSPermission.TestAccess(SelectedSite, SelectedFilesystemEntry, CSUser.GetByUsername(userName));

                clbPermissionsForPrincipal.SetItemChecked(0, acl.CanFullControl);
                clbPermissionsForPrincipal.SetItemChecked(1, acl.CanContribute);
                clbPermissionsForPrincipal.SetItemChecked(2, acl.CanRead);
            }
        }

        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            bool openFileAfterDownload = false;
            if (UI.ShowMessage(this, "Do you wish to open the file after it is downloaded?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                openFileAfterDownload = true;
            }

            sfdSaveFile.FileName = SelectedFilesystemEntry.FilenameWithExtension;
            if (sfdSaveFile.ShowDialog(this) == DialogResult.OK)
            {
                using (CSFileSystemEntryFile file = new CSFileSystemEntryFile(SelectedFilesystemEntry))
                {
                    if (file.Open(FileAccess.Read))
                    {
                        byte[] buffer = new byte[file.Size];
                        int bytes = file.Read(buffer, 0, file.Size);
                        if (bytes > 0)
                        {
                            Stream stream = sfdSaveFile.OpenFile();
                            stream.Write(buffer, 0, bytes);
                            stream.Close();

                            if (openFileAfterDownload)
                            {
                                System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll,ShellExec_RunDLL \"" + sfdSaveFile.FileName + "\"");
                            }
                            else
                            {
                                UI.ShowMessage(this, "File was saved to: " + sfdSaveFile.FileName);
                            }
                        }

                        file.Close();
                    }
                }
            }
        }

        private string GetShortSize(long size)
        {
            string _human = "0 bytes";
            const int ONE_KILOBYTE = 1024;
            const int ONE_MEGABYTE = 1024 * 1024;
            const int ONE_GIGABYTE = 1024 * 1024 * 1024;

            if (size < 1024)
            {
                _human = string.Format("{0} bytes", size);
            }
            else if ((size >= ONE_KILOBYTE) && (size < ONE_MEGABYTE))
            {
                _human = string.Format("{0:F2} KB", (size / ONE_KILOBYTE));
            }
            else if ((size >= ONE_MEGABYTE) && (size < ONE_GIGABYTE))
            {
                _human = string.Format("{0:F2} MB", (size / ONE_MEGABYTE));
            }
            else
            {
                _human = string.Format("{0:F2} GB", (size / ONE_GIGABYTE));
            }

            return _human;
        }

        private long Higher4KB(long size)
        {
            long kb = size / 4096;
            long bytes = 0, higherSize = size;

            if ((4096 * kb) < size)
            {
                bytes = (size - (4096 * kb));
                higherSize = (4096 * (kb + 1)) + bytes;
            }

            return higherSize;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveChanges();
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }


        private void SaveChanges()
        {
            if (SelectedFilesystemEntry != null)
            {
                // do for FS
                if (! tbItemName.Text.Equals(SelectedFilesystemEntry.FilenameWithExtension))
                {
                    SelectedFilesystemEntry.Filename = Path.GetFileName(tbItemName.Text);
                    SelectedFilesystemEntry.FilenameExtension = Path.GetExtension(tbItemName.Text);
                }

                SelectedFilesystemEntry.IsReadonly = chkReadonly.Checked;
                SelectedFilesystemEntry.IsHidden = chkHidden.Checked;
                SelectedFilesystemEntry.Save();
            }
            else if (SelectedSite != null)
            {
                if (! tbItemName.Text.Equals(SelectedSite.Name))
                {
                    SelectedSite.Name = tbItemName.Text;
                    SelectedSite.Save();
                }
            }
        }

        private void btnPermissionsEdit_Click(object sender, EventArgs e)
        {
            using (frmItemPermissions frm = new frmItemPermissions())
            {
                frm.SelectedSite = this.SelectedSite;
                frm.SelectedFilesystemEntry = this.SelectedFilesystemEntry;
                frm.ShowDialog(this);
            }

            // redo the permissions listbox
            lbUsersGroupsNames.Items.Clear();
            CSPermissionCollection acls = new CSPermissionCollection(SelectedSite, SelectedFilesystemEntry, null);

            // Always add...
            lbUsersGroupsNames.Items.Add(CSUser.CreateSystemUser().Username);

            if (SelectedSite.IsConfigSite)
            {
                // only if config site, add anon
                lbUsersGroupsNames.Items.Add(CSUser.CreateAnonymousUser().Username);
            }

            foreach (CSPermission acl in acls)
            {
                if ((acl.SecurityPrincipal != null) && (!lbUsersGroupsNames.Items.Contains(acl.SecurityPrincipal.Username)))
                {
                    lbUsersGroupsNames.Items.Add(acl.SecurityPrincipal.Username);
                }
            }
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void lvItemHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Disabled for v1.0

            /*
            if ((lvItemHistory.SelectedIndices.Count == 1) && (lvItemHistory.SelectedIndices[0] != -1))
            {
                if ((SelectedFilesystemEntry != null) && (!SelectedFilesystemEntry.IsFolder))
                {
                    Guid fileHistoryId = Utility.SafeConvertToGuid(lvItemHistory.SelectedItems[0].SubItems[2]);
                    if ((fileHistoryId != Guid.Empty) && (Utility.SafeConvertToGuid(lvItemHistory.SelectedItems[0].Tag) != Guid.Empty))
                    {
                        btnViewVersion.Enabled = true;
                        btnRestoreVersion.Enabled = true;
                    }
                }
                else if ((SelectedFilesystemEntry == null) && (SelectedSite != null))
                {
                    long siteHistoryId = Utility.SafeConvertToLong(lvItemHistory.SelectedItems[0].SubItems[2]);
                    if (siteHistoryId != default(long))
                    {
                        btnRestoreVersion.Enabled = true;
                    }
                }
            }
            */
        }

        private void btnViewVersion_Click(object sender, EventArgs e)
        {
            if ((lvItemHistory.SelectedIndices.Count == 1) && (lvItemHistory.SelectedIndices[0] != -1))
            {
                if ((SelectedFilesystemEntry != null) && (!SelectedFilesystemEntry.IsFolder))
                {
                    Guid fileHistoryId = Utility.SafeConvertToGuid(lvItemHistory.SelectedItems[0].SubItems[2]);
                    if ((fileHistoryId != Guid.Empty) && (Utility.SafeConvertToGuid(lvItemHistory.SelectedItems[0].Tag) != Guid.Empty))
                    {
                        foreach(CSFileSystemHistoryRecord history in SelectedFilesystemEntry.History)
                        {
                            if (history.Id == fileHistoryId)
                            {
                                string localFileName = Path.Combine(Path.GetTempPath(), history.FilenameWithExtension);
                                Stream stream = File.OpenWrite(localFileName);
                                stream.Write(history.PreviousData, 0, history.PreviousDataSize);
                                stream.Close();

                                // cheat so that we know if the file was edited by the user
                                File.SetCreationTime(localFileName, history.Created);
                                File.SetLastWriteTime(localFileName, history.Modified);

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
                                        File.SetCreationTime(sfdSaveFile.FileName, history.Created);
                                        File.SetLastWriteTime(sfdSaveFile.FileName, history.Modified);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnRestoreVersion_Click(object sender, EventArgs e)
        {
            if ((lvItemHistory.SelectedIndices.Count == 1) && (lvItemHistory.SelectedIndices[0] != -1))
            {
                if (SelectedFilesystemEntry != null)
                {
                    Guid fileHistoryId = Utility.SafeConvertToGuid(lvItemHistory.SelectedItems[0].SubItems[2]);
                    if (fileHistoryId != Guid.Empty)
                    {
                        btnViewVersion.Enabled = true;
                        btnRestoreVersion.Enabled = true;
                    }
                }
                else if (SelectedSite != null)
                {
                    long siteHistoryId = Utility.SafeConvertToLong(lvItemHistory.SelectedItems[0].SubItems[2]);
                    if (siteHistoryId != default(long))
                    {
                        btnRestoreVersion.Enabled = true;
                    }
                }
            }
        }
    }
}
