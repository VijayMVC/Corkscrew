using Corkscrew.API.business;
using Corkscrew.API.datacontracts;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Corkscrew.API.services
{
    /// <summary>
    /// This service allows interaction with the Corkscrew system at the Site level.
    /// </summary>
    public class Site : ISite
    {

        private CSSite GetSite(string token, Guid siteId)
        {
            CSUser user = Tools.GetCurrentlyAuthenticatedUser(token);
            if (user == null)
            {
                throw new FaultException("Token is not valid.");
            }

            CSSite site = CSFarm.Open(user).AllSites.Find(siteId);
            if (site == null)
            {
                throw new FaultException("Site could not be opened.");
            }

            return site;
        }

        /// <summary>
        /// Returns all Site administrators for this site. Will also include Farm administrators
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <returns>Array of CSUser objects</returns>
        public CSPrincipalDataContract[] GetAllSiteAdministrators(string token, Guid siteId)
        {
            List<CSPrincipalDataContract> principals = new List<CSPrincipalDataContract>();
            foreach(CSUser user in GetSite(token, siteId).AllSiteAdministrators)
            {
                principals.Add(new CSPrincipalDataContract(user));
            }
            return principals.ToArray();
        }

        /// <summary>
        /// Get directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the directory</param>
        /// <returns>The directory object</returns>
        public CSFilesystemEntryDataContract GetDirectoryById(string token, Guid siteId, Guid id)
        {
            return new CSFilesystemEntryDataContract(GetSite(token, siteId).GetDirectory(id));
        }

        /// <summary>
        /// Get directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="resourceUri">Corkscrew resource Uri</param>
        /// <returns>The directory object</returns>
        public CSFilesystemEntryDataContract GetDirectoryByUri(string token, Guid siteId, string resourceUri)
        {
            return new CSFilesystemEntryDataContract(GetSite(token, siteId).GetDirectory(resourceUri));
        }

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the file</param>
        /// <returns>The file object</returns>
        public CSFilesystemEntryDataContract GetFileById(string token, Guid siteId, Guid id)
        {
            return new CSFilesystemEntryDataContract(GetSite(token, siteId).GetFile(id));
        }

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="resourceUri">Corkscrew resource Uri</param>
        /// <returns>The file object</returns>
        public CSFilesystemEntryDataContract GetFileByUri(string token, Guid siteId, string resourceUri)
        {
            return new CSFilesystemEntryDataContract(GetSite(token, siteId).GetFile(resourceUri));
        }

        /// <summary>
        /// Gets a file or directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the item</param>
        /// <returns>The filesystem object</returns>
        public CSFilesystemEntryDataContract GetItemById(string token, Guid siteId, Guid id)
        {
            return new CSFilesystemEntryDataContract(GetSite(token, siteId).GetFileSystemItem(id));
        }

        /// <summary>
        /// Gets a file or directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="resourceUri">Corkscrew resource Uri</param>
        /// <returns>The filesystem object</returns>
        public CSFilesystemEntryDataContract GetItemByUri(string token, Guid siteId, string resourceUri)
        {
            return new CSFilesystemEntryDataContract(GetSite(token, siteId).GetFileSystemItem(resourceUri));
        }

        /// <summary>
        /// Update the metadata for the site
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="name">New name for site</param>
        /// <param name="description">New description for the site</param>
        /// <param name="databaseServer">Name of the server hosting the site database. Set to NULL to use the same server as the farm.</param>
        /// <param name="databaseName">Name of the site database. Set to NULL to auto-generate.</param>
        /// <param name="quota">Quota for content size in the site. Set to 0 to disable quota.</param>
        /// <returns>True if the update was successful</returns>
        public bool UpdateSite(string token, Guid siteId, string name, string description, string databaseServer, string databaseName, long quota)
        {
            CSSite site = GetSite(token, siteId);
            site.Name = name;
            site.Description = description;
            site.QuotaBytes = quota;

            return site.Save();
        }

        /// <summary>
        /// Update the filesystem item
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the filesystem item</param>
        /// <param name="fileName">File name</param>
        /// <param name="filenameExtension">Filename extension</param>
        /// <param name="isReadonly">Mark as readonly</param>
        /// <param name="isHidden">Mark as hidden</param>
        /// <returns>True if the update was successful</returns>
        public bool UpdateFilesystemItem(string token, Guid siteId, Guid id, string fileName, string filenameExtension, bool isReadonly, bool isHidden)
        {
            CSSite site = GetSite(token, siteId);
            CSFileSystemEntry entry = site.GetFileSystemItem(id);
            if (entry == null)
            {
                throw new FaultException("Cannot find the requested filesystem item");
            }

            entry.Filename = fileName;
            entry.FilenameExtension = filenameExtension;
            entry.IsReadonly = isReadonly;
            entry.IsHidden = isHidden;
            return entry.Save(true);
        }

        /// <summary>
        /// Update data content for a file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the file</param>
        /// <param name="data">Byte data content to set</param>
        /// <returns>True if the update was successful</returns>
        public bool UpdateFileContent(string token, Guid siteId, Guid id, byte[] data)
        {
            CSSite site = GetSite(token, siteId);
            CSFileSystemEntry entry = site.GetFileSystemItem(id);
            if ((entry == null) || (entry.IsFolder))
            {
                throw new FaultException("Cannot find the requested filesystem item");
            }

            CSFileSystemEntryFile file = new CSFileSystemEntryFile(entry);
            if (!file.Open(System.IO.FileAccess.Write))
            {
                throw new FaultException("Cannot open the requested filesystem item");
            }

            file.Write(data, 0, ((data != null) ? 0 : data.Length));
            file.Close();
            file = null;

            return true;
        }

        /// <summary>
        /// Deletes the given filesystem item
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the file or directory</param>
        /// <returns>True if the operation was successful</returns>
        public bool DeleteItem(string token, Guid siteId, Guid id)
        {
            CSSite site = GetSite(token, siteId);
            CSFileSystemEntry entry = site.GetFileSystemItem(id);
            if (entry == null)
            {
                throw new FaultException("Cannot find the requested filesystem item");
            }

            return entry.Delete();
        }

        /// <summary>
        /// Copy the filesystem item to the given location
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the item to copy</param>
        /// <param name="targetDirectoryPath">The target directory to copy to</param>
        /// <param name="overwrite">If set, overwrites the target</param>
        /// <returns>True if the operation was successful</returns>
        public bool CopyItem(string token, Guid siteId, Guid id, string targetDirectoryPath, bool overwrite)
        {
            CSSite site = GetSite(token, siteId);
            CSFileSystemEntry entry = site.GetFileSystemItem(id);
            if (entry == null)
            {
                throw new FaultException("Cannot find the requested filesystem item");
            }

            entry.CopyTo(targetDirectoryPath, true);
            return true;
        }

        /// <summary>
        /// Move the filesystem item to the given location
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the item to copy</param>
        /// <param name="targetDirectoryPath">The target directory to copy to</param>
        /// <param name="overwrite">If set, overwrites the target</param>
        /// <returns>True if the operation was successful</returns>
        public bool MoveItem(string token, Guid siteId, Guid id, string targetDirectoryPath, bool overwrite)
        {
            CSSite site = GetSite(token, siteId);
            CSFileSystemEntry entry = site.GetFileSystemItem(id);
            if (entry == null)
            {
                throw new FaultException("Cannot find the requested filesystem item");
            }

            entry.MoveTo(targetDirectoryPath, true);
            return true;
        }

        /// <summary>
        /// Creates a new directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="parentDirectoryId">Guid of the parent directory</param>
        /// <param name="name">Name of the directory</param>
        /// <returns>Guid of the new directory</returns>
        public Guid CreateDirectory(string token, Guid siteId, Guid parentDirectoryId, string name)
        {
            CSFileSystemEntryDirectory parentDirectory = GetSite(token, siteId).GetDirectory(parentDirectoryId);
            if (parentDirectory == null)
            {
                return Guid.Empty;
            }

            CSFileSystemEntry entry = parentDirectory.CreateDirectory(name);
            if (entry == null)
            {
                return Guid.Empty;
            }

            return entry.Id;
        }

        /// <summary>
        /// Creates a new file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="parentDirectoryId">Guid of the parent directory</param>
        /// <param name="fileName">File name</param>
        /// <param name="filenameExtension">Filename extension</param>
        /// <param name="data">Data content of the file</param>
        /// <returns>Guid of the new file</returns>
        public Guid CreateFile(string token, Guid siteId, Guid parentDirectoryId, string fileName, string filenameExtension, byte[] data)
        {
            CSFileSystemEntryDirectory parentDirectory = GetSite(token, siteId).GetDirectory(parentDirectoryId);
            if (parentDirectory == null)
            {
                return Guid.Empty;
            }

            CSFileSystemEntry entry = parentDirectory.CreateFile(fileName, filenameExtension, data);
            if (entry == null)
            {
                return Guid.Empty;
            }

            return entry.Id;
        }

        /// <summary>
        /// Gets the listing of child items of the given directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="parentDirectoryId">Guid of the directory to fetch the listing for</param>
        /// <returns>Dictionary. Key is the Guid of the child item, Value is a Boolean that is True if it is a folder, False if it is a file.</returns>
        public Dictionary<Guid, bool> GetDirectoryListing(string token, Guid siteId, Guid parentDirectoryId)
        {
            CSFileSystemEntryDirectory parent = GetSite(token, siteId).GetDirectory(parentDirectoryId);
            if (parent == null)
            {
                return new Dictionary<Guid, bool>();
            }

            Dictionary<Guid, bool> result = new Dictionary<Guid, bool>();
            foreach(CSFileSystemEntry entry in parent.Items)
            {
                result.Add(entry.Id, entry.IsFolder);
            }
            return result;
        }

    }
}
