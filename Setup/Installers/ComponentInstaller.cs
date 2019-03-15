using Corkscrew.SDK.objects;
using Corkscrew.SDK.providers.database;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// A component installer is a collection of installers for a particular component. A component installer will take care of 
    /// installing, uninstalling, repairing and rolling back changes in event of failure, of the entire component.
    /// </summary>
    public class ComponentInstaller
    {

        #region Properties

        /// <summary>
        /// Name of the component (to be shown on the UI).
        /// </summary>
        public string ComponentName
        {
            get;
            set;
        }

        /// <summary>
        /// A friendly description explaning the nature of the component, why/who should install it and so on.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// State of the Corkscrew Farm as required before installation can begin. 
        /// This value is valid only for Install action and is not evaluated for other actions.
        /// </summary>
        public RequiredFarmStateEnum RequiredFarmState
        {
            get;
            set;
        }

        /// <summary>
        /// The current component depends on the components in this collection. What this means is that if this component is selected for 
        /// installation, all the components in this collection must be installed as well. However, uninstalling components will not uninstall 
        /// components in this collection. NOTE that the order in the collection will be followed. Therefore, do add the items in the right order of 
        /// dependency.
        /// </summary>
        public ComponentInstallerCollection Dependencies
        {
            get
            {
                if (_dependencies == null)
                {
                    _dependencies = new ComponentInstallerCollection();
                }

                return _dependencies;
            }
        }
        private ComponentInstallerCollection _dependencies = null;

        /// <summary>
        /// If set, during an INSTALL, if the component fails to install, the entire installation should fail and be rolled back. If FALSE, 
        /// failure is considered non-fatal and other components will be installed.
        /// </summary>
        public bool InstallFailureIsFatal
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the .CAB file this component ships in.
        /// </summary>
        public string CABFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the folder the component installs to. This can be NULL if the component is not a disk-based component (eg: a database). 
        /// This value will be used to create the target directory if it does not exist before firing off the various installers for the component 
        /// and its dependencies. NOTE This should be only the individual folder name like "Website1" and not the full folder path! It can be a 
        /// relative path (like "Websites\Website1").
        /// </summary>
        public string InstallFolderName
        {
            get;
            set;
        }

        /// <summary>
        /// The installers that are to be run. Order in the collection will be followed, therefore be sure to add items in the correct order. 
        /// </summary>
        public WindowsInstallerCollection Installers
        {
            get
            {
                if (_installers == null)
                {
                    _installers = new WindowsInstallerCollection();
                }

                return _installers;
            }
        }
        private WindowsInstallerCollection _installers = null;

        /// <summary>
        /// The last status of the component installer.
        /// </summary>
        public LastActionState Status
        {
            get;
            set;
        } = LastActionState.NotExecuted;

        /// <summary>
        /// Original status of the component (at Setup launch)
        /// </summary>
        public LastActionState OriginalStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The action that is to be executed on the component
        /// </summary>
        public ActionTypeEnum ActionToExecute
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the component</param>
        /// <param name="description">Description of component</param>
        /// <param name="cabName">Name of .CAB file for the component in shipping package</param>
        /// <param name="targetFolderName">Name of the folder to install to</param>
        /// <param name="isCriticalComponent">If set, installation failure will rollback entire installation</param>
        public ComponentInstaller(string name, string description, string cabName, string targetFolderName, bool isCriticalComponent)
        {
            ComponentName = name;
            Description = description;
            CABFileName = cabName;
            InstallFolderName = targetFolderName;
            InstallFailureIsFatal = isCriticalComponent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Installs the component
        /// </summary>
        /// <param name="isRetry">If set, component's last install state is ignored and installation is retried. 
        /// This should be set if the user goes back in the UI and tries to install, without restarting the installer program</param>
        /// <returns>True if the installation succeeded</returns>
        public bool Install(bool isRetry = false)
        {
            if (Status != LastActionState.NotExecuted)
            {
                if (!isRetry)
                {
                    return (Status == LastActionState.Installed);
                }

                // it is a retry, reset the status
                Status = LastActionState.NotExecuted;
            }

            ComponentInstallerCollection flattenedDependencyCollection = ResolveDependencies();
            Status = LastActionState.NotExecuted;

            // install dependencies first
            foreach (ComponentInstaller installer in flattenedDependencyCollection)
            {
                if (installer.Status == LastActionState.NotExecuted)
                {
                    if (!installer.Install())
                    {
                        Status = LastActionState.InstallFailed;
                        return false;
                    }
                }
            }

            // if we got here, either there were no dependencies to install or all of them installed fine
            // execute the main installers
            foreach (WindowsInstaller installer in Installers)
            {
                if (!installer.Install())
                {
                    Status = LastActionState.InstallFailed;
                    return false;
                }
            }

            Status = LastActionState.Installed;
            return true;
        }

        /// <summary>
        /// Uninstalls the component
        /// </summary>
        /// <returns>True if the uninstallation succeeded</returns>
        public bool Uninstall()
        {
            ComponentInstallerCollection flattenedDependencyCollection = ResolveDependencies();

            Status = LastActionState.NotExecuted;

            // execute the main installers
            foreach (WindowsInstaller installer in Installers)
            {
                installer.Uninstall();
                if (installer.LastStatus == LastActionState.UninstallFailed)
                {
                    Status = LastActionState.UninstallFailed;
                    return false;
                }
            }

            // uninstall dependencies last
            foreach (ComponentInstaller installer in flattenedDependencyCollection)
            {
                if (installer.Status == LastActionState.NotExecuted)
                {
                    if (!installer.Uninstall())
                    {
                        Status = LastActionState.UninstallFailed;
                        return false;
                    }
                }
            }


            Status = LastActionState.Uninstalled;
            return true;
        }

        /// <summary>
        /// Repairs the installation
        /// </summary>
        /// <returns>True if repair succeeded</returns>
        public bool Repair()
        {
            // this is nothing but install
            return Install(true);
        }


        /// <summary>
        /// Executes the Undo commands that were added during the install or uninstall phases
        /// </summary>
        /// <returns>Always returns true</returns>
        public bool Rollback()
        {
            // first remove the core component items
            ComponentInstallerCollection flattenedDependencyCollection = ResolveDependencies();

            // execute the main installers
            foreach (WindowsInstaller installer in Installers)
            {
                if (installer.LastStatus == LastActionState.Installed)
                {
                    installer.Uninstall();
                }
            }

            // uninstall dependencies last
            foreach (ComponentInstaller installer in flattenedDependencyCollection)
            {
                if (installer.Status == LastActionState.Installed)
                {
                    installer.Uninstall();
                }
            }


            Status = LastActionState.RolledBack;
            return true;
        }


        /// <summary>
        /// Resolves dependencies, removing any duplicate references or unnecessary dependencies
        /// </summary>
        /// <returns>True if dependencies check out, false if there are dependency validation problems (for developers to fix)</returns>
        public ComponentInstallerCollection ResolveDependencies()
        {
            // since Dependencies property will never be NULL, this is fine
            return new ComponentInstallerCollection(Dependencies.GetFlattenedComponentList());
        }

        /// <summary>
        /// Resolve all path properties for all installers in the component
        /// </summary>
        /// <param name="environment">Key is special variable name, Value is value of that variable</param>
        /// <remarks>Variables are: 
        ///  $(InstallSource) - will be Application.StartupPath + "_layout" (absolute path)
        ///  $(InstallTargetRoot) - OperationManifest.InstallBaseDirectory (absolute path)
        ///  $(ComponentSource) - the particular path within $(InstallSource) where the files for this component are located (absolute path)
        ///  $(ComponentDestination) - ComponentInstaller.InstallFolderName (relative path)
        ///  $(ComponentName) - name of the current component from the manifest Xml
        /// </remarks>
        public void ResolvePathVariables(Dictionary<string, string> environment)
        {
            // first create a copy of the env to avoid polluting it with local data
            Dictionary<string, string> localEnvironment = new Dictionary<string, string>();
            foreach (string key in environment.Keys)
            {
                localEnvironment.Add(key, environment[key]);
            }

            // add component specific variables
            localEnvironment.Add("$(ComponentName)", ComponentName);
            localEnvironment.Add("$(ComponentDestination)", InstallFolderName);

            // resolve local references to the vars within the evironment keys
            IEnumerable<string> keysList = localEnvironment.Keys.ToList();
            foreach(string env in keysList)
            {
                List<string> vars = ExtractInstallerVar(localEnvironment[env]);
                foreach(string item in vars)
                {
                    // if it is not found at this level, we will leave the var to be resolved downlevel
                    if (localEnvironment.ContainsKey(item))
                    {
                        localEnvironment[env] = localEnvironment[env].Replace(item, localEnvironment[item]);
                    }
                }
            }

            // $(ComponentSource) is added by the caller

            foreach (WindowsInstaller installer in Installers)
            {
                installer.ResolvePathProperties(localEnvironment);
            }

        }

        /// <summary>
        /// Extracts all the installer variables from the given string
        /// </summary>
        /// <param name="value">String to extract values from</param>
        /// <returns>List of extracted variables. Will never be NULL</returns>
        protected List<string> ExtractInstallerVar(string value)
        {
            List<string> result = new List<string>();

            Regex expression = new Regex(@"\$\(\w+\)");
            foreach(Match m in expression.Matches(value))
            {
                result.Add(m.Value);
            }

            return result;
        }


        /// <summary>
        /// Evaluate if the condition mentioned in RequiredFarmState is met
        /// </summary>
        /// <returns>True if condition is met (installation can proceed).</returns>
        public bool EvaluateFarmState(OperationManifest manifest)
        {
            if ((ActionToExecute != ActionTypeEnum.Install) || (RequiredFarmState == RequiredFarmStateEnum.Any))
            {
                // if it is not install or component doesnt care, then return true
                return true;
            }

            string configdbConnectionString = manifest.DatabaseConfiguration.GetConnectionStringForDatabase("Corkscrew_ConfigDB");
            ICSDatabaseProvider dbProvider = CSDatabaseProviderFactory.GetProviderByConnectionString(configdbConnectionString);
            bool configDbExists = false;
            foreach (string dbName in dbProvider.GetAllDatabases(configdbConnectionString))
            {
                if (dbName.Equals("Corkscrew_ConfigDB", StringComparison.InvariantCultureIgnoreCase))
                {
                    configDbExists = true;
                    break;
                }
            }

            if ((!configDbExists) && (RequiredFarmState == RequiredFarmStateEnum.NoFarm))
            {
                return true;
            }

            if (configDbExists && (RequiredFarmState == RequiredFarmStateEnum.MustExist))
            {
                return true;
            }

            // configdb exists.. Test if we have non-OOBE data
            CSFarm farm = CSFarm.Open(CSUser.CreateSystemUser());
            if ((farm.AllSites.Count > 1) || (farm.AllWorkflowDefinitions.Count > 0))
            {
                if (RequiredFarmState == RequiredFarmStateEnum.EstablishedFarm)
                {
                    return true;
                }
            }
            else
            {
                if (RequiredFarmState == RequiredFarmStateEnum.FreshInstall)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


    }
}
