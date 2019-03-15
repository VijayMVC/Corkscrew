using Corkscrew.SDK.tools;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Installs, uninstalls and repairs program shortcuts
    /// </summary>
    public class ProgramShortcutsInstaller : WindowsInstaller
    {

        #region Properties

        /// <summary>
        /// Full path to the file that is the target of this shortcut
        /// </summary>
        public string ShortcutTargetFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the shortcut (as visible on the desktop or the programs folder)
        /// </summary>
        public string ShortcutTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Location (folder path) where the shortcut is to be placed
        /// </summary>
        public string ShortcutLocationPath
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the fullpath and filename of the LNK file to be created
        /// </summary>
        private string LnkFilename
        {
            get
            {
                return Path.Combine(ShortcutLocationPath, ShortcutTitle + ".lnk");
            }
        }

        /// <summary>
        /// Description of shortcut - visible in tooltips, etc
        /// </summary>
        public string ShortcutDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Fullpath to the file that contains the icon to the shortcut. Can terminate with ",#" where # is the index of the image to be used.
        /// </summary>
        public string ShortcutIconPath
        {
            get;
            set;
        }

        /// <summary>
        /// The working directory the shortcut is launched with
        /// </summary>
        public string LaunchWorkingDirectory
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, blank
        /// </summary>
        public ProgramShortcutsInstaller() : base() { }

        #endregion

        #region Methods

        /// <summary>
        /// Installs shortcut
        /// </summary>
        /// <returns>True if succeeded</returns>
        public override bool Install()
        {
            if (System.IO.File.Exists(LnkFilename))
            {
                OnProgressChanged(1, "Deleting old link file...");
                System.IO.File.Delete(LnkFilename);
                OnProgressChanged(0, "[Done]");
            }

            WshShell scriptingShell = null;
            IWshShortcut lnk = null;

            try
            {
                OnProgressChanged(1, "Creating shortcut to [" + ShortcutTargetFilePath + "] at [" + LnkFilename + "]... ");

                string icon = ShortcutIconPath;
                if (! icon.Contains(","))
                {
                    icon = icon + ",0";
                }

                scriptingShell = new WshShell();
                lnk = (IWshShortcut)scriptingShell.CreateShortcut(LnkFilename);
                lnk.TargetPath = ShortcutTargetFilePath;
                lnk.IconLocation = icon;
                lnk.WorkingDirectory = LaunchWorkingDirectory;
                lnk.Description = ("Shortcut to launch the Corkscrew app " + Path.GetFileNameWithoutExtension(ShortcutTargetFilePath));
                lnk.Save();

                UndoCommands.Push(
                    () =>
                    {
                        System.IO.File.Delete(LnkFilename);
                    }
                );
                OnProgressChanged(0, "[Success]");
            }
            catch (Exception ex)
            {
                OnProgressChanged(0, "[Failed]: " + ex.Message);
                LastStatus = LastActionState.InstallFailed;
                return false;
            }
            finally
            {
                lnk = null;
                scriptingShell = null;
            }

            LastStatus = LastActionState.Installed;
            return true;
        }

        /// <summary>
        /// Repairs shortcut
        /// </summary>
        /// <returns>True if succeeded</returns>
        public override bool Repair()
        {
            return Install();
        }

        /// <summary>
        /// Deletes shortcut
        /// </summary>
        public override void Uninstall()
        {
            // that's all there is to it!
            if (System.IO.File.Exists(LnkFilename))
            {
                OnProgressChanged(1, "Deleting shortcut file at " + LnkFilename);
                try
                {
                    System.IO.File.Delete(LnkFilename);
                    OnProgressChanged(0, "[Success]");
                }
                catch (Exception ex)
                {
                    LastStatus = LastActionState.UninstallFailed;
                    OnProgressChanged(0, "[Failed]: " + ex.Message);
                }
            }
            else
            {
                LastStatus = LastActionState.Uninstalled;
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
                ShortcutIconPath = ShortcutIconPath.Replace(variable, environment[variable]);
                ShortcutTargetFilePath = ShortcutTargetFilePath.Replace(variable, environment[variable]);
                LaunchWorkingDirectory = LaunchWorkingDirectory.Replace(variable, environment[variable]);
            }
        }

        #endregion

    }
}
