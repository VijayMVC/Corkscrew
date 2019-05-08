using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Windows.Forms;

namespace Corkscrew.Drive
{
    public partial class frmLogin : Form
    {

        #region Properties

        /// <summary>
        /// The farm we managed to login to
        /// </summary>
        public CSFarm Farm
        {
            get;
            private set;
        }

        /// <summary>
        /// If true, indicates a login was performed.
        /// </summary>
        public bool LoginResult
        {
            get;
            private set;
        }

        public string AccountHash
        {
            get;
            private set;
        }

        #endregion


        public frmLogin()
        {
            InitializeComponent();

            LoginResult = false;
            Farm = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                AccountHash = Utility.GetSha256Hash(tbPassword.Text);
                Farm = CSFarm.Open(tbUsername.Text, AccountHash);
                LoginResult = (Farm != null);
            }
            catch
            {
            }

            if (!LoginResult)
            {
                if (MessageBox.Show("Login failed. Would you like to try again?", "Corkscrew Drive - Login", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    return;
                }

                DialogResult = DialogResult.Cancel;
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            Program.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
