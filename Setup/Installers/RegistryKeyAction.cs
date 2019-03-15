using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Defines an action to be taken on a registry key
    /// </summary>
    public class RegistryKeyAction
    {

        private static string CorkscrewUninstallGuid = "{99999999-0000-0043-4f52-4b5343524557}";

        #region Properties

        /// <summary>
        /// The registry hive this belongs to
        /// </summary>
        public RegistryHive RegistryHiveRoot
        {
            get;
            set;
        } = RegistryHive.LocalMachine;

        /// <summary>
        /// Parent registry key. This will be opened from Registry Hive Root
        /// </summary>
        public string RegistryKeyParentPath
        {
            get;
            set;
        } = "SOFTWARE\\Aquarius Operating Systems\\CMS";

        /// <summary>
        /// The relative path to the registry key. This will be passed into RegistryKey.OpenSubKey
        /// </summary>
        public string RegistryKeyPath
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Type of key
        /// </summary>
        public RegistryKeyPathTypeEnum KeyType
        {
            get;
            set;
        } = RegistryKeyPathTypeEnum.ValueName;

        /// <summary>
        /// Value of the registry key
        /// </summary>
        public object RegistryKeyValue
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Action to be performed on the registry key
        /// </summary>
        public ActionTypeEnum InstallerAction
        {
            get;
            set;
        } = ActionTypeEnum.Undefined;


        /// <summary>
        /// Undo commands filed by the installer
        /// </summary>
        public Stack<Action> UndoCommands
        {
            get
            {
                if (_undoList == null)
                {
                    _undoList = new Stack<Action>();
                }
                return _undoList;
            }
        }
        private Stack<Action> _undoList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, Blank
        /// </summary>
        public RegistryKeyAction(string keyName, object keyValue, ActionTypeEnum action)
        {
            RegistryKeyPath = keyName;
            RegistryKeyValue = keyValue;
            InstallerAction = action;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform the changes as required by the properties
        /// </summary>
        /// <returns>True if action was performed successfully</returns>
        public bool RunAction()
        {
            RegistryKey approotKey = null;
            switch (RegistryHiveRoot)
            {
                case RegistryHive.ClassesRoot:
                    approotKey = Registry.ClassesRoot;
                    break;

                case RegistryHive.CurrentConfig:
                    approotKey = Registry.CurrentConfig;
                    break;

                case RegistryHive.CurrentUser:
                    approotKey = Registry.CurrentUser;
                    break;

                case RegistryHive.LocalMachine:
                    approotKey = Registry.LocalMachine;
                    break;

                case RegistryHive.PerformanceData:
                    approotKey = Registry.PerformanceData;
                    break;

                case RegistryHive.Users:
                    approotKey = Registry.Users;
                    break;
            }

            if (approotKey == null)
            {
                // unsupported parent key
                return false;
            }

            try
            {
                RegistryKey parentKey = approotKey.OpenSubKey(RegistryKeyParentPath, true);
                if (parentKey == null)
                {
                    parentKey = approotKey.CreateSubKey(RegistryKeyParentPath, true);
                    UndoCommands.Push(
                        () =>
                        {
                            approotKey.DeleteSubKey(RegistryKeyParentPath, false);
                        }
                    );
                }

                switch (InstallerAction)
                {
                    case ActionTypeEnum.Install:
                        switch (KeyType)
                        {
                            case RegistryKeyPathTypeEnum.SubKey:
                                parentKey.CreateSubKey(RegistryKeyPath, true);
                                UndoCommands.Push(
                                    () =>
                                    {
                                        parentKey.DeleteSubKey(RegistryKeyPath, false);
                                    }
                                );
                                break;

                            case RegistryKeyPathTypeEnum.ValueName:
                                parentKey.SetValue(RegistryKeyPath, RegistryKeyValue);
                                UndoCommands.Push(
                                    () =>
                                    {
                                        parentKey.DeleteValue(RegistryKeyPath, false);
                                    }
                                );
                                break;
                        }
                        break;

                    case ActionTypeEnum.Uninstall:
                        switch (KeyType)
                        {
                            case RegistryKeyPathTypeEnum.SubKey:
                                parentKey.DeleteSubKey(RegistryKeyPath, false);
                                break;

                            case RegistryKeyPathTypeEnum.ValueName:
                                parentKey.DeleteValue(RegistryKeyPath, false);
                                break;
                        }
                        break;

                    case ActionTypeEnum.Repair:
                        // only valid for key/value pairs
                        if (KeyType == RegistryKeyPathTypeEnum.ValueName)
                        {
                            parentKey.SetValue(RegistryKeyPath, RegistryKeyValue);
                            UndoCommands.Push(
                                () =>
                                {
                                    parentKey.DeleteValue(RegistryKeyPath, false);
                                }
                            );
                        }
                        break;
                }
            }
            catch
            {
                // some error
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns if the given component is marked installed in the registry
        /// </summary>
        /// <param name="componentName">Name of the component to check</param>
        /// <returns>True if value found</returns>
        public static bool IsComponentInstalled(string componentName)
        {
            RegistryKey installKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\CMS\\InstalledComponents", false);
            if (installKey != null)
            {
                string installed = (string)installKey.GetValue(componentName, "0");
                return (installed == "1");
            }

            return false;
        }

        /// <summary>
        /// Cleans up vestige registry keys if they are no longer required
        /// </summary>
        public static void CleanupRegistryKey()
        {
            RegistryKey parentKey = null;

            RegistryKey installKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\CMS\\InstalledComponents", true);
            if (installKey != null)
            {
                // first delete all the 0-valued components
                foreach (string componentName in installKey.GetValueNames())
                {
                    if (((string)installKey.GetValue(componentName, "0")) == "0")
                    {
                        installKey.DeleteValue(componentName, false);
                    }
                }

                if (installKey.GetValueNames().Length == 0)
                {
                    parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\CMS", true);
                    parentKey.DeleteSubKeyTree("InstalledComponents", false);
                }
            }

            installKey = null;

            if (parentKey == null)
            {
                parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\CMS", true);
            }

            // now recursively go higher and delete keys
            if ((parentKey.GetSubKeyNames().Length == 0) && (parentKey.GetValueNames().Length == 0))
            {
                parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Aquarius Operating Systems", true);
                parentKey.DeleteSubKeyTree("CMS", false);

                if ((parentKey.GetSubKeyNames().Length == 0) && (parentKey.GetValueNames().Length == 0))
                {
                    parentKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                    parentKey.DeleteSubKeyTree("Aquarius Operating Systems", false);
                }
            }

            parentKey = null;

            // delete uninstall info
            RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", true);
            if (uninstallKey != null)
            {
                RegistryKey corkscrewUninstallKey = uninstallKey.OpenSubKey(CorkscrewUninstallGuid);
                if (corkscrewUninstallKey != null)
                {
                    // Windows will not allow us to update the keys. Only option is to delete it and create afresh
                    corkscrewUninstallKey.Close();
                    uninstallKey.DeleteSubKeyTree(CorkscrewUninstallGuid, false);
                }

                uninstallKey.Close();
                uninstallKey = null;
            }
        }

        /// <summary>
        /// Registers with the "Add/Remove Programs" or "Programs and Features" system of Windows for easy uninstallation
        /// </summary>
        public static void RegisterWithWindows(string installRootDirectory)
        {
            RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", true);
            if (uninstallKey != null)
            {
                RegistryKey corkscrewUninstallKey = uninstallKey.OpenSubKey(CorkscrewUninstallGuid);
                if (corkscrewUninstallKey != null)
                {
                    // Windows will not allow us to update the keys. Only option is to delete it and create afresh
                    corkscrewUninstallKey.Close();
                    uninstallKey.DeleteSubKeyTree(CorkscrewUninstallGuid, false);
                }

                corkscrewUninstallKey = uninstallKey.CreateSubKey(CorkscrewUninstallGuid);
                if (corkscrewUninstallKey != null)
                {
                    // copy program to install root
                    string installerCacheFolder = Path.Combine(installRootDirectory, "InstallerCache");
                    string uninstallProgram = Path.Combine(installerCacheFolder, Path.GetFileName(Application.ExecutablePath));
                    long installedEstimateSize = ((long)(GetInstalledSize(installRootDirectory) / 1024L));  // get this before we copy in our cache

                    if (!Directory.Exists(installerCacheFolder))
                    {
                        Directory.CreateDirectory(installerCacheFolder);
                    }

                    // copy required files
                    foreach (string file in Directory.GetFiles(Application.StartupPath, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            File.Copy(file, Path.Combine(installerCacheFolder, Path.GetFileName(file)), true);
                        }
                        catch (IOException)
                        {
                            // file maybe in use. ignore since our copy would be the same.
                        }
                    }

                    corkscrewUninstallKey.SetValue("DisplayName", "Aquarius Corkscrew CMS");
                    corkscrewUninstallKey.SetValue("ApplicationVersion", "1.0.0.0");
                    corkscrewUninstallKey.SetValue("Publisher", "Aquarius Operating Systems");
                    corkscrewUninstallKey.SetValue("DisplayIcon", uninstallProgram);
                    corkscrewUninstallKey.SetValue("DisplayVersion", "1.0");
                    corkscrewUninstallKey.SetValue("URLInfoAbout", "https://corkscrewcms.com");
                    corkscrewUninstallKey.SetValue("Contact", "support@corkscrewcms.com");
                    corkscrewUninstallKey.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                    corkscrewUninstallKey.SetValue("UninstallString", uninstallProgram);

                    corkscrewUninstallKey.SetValue("RegOwner", "Not registered");
                    corkscrewUninstallKey.SetValue("RegCompany", "Not registered");

                    corkscrewUninstallKey.SetValue("HelpLink", "https://support.corkscrewcms.com");
                    corkscrewUninstallKey.SetValue("URLUpdateInfo", "https://support.corkscrewcms.com/updates");
                    corkscrewUninstallKey.SetValue("VersionMajor", 1, RegistryValueKind.DWord);
                    corkscrewUninstallKey.SetValue("VersionMinor", 0, RegistryValueKind.DWord);
                    corkscrewUninstallKey.SetValue("NoModify", 0, RegistryValueKind.DWord);
                    corkscrewUninstallKey.SetValue("NoRepair", 0, RegistryValueKind.DWord);

                    corkscrewUninstallKey.SetValue("InstallLocation", installRootDirectory);
                    corkscrewUninstallKey.SetValue("EstimatedSize", installedEstimateSize, RegistryValueKind.DWord);

                    corkscrewUninstallKey.Close();
                }
            }
        }

        /// <summary>
        /// Returns the previous path of installation
        /// </summary>
        /// <returns>Installation path or NULL</returns>
        public static string GetPreviousInstallationPath()
        {
            RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);
            if (uninstallKey != null)
            {
                uninstallKey = uninstallKey.OpenSubKey(CorkscrewUninstallGuid, false);
                if (uninstallKey != null)
                {
                    return (string)uninstallKey.GetValue("InstallLocation", null);
                }
            }

            return null;
        }

        private static long GetInstalledSize(string directory)
        {
            long localSize = 0;

            DirectoryInfo di = new DirectoryInfo(directory);
            foreach (FileInfo fi in di.GetFiles())
            {
                localSize += fi.Length;
            }

            foreach (DirectoryInfo sdi in di.GetDirectories())
            {
                localSize += GetInstalledSize(sdi.FullName);
            }

            return localSize;
        }

        #endregion

    }
}
