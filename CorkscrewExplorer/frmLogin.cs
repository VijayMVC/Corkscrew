using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
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

            string selectedUsername = tbUsername.Text;
            if (selectedUsername.Equals("Corkscrew System User", StringComparison.InvariantCultureIgnoreCase))
            {
                Farm = CSFarm.Open(CSUser.CreateSystemUser());
                LoginResult = true;
            }
            else if (selectedUsername.Equals("Anonymous User", StringComparison.InvariantCultureIgnoreCase))
            {
                Farm = CSFarm.Open(CSUser.CreateAnonymousUser());
                LoginResult = true;
            }
            else
            {
                try
                {
                    Farm = CSFarm.Open(selectedUsername, Utility.GetSha256Hash(tbPassword.Text));
                    LoginResult = (Farm != null);
                }
                catch
                {
                    LoginResult = false;
                }
            }

            if (!LoginResult)
            {
                if (UI.ShowMessage(this, "Login failed. Would you like to try again?", MessageBoxButtons.YesNo, "Login failed.") == DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    return;
                }

                DialogResult = DialogResult.Cancel;
                return;
            }

            DialogResult = DialogResult.OK;
        }
        

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
