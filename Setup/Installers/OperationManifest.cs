using Corkscrew.SDK.tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Class to store configuration information for the installer
    /// </summary>
    public class OperationManifest
    {

        #region Properties

        /// <summary>
        /// Base directory under which all apps and files are to be installed
        /// </summary>
        public string InstallBaseDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Root of directory where all the component CAB files will be extracted (temp folder)
        /// </summary>
        public string CabExtractDirectoryRoot
        {
            get;
            set;
        }

        /// <summary>
        /// Installers to be executed. They would have been added to the collection in the desired sequence.
        /// </summary>
        public ComponentInstallerCollection Installers
        {
            get
            {
                if (_installers == null)
                {
                    _installers = new ComponentInstallerCollection();
                }

                return _installers;
            }
        }
        private ComponentInstallerCollection _installers = null;

        /// <summary>
        /// Returns the currently configured database settings. This is passed into any DatabaseInstallers that we fire
        /// </summary>
        public DatabaseSettings DatabaseConfiguration
        {
            get
            {
                if (_dbSettings == null)
                {
                    _dbSettings = DatabaseSettings.GetSQLServerDefaultSettings();
                }

                return _dbSettings;
            }
        }
        private DatabaseSettings _dbSettings = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public OperationManifest()
        {
            LoadInstallManifest();
        }

        #endregion

        #region Methods
        
        private void LoadInstallManifest()
        {
            string manifestPath = Path.Combine(Application.StartupPath, "manifest.xml");
            if (!File.Exists(manifestPath))
            {
                throw new FileNotFoundException("Could not find installation manifest xml.");
            }

            XmlDocument manifestXml = new XmlDocument();
            manifestXml.Load(manifestPath);

            Dictionary<string, List<ComponentInstaller>> pendingDependencyMaps = new Dictionary<string, List<ComponentInstaller>>();

            XmlNode componentsNode = manifestXml.DocumentElement.SelectSingleNode("//Corkscrew/Components");
            foreach (XmlNode componentNode in componentsNode.ChildNodes)
            {
                ComponentInstaller installer = new ComponentInstaller(
                    componentNode.Attributes["Name"].Value,
                    "",
                    Path.Combine(Application.StartupPath, componentNode.Attributes["CabFileName"].Value + ".cab"),
                    componentNode.Attributes["InstallFolderName"].Value,
                    Utility.SafeConvertToBool(componentNode.Attributes["InstallFailureIsFatal"].Value)
                );

                string reqFarmState = ((componentNode.Attributes["RequiredFarmState"] == null) ? null : componentNode.Attributes["RequiredFarmState"].Value);
                installer.RequiredFarmState = (RequiredFarmStateEnum)Enum.Parse(typeof(RequiredFarmStateEnum), Utility.SafeString(reqFarmState, "Any"));

                if (RegistryKeyAction.IsComponentInstalled(installer.ComponentName))
                {
                    installer.OriginalStatus = LastActionState.Installed;
                }
                else
                {
                    installer.OriginalStatus = LastActionState.NotExecuted;
                }

                installer.ActionToExecute = ActionTypeEnum.Undefined;

                // load the description
                XmlNode descriptionNode = componentNode.SelectSingleNode("description");
                if (descriptionNode != null)
                {
                    installer.Description = descriptionNode.InnerText.Replace(Environment.NewLine, "").Replace("\t", "").Replace("  ", " ");
                }

                XmlNodeList dependencies = componentNode.SelectNodes("Dependencies/DependsOn");
                if (dependencies != null)
                {
                    foreach (XmlNode dependency in dependencies)
                    {
                        string depName = dependency.Attributes["Name"].Value;
                        ComponentInstaller dep = Installers.Find(depName);
                        if (dep != null)
                        {
                            installer.Dependencies.Add(dep);
                        }
                        else
                        {
                            // installer not yet added
                            if (pendingDependencyMaps.ContainsKey(depName.ToUpper()))
                            {
                                pendingDependencyMaps[depName.ToUpper()].Add(installer);
                            }
                            else
                            {
                                pendingDependencyMaps.Add(
                                    depName.ToUpper(),
                                    new List<ComponentInstaller>() { installer }
                                );
                            }
                        }
                    }
                }

                XmlNodeList installers = componentNode.SelectNodes("Installers/Installer");
                if (installers != null)
                {
                    foreach(XmlNode installerNode in installers)
                    {
                        string typeName = installerNode.Attributes["type"].Value + "Installer";
                        Type installerType = Type.GetType("CMS.Setup.Installers." + typeName);
                        if (installerType == null)
                        {
                            throw new InvalidOperationException("Installer of type " + typeName + " does not exist.");
                        }

                        // based on type of installer, initialize them
                        switch (typeName)
                        {
                            case "ConnectionStringInstaller":
                                installer.Installers.Add(Initialize(new ConnectionStringInstaller("", DatabaseConfiguration), installerNode));
                                break;

                            case "DatabaseInstaller":
                                installer.Installers.Add(Initialize(new DatabaseInstaller(DatabaseConfiguration), installerNode));
                                break;

                            case "FilesystemInstaller":
                                installer.Installers.Add(Initialize(new FilesystemInstaller("", "", true), installerNode));
                                break;

                            case "IISInstaller":
                                installer.Installers.Add(Initialize(new IISInstaller(IISSettings.GetDefaultBinding()), installerNode));
                                break;

                            case "ProgramShortcutsInstaller":
                                installer.Installers.Add(Initialize(new ProgramShortcutsInstaller(), installerNode));
                                break;

                            case "RegistryInstaller":
                                installer.Installers.Add(Initialize(new RegistryInstaller(), installerNode));
                                break;

                            case "ToolRunnerInstaller":
                                installer.Installers.Add(Initialize(new ToolRunnerInstaller(), installerNode));
                                break;

                            case "WindowsServiceInstaller":
                                installer.Installers.Add(Initialize(new WindowsServiceInstaller(""), installerNode));
                                break;
                        }
                    }
                }

                this.Installers.Add(installer);

            }

            // check and resolve pending dependencies
            foreach(string componentName in pendingDependencyMaps.Keys)
            {
                ComponentInstaller dep = Installers.Find(componentName);
                if (dep != null)
                {
                    foreach(ComponentInstaller componentInstaller in pendingDependencyMaps[componentName])
                    {
                        componentInstaller.Dependencies.Add(dep);
                    }
                }
                else
                {
                    // bad !!!
                    throw new InvalidOperationException("Dependency " + componentName + " was not resolved. Did you miss CABing a component?");
                }
            }

        }

        #region Installer initializers

        private WindowsInstaller Initialize(ConnectionStringInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for ConnectionString installer.");
            }

            installer.ConfigFilePath = paramsNode.Attributes["configFile"].Value;
            installer.DatabaseConfiguration = DatabaseConfiguration;

            return installer;
        }

        private WindowsInstaller Initialize(DatabaseInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for Database installer.");
            }

            installer.DatabaseScriptsRootDirectory = paramsNode.Attributes["source"].Value;
            installer.DatabasePatchScriptsPath = paramsNode.Attributes["patches"].Value;
            installer.SkipWorkflowElements = Utility.SafeConvertToBool(Utility.SafeString(paramsNode.Attributes["skipWorkflow"].Value, "false"));
            installer.DatabaseName = paramsNode.Attributes["databaseName"].Value;
            installer.DatabaseConfiguration = DatabaseConfiguration;
            installer.DatabaseConfiguration.DatabaseName = installer.DatabaseName;

            return installer;
        }

        private WindowsInstaller Initialize(FilesystemInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for Filesystem installer.");
            }

            installer.SourceDirectory = paramsNode.Attributes["source"].Value;
            installer.DestinationDirectory = manifestNode.ParentNode.ParentNode.Attributes["InstallFolderName"].Value;      // this value is all the way up in the Component node
            installer.OverwriteExistingFiles = Utility.SafeConvertToBool(Utility.SafeString(paramsNode.Attributes["overwrite"].Value, "true"));
            return installer;
        }

        private WindowsInstaller Initialize(IISInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for ConnectionString installer.");
            }

            installer.WebConfiguration.AppName = paramsNode.Attributes["appName"].Value;
            installer.WebConfiguration.WebsiteName = paramsNode.Attributes["webName"].Value;
            installer.WebConfiguration.BindingPort = Utility.SafeConvertToInt(paramsNode.Attributes["defaultPort"].Value);
            installer.WebConfiguration.WebApplicationFolder = paramsNode.Attributes["appFolder"].Value;
            installer.WebConfiguration.IsSSLBinding = Utility.SafeConvertToBool(Utility.SafeString(paramsNode.Attributes["requireSSL"].Value, "false"));


            return installer;
        }

        private WindowsInstaller Initialize(ProgramShortcutsInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for ProgramShortcuts installer.");
            }

            installer.ShortcutTargetFilePath = paramsNode.Attributes["targetFilePath"].Value;
            installer.ShortcutIconPath = paramsNode.Attributes["icon"].Value;
            installer.LaunchWorkingDirectory = paramsNode.Attributes["workingDirectory"].Value;
            installer.ShortcutTitle = paramsNode.Attributes["title"].Value;
            installer.ShortcutDescription = paramsNode.Attributes["description"].Value;

            Environment.SpecialFolder folder = (Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), Utility.SafeString(paramsNode.Attributes["placeIn"].Value, "Desktop"));
            installer.ShortcutLocationPath = Environment.GetFolderPath(folder);

            return installer;
        }

        private WindowsInstaller Initialize(ToolRunnerInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for ToolRunner installer.");
            }

            if (paramsNode.Attributes["installer"] != null)
            {
                installer.InstallToolName = paramsNode.Attributes["installer"].Value;
                if (paramsNode.Attributes["installArguments"] != null)
                {
                    installer.InstallToolArguments = paramsNode.Attributes["installArguments"].Value;
                }
            }

            if (paramsNode.Attributes["repair"] != null)
            {
                installer.RepairToolName = paramsNode.Attributes["repair"].Value;
                if (paramsNode.Attributes["repairArguments"] != null)
                {
                    installer.RepairToolArguments = paramsNode.Attributes["repairArguments"].Value;
                }
            }

            if (paramsNode.Attributes["uninstaller"] != null)
            {
                installer.UninstallToolName = paramsNode.Attributes["uninstaller"].Value;
                if (paramsNode.Attributes["uninstallArguments"] != null)
                {
                    installer.UninstallToolArguments = paramsNode.Attributes["uninstallArguments"].Value;
                }
            }

            return installer;
        }

        private WindowsInstaller Initialize(WindowsServiceInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for WindowsService installer.");
            }

            installer.ServiceName = paramsNode.Attributes["serviceName"].Value;
            installer.ServiceFilename = paramsNode.Attributes["executableName"].Value;

            return installer;
        }

        private WindowsInstaller Initialize(RegistryInstaller installer, XmlNode manifestNode)
        {
            XmlNodeList actions = manifestNode.SelectNodes("Actions/Action");
            if (actions == null)
            {
                throw new InvalidOperationException("Cannot find the Actions/Action node for Registry installer.");
            }

            foreach(XmlNode action in actions)
            {
                ActionTypeEnum actionType = (ActionTypeEnum)Enum.Parse(typeof(ActionTypeEnum), Utility.SafeString(action.Attributes["type"].Value, "Install"));

                RegistryKeyAction keyAction = new RegistryKeyAction(
                    action.Attributes["keyPath"].Value,
                    action.Attributes["value"].Value,
                    actionType
                )
                {
                    KeyType = (RegistryKeyPathTypeEnum)Enum.Parse(typeof(RegistryKeyPathTypeEnum), Utility.SafeString(action.Attributes["keyType"].Value, "ValueName")),
                    RegistryHiveRoot = (RegistryHive)Enum.Parse(typeof(RegistryHive), Utility.SafeString(action.Attributes["hive"].Value, "LocalMachine")), 
                    RegistryKeyParentPath = action.Attributes["parentKey"].Value
                };

                installer.Actions.Add(keyAction);
            }

            return installer;
        }

        private WindowsInstaller Initialize(NewFarmInstaller installer, XmlNode manifestNode)
        {
            XmlNode paramsNode = manifestNode.SelectSingleNode("parameters");
            if (paramsNode == null)
            {
                throw new InvalidOperationException("Cannot find the parameters node for WindowsService installer.");
            }

            installer.AdminUsername = paramsNode.Attributes["adminUserName"].Value;
            installer.AdminPassword = paramsNode.Attributes["adminPassword"].Value;
            installer.AdminDisplayName = paramsNode.Attributes["adminName"].Value;
            installer.AdminEmailAddress = paramsNode.Attributes["adminEmail"].Value;

            return installer;
        }

        #endregion

        public void ResolvePaths()
        {
            Dictionary<string, string> localEnvironment = new Dictionary<string, string>();
            localEnvironment.Add("$(InstallSource)", CabExtractDirectoryRoot);
            localEnvironment.Add("$(InstallTargetRoot)", InstallBaseDirectory);

            foreach(ComponentInstaller component in Installers)
            {
                component.ResolvePathVariables(localEnvironment);
            }
        }


        #endregion

    }
}
