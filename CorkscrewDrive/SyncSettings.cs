using Corkscrew.SDK.objects;
using System;
using System.IO;
using System.Windows.Forms;

namespace Corkscrew.Drive
{
    public partial class SyncSettings : Form
    {

        private SyncConfiguration config = null;
        private CSFarm AuthorizedFarm = null;

        public SyncSettings()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (frmLogin frm = new frmLogin())
            {
                frm.ShowDialog(this);

                if ((frm.LoginResult) && (frm.Farm != null))
                {
                    config.CorkscrewUsername = frm.Farm.AuthenticatedUser.Username;
                    config.CorkscrewPasswordHash = frm.AccountHash;
                    config.Save();

                    AuthorizedFarm = frm.Farm;

                    // set connected
                    lblConnectionStatus.Text = "Logged in as " + config.CorkscrewUsername;
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                }
            }

            if ((!string.IsNullOrEmpty(config.CorkscrewUsername)) && (!string.IsNullOrEmpty(config.CorkscrewPasswordHash)))
            {
                cbTargetSite.Items.Clear();
                foreach (CSSite site in AuthorizedFarm.AllSites)
                {
                    cbTargetSite.Items.Add(site.Name);
                }

                if (cbTargetSite.Items.Count == 0)
                {
                    cbTargetSite.Enabled = false;
                    Program.ShowMessage("You have not been granted access to any Site in this farm! Contact your Farm Administrator for help.");
                }
                else
                {
                    cbTargetSite.Enabled = true;
                    cbTargetSite.Focus();
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (config.DeleteSyncedFoldersFromLocalOnAccountDisconnect)
            {
                try
                {
                    Directory.Delete(config.SourceDirectory, true);
                }
                catch { }
            }

            config.ClearSettings();

            cbTargetSite.SelectedIndex = -1;
            cbTargetSite.Enabled = false;
            tbCorkscrewTargetFolder.Text = "";
            tbCorkscrewTargetFolder.Enabled = false;

            tbSyncRootFolder.Text = "";
            tbExclusionPatterns.Text = "";

            chkOptionDownloadToLocalIfNotPresent.Checked = true;
            chkOptionDeleteLocalOnRemoteDelete.Checked = true;
            chkOptionDeleteRemoteOnLocalDelete.Checked = false;
            chkOptionDeleteSyncedFilesOnDisconnect.Checked = true;
            chkOptionRunOnlyOnIdle.Checked = false;
            nudIdleDurationValue.Enabled = false;
            cbIdleDurationUnit.Enabled = false;

            lblConnectionStatus.Text = "not connected";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (config.IsConnected)
            {
                config.Save();
            }
            
            this.Close();
        }

        private void SyncSettings_Shown(object sender, EventArgs e)
        {
            config = new SyncConfiguration();

            cbTargetSite.Enabled = false;
            tbCorkscrewTargetFolder.Enabled = false;

            if (!config.IsConnected)
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                lblConnectionStatus.Text = "not connected";

                btnConnect.Focus();

                cbTargetSite.SelectedIndex = -1;
                cbTargetSite.Enabled = false;
                tbCorkscrewTargetFolder.Text = "";
                tbCorkscrewTargetFolder.Enabled = false;

                tbSyncRootFolder.Text = "";
                tbExclusionPatterns.Text = "";

                chkOptionDownloadToLocalIfNotPresent.Checked = true;
                chkOptionDeleteLocalOnRemoteDelete.Checked = true;
                chkOptionDeleteRemoteOnLocalDelete.Checked = false;
                chkOptionDeleteSyncedFilesOnDisconnect.Checked = true;
                chkOptionRunOnlyOnIdle.Checked = false;
                nudIdleDurationValue.Enabled = false;
                cbIdleDurationUnit.Enabled = false;
            }
            else
            {
                AuthorizedFarm = CSFarm.Open(config.CorkscrewUsername, config.CorkscrewPasswordHash);

                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                lblConnectionStatus.Text = "connected as " + config.CorkscrewUsername;

                if (config.TargetSite != null)
                {
                    cbTargetSite.Items.Clear();
                    cbTargetSite.Items.Add(config.TargetSite.Name);
                    cbTargetSite.SelectedIndex = 0;
                    cbTargetSite.Enabled = false;

                    if (config.TargetDirectory != null)
                    {
                        tbCorkscrewTargetFolder.Text = config.TargetDirectory.FullPath;
                        tbCorkscrewTargetFolder.Enabled = false;
                    }
                }

                tbSyncRootFolder.Text = config.SourceDirectory;

                foreach (string s in config.Exclusions)
                {
                    tbExclusionPatterns.Text += s + Environment.NewLine;
                }

                chkOptionDownloadToLocalIfNotPresent.Checked = config.DownloadToLocalIfPresentRemotely;
                chkOptionDeleteRemoteOnLocalDelete.Checked = config.DeleteFromRemoteWhenDeletedLocally;
                chkOptionDeleteLocalOnRemoteDelete.Checked = config.DeleteFromLocalWhenDeletedRemotely;
                chkOptionDeleteSyncedFilesOnDisconnect.Checked = config.DeleteSyncedFoldersFromLocalOnAccountDisconnect;
                chkOptionRunOnlyOnIdle.Checked = config.SyncOnlyWhenIdle;

                nudIdleDurationValue.Enabled = config.SyncOnlyWhenIdle;
                cbIdleDurationUnit.Enabled = config.SyncOnlyWhenIdle;
                if (config.SyncOnlyWhenIdle)
                {
                    if ((config.IdleDuration.Hours > 0) && (config.IdleDuration.Minutes == 0) && (config.IdleDuration.Seconds == 0) && (config.IdleDuration.Milliseconds == 0))
                    {
                        nudIdleDurationValue.Value = config.IdleDuration.Hours;
                        cbIdleDurationUnit.SelectedIndex = cbIdleDurationUnit.Items.IndexOf("hours");
                    }
                    else if ((config.IdleDuration.Minutes > 0) && (config.IdleDuration.Seconds == 0) && (config.IdleDuration.Milliseconds == 0))
                    {
                        nudIdleDurationValue.Value = config.IdleDuration.Minutes;
                        cbIdleDurationUnit.SelectedIndex = cbIdleDurationUnit.Items.IndexOf("minutes");
                    }
                    else if ((config.IdleDuration.Seconds > 0) && (config.IdleDuration.Milliseconds == 0))
                    {
                        nudIdleDurationValue.Value = config.IdleDuration.Seconds;
                        cbIdleDurationUnit.SelectedIndex = cbIdleDurationUnit.Items.IndexOf("seconds");
                    }
                    else
                    {
                        nudIdleDurationValue.Value = (long)config.IdleDuration.TotalMilliseconds;
                        cbIdleDurationUnit.SelectedIndex = cbIdleDurationUnit.Items.IndexOf("milliseconds");
                    }
                }
            }
        }

