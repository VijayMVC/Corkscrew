using Corkscrew.API.datacontracts;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Corkscrew.API.services
{

    /// <summary>
    /// This service allows interaction with the Corkscrew system at the Site level.
    /// </summary>
    [ServiceContract]
    public interface ISite
    {

        /// <summary>
        /// Returns all Site administrators for this site. Will also include Farm administrators
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <returns>Array of CSUser objects</returns>
        [OperationContract]
        CSPrincipalDataContract[] GetAllSiteAdministrators(string token, Guid siteId);

        /// <summary>
        /// Get directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the directory</param>
        /// <returns>The directory object</returns>
        [OperationContract]
        CSFilesystemEntryDataContract GetDirectoryById(string token, Guid siteId, Guid id);

        /// <summary>
        /// Get directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="resourceUri">Corkscrew resource Uri</param>
        /// <returns>The directory object</returns>
        [OperationContract]
        CSFilesystemEntryDataContract GetDirectoryByUri(string token, Guid siteId, string resourceUri);

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the file</param>
        /// <returns>The file object</returns>
        [OperationContract]
        CSFilesystemEntryDataContract GetFileById(string token, Guid siteId, Guid id);

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="resourceUri">Corkscrew resource Uri</param>
        /// <returns>The file object</returns>
        [OperationContract]
        CSFilesystemEntryDataContract GetFileByUri(string token, Guid siteId, string resourceUri);

        /// <summary>
        /// Gets a file or directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the item</param>
        /// <returns>The filesystem object</returns>
        [OperationContract]
        CSFilesystemEntryDataContract GetItemById(string token, Guid siteId, Guid id);

        /// <summary>
        /// Gets a file or directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="resourceUri">Corkscrew resource Uri</param>
        /// <returns>The filesystem object</returns>
        [OperationContract]
        CSFilesystemEntryDataContract GetItemByUri(string token, Guid siteId, string resourceUri);

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
        [OperationContract]
        bool UpdateSite(string token, Guid siteId, string name, string description, string databaseServer, string databaseName, long quota);

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
        [OperationContract]
        bool UpdateFilesystemItem(string token, Guid siteId, Guid id, string fileName, string filenameExtension, bool isReadonly, bool isHidden);

        /// <summary>
        /// Update data content for a file
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the file</param>
        /// <param name="data">Byte data content to set</param>
        /// <returns>True if the update was successful</returns>
        [OperationContract]
        bool UpdateFileContent(string token, Guid siteId, Guid id, byte[] data);

        /// <summary>
        /// Deletes the given filesystem item
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the file or directory</param>
        /// <returns>True if the operation was successful</returns>
        [OperationContract]
        bool DeleteItem(string token, Guid siteId, Guid id);

        /// <summary>
        /// Copy the filesystem item to the given location
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the item to copy</param>
        /// <param name="targetDirectoryPath">The target directory to copy to</param>
        /// <param name="overwrite">If set, overwrites the target</param>
        /// <returns>True if the operation was successful</returns>
        [OperationContract]
        bool CopyItem(string token, Guid siteId, Guid id, string targetDirectoryPath, bool overwrite);

        /// <summary>
        /// Move the filesystem item to the given location
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="id">Guid of the item to copy</param>
        /// <param name="targetDirectoryPath">The target directory to copy to</param>
        /// <param name="overwrite">If set, overwrites the target</param>
        /// <returns>True if the operation was successful</returns>
        [OperationContract]
        bool MoveItem(string token, Guid siteId, Guid id, string targetDirectoryPath, bool overwrite);

        /// <summary>
        /// Creates a new directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="parentDirectoryId">Guid of the parent directory</param>
        /// <param name="name">Name of the directory</param>
        /// <returns>Guid of the new directory</returns>
        [OperationContract]
        Guid CreateDirectory(string token, Guid siteId, Guid parentDirectoryId, string name);

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
        [OperationContract]
        Guid CreateFile(string token, Guid siteId, Guid parentDirectoryId, string fileName, string filenameExtension, byte[] data);

        /// <summary>
        /// Gets the listing of child items of the given directory
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="siteId">Guid of the site</param>
        /// <param name="parentDirectoryId">Guid of the directory to fetch the listing for</param>
        /// <returns>Dictionary. Key is the Guid of the child item, Value is a Boolean that is True if it is a folder, False if it is a file.</returns>
        [OperationContract]
        Dictionary<Guid, bool> GetDirectoryListing(string token, Guid siteId, Guid parentDirectoryId);
    }
}
