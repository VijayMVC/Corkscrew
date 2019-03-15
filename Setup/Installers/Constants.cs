namespace CMS.Setup.Installers
{
    /// <summary>
    /// Type of action to be performed
    /// </summary>
    public enum ActionTypeEnum
    {

        /// <summary>
        /// Default, undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Create the item
        /// </summary>
        Install,

        /// <summary>
        /// Delete the item
        /// </summary>
        Uninstall,

        /// <summary>
        /// Replace the existing item with the new one
        /// </summary>
        Repair

    }

    /// <summary>
    /// The state after the last execution of the installer
    /// </summary>
    public enum LastActionState
    {
        /// <summary>
        /// Installer has not yet executed
        /// </summary>
        NotExecuted = 0,

        /// <summary>
        /// Installation was successful, component is installed
        /// </summary>
        Installed,

        /// <summary>
        /// Uninstallation was successful, component is uninstalled
        /// </summary>
        Uninstalled,

        /// <summary>
        /// Installation failed. Changes have not been rolled back (pending).
        /// </summary>
        InstallFailed,

        /// <summary>
        /// Uninstallation failed.
        /// </summary>
        UninstallFailed,

        /// <summary>
        /// Installation or repair failed. Changes were rolled back. This state will never 
        /// apply to an uninstall, since an uninstall is never rolled back.
        /// </summary>
        RolledBack
    }


    /// <summary>
    /// In RegistryKeyAction class, the meaning of the RegistryKeyPath property
    /// </summary>
    public enum RegistryKeyPathTypeEnum
    {
        /// <summary>
        /// RegistryKeyPath points to a subkey under the RegistryKeyParentPath
        /// </summary>
        SubKey = 0,

        /// <summary>
        /// RegistryKeyPath is a key/value pair under RegistryKeyParentPath
        /// </summary>
        ValueName
    }

    /// <summary>
    /// Type of item to install into Corkscrew
    /// </summary>
    public enum CorkscrewObjectTypeEnum
    {
        
        Configuration = 0,

        Site,

        User,

        UserGroup,

        Directory,

        File,

        WorkflowDefinition,

        WorkflowAssociation
    }

    /// <summary>
    /// The state of the farm demanded by the component
    /// </summary>
    public enum RequiredFarmStateEnum
    {
        /// <summary>
        /// Any state is acceptable (does not matter during installer action)
        /// </summary>
        Any = 0,

        /// <summary>
        /// A farm should not be present
        /// </summary>
        NoFarm,

        /// <summary>
        /// A clean or freshly installed farm with no data
        /// </summary>
        FreshInstall,

        /// <summary>
        /// A farm that contains sites or files other than the root folder
        /// </summary>
        EstablishedFarm, 

        /// <summary>
        /// A farm must exist
        /// </summary>
        MustExist

    }

}
