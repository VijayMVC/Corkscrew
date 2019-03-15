using CMS.Setup.Installers;
using Corkscrew.SDK.tools;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class IISConnectionScreen : ScreenTemplate
    {

        private InstallWizardForm _parentForm = null;
        private IISSettings _apiSettings = null;
        private IISSettings _ccSettings = null;

        public IISConnectionScreen()
        {
            InitializeComponent();
        }

        private void tbCertificateAPI_DoubleClick(object sender, EventArgs e)
        {
            GetCertificateFile(_apiSettings, tbCertificateAPI);
        }

        private void tbCertificateCC_DoubleClick(object sender, EventArgs e)
        {
            GetCertificateFile(_ccSettings, tbCertificateCC);
        }

        private void IISConnectionScreen_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeUI()
        {
            _parentForm = (InstallWizardForm)this.ParentForm;

            lblControlsDisableAPI.Visible = true;
            lblControlsDisableCC.Visible = true;

            // check if IIS is installed
            if (! WindowsServiceInstaller.ServiceExists("W3SVC"))
            {
                UI.ShowMessage(this.ParentForm, "IIS is not installed on this system. If you wish to install web components, please install IIS before starting this wizard.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            cbIPAddressAPI.Items.Clear();
            cbIPAddressCC.Items.Clear();

            // load IP addresses
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress addr in host.AddressList.AsEnumerable().Where(h => IsAcceptableIPAddress(h)).OrderBy(h => h.AddressFamily.ToString()))
            {
                string addrStr = addr.ToString();

                cbIPAddressAPI.Items.Add(addrStr);
                cbIPAddressCC.Items.Add(addrStr);
            }

            foreach (ComponentInstaller component in _parentForm.SetupManifest.Installers)
            {
                if (
                        (component.ComponentName.Equals("APIService", StringComparison.InvariantCultureIgnoreCase) || component.ComponentName.Equals("ControlCenter", StringComparison.InvariantCultureIgnoreCase))
                        && ((component.ActionToExecute == ActionTypeEnum.Install) || (component.ActionToExecute == ActionTypeEnum.Repair))
                )
                {
                    foreach (WindowsInstaller installer in component.Installers)
                    {
                        if (installer is IISInstaller)
                        {
                            switch (component.ComponentName)
                            {
                                case "APIService":
                                    _apiSettings = ((IISInstaller)installer).WebConfiguration;
                                    lblControlsDisableAPI.Visible = false;
                                    break;

                                case "ControlCenter":
                                    _ccSettings = ((IISInstaller)installer).WebConfiguration;
                                    lblControlsDisableCC.Visible = false;
                                    break;
                            }
                        }
                    }
                }
            }

            // rebind settings to controls
            if (_apiSettings != null)
            {
                cbIPAddressAPI.SelectedItem = _apiSettings.BindingHostAddress.ToString();
                tbPortAPI.Text = _apiSettings.BindingPort.ToString();
                tbHostnameAPI.Text = _apiSettings.BindingHostname;
                cbSSLAPI.Checked = _apiSettings.IsSSLBinding;
                if ((_apiSettings.IsSSLBinding) && (_apiSettings.SSLCertificate != null))
                {
                    tbCertificateAPI.Text = _apiSettings.SSLCertificate.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);
                }
                else
                {
                    _apiSettings.IsSSLBinding = false;
                    _apiSettings.SSLCertificate = null;
                }
            }

            if (_ccSettings != null)
            {
                cbIPAddressCC.SelectedItem = _ccSettings.BindingHostAddress.ToString();
                tbPortCC.Text = _ccSettings.BindingPort.ToString();
                tbHostnameCC.Text = _ccSettings.BindingHostname;
                cbSSLCC.Checked = _ccSettings.IsSSLBinding;
                if ((_ccSettings.IsSSLBinding) && (_ccSettings.SSLCertificate != null))
                {
                    tbCertificateCC.Text = _ccSettings.SSLCertificate.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);
                }
                else
                {
                    _ccSettings.IsSSLBinding = false;
                    _ccSettings.SSLCertificate = null;
                }
            }
        }

        private bool IsAcceptableIPAddress(IPAddress addr)
        {
            return (
                (addr.AddressFamily.HasFlag(AddressFamily.InterNetwork) || addr.AddressFamily.HasFlag(AddressFamily.InterNetworkV6))
                && (!addr.IsIPv6Multicast) && (!addr.IsIPv6Teredo) && (!addr.IsIPv6LinkLocal)
            );
        }

        private void GetCertificateFile(IISSettings setting, TextBox field)
        {
            using (CertificateSelectionScreen css = new CertificateSelectionScreen())
            {
                css.RefreshCertificates();

                if (css.ShowDialog(this) != DialogResult.Cancel)
                {
                    field.Text = css.SelectedCertificate.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);

                    setting.IsSSLBinding = true;

                    try
                    {
                        // this will throw various validation errors
                        setting.SSLCertificate = css.SelectedCertificate;
                    }
                    catch (Exception ex)
                    {
                        UI.ShowMessage(_parentForm, "That certificate cannot be assigned to this site. Select a different certificate. Reason: " + ex.Message);
                    }
                }
            }
        }

        private void tbPortAPI_TextChanged(object sender, EventArgs e)
        {
            _apiSettings.BindingPort = Utility.SafeConvertToInt(tbPortAPI.Text);
        }

        private void tbPortCC_TextChanged(object sender, EventArgs e)
        {
            _ccSettings.BindingPort = Utility.SafeConvertToInt(tbPortCC.Text);
        }

        private void tbHostnameAPI_TextChanged(object sender, EventArgs e)
        {
            _apiSettings.BindingHostname = tbHostnameAPI.Text;
        }

        private void tbHostnameCC_TextChanged(object sender, EventArgs e)
        {
            _ccSettings.BindingHostname = tbHostnameCC.Text;
        }

        private void cbSSLAPI_CheckedChanged(object sender, EventArgs e)
        {
            _apiSettings.IsSSLBinding = cbSSLAPI.Checked;
            if (! _apiSettings.IsSSLBinding)
            {
                _apiSettings.SSLCertificate = null;
            }
        }

        private void cbSSLCC_CheckedChanged(object sender, EventArgs e)
        {
            _ccSettings.IsSSLBinding = cbSSLCC.Checked;
            if (!_ccSettings.IsSSLBinding)
            {
                _ccSettings.SSLCertificate = null;
            }
        }

        private void cbIPAddressAPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPAddress address = IPAddress.Parse(cbIPAddressAPI.SelectedItem.ToString());
            _apiSettings.BindingHostAddress = address;
        }

        private void cbIPAddressCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPAddress address = IPAddress.Parse(cbIPAddressCC.SelectedItem.ToString());
            _ccSettings.BindingHostAddress = address;
        }
    }
}
