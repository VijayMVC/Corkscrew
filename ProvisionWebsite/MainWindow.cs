using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Microsoft.Web.Administration;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Web.Configuration;
using System.Windows.Forms;

namespace Corkscrew.Tools.ProvisionWebsite
{
    public partial class MainWindow : Form
    {

        private CSFarm _farm = null;
        private string configDbCn = null, siteDbCn = null;
        private X509Certificate2 sslCertificate = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessMouseMove(this, e);
        }

        private void DatabaseLoginButton_Click(object sender, EventArgs e)
        {
            const string connectionStringUidFormat = "Data Source=$(DBServer);Initial Catalog=$(DBName);User Id=$(Username);Password=$(Password);Connect Timeout=15;Persist Security Info=true;Pooling=true;";
            const string connectionStringIAuthFormat = "Data Source=$(DBServer);Initial Catalog=$(DBName);Trusted_Connection=yes;Connect Timeout=15;Persist Security Info=true;Pooling=true;";

            if (UseIntegratedAuth.Checked)
            {
                configDbCn = connectionStringIAuthFormat.Replace("$(DBServer)", ConfigDBServerName.Text).Replace("$(DBName)", "Corkscrew_ConfigDB");
                siteDbCn = connectionStringIAuthFormat;
            }
            else
            {
                configDbCn = connectionStringUidFormat.Replace("$(DBServer)", ConfigDBServerName.Text).Replace("$(DBName)", "Corkscrew_ConfigDB").Replace("$(Username)", DBUsername.Text).Replace("$(Password)", DBPassword.Text);
                siteDbCn = connectionStringUidFormat.Replace("$(Username)", DBUsername.Text).Replace("$(Password)", DBPassword.Text);
            }

            if (
                    ((ConfigurationManager.ConnectionStrings["configdb"] == null) || (ConfigurationManager.ConnectionStrings["configdb"].ConnectionString != configDbCn))
                    || ((ConfigurationManager.ConnectionStrings["sitedb"] == null) || (ConfigurationManager.ConnectionStrings["sitedb"].ConnectionString != siteDbCn))
            )
            {
                Log.AppendText("Changing configuration string in application..." + Environment.NewLine);

                ExeConfigurationFileMap map = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = (System.Windows.Forms.Application.ExecutablePath + ".config")
                };

                System.Configuration.Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                ConnectionStringSettingsCollection strings = cfg.ConnectionStrings.ConnectionStrings;
                cfg.ConnectionStrings.ConnectionStrings.Clear();

                // add the connection strings
                cfg.ConnectionStrings.ConnectionStrings.EmitClear = true;
                cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("configdb", configDbCn, "System.Data.SqlClient"));
                cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("sitedb", siteDbCn, "System.Data.SqlClient"));

                try
                {
                    cfg.Save(ConfigurationSaveMode.Modified);
                }
                catch (Exception ex)
                {
                    Log.AppendText("Failed saving config file. " + ex.Message + Environment.NewLine);
                    MessageBox.Show("Unable to save connection details to login.");
                    return;
                }

                Log.AppendText("Configuration changed. Please restart this application for changes to take effect." + Environment.NewLine);
                MessageBox.Show("Configuration changed. Please restart this application for changes to take effect.");
                this.Close();
                System.Windows.Forms.Application.Exit();
                return;
            }

            try
            {
                Log.AppendText("Attempting to login to farm with provided credentials...");
                _farm = (UseSystemUser.Checked ? CSFarm.Open(CSUser.CreateSystemUser()) : CSFarm.Open(CSUsername.Text, Utility.GetSha256Hash(CSPassword.Text)));
            }
            catch (Exception ex)
            {
                Log.AppendText("Cannot login to farm. " + ex.Message + Environment.NewLine);
                MessageBox.Show("Unable to login to the farm. Please check the Corkscrew authentication details.");
                return;
            }

            Log.AppendText("[Done]" + Environment.NewLine);

            CorkscrewSite.Items.Clear();
            foreach (CSSite site in _farm.AllSites)
            {
                CorkscrewSite.Items.Add(site.Name);
            }

            CorkscrewSite.Items.Add("(Create New...)");

            Log.AppendText("Please select a site from the Sites dropdown..." + Environment.NewLine);
            CorkscrewSite.Focus();
        }

        private void CorkscrewSite_DropDown(object sender, EventArgs e)
        {
            if (_farm == null)
            {
                MessageBox.Show("Please enter the database information above and click the \"Get sites\" button before using this field.", "Corkscrew - Provision Site", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void PickSSLCertificateButton_Click(object sender, EventArgs e)
        {
            using (CertificateSelectionScreen css = new CertificateSelectionScreen())
            {
                css.RefreshCertificates();

                if ((css.ShowDialog(this) != DialogResult.Cancel) && (css.SelectedCertificate != null))
                {
                    sslCertificate = css.SelectedCertificate;
                    IISSSLCertificateName.Text = css.SelectedCertificate.GetNameInfo(X509NameType.SimpleName, false);

                    if (IISPort.Text != "443")
                    {
                        IISPort.Text = "443";
                    }

                    string certificateHostName = sslCertificate.GetNameInfo(X509NameType.DnsName, false);
                    if (!IISHostname.Text.Equals(certificateHostName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        IISHostname.Text = certificateHostName;
                    }

                    Log.AppendText("SSL certificate selected.");
                }
            }
        }

        private void CanceButton_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Windows.Forms.Application.Exit();
        }

        private void ProvisionButton_Click(object sender, EventArgs e)
        {
            string webAppFolder = IISWebAppDiskFolderPath.Text;
            string corkscrewSiteName = (string)CorkscrewSite.SelectedItem;

            if (webAppFolder == "(Not selected)")
            {
                Log.AppendText("ERROR: No path selected for Web application disk folder. Please select and click Provision again." + Environment.NewLine);
                MessageBox.Show("Please select a valid disk path for \"Web application disk folder\" field");
                SelectWebApplicationFolderButton.Focus();
                return;
            }

            if (!Directory.Exists(webAppFolder))
            {
                Log.AppendText("Creating directory " + webAppFolder + "... ");
                Directory.CreateDirectory(webAppFolder);
                Log.AppendText("[Done]" + Environment.NewLine);
            }

            // copy basic SDK files
            Log.AppendText("Copying Corkscrew DLLs to " + webAppFolder + "\bin...");
            string webAppBinFolder = Path.Combine(webAppFolder, "bin");
            if (! Directory.Exists(webAppBinFolder))
            {
                Directory.CreateDirectory(webAppBinFolder);
            }

            File.Copy(Path.Combine(System.Windows.Forms.Application.StartupPath, "Corkscrew.SDK.dll"), Path.Combine(webAppBinFolder, "Corkscrew.SDK.dll"), true);
            if (File.Exists(Path.Combine(System.Windows.Forms.Application.StartupPath, "Corkscrew.SDK.pdb")))
            {
                File.Copy(Path.Combine(System.Windows.Forms.Application.StartupPath, "Corkscrew.SDK.pdb"), Path.Combine(webAppBinFolder, "Corkscrew.SDK.pdb"), true);
            }
            Log.AppendText(" [Done]" + Environment.NewLine);

            // write global asax
            Log.AppendText("Creating Global.asax...");
            using (StreamWriter sw = new StreamWriter(Path.Combine(webAppFolder, "global.asax"), false))
            {
                sw.WriteLine("<%@ Application Language=\"C#\" %>");
                sw.WriteLine("<script runat=\"server\">");
                sw.WriteLine("static Corkscrew.SDK.providers.filesystem.CSFileSystemProvider _filesystemProvider = null;");
                sw.WriteLine("void Application_Start(object sender, EventArgs e) { ");
                sw.WriteLine("\t _filesystemProvider = new Corkscrew.SDK.providers.filesystem.CSFileSystemProvider();");
                sw.WriteLine("\t _filesystemProvider.RegisterWithHostingEnvironment();");
                sw.WriteLine("}");
                sw.WriteLine("</script>");

                sw.Flush();
                sw.Close();
            }
            Log.AppendText("[Done]" + Environment.NewLine);

            Log.AppendText("Creating web.config...");

            // create web.config
            string webconfigFile = Path.Combine(webAppFolder, "web.config");
            if (File.Exists(webconfigFile))
            {
                File.Delete(webconfigFile);
            }

            ExeConfigurationFileMap map = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = webconfigFile
            };

            System.Configuration.Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ConnectionStringSettingsCollection strings = cfg.ConnectionStrings.ConnectionStrings;
            cfg.ConnectionStrings.ConnectionStrings.Clear();

            // add the connection strings
            cfg.ConnectionStrings.ConnectionStrings.EmitClear = true;
            cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("configdb", configDbCn, "System.Data.SqlClient"));
            cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("sitedb", siteDbCn, "System.Data.SqlClient"));

            // add app settings
            cfg.AppSettings.Settings.EmitClear = true;
            cfg.AppSettings.Settings.Add("RequestHandler:IgnoreExtensions", ".aspx,.master");

            SystemWebSectionGroup webSection = (SystemWebSectionGroup)cfg.GetSectionGroup("system.web");
            webSection.Compilation.Debug = false;
            webSection.Compilation.TargetFramework = "4.6";
            webSection.HttpRuntime.TargetFramework = "4.6";
            webSection.HttpRuntime.MaxRequestLength = int.MaxValue;
            webSection.HttpRuntime.EnableVersionHeader = false;

            try
            {
                cfg.Save(ConfigurationSaveMode.Modified);
            }
            catch
            {
                MessageBox.Show("Unable to save connection details to login.");
                return;
            }

            // now add the system.webServer section
            string webServerSectionXml =
                "    <system.webServer>" +
                "        <security>" +
                "            <requestFiltering>" +
                "                <requestLimits maxAllowedContentLength=\"2147483647\" />" +
                "            </requestFiltering>" +
                "        </security>" +
                "        <httpProtocol>" +
                "            <customHeaders>" +
                "                <remove name=\"X-Powered-By\" />" +
                "            </customHeaders>" +
                "        </httpProtocol>" +
                "        <modules runAllManagedModulesForAllRequests=\"true\">" +
                "            <add name=\"CorkscrewMapRequestHandlerModule\" preCondition=\"integratedMode\" type=\"Corkscrew.SDK.providers.httpmodules.CorkscrewMapRequestHandlerModule, Corkscrew.SDK\" />" +
                "            <add name=\"CorkscrewHttpResponseModule\" preCondition=\"integratedMode\" type=\"Corkscrew.SDK.providers.httpmodules.CorkscrewHttpResponseModule, Corkscrew.SDK\" />" +
                "        </modules>" +
                "        <httpRedirect enabled=\"true\" exactDestination=\"false\" httpResponseStatus=\"Permanent\">" +
                "            <add wildcard=\"*/\" destination=\"default.aspx\" />" +
                "        </httpRedirect>" +
                "    </system.webServer>";

            string existingConfigXml = File.ReadAllText(webconfigFile);
            existingConfigXml = existingConfigXml.Replace("</configuration>", "");
            existingConfigXml = existingConfigXml + webServerSectionXml + "</configuration>";
            File.WriteAllText(webconfigFile, existingConfigXml);
            Log.AppendText("[Done]" + Environment.NewLine);

            // next, setup IIS
            // run aspnet_regiis if we are on Win7 or Win2008
            if ((Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 1))
            {
                // Windows 7 or Windows 2008

                string commandLine = Path.Combine(
                                        Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                                        "Microsoft.NET",
                                        (Environment.Is64BitOperatingSystem ? "Framework64" : "Framework"),
                                        "v4.0.30319",
                                        "aspnet_regiis.exe"
                                     );

                if (File.Exists(commandLine))
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(commandLine, "-i")
                        {
                            CreateNoWindow = false,
                            ErrorDialog = false,
                            LoadUserProfile = false,
                            Verb = "runas"                          // aspnet_regiis requires UAC elevation
                        };

                        Process regProcess = Process.Start(startInfo);
                        regProcess.WaitForExit();
                    }
                    catch (Exception)
                    {
                        Log.AppendText("This system requires that aspnet_regiis.exe be run at least once. Please run it manually from " + commandLine + Environment.NewLine);
                    }
                }
            }

            // Deal with IIS -- app pool & website
            string webUrl = string.Empty;
            try
            {
                string protocol = (IISSSLCertificateName.Text.Equals("(No SSL)") ? "http" : "https");
                webUrl = string.Format("{0}://{1}", protocol, (string.IsNullOrEmpty(IISHostname.Text) ? IISIPAddress.Text : IISHostname.Text));
                if (((protocol == "http") && (IISPort.Text != "80")) || ((protocol == "https") && (IISPort.Text != "443")))
                {
                    webUrl += ":" + IISPort.Text;
                }
                webUrl += "/";

                using (ServerManager iis = new ServerManager())
                {
                    ApplicationPool webAppPool = null;
                    Microsoft.Web.Administration.Site webSite = null;
                    string appPoolName = "Corkscrew - Web App - " + corkscrewSiteName;
                    string webSiteName = "Corkscrew - Web Site - " + corkscrewSiteName;

                    foreach (ApplicationPool p in iis.ApplicationPools)
                    {
                        if (p.Name.Equals(appPoolName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            webAppPool = p;
                            Log.AppendText("Web application pool already exists. Will use existing." + Environment.NewLine);
                            break;
                        }
                    }

                    // create the App Pool
                    if (webAppPool == null)
                    {
                        Log.AppendText("Creating new web application pool in IIS...");
                        webAppPool = iis.ApplicationPools.Add(appPoolName);

                        // Corkscrew Apps are v4, Integrated only
                        webAppPool.ManagedRuntimeVersion = "v4.0";
                        webAppPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                        iis.CommitChanges();
                        Log.AppendText("[Done]" + Environment.NewLine);
                    }

                    // create the website
                    webSite = iis.Sites[webSiteName];
                    if (webSite == null)
                    {
                        Log.AppendText("Creating new website in IIS...");

                        string bindingInfo = string.Format("{0}:{1}:{2}", IISIPAddress.Text, IISPort.Text, IISHostname.Text);

                        if (IISSSLCertificateName.Text != "(No SSL)")
                        {
                            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

                            webSite = iis.Sites.Add(webSiteName, bindingInfo, IISWebAppDiskFolderPath.Text, sslCertificate.GetCertHash(), store.Name);
                            webSite.ApplicationDefaults.EnabledProtocols = "http,https";
                        }
                        else
                        {
                            webSite = iis.Sites.Add(webSiteName, protocol, bindingInfo, IISWebAppDiskFolderPath.Text);
                        }

                        webSite.ServerAutoStart = true;
                        webSite.Applications[0].ApplicationPoolName = appPoolName;

                        iis.CommitChanges();

                        Log.AppendText("[Done]" + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AppendText("ERROR: creating website in IIS failed: " + ex.Message);
            }

            CSSite site = _farm.AllSites.Find(corkscrewSiteName);

            // Add DNS hostname to the CSSite
            string siteDnsName = ((! string.IsNullOrEmpty(IISHostname.Text)) ? IISHostname.Text : string.Format("{0}:{1}", IISIPAddress.Text, IISPort.Text));
            if (site != null)
            {
                if (!site.DNSNames.ContainsNoCase(siteDnsName))
                {
                    Log.AppendText("Adding DNS hostname to Corkscrew Site...");
                    site.DNSNames.Add(siteDnsName);
                    Log.AppendText("[Done]" + Environment.NewLine);
                }
            }

            // check if Site already has a Default.aspx. If it does, do not overwrite it!
            if (site.RootFolder.Files.Find("default.aspx") == null)
            {
                // create a default.aspx in the CSSite
                Log.AppendText("Creating Default.aspx...");

                // write a welcome page. Since this is IIS, use default.aspx
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<%@ Page Title=\"Welcome to Corkscrew CMS\" Language=\"C#\" AutoEventWireup=\"true\" %>");
                sb.AppendLine("<%@ Import Namespace=\"Corkscrew.SDK.objects\" %>");
                sb.AppendLine("<%@ Import Namespace=\"Corkscrew.SDK.security\" %>");
                sb.AppendLine("<script runat=\"server\">");
                sb.AppendLine("\tvoid Page_Load(object sender, EventArgs e) {");
                sb.AppendLine("\t\tCSUser user = CSUser.CreateSystemUser();");
                sb.AppendLine("\t\tCSFarm farm = CSFarm.Open(user);");
                sb.AppendLine("\t\tCSSite site = farm.AllSites.FindByDnsName(Request.Url.Authority);");
                sb.AppendLine("\t\tResponse.Write(\"<h1>Welcome to Corkscrew site!</h1>\");");
                sb.AppendLine("\t\tResponse.Write(\"<p>The current site is: \" + site.Name + \"</p>\");");
                sb.AppendLine("}");
                sb.AppendLine("</script>");

                site.RootFolder.CreateFile("default", ".aspx", System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
            }
            else
            {
                Log.AppendText("Site already contains a /Default.aspx file. Not overwriting it.");
            }

            Log.AppendText("[Done]" + Environment.NewLine);

            // Ensure Anonymous user can access the site
            CSUser anon = CSUser.CreateAnonymousUser();
            CSPermission acl = CSPermission.TestAccess(site.FullPath, anon);
            if (!acl.CanRead)
            {
                Log.AppendText("Granting anonymous user access to the site (you can change this from Corkscrew Control Center or Explorer UIs)...");
                acl.CanRead = true;
                acl.Save();
                Log.AppendText("[Done]" + Environment.NewLine);
            }

            // that's it!
            Log.AppendText("Configuration complete. You may now browse to the site at: " + webUrl + Environment.NewLine);
            MessageBox.Show("The website has been setup. The website Url is [" + webUrl + "].");
        }

        private void UseIntegratedAuth_CheckedChanged(object sender, EventArgs e)
        {
            DBUsername.Enabled = (!UseIntegratedAuth.Checked);
            DBPassword.Enabled = (!UseIntegratedAuth.Checked);
        }

        private void UseSystemUser_CheckedChanged(object sender, EventArgs e)
        {
            CSUsername.Enabled = (!UseSystemUser.Checked);
            CSPassword.Enabled = (!UseSystemUser.Checked);
        }

        private void CorkscrewSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (preventRentrantFiring)
            {
                return;
            }

            string selectedSiteName = (string)CorkscrewSite.SelectedItem;
            if (selectedSiteName.Equals("(Create New...)"))
            {
                using (CreateNewSite frm = new CreateNewSite())
                {
                    if (frm.ShowDialog(this) != DialogResult.Cancel)
                    {
                        try
                        {
                            CSSite site = _farm.AllSites.Add(frm.EnteredSiteName, _farm.AuthenticatedUser, frm.EnteredSiteDescription);
                            if (site != null)
                            {
                                preventRentrantFiring = true;
                                CorkscrewSite.Items.Add(site.Name);
                                CorkscrewSite.SelectedIndex = (CorkscrewSite.Items.Count - 1);    // set it to the just added item
                                preventRentrantFiring = false;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unable to create new site. Ensure the account you are using has sufficient privileges to do so.");
                            return;
                        }
                    }
                }
            }
        }
        bool preventRentrantFiring = false;

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // detect elevated mode
            if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("You must run this program as Administrator. Right-click on the app or shortcut and select \"Run as Administrator\"", "STOP!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                System.Windows.Forms.Application.Exit();
                return;
            }

            bool isIISinstalled = false;
            ServiceController[] windowsServices = ServiceController.GetServices();
            foreach (ServiceController sc in windowsServices)
            {
                if (sc.ServiceName.Equals("W3SVC", StringComparison.InvariantCultureIgnoreCase))
                {
                    isIISinstalled = true;
                    break;
                }
            }

            if (!isIISinstalled)
            {
                MessageBox.Show("You do not seem to have IIS installed on this system. Please install IIS and try again." +
                    Environment.NewLine + "NOTE: This tool should be run on the system where you want to host the website.");

                this.Close();
                System.Windows.Forms.Application.Exit();
                return;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress addr in host.AddressList.AsEnumerable().Where(h => IsAcceptableIPAddress(h)).OrderBy(h => h.AddressFamily.ToString()))
            {
                IISIPAddress.Items.Add(addr.ToString());
            }

            // load the connection string and populate controls
            if ((ConfigurationManager.ConnectionStrings != null) && (ConfigurationManager.ConnectionStrings["configdb"] != null))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["configdb"].ConnectionString;
                string[] parameters = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string param in parameters)
                {
                    string[] pair = param.Split(new char[] { '=' });
                    switch (pair[0].ToUpper())
                    {
                        case "DATA SOURCE":
                            ConfigDBServerName.Text = pair[1];
                            break;

                        case "TRUSTED_CONNECTION":
                            if (pair[1].ToUpper().Equals("YES"))
                            {
                                UseIntegratedAuth.Checked = true;
                            }
                            else
                            {
                                UseIntegratedAuth.Checked = false;
                            }
                            break;

                        case "USER ID":
                            DBUsername.Text = pair[1];
                            break;

                        case "PASSWORD":
                            DBPassword.Text = pair[1];
                            break;
                    }
                }

                DBUsername.Enabled = (!UseIntegratedAuth.Checked);
                DBPassword.Enabled = (!UseIntegratedAuth.Checked);
                if (UseIntegratedAuth.Checked)
                {
                    DBUsername.Text = "";
                    DBPassword.Text = "";
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

        private void SelectWebApplicationFolderButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the folder that would host the IIS application for this site";
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog(this) != DialogResult.Cancel)
                {
                    IISWebAppDiskFolderPath.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
