using Corkscrew.SDK.ActiveDirectory;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmImportUsersFromAD : Form
    {

        public CSFarm Farm
        {
            get;
            set;
        }

        private PrincipalContext _principalContext = null;

        public frmImportUsersFromAD()
        {
            InitializeComponent();
        }

        private void frmImportUsersFromAD_Shown(object sender, EventArgs e)
        {
            _principalContext = null;
            btnImportUsers.Enabled = false;

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity.IsAuthenticated && (!identity.IsGuest) && (identity.Name.Contains("\\") || identity.Name.Contains("@")))
            {
                // logged onto domain
                string[] domainNameArray = identity.Name.Split(new char[] { '\\', '@' }, StringSplitOptions.RemoveEmptyEntries);
                string domain = domainNameArray[0], username = domainNameArray[1];
                if (identity.Name.Contains("@"))
                {
                    // username@domain
                    domain = domainNameArray[1];
                    username = domainNameArray[0];
                }

                tbUsername.Text = username;
                tbDomainName.Text = domain;

                Application.DoEvents(); // wait for stuff to stabilize

                // Set principal context. If user is already logged into Domain
                try
                {
                    _principalContext = new PrincipalContext(ContextType.Domain);
                    btnLoginToAD_Click(btnLoginToAD, null);
                }
                catch
                {
                    _principalContext = null;
                }
            }
        }

        private void btnLoginToAD_Click(object sender, EventArgs e)
        {
            btnImportUsers.Enabled = false;
            lvUsersToImport.Items.Clear();

            if (_principalContext == null)
            {
                try
                {
                    _principalContext = new PrincipalContext(ContextType.Domain, tbDomainName.Text, tbUsername.Text, tbPassword.Text);
                }
                catch (System.DirectoryServices.AccountManagement.PrincipalServerDownException)
                {
                    _principalContext = null;
                }
            }

            if (_principalContext == null)
            {
                btnImportUsers.Enabled = false;
                UI.ShowMessage(this, "Could not connect to Active Directory with given credentials. Please check the username, password, domain name and connectivity.", MessageBoxButtons.OK, "Error connecting to Active Directory!");
                return;
            }

            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;

            CSUserCollection allFarmUsers = Farm.AllUsers;

            using (frmProgressBar progress = new frmProgressBar())
            {
                progress.Status = "Loading users...";
                progress.Progress = 0;
                progress.Show(this);
                Application.DoEvents();

                Principal queryFilter = new UserPrincipal(_principalContext);
                //DateTime startTime = DateTime.Now;      //DEBUG:

                using (PrincipalSearcher searcher = new PrincipalSearcher(queryFilter))
                {
                    PrincipalSearchResult<Principal> results = searcher.FindAll();

                    foreach (Principal user in results)
                    {
                        UserPrincipal userPrincipal = (UserPrincipal)user;
                        bool import = ((cbDoNotImportDisabledUsers.Checked && (bool)userPrincipal.Enabled) || (!cbDoNotImportDisabledUsers.Checked));
                        import = import && ((cbDoNotImportUsersWithoutEmailAddress.Checked && (!string.IsNullOrEmpty(userPrincipal.EmailAddress))) || (!cbDoNotImportUsersWithoutEmailAddress.Checked));

                        progress.Status = "Processing user [" + userPrincipal.DisplayName + "]...";
                        progress.Progress += 10;

                        if (import)
                        {
                            if (allFarmUsers.Find(userPrincipal.UserPrincipalName) != null)
                            {
                                continue;
                            }

                            ListViewItem lvi = new ListViewItem
                            (
                                new string[]
                                {
                                    userPrincipal.UserPrincipalName,
                                    userPrincipal.DisplayName,
                                    userPrincipal.Surname,
                                    userPrincipal.EmailAddress,
                                    ((bool)userPrincipal.Enabled ? "Yes" : "No")
                                }
                            );
                            lvi.Tag = userPrincipal;

                            lvUsersToImport.Items.Add(lvi);
                        }
                    }
                }

                //UI.ShowMessage(this, "Time elapsed: " + DateTime.Now.Subtract(startTime).TotalMilliseconds);

                progress.Close();
            }

            this.Cursor = Cursors.Default;
            this.UseWaitCursor = false;

            btnImportUsers.Enabled = true;
        }

        private void btnImportUsers_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;      //DEBUG:

            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;

            using (frmProgressBar progress = new frmProgressBar())
            {
                progress.Status = "Loading users...";
                progress.Progress = 0;
                progress.Show(this);
                Application.DoEvents();

                string ldapPrefix = "LDAP://" + tbDomainName.Text + "/CN=";
                string dcString = tbDomainName.Text.Replace(".", ", DC=");
                CSUserCollection allFarmUsers = Farm.AllUsers;

                foreach (ListViewItem lvi in lvUsersToImport.CheckedItems)
                {
                    progress.Status = "Processing user [" + lvi.Text + "]...";
                    progress.Progress += 10;

                    string userName = lvi.Text;

                    if (allFarmUsers.Find(userName) != null)
                    {
                        lvi.ForeColor = Color.Red;
                        continue;
                    }

                    UserPrincipal user = (UserPrincipal)lvi.Tag;
                    if (user != null)
                    {
                        string emailAddress = user.EmailAddress;
                        if (string.IsNullOrEmpty(emailAddress))
                        {
                            emailAddress = user.UserPrincipalName;
                        }

                        CSActiveDirectoryUser.CreateUser(username: userName, displayName: user.DisplayName, emailAddress: user.EmailAddress);
                    }
                }

                progress.Close();
            }

            this.Cursor = Cursors.Default;
            this.UseWaitCursor = false;

            UI.ShowMessage(this, "Time elapsed: " + DateTime.Now.Subtract(startTime).TotalMilliseconds);
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool state = cbSelectAll.Checked;

            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;

            foreach (ListViewItem lvi in lvUsersToImport.Items)
            {
                lvi.Checked = state;
            }

            this.Cursor = Cursors.Default;
            this.UseWaitCursor = false;
        }

        private void frmImportUsersFromAD_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
