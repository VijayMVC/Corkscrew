using Corkscrew.SDK.exceptions;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.workflow;
using System;

namespace Corkscrew.Workflows.InstantFileBackupWorkflow
{
    public class ArchivalWorkflow : CSWorkflow
    {


        private string ArchivalSiteName = "ArchivalWorkflow_ArchiveSite";
        private CSSite _archivalSiteInstance = null;


        public ArchivalWorkflow(CSWorkflowInstance instance)
            : base(instance)
        {
            // check if site exists
            _archivalSiteInstance = instance.Farm.AllSites.Find(ArchivalSiteName);
            if (_archivalSiteInstance == null)
            {
                // create it. Workflow runs under Farm Admin rights, so this can be done.
                _archivalSiteInstance = instance.Farm.CreateSite
                (
                    ArchivalSiteName,
                    "This site is used by the ArchivalWorkflow to hold the archived files.",
                    CSFarm.FARM_DATABASENAME,
                    0                                                                           // quota should *not* be enabled for this site
                );
            }

            if (_archivalSiteInstance == null)
            {
                // cannot archive!
                throw new CSWorkflowException("Could not open or create the archival site.", instance);
            }
        }

        protected override void OnStarted(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            base.OnStarted(sender, e);

            if (sender.InstantiableEntity is CSFileSystemEntry)
            {
                CSFileSystemEntry archiveItem = (CSFileSystemEntry)sender.InstantiableEntity;
                string archivedItemPath = archiveItem.FullPath.Replace(sender.Site.RootFolder.FullPath, _archivalSiteInstance.RootFolder.FullPath);
                string archivedItemParentPath = archiveItem.ParentDirectoryPath.Replace(sender.Site.RootFolder.FullPath, _archivalSiteInstance.RootFolder.FullPath);

                sender.TraceWriter.WriteLine("Invoked for change for " + (archiveItem.IsFolder ? "directory" : "file") + ": " + archiveItem.FullPath);

                if ((!string.IsNullOrEmpty(archivedItemPath)) && (!string.IsNullOrEmpty(archivedItemParentPath)))
                {
                    CSFileSystemEntryDirectory archiveParentDirectory = _archivalSiteInstance.GetDirectory(archivedItemParentPath);
                    if (archiveParentDirectory == null)
                    {
                        archiveParentDirectory = _archivalSiteInstance.RootFolder.CreateDirectoryTree(archivedItemParentPath);
                    }

                    if (archiveParentDirectory != null)
                    {
                        sender.CopyItem(archiveItem, archiveParentDirectory, true);
                        sender.TraceWriter.WriteLine("Successfully archived item to: " + archivedItemPath);
                    }
                    else
                    {
                        string message = "ERROR: Could not create archival directory at: " + archivedItemParentPath;
                        sender.TraceWriter.WriteLine(message);
                        sender.MarkErrored(message);
                    }
                }
                else
                {
                    string message = "ERROR: Could not determine path of the source item. Quite odd... ";
                    sender.TraceWriter.WriteLine(message);
                    sender.MarkErrored(message);
                }
            }
        }

    }
}
