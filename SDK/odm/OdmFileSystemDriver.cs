using Corkscrew.SDK.constants;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{

    /// <summary>
    /// Object/data manager for CSFileSystemEntry and derived classes
    /// </summary>
    internal class OdmFileSystemDriver : OdmBase
    {

        /// <summary>
        /// Reference to site 
        /// </summary>
        private CSSite _site = null;

        private OdmUsers _odmUsers = null;

        /// <summary>
        /// Constructor for use with SiteDB
        /// </summary>
        /// <param name="site">CSSite. Set to NULL to manage ConfigDB filesystem</param>
        public OdmFileSystemDriver(CSSite site)
            : base(site)
        {
           if (site == null)
            {
                throw new ArgumentNullException("site");
            }

            _site = site;

            _odmUsers = new OdmUsers();
        }

        /// <summary>
        /// Saves an instance of the filesystem entry (only called from CSFileSystemEntry)
        /// </summary>
        /// <param name="entry">CSFileSystemEntry entry to persist</param>
        public bool Save(CSFileSystemEntry entry)
        {
            // perform some sanity checks, reject save if something is wrong
            if (!entry.IsValid())
            {
                return false;
            }

            return base.CommitChanges
            (
                "FileSystemSave",
                new Dictionary<string, object>()
                {
                    { "@Id", entry.Id },
                    { "@SiteId", _site.Id },
                    { "@Filename", entry.Filename },
                    { "@FileExtension", entry.FilenameExtension },
                    { "@DirectoryName", entry.ParentDirectoryPath },
                    { "@Created", entry.Created },
                    { "@CreatedBy", entry.CreatedBy.Id },
                    { "@Modified", entry.Modified },
                    { "@ModifiedBy", entry.ModifiedBy.Id },
                    { "@is_directory", entry.IsFolder },
                    { "@is_readonly", entry.IsReadonly },
                    { "@is_archive", entry.IsArchive },
                    { "@is_system", entry.IsSystem },
                    { "@DoNotUpdate", 0 }
                }
            );
        }

        /// <summary>
        /// Saves the content of the filesystem file entry (only called from CSVirtualFileStream.Flush())
        /// </summary>
        /// <param name="entry">CorkscrewFileSystemFileEntry entry to persist</param>
        /// <param name="content">byte[] content to persist. Needs to be lesser than 2GB in size (check with Int32.MaxValue).</param>
        public bool Save(CSFileSystemEntryFile entry, byte[] content)
        {
            // perform some sanity checks, reject save if something is wrong
            if (!entry.IsValid())
            {
                return false;
            }

            return base.CommitChanges
            (
                "FileSystemSaveContent",
                new Dictionary<string, object>()
                {
                    { "@Id", entry.Id },
                    { "@Modified", entry.Modified },
                    { "@ModifiedBy", entry.ModifiedBy.Id },
                    { "@Content", content }
                }
            );
        }

        /// <summary>
        /// Updates the directory name (containing folder) throughout the system
        /// </summary>
        /// <param name="oldDirectoryName">Old directory name</param>
        /// <param name="newDirectoryName">New directory name</param>
        /// <returns>True if update was successful.</returns>
        public bool UpdateDirectoryName(string oldDirectoryName, string newDirectoryName)
        {
            if (string.IsNullOrEmpty(oldDirectoryName) || string.IsNullOrEmpty(newDirectoryName))
            {
                throw new ArgumentNullException("*directoryName");
            }

            if (oldDirectoryName.EndsWith("/") && (!newDirectoryName.EndsWith("/")))
            {
                newDirectoryName = string.Format("{0}/", newDirectoryName);
            }
            else if ((!oldDirectoryName.EndsWith("/")) && newDirectoryName.EndsWith("/"))
            {
                newDirectoryName = Utility.SafeString(newDirectoryName, removeAtEnd: "/");
            }

            // perform a case-sensitive comparison. This allows renames to change weird-cased names
            if (newDirectoryName.Equals(oldDirectoryName))
            {
                throw new ArgumentException("Old and New directory names are identical.");
            }

            return base.CommitChanges
            (
                "FileSystemUpdateDirectoryName",
                new Dictionary<string, object>()
                {
                    { "@SiteId", _site.Id },
                    { "@OldDirectoryName", oldDirectoryName },
                    { "@NewDirectoryName", newDirectoryName },
                    { "@Modified", DateTime.Now },
                    { "@ModifiedBy", _site.AuthenticatedUser }
                }
            );
        }

        /// <summary>
        /// Copies the content of [source] to [destination]
        /// </summary>
        /// <param name="source">Source file</param>
        /// <param name="destination">Destination file</param>
        /// <returns>Whether copy was successful</returns>
        public bool CopyFileContent(CSFileSystemEntryFile source, CSFileSystemEntryFile destination)
        {
            if ((source == null) || (source.Id.Equals(Guid.Empty)))
            {
                return false;
            }

            if ((destination == null) || (destination.Id.Equals(Guid.Empty)))
            {
                return false;
            }

            if (destination.Id.Equals(source.Id))
            {
                return false;
            }

            return base.CommitChanges
            (
                "FileSystemCopyContent",
                new Dictionary<string, object>()
                {
                    { "@SourceId", source.Id },
                    { "@DestinationId", destination.Id },
                    { "@ModifiedBy", destination.ModifiedBy.Id },
                    { "@Modified", destination.Modified }
                }
            );
        }

        /// <summary>
        /// Delete a CSFileSystemEntry entry
        /// </summary>
        /// <param name="entry">The entry to delete</param>
        public bool DeleteById(CSFileSystemEntry entry)
        {
            return base.CommitChanges
            (
                "FileSystemDeleteById",
                new Dictionary<string, object>()
                {
                    { "@Id", entry.Id },
                    { "@DeleteUserId", entry.AuthenticatedUser.Id.ToString("d") }
                }
            );
        }


        /// <summary>
        /// Gets a CSFileSystemEntry object given its Guid
        /// </summary>
        /// <param name="id">Guid of the entry to fetch</param>
        /// <returns>CSFileSystemEntry object if found. Else NULL.</returns>
        public CSFileSystemEntry GetById(Guid id)
        {
            DataSet ds = base.GetData
                (
                    "FileSystemGetById",
                    new Dictionary<string, object>()
                    {
                        { "@Id", id },
                        { "@UserId", _site.AuthenticatedUser.Id }
                    }
                );


            if (!base.HasData(ds))
            {
                return null;
            }

            return Populate(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Get CSFileSystemEntry objects under the given directory
        /// </summary>
        /// <param name="directoryPath">The path of the directory to fetch children of</param>
        /// <param name="returnFiles">If set, returns FILE elements</param>
        /// <param name="returnFolders">If set, returns FOLDER elements</param>
        /// <returns>Set returnFiles and returnFolders to TRUE to fetch both files and folders</returns>
        public IEnumerable<CSFileSystemEntry> GetByDirectory(string directoryPath, bool returnFiles = true, bool returnFolders = true)
        {
            DataSet ds = base.GetData
                (
                    "FileSystemGetByDirectory",
                    new Dictionary<string, object>()
                    {
                        { "@DirectoryName", directoryPath },
                        { "@SiteId", _site.Id },
                        { "@UserId",  _site.AuthenticatedUser.Id },
                        { "@ReturnFolders", returnFolders },
                        { "@ReturnFiles", returnFiles }
                    }
                );

            List<CSFileSystemEntry> items = new List<CSFileSystemEntry>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    items.Add(Populate(row));
                }
            }

            return items;
        }


        /// <summary>
        /// Gets a CSFileSystemEntry object given its full path
        /// </summary>
        /// <param name="fullpath">Fullpath of the entry to fetch</param>
        /// <returns>CSFileSystemEntry object if found. Else NULL.</returns>
        public CSFileSystemEntry GetByFullPath(string fullpath)
        {
            DataSet ds = base.GetData
                (
                    "FileSystemGetByFullPath",
                    new Dictionary<string, object>()
                    {
                        { "@FullPath", fullpath },
                        { "@SiteId", _site.Id },
                        { "@UserId",  _site.AuthenticatedUser.Id }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            return Populate(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Checks if an entry exists at the given path
        /// </summary>
        /// <param name="fullpath">Fullpath of the entry to fetch</param>
        /// <returns>True if entry is found, false if not.</returns>
        public bool Exists(string fullpath)
        {
            DataSet ds = base.GetData
                (
                    "FileSystemGetByFullPath",
                    new Dictionary<string, object>()
                    {
                        { "@FullPath", fullpath },
                        { "@SiteId", _site.Id },
                        { "@UserId",  _site.AuthenticatedUser.Id }
                    }
                );

            if (base.HasData(ds))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the content of a file given its Guid
        /// </summary>
        /// <param name="id">Guid of the file</param>
        /// <returns>byte array with content</returns>
        public byte[] GetContentById(Guid id)
        {
            return base.GetBinaryData
            (
                "FileSystemGetDataById",
                new Dictionary<string, object>()
                {
                    { "@Id", id },
                    { "@UserId",  _site.AuthenticatedUser.Id }
                }
            );
        }

        /// <summary>
        /// Gets the total size of all items under the given directory path
        /// </summary>
        /// <param name="path">Path to the directory to count</param>
        /// <returns>Total size in bytes (0 if no size was returned, -1 if there was some error)</returns>
        public long GetContentSizeUnderDirectory(string path)
        {
            long size = 0;

            DataSet ds = base.GetData
            (
                "FileSystemGetDirectoryContentSize",
                new Dictionary<string, object>()
                {
                    { "@DirectoryName", path },
                    { "@UserId",  _site.AuthenticatedUser.Id }
                }
            );

            if (!base.HasData(ds))
            {
                return 0;
            }

            try
            {
                size = Utility.SafeConvertToLong(ds.Tables[0].Rows[0][0]);
            }
            catch
            {
                size = -1;
            }

            return size;
        }

        public List<CSFileSystemHistoryRecord> GetAllHistory(CSFileSystemEntry entry)
        {
            List<CSFileSystemHistoryRecord> list = new List<CSFileSystemHistoryRecord>();

            DataSet ds = base.GetData
            (
                "FileSystemGetHistoryByItemId",
                new Dictionary<string, object>()
                {
                    { "Id", entry.Id.ToString("d") }
                }
            );

            if (HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateHistoryRecord(row));
                }
            }

            return list;
        }

        public CSFileSystemHistoryRecord GetHistoryRecord(Guid recordId)
        {
            DataSet ds = base.GetData
            (
                "FileSystemGetHistoryById",
                new Dictionary<string, object>()
                {
                    { "Id", recordId.ToString("d") }
                }
            );

            if (!HasData(ds))
            {
                return null;
            }

            return PopulateHistoryRecord(ds.Tables[0].Rows[0]);
        }

        private CSFileSystemHistoryRecord PopulateHistoryRecord(DataRow row)
        {
            OdmUsers odmUsr = new OdmUsers();

            CSFileSystemHistoryRecord record = new CSFileSystemHistoryRecord()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                FileSystemEntry = GetById(Utility.SafeConvertToGuid(row["FileSystemId"])),
                ChangeTimestamp = Utility.SafeConvertToDateTime(row["ChangeTimeStamp"]),
                ChangedBy = odmUsr.GetUserById(Utility.SafeConvertToGuid(row["ChangedBy"])),
                PreviousData = (byte[])row["PreviousData"],
                Filename = Utility.SafeString(row["PreviousFilename"]),
                FilenameExtension = Utility.SafeString(row["PreviousFilenameExtension"]),
                ParentDirectoryPath = Utility.SafeString(row["PreviousDirectoryName"]),
                Created = Utility.SafeConvertToDateTime(row["PreviousCreated"]),
                CreatedBy = odmUsr.GetUserById(Utility.SafeConvertToGuid(row["PreviousCreatedBy"])),
                Modified = Utility.SafeConvertToDateTime(row["PreviousModified"]),
                ModifiedBy = odmUsr.GetUserById(Utility.SafeConvertToGuid(row["PreviousModifiedBy"])),
                IsArchive = Utility.SafeConvertToBool(row["was_archive"]),
                IsSystem = Utility.SafeConvertToBool(row["was_system"]),
                IsFolder = Utility.SafeConvertToBool(row["was_directory"])
            };

            record.PreviousDataSize = ((record.PreviousData != null) ? record.PreviousData.Length : -1);

            switch (Utility.SafeString(row["ChangeType"]).Trim())
            {
                case "I":
                    record.ChangeType = constants.ChangeTypeEnum.Create;
                    break;

                case "U":
                case "UP":
                case "UC":
                    record.ChangeType = constants.ChangeTypeEnum.Update;
                    break;

                case "D":
                    record.ChangeType = constants.ChangeTypeEnum.Delete;
                    break;
            }

            return record;
        }

        // populates the filesystem object
        private CSFileSystemEntry Populate(DataRow row)
        {
            return new CSFileSystemEntry()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                ItemLocation = (_site.IsConfigSite ? FileSystemEntryLocationEnum.ConfigDB : FileSystemEntryLocationEnum.SiteDB),
                Site = _site,
                ParentDirectoryPath = Utility.SafeString(row["DirectoryName"]),
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                ModifiedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["ModifiedBy"])),
                LastAccessed = Utility.SafeConvertToDateTime(row["LastAccessed"]),
                LastAccessedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["LastAccessedBy"])),
                IsArchive = Utility.SafeConvertToBool(row["is_archive"]),
                IsSystem = Utility.SafeConvertToBool(row["is_system"]),
                IsFolder = Utility.SafeConvertToBool(row["is_directory"]),
                Filename = Utility.SafeString(row["Filename"]),
                FilenameExtension = Utility.SafeString(row["FilenameExtension"], null),
                IsReadonly = Utility.SafeConvertToBool(row["is_readonly"]),
                ContentSize = Utility.SafeConvertToInt(row["ContentSize"])
            };
        }


    }
}
