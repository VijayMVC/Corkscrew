using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System.Collections.Generic;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Installs a new farm
    /// </summary>
    public class NewFarmInstaller : WindowsInstaller
    {

        #region Properties

        /// <summary>
        /// First farm-administrator username
        /// </summary>
        public string AdminUsername { get; set; }

        /// <summary>
        /// First farm-administrator password
        /// </summary>
        public string AdminPassword { get; set; }

        /// <summary>
        /// First farm-administrator display name
        /// </summary>
        public string AdminDisplayName { get; set; }

        /// <summary>
        /// First farm-administrator email address
        /// </summary>
        public string AdminEmailAddress { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NewFarmInstaller() { }

        #endregion

        #region Methods

        public override bool Install()
        {
            CSUser admin = CSUser.GetByUsername(AdminUsername);
            if (admin == null)
            {
                admin = CSUser.CreateUser(AdminUsername, AdminDisplayName, AdminPassword, AdminEmailAddress);
                if (admin != null)
                {
                    UndoCommands.Push(
                        () =>
                        {
                            CSUser undoAdmin = CSUser.GetByUsername(AdminUsername);
                            undoAdmin.Delete();
                        }
                    );
                }
            }
            
            if (admin != null)
            {
                CSPermission adminAcl = CSPermission.TestAccess(CSPath.CmsPathPrefix, admin);
                if (!adminAcl.CanFullControl)
                {
                    adminAcl.CanFullControl = true;
                    adminAcl.Save();
                }

                UndoCommands.Push(
                    () =>
                    {
                        CSPermission undoAdminAcl = CSPermission.TestAccess(CSPath.CmsPathPrefix, admin);
                        if (undoAdminAcl.CanFullControl)
                        {
                            undoAdminAcl.CanFullControl = false;
                            undoAdminAcl.Save();
                        }
                    }
                );

                LastStatus = LastActionState.Installed;
                return true;
            }

            LastStatus = LastActionState.InstallFailed;
            return false;
        }

        public override bool Repair()
        {
            return Install();
        }

        public override void Uninstall()
        {
            CSUser admin = CSUser.GetByUsername(AdminUsername);
            if (admin != null)
            {
                admin.Delete();     // this will also drop ACLs
            }

            LastStatus = LastActionState.Uninstalled;
        }

        public override void ResolvePathProperties(Dictionary<string, string> environment)
        {
            // nothing to do (no paths)
        }

        #endregion

    }
}
