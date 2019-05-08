using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmItemPermissions : Form
    {
        #region Properties

        public CSSite SelectedSite { get; set; }

        public CSFileSystemEntry SelectedFilesystemEntry { get; set; }

        #endregion

        public frmItemPermissions()
        {
            InitializeComponent();
        }

        private void frmItemPermissions_Shown(object sender, EventArgs e)
        {

            CSUser authorizedUser = null;

            if (SelectedFilesystemEntry != null)
            {
                tbId.Text = SelectedFilesystemEntry.Id.ToString("d");
                tbFullPath.Text = SelectedFilesystemEntry.FullPath;
                authorizedUser = SelectedFilesystemEntry.AuthenticatedUser;
            }
            else if (SelectedSite != null)
            {
                tbId.Text = SelectedSite.Id.ToString("d");
                tbFullPath.Text = SelectedSite.RootFolder.FullPath;
                authorizedUser = SelectedSite.AuthenticatedUser;
            }

            CSFarm farm = CSFarm.Open(authorizedUser);
            cbSecurityPrincipals.Items.Clear();
            cbSecurityPrincipals.DisplayMember = "Username";
            cbSecurityPrincipals.ValueMember = "Id";

            foreach(CSUserGroup group in farm.AllUserGroups)
            {
                cbSecurityPrincipals.Items.Add((CSSecurityPrincipal)group);
            }

            foreach (CSUser user in farm.AllUsers)
            {
                int index = cbSecurityPrincipals.Items.Add((CSSecurityPrincipal)user);
                if (user.Id.Equals(authorizedUser.Id))
                {
                    cbSecurityPrincipals.SelectedIndex = index;
                }
            }
        }

        private void cbSecurityPrincipals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSecurityPrincipals.SelectedIndex >= 0)
            {
                CSSecurityPrincipal principal = (CSSecurityPrincipal)cbSecurityPrincipals.SelectedItem;
                string userName = principal.Username;

                CSPermission acl = CSPermission.TestAccess(SelectedSite, SelectedFilesystemEntry, principal);
                if (acl.IsHierarchicalAccess)
                {
                    rbAclInheritedRead.Checked = acl.CanRead;
                    rbAclInheritedContribute.Checked = acl.CanContribute;
                    rbAclInheritedFullControl.Checked = acl.CanFullControl;
                }
                else
                {
                    CSPermission hAcl = CSPermission.TestAccess(SelectedSite, null, principal);
                    if (acl.HasAny)
                    {
                        rbAclInheritedRead.Checked = hAcl.CanRead;
                        rbAclInheritedContribute.Checked = hAcl.CanContribute;
                        rbAclInheritedFullControl.Checked = hAcl.CanFullControl;
                    }
                }

                rbAclCurrentRead.Checked = acl.CanRead;
                rbAclCurrentContribute.Checked = acl.CanContribute;
                rbAclCurrentFullControl.Checked = acl.CanFullControl;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbSecurityPrincipals.SelectedItem == null)
            {
                MessageBox.Show("Please select a user or group from the Security Principal dropdown.");
            }

            CSSecurityPrincipal principal = (CSSecurityPrincipal)cbSecurityPrincipals.SelectedItem;
            CSPermission acl = CSPermission.TestAccess(SelectedSite, SelectedFilesystemEntry, principal);
            acl.CanRead = rbAclNewRead.Checked;
            acl.CanContribute = rbAclNewContribute.Checked;
            acl.CanFullControl = rbAclNewFullControl.Checked;
            acl.IsHierarchicalAccess = false;
            acl.Save();

            this.Close();
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