        private void chkOptionRunOnlyOnIdle_CheckedChanged(object sender, EventArgs e)
        {
            nudIdleDurationValue.Enabled = chkOptionRunOnlyOnIdle.Checked;
            cbIdleDurationUnit.Enabled = chkOptionRunOnlyOnIdle.Checked;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            config.Exclusions.Clear();
            foreach(string pattern in tbExclusionPatterns.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                config.Exclusions.Add(pattern);
            }

            config.Save();
        }

        private void cbTargetSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string siteName = (string)cbTargetSite.SelectedItem;

            CSSite site = AuthorizedFarm.AllSites.Find(siteName);
            if (site != null)
            {
                config.TargetSite = site;
            }
            else
            {
                Program.ShowMessage("Fatal error! Could not find the site you just selected. Perhaps connectivity was lost?");
                return;
            }

            tbCorkscrewTargetFolder.Enabled = true;
            tbCorkscrewTargetFolder.Focus();
        }

        private void btnBrowseLocalFolder_Click(Object sender, EventArgs e)
        {
            if (fbdBrowseForFolder.ShowDialog(this) != DialogResult.Cancel)
            {
                config.SourceDirectory = fbdBrowseForFolder.SelectedPath;
                tbSyncRootFolder.Text = config.SourceDirectory;
            }
        }

        private void btnBrowseCorkscrewFolder_Click(object sender, EventArgs e)
        {
            using (frmCSFolderBrowserDialog frm = new frmCSFolderBrowserDialog())
            {
                frm.BrowseSite = config.TargetSite;

                if (frm.ShowDialog(this) != DialogResult.Cancel)
                {
                    tbCorkscrewTargetFolder.Text = frm.SelectedPath;
                    config.TargetDirectory = config.TargetSite.GetDirectory(frm.SelectedPath);
                }
            }
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            Program.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
