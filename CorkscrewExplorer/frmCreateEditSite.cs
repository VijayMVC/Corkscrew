using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmCreateEditSite : Form
    {

        #region Properties

        /// <summary>
        /// The farm
        /// </summary>
        public CSFarm Farm
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Site
        /// </summary>
        public CSSite CurrentSite
        {
            get;
            set;
        } = null;

        #endregion

        #region Form events
        public frmCreateEditSite()
        {
            InitializeComponent();
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // validate
            if (string.IsNullOrEmpty(tbSiteName.Text))
            {
                MessageBox.Show("Site name cannot be empty.");
            }

            if (CurrentSite == null)
            {
                CurrentSite = Farm.CreateSite(tbSiteName.Text, tbSiteDescription.Text, CSFarm.FARM_DATABASENAME, Utility.SafeConvertToLong(nudSiteQuotaValue.Value));

                if (CurrentSite == null)
                {
                    UI.ShowMessage(this, "Site was not created!");
                    return;
                }

                tbId.Text = CurrentSite.Id.ToString("D");
                lblCreated.Text = CurrentSite.Created.ToString("MMM dd, yyyy HH:mm:ss");
                lblModified.Text = CurrentSite.Modified.ToString("MMM dd, yyyy HH:mm:ss");
            }
            else
            {
                CurrentSite.Name = tbSiteName.Text;
                CurrentSite.Description = tbSiteDescription.Text;
                CurrentSite.QuotaBytes = Utility.SafeConvertToLong(nudSiteQuotaValue.Value);

                CurrentSite.Save();

                tbDbName.Text = CurrentSite.ContentDatabaseName;
                lblCreated.Text = CurrentSite.Created.ToString("MMM dd, yyyy HH:mm:ss");
                lblModified.Text = CurrentSite.Modified.ToString("MMM dd, yyyy HH:mm:ss");
            }

            MessageBox.Show("Site saved successfully.");
        }

        private void frmCreateEditSite_Shown(object sender, EventArgs e)
        {
            if (CurrentSite != null)
            {
                tbId.Text = CurrentSite.Id.ToString("D");
                tbSiteName.Text = CurrentSite.Name;
                tbSiteDescription.Text = CurrentSite.Description;
                tbDbName.Text = CurrentSite.ContentDatabaseName;
                tbDnsNames.Text = "";
                foreach(string dns in CurrentSite.DNSNames)
                {
                    tbDnsNames.Text = tbDnsNames.Text 
                                        + Environment.NewLine + dns;
                }

                nudSiteQuotaValue.Value = CurrentSite.QuotaBytes;

                if (CurrentSite.QuotaBytes == 0)
                {
                    lblQuotaUsed.Text = "Quota disabled.";
                }
                else
                {
                    lblQuotaUsed.Text = string.Format("{0} bytes", CurrentSite.QuotaBytes);
                }
                
                lblCreated.Text = CurrentSite.Created.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = CurrentSite.CreatedBy.Username;
                lblModified.Text = CurrentSite.Modified.ToString("MMM dd, yyyy HH:mm:ss");
                lblModifiedBy.Text = CurrentSite.ModifiedBy.Username;

                tbSiteName.Focus();
            }
            else
            {
                Guid g = Guid.NewGuid();
                DateTime now = DateTime.Now;

                tbId.Text = g.ToString("D");
                tbSiteName.Text = "";
                tbSiteDescription.Text = "";
                tbDbName.Text = CSFarm.FARM_DATABASENAME;
                tbDnsNames.Text = "";

                nudSiteQuotaValue.Value = 102400000;  //100 MB

                lblQuotaUsed.Text = "0 MB";
                lblCreated.Text = now.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = Farm.AuthenticatedUser.Username;
                lblModified.Text = now.ToString("MMM dd, yyyy HH:mm:ss");
                lblModifiedBy.Text = Farm.AuthenticatedUser.Username;

                tbSiteName.Focus();
            }

        }

        private string GetSizeHuman(long ContentSize)
        {
            int ONE_KILOBYTE = 1024;
            int ONE_MEGABYTE = 1024 * 1024;
            int ONE_GIGABYTE = 1024 * 1024 * 1024;

            string _human = "0 bytes";

            if (ContentSize < 1024)
            {
                _human = string.Format("{0} bytes", ContentSize);
            }
            else if ((ContentSize >= ONE_KILOBYTE) && (ContentSize < ONE_MEGABYTE))
            {
                _human = string.Format("{0:F2} KB", (ContentSize / ONE_KILOBYTE));
            }
            else if ((ContentSize >= ONE_MEGABYTE) && (ContentSize < ONE_GIGABYTE))
            {
                _human = string.Format("{0:F2} MB", (ContentSize / ONE_MEGABYTE));
            }
            else
            {
                _human = string.Format("{0:F2} GB", (ContentSize / ONE_GIGABYTE));
            }

            return _human;
        }

        private void cbDoNotDeploySiteDB_CheckedChanged(object sender, EventArgs e)
        {
            tbDbName.Enabled = !cbDoNotDeploySiteDB.Checked;
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
