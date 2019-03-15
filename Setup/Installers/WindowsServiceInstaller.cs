using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Installs Windows Services
    /// </summary>
    public class WindowsServiceInstaller : WindowsInstaller
    {

        #region Properties

        /// <summary>
        /// Name of the Windows Service (ServiceController.ServiceName). This is not the "friendly name", but 
        /// is the name Windows knows the service as (net start.... ).
        /// </summary>
        public string ServiceName
        {
            get;
            set;
        }

        /// <summary>
        /// Full path to the EXE containing the service
        /// </summary>
        public string ServiceFilename
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceName">Internal name of the Windows Service</param>
        public WindowsServiceInstaller(string serviceName)
            : base()
        {
            ServiceName = serviceName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Install the service
        /// </summary>
        /// <returns>True if successful</returns>
        public override bool Install()
        {
            Uninstall();

            try
            {
                OnProgressChanged(1, "Installing Windows Service [" + ServiceName + "]... ");
                IDictionary installerState = new Hashtable();
                using (AssemblyInstaller installer = new AssemblyInstaller(ServiceFilename, new string[] { }))
                {
                    installer.UseNewContext = true;
                    installer.Install(installerState);
                    installer.Commit(installerState);
                }

                UndoCommands.Push(
                    () =>
                    {
                        Uninstall();
                    }
                );

                LastStatus = LastActionState.Installed;
                OnProgressChanged(0, "[Success]");
            }
            catch (Exception ex)
            {
                LastStatus = LastActionState.InstallFailed;
                OnProgressChanged(0, "[Failed]: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Repair the service
        /// </summary>
        /// <returns>True if repair succeeded</returns>
        public override bool Repair()
        {
            return Install();
        }

        /// <summary>
        /// Uninstall the service
        /// </summary>
        public override void Uninstall()
        {
            OnProgressChanged(1, "Stopping service [" + ServiceName + "]... ");
            StopServiceIfRunning();
            OnProgressChanged(0, "[Success]");

            try
            {
                IDictionary installerState = new Hashtable();
                using (AssemblyInstaller installer = new AssemblyInstaller(ServiceFilename, new string[] { }))
                {
                    installer.UseNewContext = true;
                    installer.Uninstall(installerState);
                }

                LastStatus = LastActionState.Uninstalled;
                OnProgressChanged(0, "[Success]");
            }
            catch (Exception ex)
            {
                LastStatus = LastActionState.UninstallFailed;
                OnProgressChanged(0, "[Failed]: " + ex.Message);
            }
        }


        private void StopServiceIfRunning()
        {
            ServiceController[] windowsServices = ServiceController.GetServices();
            foreach (ServiceController service in windowsServices)
            {
                if (service.ServiceName.Equals(ServiceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        OnProgressChanged(1, "Sending STOP signal to service [" + ServiceName + "]... ");
                        service.Stop();
                    }
                    catch
                    {
                        // eat
                    }
                    finally
                    {
                        OnProgressChanged(1, "Waiting for service [" + ServiceName + "] to stop... ");
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                        OnProgressChanged(0, "[Done]");
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Checks if the given Windows service exists
        /// </summary>
        /// <param name="serviceName">Service name (ServiceController.ServiceName)</param>
        /// <returns>True if service exists</returns>
        public static bool ServiceExists(string serviceName)
        {
            ServiceController[] windowsServices = ServiceController.GetServices();
            foreach (ServiceController service in windowsServices)
            {
                if (service.ServiceName.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
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
                ServiceFilename = ServiceFilename.Replace(variable, environment[variable]);
            }
        }

        #endregion

    }
}
