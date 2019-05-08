using Corkscrew.SDK.constants;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// Represents the CSFileSystemEntry object as a data contract
    /// </summary>
    [DataContract]
    public class CSFilesystemEntryDataContract
    {

        private CSSite _site = null;

        #region Properties

        /// <summary>
        /// Guid of the entry
        /// </summary>
        [DataMember]
        public Guid Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Location of the filesystem item. This can be either ConfigDB or SiteDB.
        /// </summary>
        [DataMember]
        public string ItemLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Guid to the Directory containing this entry
        /// </summary>
        [DataMember]
        public Guid ParentDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Filename
        /// </summary>
        [DataMember]
        public string Filename
        {
            get;
            private set;
        }

        /// <summary>
        /// Filename extension
        /// </summary>
        [DataMember]
        public string FilenameExtension
        {
            get;
            private set;
        }

        /// <summary>
        /// Full site-rooted path to the parent directory
        /// </summary>
        [DataMember]
        public string ParentDirectoryPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date/time of creation
        /// </summary>
        [DataMember]
        public DateTime Created
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user who created the entry.
        /// </summary>
        [DataMember]
        public CSUser CreatedBy
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date/time of last modification
        /// </summary>
        [DataMember]
        public DateTime Modified
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user who last modified the entry
        /// </summary>
        [DataMember]
        public CSUser ModifiedBy
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date/time of last access. 
        /// Will always be equal to current time.
        /// </summary>
        [DataMember]
        public DateTime LastAccessed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user who last accessed the entry.
        /// Will typically be equal to current user.
        /// </summary>
        [DataMember]
        public CSUser LastAccessedBy
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets if item is readonly. Content of readonly files cannot be changed.
        /// If is a directory, no new files can be added or removed. 
        /// </summary>
        [DataMember]
        public bool IsReadonly
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets if item is archive ready. Not used by Corkscrew system.
        /// </summary>
        [DataMember]
        public bool IsArchive
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets if is a system item. Cannot be set through business layer. 
        /// Can only be modified by Features during (un)installation.
        /// </summary>
        [DataMember]
        public bool IsSystem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets if is a hidden item. Hidden items are not rendered to UI, 
        /// but can be accessed programmatically.
        /// </summary>
        [DataMember]
        public bool IsHidden
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets if is a folder. Cannot be changed.
        /// </summary>
        [DataMember]
        public bool IsFolder
        {
            get;
            private set;
        }

        /// <summary>
        /// The full path to this item
        /// </summary>
        public string FullPath
        {
            get;
            private set;
        }


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entry">The filesystem entry to create the data contract from</param>
        public CSFilesystemEntryDataContract(CSFileSystemEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            _site = entry.Site;

            Id = entry.Id;
            Filename = entry.Filename;
            FilenameExtension = entry.FilenameExtension;
            ParentDirectory = entry.ParentDirectory.Id;
            ParentDirectoryPath = entry.ParentDirectoryPath;
            FullPath = entry.FullPath;
            Created = entry.Created;
            CreatedBy = entry.CreatedBy;
            Modified = entry.Modified;
            ModifiedBy = entry.ModifiedBy;
            LastAccessed = entry.LastAccessed;
            LastAccessedBy = entry.LastAccessedBy;
            IsArchive = entry.IsArchive;
            IsFolder = entry.IsFolder;
            IsHidden = entry.IsHidden;
            IsSystem = entry.IsSystem;
            IsReadonly = entry.IsReadonly;
            ItemLocation = Enum.GetName(typeof(FileSystemEntryLocationEnum), entry.ItemLocation);
        }

        
        private string GetFileNameWithExtension(string name, string extension)
        {
            return
                    Utility.SafeString
                    (
                        string.Format(
                            "{0}{1}",
                            Utility.SafeString(name, ""),
                            Utility.SafeString(extension, "", expectStart: ".")
                        )
                        , removeAtEnd: "."      // full filename cannot end with a ".", but it can start with a "." if the filename is missing.
                    );
        }
 
        private string GetFullPath(string directory, string name, string extension)
        {
            if (string.IsNullOrEmpty(directory))
            {
                directory = CSPath.CmsPathPrefix;
            }

            directory = CSPath.GetFullPath(_site, directory).SafeString(expectEnd: (IsFolder ? "/" : ""));

            string fne = GetFileNameWithExtension(name, extension);
            return directory + ((fne == "/") ? "" : (directory.EndsWith("/") ? fne : "/" + fne));
        }

    }
}