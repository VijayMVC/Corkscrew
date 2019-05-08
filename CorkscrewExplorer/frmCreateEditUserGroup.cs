using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmCreateEditUserGroup : Form
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
        public CSUserGroup UserGroup
        {
            get;
            set;
        }


        public frmCreateEditUserGroup()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (UserGroup == null)
            {
                UserGroup = Farm.AllUserGroups.Add(tbUsername.Text, tbDisplayName.Text, tbEmailAddress.Text);
            }
            else
            {
                UserGroup.DisplayName = tbDisplayName.Text;
                UserGroup.EmailAddress = tbEmailAddress.Text;
                UserGroup.Save();

                tbId.Text = UserGroup.Id.ToString("D");
            }

            MessageBox.Show("User group was saved successfully.");
        }

        private void frmCreateEditUser_Shown(object sender, EventArgs e)
        {
            if (UserGroup != null)
            {
                tbId.Text = UserGroup.Id.ToString("d");
                tbUsername.Text = UserGroup.Username;
                tbDisplayName.Text = UserGroup.DisplayName;
                tbEmailAddress.Text = UserGroup.EmailAddress;

                tbUsername.Enabled = false;
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
