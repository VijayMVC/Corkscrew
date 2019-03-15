using CMS.Setup.Installers;
using Corkscrew.SDK.tools;
using System;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class DatabaseConnectionScreen : ScreenTemplate
    {

        private InstallWizardForm _parentForm = null;
        private DatabaseSettings _databaseSettings = null;

        public DatabaseConnectionScreen()
        {
            InitializeComponent();
        }

        private void cbAuthenticationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string authTypeName = (string)cbAuthenticationType.Items[cbAuthenticationType.SelectedIndex];
            switch (authTypeName)
            {
                case "Windows Authentication":
                    _databaseSettings.IsUsingWindowsIntegratedAuthentication = true;
                    tbUsername.Enabled = false;
                    tbPassword.Enabled = false;
                    break;

                default:
                    _databaseSettings.IsUsingWindowsIntegratedAuthentication = false;
                    tbUsername.Enabled = true;
                    tbPassword.Enabled = true;
                    break;
            }

            tbConnectionString.Text = _databaseSettings.GetPrimaryDatabaseConnectionString();
        }

        private void DatabaseConnectionScreen_Load(object sender, EventArgs e)
        {
            
        }

        public override void InitializeUI()
        {
            _parentForm = (InstallWizardForm)this.ParentForm;
            _databaseSettings = _parentForm.SetupManifest.DatabaseConfiguration;

            // set screen for Windows auth
            cbAuthenticationType.SelectedIndex = 0; // this will fire the SelectedIndexChanged event

            tbServerName.Text = _databaseSettings.DatabaseServerAddress + "," + _databaseSettings.DatabaseServerPort;
            tbConnectionString.Text = _databaseSettings.GetPrimaryDatabaseConnectionString();
        }

        private void TextField_Changed(object sender, EventArgs e)
        {
            TextBox field = (TextBox)sender;
            if (field.Name == "tbServerName")
            {
                string[] serverAndPort = field.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                _databaseSettings.DatabaseServerAddress = serverAndPort[0];
                if (serverAndPort.Length == 2)
                {
                    _databaseSettings.DatabaseServerPort = Utility.SafeConvertToInt(serverAndPort[1]);
                }
                else
                {
                    _databaseSettings.DatabaseServerPort = 1433;
                }
            }

            if (!_databaseSettings.IsUsingWindowsIntegratedAuthentication)
            {
                if (field.Name == "tbUsername")
                {
                    _databaseSettings.Username = field.Text;
                }

                if (field.Name == "tbPassword")
                {
                    _databaseSettings.Password = field.Text;
                }
            }

            tbConnectionString.Text = _databaseSettings.GetPrimaryDatabaseConnectionString();
        }
    }
}
