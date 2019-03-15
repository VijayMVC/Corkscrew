using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Installs files and directories into the computer's disk file system
    /// </summary>
    public class FilesystemInstaller : WindowsInstaller
    {
        

        #region Properties

        /// <summary>
        /// The directory that contains the source files for this installer
        /// </summary>
        public string SourceDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// The directory into which the files and directories are to be installed
        /// </summary>
        public string DestinationDirectory
        {
            get;
            set;
        }

        public string UndoDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_undoDirectory))
                {
                    _undoDirectory = Path.Combine(Path.GetTempPath(), "Filesystem", Guid.NewGuid().ToString("n")) ;
                    if (! Directory.Exists(_undoDirectory))
                    {
                        Directory.CreateDirectory(_undoDirectory);
                    }
                }
                return _undoDirectory;
            }
        }
        private string _undoDirectory = null;

        /// <summary>
        /// If set, existing files and directories will be overwritten without prompting. Otherwise they are not overwritten.
        /// </summary>
        public bool OverwriteExistingFiles
        {
            get;
            set;
        } = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, blank
        /// </summary>
        public FilesystemInstaller(string source, string destination, bool overwrite = true)
            : base()
        {
            SourceDirectory = source;
            DestinationDirectory = destination;
            OverwriteExistingFiles = overwrite;
        }

        #endregion

        #region Methods

        private bool ValidateDirectories()
        {

            if (!Directory.Exists(SourceDirectory))
            {
                return false;
            }

            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            return true;
        }

        /// <summary>
        /// Installs the filesystem
        /// </summary>
        /// <returns>True if installation succeeded</returns>
        public override bool Install()
        {
            if (ValidateDirectories())
            {
                if (CopyFolderTree(SourceDirectory, DestinationDirectory, UndoDirectory))
                {
                    LastStatus = LastActionState.Installed;
                    return true;
                }
            }

            LastStatus = LastActionState.InstallFailed;
            return false;
        }

        /// <summary>
        /// Repairs the filesystem
        /// </summary>
        /// <returns>True if repair succeeded</returns>
        public override bool Repair()
        {
            return Install();
        }

        /// <summary>
        /// Uninstalls the filesystem
        /// </summary>
        public override void Uninstall()
        {
            if (Directory.Exists(DestinationDirectory))
            {
                OnProgressChanged(1, "Deleting [" + DestinationDirectory + "] directory...");
                try
                {
                    TakeBackupOfFolder(DestinationDirectory, UndoDirectory);
                    Directory.Delete(DestinationDirectory, true);
                    Directory.Delete(UndoDirectory, true);

                    LastStatus = LastActionState.Uninstalled;
                    OnProgressChanged(0, "[Success]");
                }
                catch (Exception ex)
                {
                    TakeBackupOfFolder(UndoDirectory, DestinationDirectory);        // restore
                    LastStatus = LastActionState.UninstallFailed;
                    OnProgressChanged(0, "[Failed]: " + ex.ToString());
                }
            }
            else
            {
                LastStatus = LastActionState.Uninstalled;
            }
        }

        private bool TakeBackupOfFolder(string source, string backupFolder)
        {
            if (! Directory.Exists(source))
            {
                return false;
            }

            if (! Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            try
            {
                foreach (string path in Directory.GetFiles(source, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string destinationPath = Path.Combine(backupFolder, Path.GetFileName(path));
                    File.Copy(path, destinationPath);
                }

                foreach (string path in Directory.GetDirectories(source, "*.*", SearchOption.TopDirectoryOnly))
                {
                    string destinationPath = Path.Combine(backupFolder, Path.GetFileName(path));
                    if (!TakeBackupOfFolder(path, destinationPath))
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;

        }

        private bool CopyFolderTree(string source, string destination, string undoDestination)
        {
            if (! Directory.Exists(source))
            {
                return false;
            }

            if (!CreateDirectoryIfNotExist(destination))
            {
                return false;
            }

            foreach(string path in Directory.GetFiles(source, "*.*", SearchOption.TopDirectoryOnly))
            {
                string destinationPath = Path.Combine(destination, Path.GetFileName(path));
                if (!CopyFileWithUndo(path, destinationPath, undoDestination))
                {
                    return false;
                }
            }

            foreach(string path in Directory.GetDirectories(source, "*.*", SearchOption.TopDirectoryOnly))
            {
                string destinationPath = Path.Combine(destination, Path.GetFileName(path));
                if (!CopyFolderTree(path, destinationPath, ((undoDestination != null) ? Path.Combine(undoDestination, Path.GetFileName(path)) : null)))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CopyFileWithUndo(string source, string destination, string undoDestination)
        {
            if (! File.Exists(source))
            {
                return false;
            }

            if (! Directory.Exists(undoDestination))
            {
                Directory.CreateDirectory(undoDestination);
            }

            OnProgressChanged(1, "Copying file [" + destination + "]...");
            try
            {
                if ((!File.Exists(destination)) || (undoDestination == null))
                {
                    File.Copy(source, destination, false);
                    UndoCommands.Push(
                        () =>
                        {
                            File.Delete(destination);
                        }
                    );
                }
                else if ((File.Exists(destination)) && (undoDestination != null))
                {
                    string tempFileName = Path.Combine(undoDestination, Path.GetFileName(destination));
                    File.Copy(destination, tempFileName, true);

                    try
                    {
                        File.Delete(destination);
                        File.Copy(source, destination, true);
                    }
                    catch
                    {
                        // failed to copy or delete, restore file
                        try
                        {
                            File.Copy(tempFileName, destination, true);

                            UndoCommands.Push(
                                () =>
                                {
                                    File.Copy(tempFileName, destination);
                                    File.Delete(tempFileName);
                                }
                            );
                        }
                        catch (Exception ex1)
                        {
                            // huh?
                            OnProgressChanged(0, "[Failed]: " + ex1.Message);
                            return false;
                        }
                    }
                }

                OnProgressChanged(0, "[Success]");
            }
            catch (Exception ex)
            {
                OnProgressChanged(0, "[Failed]: " + ex.Message);
                return false;
            }

            return true;
        }

        private bool CreateDirectoryIfNotExist(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    OnProgressChanged(1, "Creating directory [" + directory + "]...");
                    Directory.CreateDirectory(directory);
                    UndoCommands.Push(
                        () =>
                        {
                            Directory.Delete(directory, true);
                        }
                    );

                    OnProgressChanged(1, "[Success]");
                }
            }
            catch (Exception ex)
            {
                OnProgressChanged(0, "[Failed]: " + ex.Message);
                return false;
            }

            return true;
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
                SourceDirectory = SourceDirectory.Replace(variable, environment[variable]);
                DestinationDirectory = DestinationDirectory.Replace(variable, environment[variable]);
            }

            // check if source & dest point correctly
            if ((! Path.IsPathRooted(SourceDirectory)) && (! SourceDirectory.StartsWith(environment["$(ComponentSource)"])))
            {
                SourceDirectory = Path.Combine(environment["$(ComponentSource)"], SourceDirectory);
            }

            if (!Path.IsPathRooted(DestinationDirectory))
            {
                string properInstallDestination = Path.Combine(environment["$(InstallTargetRoot)"], environment["$(ComponentDestination)"]);
                if (!DestinationDirectory.Equals(properInstallDestination, StringComparison.InvariantCultureIgnoreCase))
                {
                    DestinationDirectory = Path.Combine(environment["$(InstallTargetRoot)"], environment["$(ComponentDestination)"]);
                }
            }

        }

        #endregion

    }
}
