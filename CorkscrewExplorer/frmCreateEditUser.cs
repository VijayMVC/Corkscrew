using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmCreateEditUser : Form
    {

        /// <summary>
        /// The Farm
        /// </summary>
        public CSFarm Farm
        {
            get;
            set;
        }

        /// <summary>
        /// The user
        /// </summary>
        public CSUser User
        {
            get;
            set;
        }


        public frmCreateEditUser()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((tbPassword.Text != "") && (cbGenerateNewPassword.Checked))
            {
                MessageBox.Show("A password text has been entered and Generate New Password option is checked. Please select only one of the two options!");
                return;
            }


            if (User == null)
            {
                string password = tbPassword.Text;
                if (cbGenerateNewPassword.Checked)
                {
                    password = CSUser.GenerateNewPassword();
                    tbPassword.Text = password;
                }

                User = Farm.CreateUser(tbUsername.Text, tbDisplayName.Text, password, tbEmailAddress.Text);
            }
            else
            {
                User.DisplayName = tbDisplayName.Text;
                User.EmailAddress = tbEmailAddress.Text;
                User.Save();

                tbId.Text = User.Id.ToString("D");

                if (cbGenerateNewPassword.Checked)
                {
                    string password = CSUser.GenerateNewPassword();
                    tbPassword.Text = password;
                    User.ChangePassword(password);
                }
                else
                {
                    if ((tbPassword.Text != "") && (!User.IsCurrentPassword(tbPassword.Text)))
                    {
                        User.ChangePassword(tbPassword.Text);
                    }
                }
            }

            CSPermission acl = CSPermission.TestAccess(null, null, User);
            if ((acl.IsFarmAdministrator) && (! cbMakeFarmAdmin.Checked))
            {
                acl.Delete();
            }
            else if ((! acl.IsFarmAdministrator) && (cbMakeFarmAdmin.Checked))
            {
                acl.CanRead = true;
                acl.CanContribute = true;
                acl.CanFullControl = true;

                acl.Save();
            }

            MessageBox.Show("User was saved successfully.");
        }

        private void frmCreateEditUser_Shown(object sender, EventArgs e)
        {
            if (User != null)
            {
                tbId.Text = User.Id.ToString("d");
                tbUsername.Text = User.Username;
                tbDisplayName.Text = User.DisplayName;
                tbEmailAddress.Text = User.EmailAddress;

                tbUsername.Enabled = false;
                cbGenerateNewPassword.Checked = false;
                tbPassword.Text = "";

                CSPermission acl = CSPermission.TestAccess(null, null, User);
                cbMakeFarmAdmin.Checked = acl.IsFarmAdministrator;
            }
            else
            {
                cbGenerateNewPassword.Checked = true;
            }

            tbUsername.Focus();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
