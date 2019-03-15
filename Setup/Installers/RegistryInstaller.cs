using System;
using System.Collections.Generic;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Performs installation, uninstallation and repair of Registry keys and data for the Installer
    /// </summary>
    public class RegistryInstaller : WindowsInstaller
    {

        #region Properties

        /// <summary>
        /// The registry keys and values to add, remove or replace
        /// </summary>
        public List<RegistryKeyAction> Actions
        {
            get
            {
                if (_registryActions == null)
                {
                    _registryActions = new List<RegistryKeyAction>();
                }

                return _registryActions;
            }
        }
        private List<RegistryKeyAction> _registryActions = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public RegistryInstaller()
            : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Installs the items to Windows
        /// </summary>
        /// <returns>True if installation succeeded</returns>
        public override bool Install()
        {
            foreach(RegistryKeyAction action in Actions)
            {
                string actionName = Enum.GetName(typeof(ActionTypeEnum), action.InstallerAction);
                actionName = actionName.Substring(0, actionName.Length - 1);

                OnProgressChanged(1, actionName + "ing registry key/value " + action.RegistryKeyParentPath + "\\" + action.RegistryKeyPath + "... ");
                bool actionResult = action.RunAction();
                AppendUndoCommands(action.UndoCommands);

                if (!actionResult)
                {
                    OnProgressChanged(0, "[Failed]");
                    LastStatus = LastActionState.InstallFailed;
                    return false;
                }

                LastStatus = LastActionState.Installed;
                OnProgressChanged(0, "[Success]");
            }

            return true;
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
            foreach (RegistryKeyAction action in Actions)
            {
                OnProgressChanged(1, "Uninstalling Registry key [" + action.RegistryKeyPath + "]... ");

                // clone object and set action to Delete
                RegistryKeyAction undoAction = new RegistryKeyAction(action.RegistryKeyPath, action.RegistryKeyValue, ActionTypeEnum.Uninstall)
                {
                    KeyType = action.KeyType, 
                    RegistryHiveRoot = action.RegistryHiveRoot,
                    RegistryKeyParentPath = action.RegistryKeyParentPath
                };

                if (action.RunAction())
                {
                    LastStatus = LastActionState.Uninstalled;
                    OnProgressChanged(0, "[Success]");
                }
                else
                {
                    LastStatus = LastActionState.UninstallFailed;
                    OnProgressChanged(0, "[Failed]");
                }
                
            }

            return;
        }


        private void AppendUndoCommands(Stack<Action> undoList)
        {
            Action undo = null;
            while ((undoList.Count > 0) && ((undo = undoList.Pop()) != null))
            {
                if (undo != null)
                {
                    UndoCommands.Push(undo);
                }
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
                foreach(RegistryKeyAction action in Actions)
                {
                    if (action.RegistryKeyValue is string)
                    {
                        action.RegistryKeyValue = ((string)action.RegistryKeyValue).Replace(variable, environment[variable]);
                    }
                    
                }
            }
        }

        #endregion

    }
}
