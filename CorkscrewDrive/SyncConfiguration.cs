using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Corkscrew.Drive
{
    internal class SyncConfiguration
    {

        #region Properties
        public string CorkscrewUsername { get; set; }
        public string CorkscrewPasswordHash { get; set; }

        public CSSite TargetSite
        {
            get;
            set;
        }

        public CSFileSystemEntryDirectory TargetDirectory
        {
            get;
            set;
        }
        public bool IsConnected
        {
            get
            {
                if (string.IsNullOrEmpty(CorkscrewUsername) || string.IsNullOrEmpty(CorkscrewPasswordHash) || (TargetSite == null) || (TargetDirectory == null))
                {
                    return false;
                }

                return true;
            }
        }

        public string SourceDirectory
        {
            get;
            set;
        }

        public List<string> Exclusions
        {
            get
            {
                if (_exclusions == null)
                {
                    _exclusions = new List<string>();
                }
                return _exclusions;
            }
        }
        private List<string> _exclusions = null;

        public bool DownloadToLocalIfPresentRemotely { get; set; }
        public bool DeleteFromRemoteWhenDeletedLocally { get; set; }
        public bool DeleteFromLocalWhenDeletedRemotely { get; set; }
        public bool DeleteSyncedFoldersFromLocalOnAccountDisconnect { get; set; }
        public bool SyncOnlyWhenIdle { get; set; }

        public TimeSpan IdleDuration { get; set; }
        #endregion

        public SyncConfiguration()
        {
            RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\Corkscrew\\Drive", true);
            if (settingsKey == null)
            {
                settingsKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Aquarius Operating Systems\\Corkscrew\\Drive", true);

                if (settingsKey == null)
                {
                    throw new Exception("Unable to create or open required registry keys. Cannot configure Corkscrew Drive.");
                }
            }

            CorkscrewUsername = (string)settingsKey.GetValue("ConnectedAccountName", null);
            CorkscrewPasswordHash = (string)settingsKey.GetValue("ConnectedAccountHash", null);

            CSUser validatedUser = CSUser.Login(CorkscrewUsername, CorkscrewPasswordHash);

            // rest of this only makes sense if the login succeeded.
            if (validatedUser != null)
            {
                CSFarm farm = CSFarm.Open(validatedUser);
                Guid targetSiteId = Utility.SafeConvertToGuid(settingsKey.GetValue("DestinationSite", Guid.Empty));
                TargetSite = ((farm == null) ? null : farm.AllSites.Find(targetSiteId));
                if (TargetSite != null)
                {
                    string targetDirectoryPath = (string)settingsKey.GetValue("DestinationDirectory", TargetSite.RootFolder.FullPath);
                    TargetDirectory = TargetSite.GetDirectory(targetDirectoryPath);
                }

                SourceDirectory = (string)settingsKey.GetValue("SourceDirectory", string.Empty);

                string[] array = (string[])settingsKey.GetValue("ExcludedItems", new string[] { });
                foreach (string item in array)
                {
                    if ((!string.IsNullOrEmpty(item)) && (!Exclusions.Contains(item)))
                    {
                        Exclusions.Add(item);
                    }
                }

                DownloadToLocalIfPresentRemotely = Utility.SafeConvertToBool(settingsKey.GetValue("DownloadToLocalIfPresentRemotely", "1").ToString().Replace("1", "true").Replace("0", "false"));
                DeleteFromRemoteWhenDeletedLocally = Utility.SafeConvertToBool(settingsKey.GetValue("DeleteFromRemoteWhenDeletedLocally", "0").ToString().Replace("1", "true").Replace("0", "false"));   // opt to retain on cloud
                DeleteFromLocalWhenDeletedRemotely = Utility.SafeConvertToBool(settingsKey.GetValue("DeleteFromLocalWhenDeletedRemotely", "1").ToString().Replace("1", "true").Replace("0", "false"));
                DeleteSyncedFoldersFromLocalOnAccountDisconnect = Utility.SafeConvertToBool(settingsKey.GetValue("DeleteSyncedFoldersFromLocalOnAccountDisconnect", "1").ToString().Replace("1", "true").Replace("0", "false"));  // privacy++
                SyncOnlyWhenIdle = Utility.SafeConvertToBool(settingsKey.GetValue("SyncOnlyWhenIdle", "0").ToString().Replace("1", "true").Replace("0", "false"));   // instant sync
                if (SyncOnlyWhenIdle)
                {
                    IdleDuration = new TimeSpan(0, 0, 0, 0, (int)settingsKey.GetValue("IdleDurationMilliseconds", 300000));    // 5 minutes
                }
                else
                {
                    IdleDuration = TimeSpan.Zero;
                }
            }

            settingsKey.Close();
        }

        public bool Save()
        {
            RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\Corkscrew\\Drive", true);

            try
            {
                settingsKey.SetValue("ConnectedAccountName", CorkscrewUsername, RegistryValueKind.String);
                settingsKey.SetValue("ConnectedAccountHash", CorkscrewPasswordHash, RegistryValueKind.String);
                settingsKey.SetValue("DestinationSite", ((TargetSite == null) ? Guid.Empty.ToString("d") : TargetSite.Id.ToString("d")), RegistryValueKind.String);
                settingsKey.SetValue("DestinationDirectory", ((TargetDirectory == null) ? TargetSite.RootFolder.FullPath : TargetDirectory.FullPath), RegistryValueKind.String);
                settingsKey.SetValue("SourceDirectory", SourceDirectory, RegistryValueKind.String);
                settingsKey.SetValue("ExcludedItems", Exclusions.ToArray(), RegistryValueKind.MultiString);
                settingsKey.SetValue("DownloadToLocalIfPresentRemotely", DownloadToLocalIfPresentRemotely, RegistryValueKind.DWord);
                settingsKey.SetValue("DeleteFromRemoteWhenDeletedLocally", DeleteFromRemoteWhenDeletedLocally, RegistryValueKind.DWord);
                settingsKey.SetValue("DeleteFromLocalWhenDeletedRemotely", DeleteFromLocalWhenDeletedRemotely, RegistryValueKind.DWord);
                settingsKey.SetValue("DeleteSyncedFoldersFromLocalOnAccountDisconnect", DeleteSyncedFoldersFromLocalOnAccountDisconnect, RegistryValueKind.DWord);
                settingsKey.SetValue("SyncOnlyWhenIdle", SyncOnlyWhenIdle, RegistryValueKind.DWord);

                if (SyncOnlyWhenIdle)
                {
                    settingsKey.SetValue("IdleDurationMilliseconds", IdleDuration.Milliseconds, RegistryValueKind.DWord);
                }
                else
                {
                    settingsKey.DeleteValue("IdleDurationMilliseconds", false);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                settingsKey.Close();
            }

            return true;
        }

        public bool ClearSettings()
        {
            RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\Corkscrew\\Drive", true);
            try
            {
                foreach (string value in settingsKey.GetValueNames())
                {
                    settingsKey.DeleteValue(value, false);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                settingsKey.Close();
            }

            return true;
        }

    }
}
