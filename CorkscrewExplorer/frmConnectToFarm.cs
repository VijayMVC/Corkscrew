using System;
using System.Configuration;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmConnectToFarm : Form
    {
        private ConnectionStringBuilder _csb = null;

        public frmConnectToFarm()
        {
            InitializeComponent();
        }

        private void frmConnectToFarm_Shown(object sender, EventArgs e)
        {
            _csb = new ConnectionStringBuilder();
            cmbDatabaseEngine.SelectedItem = cmbDatabaseEngine.Items[cmbDatabaseEngine.FindString("(Select a database engine)")];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string configDBConnectionString = _csb.ToString();
            string siteDBConnectionString = null;

            _csb.ServerAddress = "$(DBServer)";
            siteDBConnectionString = _csb.ToString().Replace("Corkscrew_ConfigDB", "$(DBName)");

            Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(null, ConfigurationUserLevel.None);
            ConnectionStringSettingsCollection strings = cfg.ConnectionStrings.ConnectionStrings;
            cfg.ConnectionStrings.ConnectionStrings.Clear();

            // add the connection strings
            cfg.ConnectionStrings.ConnectionStrings.EmitClear = true;
            cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("configdb", configDBConnectionString));
            cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("sitedb", siteDBConnectionString));

            try
            {
                cfg.Save(ConfigurationSaveMode.Modified);
            }
            catch { }
            finally
            {
                cfg = null;
            }

            this.Close();
        }

        private void tbServerAddress_TextChanged(object sender, EventArgs e)
        {
            _csb.ServerAddress = tbServerAddress.Text;
            tbConnectionString.Text = _csb.ToString();
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            _csb.Username = tbUsername.Text;
            tbConnectionString.Text = _csb.ToString();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            _csb.Password = tbPassword.Text;
            tbConnectionString.Text = _csb.ToString();
        }

        private void cmbDatabaseEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            _csb.ProviderName = DatabaseProviderEnum.NotSelected;
            _csb.ExtraOptions = string.Empty;

            switch (cmbDatabaseEngine.SelectedItem.ToString())
            {
                case "Microsoft SQL Server":
                    _csb.ProviderName = DatabaseProviderEnum.MSSQL;
                    _csb.ExtraOptions = "Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;Pooling=true;Packet Size=32768;";
                    break;

                case "MySQL":
                case "InnoDB":
                    _csb.ProviderName = DatabaseProviderEnum.MySQL;
                    _csb.ExtraOptions = "Encrypt=false;AllowBatch=True;AllowUserVariables=True;ConvertZeroDateTime=True;UseCompression=True;Pooling=True;sqlservermode=True;";
                    break;
            }

            tbConnectionString.Text = _csb.ToString();
        }

        private void rbOptionUseIntegratedAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if ((rbOptionUseIntegratedAuthentication.Checked) && (_csb.ProviderName == DatabaseProviderEnum.MySQL))
            {
                MessageBox.Show(
                    "Please ensure that the [authentication_windows] plugin for MySQL Server is installed on the target server." + Environment.NewLine +
                    "You can find more information about this plugin at: " + Environment.NewLine +
                    "https://dev.mysql.com/doc/refman/5.5/en/windows-authentication-plugin.html",
                    "Corkscrew Installer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            tbUsername.Enabled = (!rbOptionUseIntegratedAuthentication.Checked);
            tbPassword.Enabled = (!rbOptionUseIntegratedAuthentication.Checked);
            _csb.UseIntegratedAuth = rbOptionUseIntegratedAuthentication.Checked;
            tbConnectionString.Text = _csb.ToString();
        }

        private void rbOptionUseUIDPWDAuth_CheckedChanged(object sender, EventArgs e)
        {
            tbUsername.Enabled = (rbOptionUseUIDPWDAuth.Checked);
            tbPassword.Enabled = (rbOptionUseUIDPWDAuth.Checked);
            _csb.UseIntegratedAuth = (!rbOptionUseUIDPWDAuth.Checked);
            tbConnectionString.Text = _csb.ToString();
        }


        private class ConnectionStringBuilder
        {
            public DatabaseProviderEnum ProviderName { get; set; }
            public string ServerAddress { get; set; }
            public string DatabaseName { get; private set; } = "Corkscrew_ConfigDB";
            public string Username { get; set; }
            public string Password { get; set; }
            public bool UseIntegratedAuth { get; set; }

            public string ExtraOptions { get; set; }

            public ConnectionStringBuilder() { }
            public ConnectionStringBuilder(ConnectionStringBuilder copyFrom)
            {
                ProviderName = copyFrom.ProviderName;
                ServerAddress = copyFrom.ServerAddress;
                DatabaseName = copyFrom.DatabaseName;
                Username = copyFrom.Username;
                Password = copyFrom.Password;
                UseIntegratedAuth = copyFrom.UseIntegratedAuth;
                ExtraOptions = copyFrom.ExtraOptions;
            }

            public override string ToString()
            {
                string result = string.Empty;

                if (UseIntegratedAuth)
                {
                    switch (ProviderName)
                    {
                        case DatabaseProviderEnum.MSSQL:
                            result = string.Format(
                                        "Data Source={0};Initial Catalog={1};Trusted_Connection=yes;{2}",
                                        ServerAddress,
                                        DatabaseName,
                                        ExtraOptions
                                     );
                            break;

                        case DatabaseProviderEnum.MySQL:
                            result = string.Format(
                                        "Server={0};Database={1};Trusted_Connection=yes;{2}",
                                        ServerAddress?.Replace(":", ";Port="),
                                        DatabaseName,
                                        ExtraOptions
                                     );
                            break;
                    }
                }
                else
                {
                    switch (ProviderName)
                    {
                        case DatabaseProviderEnum.MSSQL:
                            result = string.Format(
                                        "Data Source={0};Initial Catalog={1};User Id={2};Password={3};Persist Security Info=true;{4}",
                                        ServerAddress,
                                        DatabaseName,
                                        Username,
                                        Password,
                                        ExtraOptions
                                     );
                            break;

                        case DatabaseProviderEnum.MySQL:
                            result = string.Format(
                                        "Server={0};Database={1};Uid={2};Pwd={3};Persist Security Info=true;{4}",
                                        ServerAddress?.Replace(":", ";Port="),
                                        DatabaseName,
                                        Username,
                                        Password,
                                        ExtraOptions
                                     );
                            break;
                    }
                }

                return result;
            }


        }

        private enum DatabaseProviderEnum
        {
            NotSelected = 0,
            MSSQL,
            MySQL
            //, InnoDB      /* not used */
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmConnectToFarm_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
