using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Corkscrew.Tools.ProvisionWebsite
{
    public partial class CertificateSelectionScreen : Form
    {

        private X509Certificate2Collection certificates = null;

        #region Properties

        public X509Certificate2 SelectedCertificate
        {
            get;
            private set;
        }

        #endregion


        public CertificateSelectionScreen()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnViewCertificate_Click(object sender, EventArgs e)
        {
            if (lvCertificates.SelectedItems.Count != 1)
            {
                MessageBox.Show("Select a certificate in the list above, before clicking on the Select button.");
                return;
            }

            X509Certificate2Collection results = certificates.Find(X509FindType.FindBySubjectName, lvCertificates.SelectedItems[0].Text, false);
            if (results.Count == 1)
            {
                SelectedCertificate = results[0];
                X509Certificate2UI.DisplayCertificate(SelectedCertificate);
            }
            else
            {
                MessageBox.Show("Perhaps certificate was deleted after display in this window?");
                RefreshCertificates();
            }
        }

        private void btnSelectCertificate_Click(object sender, EventArgs e)
        {
            if (lvCertificates.SelectedItems.Count != 1)
            {
                MessageBox.Show("Select a certificate in the list above, before clicking on the Select button.");
                return;
            }

            X509Certificate2Collection results = certificates.Find(X509FindType.FindBySubjectName, lvCertificates.SelectedItems[0].Text, false);
            if (results.Count == 1)
            {
                SelectedCertificate = results[0];

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Perhaps certificate was deleted after display in this window?");
                RefreshCertificates();
            }
        }

        private void CertificateSelectionScreen_Shown(object sender, EventArgs e)
        {

        }

        public void RefreshCertificates()
        {
            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            certificateStore.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            certificates = certificateStore.Certificates;

            lvCertificates.Items.Clear();
            foreach(X509Certificate2 certificate in certificates)
            {
                if (certificate.Verify())
                {
                    ListViewItem item = new ListViewItem(certificate.GetNameInfo(X509NameType.SimpleName, false));
                    item.SubItems.Add(certificate.GetNameInfo(X509NameType.SimpleName, true));
                    item.SubItems.Add(certificate.GetExpirationDateString());

                    lvCertificates.Items.Add(item);
                }
            }

            if (lvCertificates.Items.Count == 0)
            {
                MessageBox.Show("No valid certificates found. Close this window, import the SSL certificate into the Personal folder of your LocalMachine certificate store and try again.");
            }
        }

        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessMouseMove(this, e);
        }
    }
}
