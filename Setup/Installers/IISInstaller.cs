using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Installs web applications and sites into IIS
    /// </summary>
    public class IISInstaller : WindowsInstaller
    {

        private static bool AspNetRegIISHasBeenExecuted = false;

        #region Properties

        /// <summary>
        /// Configuration for one website in IIS
        /// </summary>
        public IISSettings WebConfiguration
        {
            get
            {
                if (_webConfig == null)
                {
                    _webConfig = IISSettings.GetDefaultBinding();
                }

                return _webConfig;
            }
            set
            {
                _webConfig = value;
            }
        }
        private IISSettings _webConfig = null;


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings for one website</param>
        public IISInstaller(IISSettings settings)
            : base()
        {
            WebConfiguration = settings;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Installs the items to Windows
        /// </summary>
        /// <returns>True if installation succeeded</returns>
        public override bool Install()
        {
            if (!IsIISInstalled())
            {
                LastStatus = LastActionState.InstallFailed;
                return false;
            }

            RunAspNetRegIISIfRequired();

            if (CreateWebsite() != null)
            {
                LastStatus = LastActionState.Installed;
                return true;
            }

            LastStatus = LastActionState.InstallFailed;
            return false;
        }

        /// <summary>
        /// Repairs the items in Windows
        /// </summary>
        /// <returns>True if repair succeeded</returns>
        public override bool Repair()
        {
            return Install();
        }

        /// <summary>
        /// Uninstalls items from Windows
        /// </summary>
        public override void Uninstall()
        {
            LastStatus = LastActionState.Uninstalled;

            if (!IsIISInstalled())
            {
                return;
            }

            // will delete app pool as well
            DeleteWebsite();
        }

        private bool IsIISInstalled()
        {
            OnProgressChanged(1, "Checking if IIS is installed... ");
            ServiceController[] windowsServices = ServiceController.GetServices();
            foreach (ServiceController sc in windowsServices)
            {
                if (sc.ServiceName.Equals("W3SVC", StringComparison.InvariantCultureIgnoreCase))
                {
                    OnProgressChanged(0, "[Yes]");
                    return true;
                }
            }

            OnProgressChanged(0, "[No]");
            return false;
        }

        private void RunAspNetRegIISIfRequired()
        {
            if (AspNetRegIISHasBeenExecuted)
            {
                return;
            }

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

                if (!File.Exists(commandLine))
                {
                    // oops! Either we do not have v4.0 installed or for some reason aspnet_regiis.exe is missing
                    OnProgressChanged(0, "Cannot run aspnet_regiis.exe for .NET Framework 4.0: File does not exist at " + commandLine);
                    return;
                }

                try
                {
                    OnProgressChanged(1, "Running aspnet_regiis.exe... ");
                    ProcessStartInfo startInfo = new ProcessStartInfo(commandLine, "-i")
                    {
                        CreateNoWindow = false,
                        ErrorDialog = false,
                        LoadUserProfile = false,
                        Verb = "runas"                          // aspnet_regiis requires UAC elevation
                    };

                    Process regProcess = Process.Start(startInfo);
                    regProcess.WaitForExit();

                    OnProgressChanged(1, "[Success]");
                    AspNetRegIISHasBeenExecuted = true;
                }
                catch (Exception ex)
                {
                    OnProgressChanged(1, "[Failed]: " + ex.Message);
                }
            }
        }

        private ApplicationPool CreateAppPool()
        {
            ApplicationPool pool = null;

            try
            {
                using (ServerManager iis = new ServerManager())
                {
                    OnProgressChanged(1, "Searching for web application (AppPool) " + WebConfiguration.AppName + "... ");
                    foreach (ApplicationPool p in iis.ApplicationPools)
                    {
                        if (p.Name.Equals(WebConfiguration.AppName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            pool = p;
                            break;
                        }
                    }

                    if (pool == null)
                    {
                        OnProgressChanged(1, "Creating web application... ");
                        pool = iis.ApplicationPools.Add(WebConfiguration.AppName);

                        // Corkscrew Apps are v4, Integrated only
                        pool.ManagedRuntimeVersion = "v4.0";
                        pool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                        iis.CommitChanges();
                    }

                    UndoCommands.Push(
                        () =>
                        {
                            DeleteAppPool();
                        }
                    );
                    OnProgressChanged(0, "[Success]");
                }
            }
            catch (Exception ex)
            {
                OnProgressChanged(0, "[Failed]: " + ex.Message);
                return null;
            }

            return pool;
        }

        private void DeleteAppPool()
        {
            try
            {
                using (ServerManager iis = new ServerManager())
                {
                    ApplicationPool pool = null;

                    OnProgressChanged(1, "Searching for web application (AppPool) " + WebConfiguration.AppName + "... ");
                    foreach (ApplicationPool p in iis.ApplicationPools)
                    {
                        if (p.Name.Equals(WebConfiguration.AppName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            pool = p;
                            break;
                        }
                    }

                    if (pool != null)
                    {
                        OnProgressChanged(1, "Deleting web application (AppPool) " + WebConfiguration.AppName + "... ");
                        iis.ApplicationPools.Remove(pool);
                        iis.CommitChanges();
                        OnProgressChanged(0, "[Success]");
                    }
                }
            }
            catch (Exception ex)
            {
                OnProgressChanged(0, "[Failed]: " + ex.Message);
            }
        }

        private Site CreateWebsite()
        {
            ApplicationPool pool = CreateAppPool();
            Site website = null;

            if (pool == null)
            {
                return null;
            }

            try
            {
                using (ServerManager iis = new ServerManager())
                {
                    OnProgressChanged(1, "Searching for website " + WebConfiguration.WebsiteName + "... ");
                    website = iis.Sites[WebConfiguration.WebsiteName];
                    if (website == null)
                    {
                        OnProgressChanged(1, "Creating website " + WebConfiguration.WebsiteName + "... ");

                        if (WebConfiguration.IsSSLBinding)
                        {
                            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                            x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

                            website = iis.Sites.Add(WebConfiguration.WebsiteName, WebConfiguration.GetBindingInfo(), WebConfiguration.WebApplicationFolder, WebConfiguration.SSLCertificate.GetCertHash(), x509Store.Name);
                        }
                        else
                        {
                            website = iis.Sites.Add(WebConfiguration.WebsiteName, WebConfiguration.GetProtocolName(), WebConfiguration.GetBindingInfo(), WebConfiguration.WebApplicationFolder);
                        }
                    }

                    OnProgressChanged(1, "Setting properties for website " + WebConfiguration.WebsiteName + "... ");
                    website.ServerAutoStart = true;
                    website.Applications[0].ApplicationPoolName = pool.Name;

                    iis.CommitChanges();
                    OnProgressChanged(0, "[Success]");
                }

                UndoCommands.Push(
                    () =>
                    {
                        DeleteWebsite();
                    }
                );
            }
            catch (Exception ex)
            {
                OnProgressChanged(0, "[Failed]: " + ex.Message);
                website = null;
            }

            return website;
        }

        private void DeleteWebsite()
        {
            try
            {
                using (ServerManager iis = new ServerManager())
                {
                    OnProgressChanged(1, "Searching for website " + WebConfiguration.WebsiteName + "... ");
                    Site website = iis.Sites[WebConfiguration.WebsiteName];
                    if (website != null)
                    {
                        OnProgressChanged(1, "Deleting website " + WebConfiguration.WebsiteName + "... ");
                        iis.Sites.Remove(website);
                        OnProgressChanged(0, "[Success]");

                        DeleteAppPool();
                    }

                    iis.CommitChanges();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Resolve all path properties in the installer
        /// </summary>
        /// <param name="environment">Key is special variable name, Value is value of that variable</param>
        /// <remarks>Variables are: 
        ///  $(InstallSource) - will be Application.StartupPath + "_layout" (absolute path)
        ///  $(InstallTargetRoot) - OperationManifest.InstallBaseDirectory (absolute path)
        ///  $(ComponentSource) - the particular path within $(InstallSource) where the files for this component are located (absolute path)
        ///  $(ComponentDestination) - ComponentInstaller.InstallFolderName (relative path)
        ///  $(ComponentName) - name of the current component from the manifest Xml
        /// </remarks>
        public override void ResolvePathProperties(Dictionary<string, string> environment)
        {
            foreach (string variable in environment.Keys)
            {
                WebConfiguration.WebApplicationFolder = WebConfiguration.WebApplicationFolder.Replace(variable, environment[variable]);
            }
        }

        #endregion

    }
}
