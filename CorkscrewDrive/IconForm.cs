using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Corkscrew.Drive
{
    public partial class IconForm : Form
    {

        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref WIN32LASTINPUTINFO pLastInputInfo);

        public struct WIN32LASTINPUTINFO
        {
            public uint cbSize;
            public Int32 dwTime;
        }

        private SyncStatusEnum currentSyncStatus = SyncStatusEnum.NotStarted;
        private SyncConfiguration config = null;
        private System.Timers.Timer FileMonitorTimer = null;
        private FileSystemWatcher FolderWatcher = null;
        private Queue<string> SyncQueue = null;
        private CSFileSystemEntryDirectory CorkscrewTargetFolder = null;
        private object syncLock = new object();


        public IconForm()
        {
            InitializeComponent();
        }

        private void SetSyncStatus(SyncStatusEnum status)
        {
            Log("Sync status will be set to " + Enum.GetName(typeof(SyncStatusEnum), status));

            switch (status)
            {
                case SyncStatusEnum.NotStarted:
                    tsmiPauseSync.Enabled = false;
                    tsmiResumeSync.Enabled = false;

                    if (FileMonitorTimer != null)
                    {
                        FileMonitorTimer = null;
                    }
                    break;

                case SyncStatusEnum.Paused:
                    tsmiPauseSync.Enabled = false;
                    tsmiResumeSync.Enabled = true;

                    FileMonitorTimer = null;
                    break;

                case SyncStatusEnum.Running:
                    if ((! string.IsNullOrEmpty(config.SourceDirectory)) && (Directory.Exists(config.SourceDirectory)))
                    {
                        // find and queue files changed since last check
                        DateTime lastSync = GetPersistedLastAlive();
                        SyncQueue = new Queue<string>();

                        CorkscrewTargetFolder = config.TargetDirectory;
                        if (CorkscrewTargetFolder == null)
                        {
                            Program.ShowMessage("Fatal error! Target directory is not set.");
                            return;
                        }

                        tsmiPauseSync.Enabled = true;
                        tsmiResumeSync.Enabled = false;

                        foreach (FileSystemInfo item in (new DirectoryInfo(config.SourceDirectory)).EnumerateFileSystemInfos("*.*", SearchOption.AllDirectories).Where(f => f.LastWriteTime > lastSync).OrderBy(f => f.FullName))
                        {
                            QueueItem(config.SourceDirectory, item.FullName);
                        }

                        FolderWatcher = new FileSystemWatcher(config.SourceDirectory)
                        {
                            Filter = "*.*",
                            IncludeSubdirectories = true,
                            NotifyFilter = NotifyFilters.LastWrite
                        };

                        FolderWatcher.Renamed += Watcher_Renamed;
                        FolderWatcher.Deleted += Watcher_Deleted;
                        FolderWatcher.Created += Watcher_Created;
                        FolderWatcher.Changed += Watcher_Changed;
                        FolderWatcher.Error += Watcher_Error;

                        FolderWatcher.EnableRaisingEvents = true;
                        Log("Enabled file watcher for " + config.SourceDirectory);

                        FileMonitorTimer = new System.Timers.Timer(15000);  // every 15 seconds
                        FileMonitorTimer.Enabled = true;
                        FileMonitorTimer.Elapsed += FileMonitorTimer_Elapsed;
                    }
                    else
                    {
                        tsmiPauseSync.Enabled = false;
                        tsmiResumeSync.Enabled = true;
                        FileMonitorTimer = null;
                        status = SyncStatusEnum.Paused;
                    }
                    break;
            }

            currentSyncStatus = status;
            NotificationAreaIcon.ShowBalloonTip(100, "Corkscrew Drive", "Sync status changed to: " + Enum.GetName(typeof(SyncStatusEnum), status), ToolTipIcon.Info);
        }

        #region Watcher events

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            NotificationAreaIcon.ShowBalloonTip(100, "Corkscrew Drive", "Error occured: " + e.GetException().Message, ToolTipIcon.Error);

            FileSystemWatcher watcher = (FileSystemWatcher)sender;
            if (e.GetException().Message.Contains("overflow"))
            {
                if (watcher.InternalBufferSize < (int.MaxValue / 2))
                {
                    watcher.InternalBufferSize = watcher.InternalBufferSize * 2;
                }
                else
                {
                    watcher.EnableRaisingEvents = false;        // disable this watcher

                    string message = "An internal error occured, unable to sync folder " + watcher.Path + ". Restarting this app may fix the problem. If problem persists, please contact Tech Support.";
                    Log(message);
                    NotificationAreaIcon.ShowBalloonTip(3000, "Corkscrew Drive", "Fatal error! Restart the Drive app.", ToolTipIcon.Error);
                }
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Log("Item " + Enum.GetName(typeof(WatcherChangeTypes), e.ChangeType) + " : " + e.FullPath);
            QueueItem(((FileSystemWatcher)sender).Path, e.FullPath);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Log("Item created : " + e.FullPath);
            QueueItem(((FileSystemWatcher)sender).Path, e.FullPath);
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Log("Item deleted : " + e.FullPath);
            QueueItem(((FileSystemWatcher)sender).Path, e.FullPath);
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Log("Item renamed : " + e.FullPath);

            string basePath = ((FileSystemWatcher)sender).Path;
            QueueItem(basePath, e.OldFullPath);     // we need to delete this
            QueueItem(basePath, e.FullPath);        // we need to add this
        }

        #endregion

        private void QueueItem(string basePath, string fullPath)
        {
            string keyPath = string.Format("{0}#{1}", basePath, Utility.SafeString(fullPath.Replace(basePath, ""), removeAtStart: Path.DirectorySeparatorChar.ToString(), removeAtEnd: Path.DirectorySeparatorChar.ToString()));

            Regex regExMatcher = null;
            bool excludeItem = false;
            foreach (string pattern in config.Exclusions)
            {
                regExMatcher = new Regex(pattern);
                if (regExMatcher.Match(fullPath).Success)
                {
                    excludeItem = true;
                    break;
                }
            }

            if (excludeItem)
            {
                return;
            }

            lock (syncLock)
            {
                if (!SyncQueue.ContainsNoCase(keyPath))
                {
                    SyncQueue.Enqueue(keyPath);
                }
            }
        }

        private void PulldownItemsFromRemote(CSFileSystemEntryDirectory remoteDirectory)
        {
            Regex regExMatcher;

            foreach (CSFileSystemEntry item in remoteDirectory.Items)
            {
                bool excludeItem = false;
                foreach (string pattern in config.Exclusions)
                {
                    regExMatcher = new Regex(pattern);
                    if (regExMatcher.Match(item.FullPath).Success)
                    {
                        excludeItem = true;
                        break;
                    }
                }

                if (excludeItem)
                {
                    continue;
                }

                string remoteRelativePath = Utility.SafeString(item.FullPath.Replace(config.TargetDirectory.FullPath, ""), removeAtStart: "/", removeAtEnd: "/").Replace("/", "\\");
                string localFullPath = Path.Combine(config.SourceDirectory, remoteRelativePath);

                if (config.DeleteFromLocalWhenDeletedRemotely)
                {
                    if (
                            (item.IsFolder && (!File.Exists(localFullPath)))
                         || ((!item.IsFolder) && (!Directory.Exists(localFullPath)))
                        )
                    {
                        item.Delete();
                        Log("Remote item " + item.FullPath + " deleted as it was deleted from local");
                        continue;
                    }
                }

                Log("Remote item " + item.FullPath + " will be synced as " + localFullPath);

                switch (item.IsFolder)
                {
                    case true:
                        try
                        {
                            FolderWatcher.EnableRaisingEvents = false;
                            if (!Directory.Exists(localFullPath))
                            {
                                Directory.CreateDirectory(localFullPath);
                            }
                        }
                        catch { }
                        finally
                        {
                            FolderWatcher.EnableRaisingEvents = true;
                        }

                        PulldownItemsFromRemote(new CSFileSystemEntryDirectory(item));      // recurse
                        break;

                    case false:
                        using (CSFileSystemEntryFile itemFile = new CSFileSystemEntryFile(item))
                        {
                            if (itemFile.Open(FileAccess.Read))
                            {
                                byte[] buffer = new byte[itemFile.Size];
                                int bytes = itemFile.Read(buffer, 0, itemFile.Size);
                                if (bytes > 0)
                                {
                                    try
                                    {
                                        FolderWatcher.EnableRaisingEvents = false;
                                        using (Stream sw = File.Create(localFullPath))
                                        {
                                            sw.Write(buffer, 0, bytes);
                                            sw.Flush();
                                        }
                                    }
                                    catch { }
                                    finally
                                    {
                                        FolderWatcher.EnableRaisingEvents = true;
                                    }
                                }

                                itemFile.Close();
                            }
                        }
                        break;
                }
            }
        }

        private void FileMonitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (config.SyncOnlyWhenIdle)
            {
                WIN32LASTINPUTINFO info = new WIN32LASTINPUTINFO();
                info.cbSize = (uint)Marshal.SizeOf(info);
                info.dwTime = 0;

                if (GetLastInputInfo(ref info))
                {
                    if ((new TimeSpan(System.Environment.TickCount - info.dwTime)).Milliseconds < config.IdleDuration.TotalMilliseconds)
                    {
                        return;
                    }
                }
            }

            FileMonitorTimer.Enabled = false;

            PulldownItemsFromRemote(config.TargetDirectory);

            while (SyncQueue.Count > 0)
            {
                string sourceItem = SyncQueue.Dequeue();

                // sourceItem contains path that is -- "<RootDirectory>#<ItemRelativePath>"
                string[] sourceSplit = sourceItem.Split(new char[] { '#' });
                string sourceItemBasePath = sourceSplit[0];
                string sourceItemRelativePath = sourceSplit[1];
                string sourceItemFullPath = Path.Combine(sourceItemBasePath, sourceItemRelativePath);

                // calculate the remote path to the item, items will be rooted to the targetdir, and relative to the sync source
                string remoteItemPath = CSPath.Combine(config.TargetDirectory.FullPath, sourceItemRelativePath);
                CSFileSystemEntry remoteItem = config.TargetSite.GetFileSystemItem(remoteItemPath);

                FileSystemInfo ntfsInfo = null;

                if (Directory.Exists(sourceItemFullPath) || File.Exists(sourceItemFullPath))
                {
                    bool isDirectory = false;

                    ntfsInfo = new FileInfo(sourceItemFullPath);
                    isDirectory = ntfsInfo.Attributes.HasFlag(FileAttributes.Directory);

                    if (remoteItem == null)
                    {
                        // local item is new

                        if (isDirectory)
                        {
                            remoteItem = config.TargetSite.RootFolder.CreateDirectoryTree(remoteItemPath);
                            if (remoteItem != null)
                            {
                                Log("Created directory at: " + remoteItemPath);
                            }
                        }
                        else
                        {
                            CSFileSystemEntryDirectory parentFolder = config.TargetSite.RootFolder.CreateDirectoryTree(CSPath.Combine(config.TargetDirectory.FullPath, Path.GetDirectoryName(sourceItemRelativePath).Replace("\\", "/")));
                            string fileName = Path.GetFileNameWithoutExtension(sourceItemRelativePath), filenameExtension = Path.GetExtension(sourceItemRelativePath);
                            remoteItem = parentFolder.CreateFile(fileName, filenameExtension, File.ReadAllBytes(sourceItemFullPath));
                            if (remoteItem != null)
                            {
                                Log("Created file at: " + remoteItemPath);
                            }
                        }
                    }
                    else
                    {
                        // remote item exists 
                        // since we found the remote item by name, obviously something else changed... aka. content. 
                        // ignore "directory" changes 
                        if ((!isDirectory) && (!remoteItem.IsFolder))
                        {
                            CSFileSystemEntryFile remoteFile = new CSFileSystemEntryFile(remoteItem);
                            if (remoteFile.Open(FileAccess.Write))
                            {
                                byte[] contentStream = File.ReadAllBytes(sourceItemFullPath);
                                remoteFile.Write(contentStream, 0, contentStream.Length);

                                remoteFile.Close();

                                remoteFile = config.TargetSite.GetFile(remoteItem.FullPath);
                                Log("Updated remote file: (" + remoteFile.SizeHuman + ")" + remoteItemPath);
                            }
                        }
                    }
                }
                else
                {
                    // item does not exist
                    if (config.DeleteFromRemoteWhenDeletedLocally)
                    {
                        if (remoteItem != null)
                        {
                            Log(sourceItemFullPath + " deleted from " + remoteItemPath);
                            remoteItem.Delete();
                        }
                    }
                }

                remoteItem = null;
            }

            SetPersistedLastAlive();

            FileMonitorTimer.Enabled = true;
        }


        #region Toolbar events
        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "Version : " + Application.ProductVersion,
                "Corkscrew Drive - About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmiPauseSync_Click(object sender, EventArgs e)
        {
            SetSyncStatus(SyncStatusEnum.Paused);
        }

        private void tsmiResumeSync_Click(object sender, EventArgs e)
        {
            SetSyncStatus(SyncStatusEnum.Running);
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            using (SyncSettings frm = new SyncSettings())
            {
                frm.ShowDialog();
            }

            LoadSettings();
        }
        #endregion

        private void LoadSettings()
        {
            if (currentSyncStatus == SyncStatusEnum.Running)
            {
                SetSyncStatus(SyncStatusEnum.Paused);
            }
            else
            {
                SetSyncStatus(SyncStatusEnum.NotStarted);
            }

            Log("Reloading configuration from registry...");
            config = new SyncConfiguration();
            if (config.IsConnected)
            {
                Log("Can run sync, enabling...");
                SetSyncStatus(SyncStatusEnum.Running);
            }
        }

        private void IconForm_Shown(object sender, EventArgs e)
        {
            tbLog.Text = "Started";
            LoadSettings();

            NotificationAreaIcon.Visible = true;
            NotificationAreaIcon.DoubleClick += NotificationAreaIcon_DoubleClick;
            this.Hide();

            NotificationAreaIcon.ShowBalloonTip(100, "Corkscrew Drive", "Corkscrew drive has started.", ToolTipIcon.Info);
        }

        private void NotificationAreaIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Log(string text)
        {
            // dont bother logging if we are not visible.
            if (this.Visible)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(Log), new object[] { text });
                    return;
                }

                tbLog.Text += Environment.NewLine + text;
            }
        }

        private DateTime GetPersistedLastAlive()
        {
            DateTime result = DateTime.MinValue;
            RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\Corkscrew\\Drive", true);
            if (settingsKey != null)
            {
                result = Utility.SafeConvertToDateTime(settingsKey.GetValue("LastAlive", DateTime.MinValue));
            }
            settingsKey.Close();

            return result;
        }

        private void SetPersistedLastAlive()
        {
            RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Aquarius Operating Systems\\Corkscrew\\Drive", true);
            if (settingsKey != null)
            {
                settingsKey.SetValue("LastAlive", DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss"), RegistryValueKind.String);
            }
            settingsKey.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            Program.EnableBorderlessFormMove(this.Handle, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
