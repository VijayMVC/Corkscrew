using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// The data contract representation of a CSWorkflowManifestItem
    /// </summary>
    [DataContract]
    public class CSWorkflowManifestItemDataContract
    {

        #region Properties

        /// <summary>
        /// Item Id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.Empty;

        /// <summary>
        /// Get/set filename. Must follow file-naming guidelines of the underlying OS.
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set
            {

                if (value != null)
                {
                    foreach (char ch in Path.GetInvalidFileNameChars())
                    {
                        if (value.Contains(ch.ToString()))
                        {
                            throw new IOException("Name contains illegal characters.");
                        }
                    }
                }

                _fileName = value.SafeString(255, true, true, "Filename", string.Empty);
            }
        }
        private string _fileName = string.Empty;

        /// <summary>
        /// Get/set filename extension
        /// </summary>
        public string FilenameExtension
        {
            get { return _fileNameExtension; }
            set
            {
                if (value != null)
                {
                    foreach (char ch in Path.GetInvalidFileNameChars())
                    {
                        if (value.Contains(ch.ToString()))
                        {
                            throw new IOException("Extension contains illegal characters.");
                        }
                    }
                }

                _fileNameExtension = value.SafeString(255, true, true, "FilenameExtension", null, ".");
            }
        }
        private string _fileNameExtension = null;

        /// <summary>
        /// Type of item
        /// </summary>
        public WorkflowManifestItemTypeEnum ItemType
        {
            get;
            set;
        } = WorkflowManifestItemTypeEnum.Unknown;

        /// <summary>
        /// Relative folder at time of building the manifest
        /// </summary>
        public string BuildtimeRelativeFolder
        {
            get { return _buildtimeRelativeFolder; }
            set
            {
                _buildtimeRelativeFolder = value.SafeString(1024, true, true, nameof(BuildtimeRelativeFolder), null);
            }
        }
        private string _buildtimeRelativeFolder = null;

        /// <summary>
        /// Relative folder at time of running the workflow
        /// </summary>
        public string RuntimeRelativeFolder
        {
            get { return _runtimeRelativeFolder; }
            set
            {
                _runtimeRelativeFolder = value.SafeString(1024, true, true, nameof(RuntimeRelativeFolder), null);
            }
        }
        private string _runtimeRelativeFolder = null;

        /// <summary>
        /// Indicates the item is required at runtime. if set, item is copied to the RuntimeRelativeFolder
        /// </summary>
        public bool RequiredForExecution
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Gets the size of the FileContent byte array
        /// </summary>
        public int FileContentSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Date and time of creation. 
        /// Set by constructor.
        /// </summary>
        public DateTime Created { get; internal set; } = DateTime.Now;

        /// <summary>
        /// User who created.
        /// Set by constructor.
        /// </summary>
        public CSUser CreatedBy { get; internal set; } = CSUser.CreateAnonymousUser();

        /// <summary>
        /// Date and time of modification. 
        /// Set internally or by persistence.
        /// </summary>
        public DateTime Modified { get; internal set; } = DateTime.Now;

        /// <summary>
        /// User who modified. 
        /// Set internally or by persistence.
        /// </summary>
        public CSUser ModifiedBy { get; internal set; } = CSUser.CreateAnonymousUser();

        #endregion


        /// <summary>
        /// Initialize with a CSManifestItem
        /// </summary>
        /// <param name="item">Manifest item to initialize with</param>
        public CSWorkflowManifestItemDataContract(CSWorkflowManifestItem item)
        {
            if (item == null)
            {
                return;
            }

            Id = item.Id;
            Filename = item.Filename;
            FilenameExtension = item.FilenameExtension;
            ItemType = item.ItemType;
            BuildtimeRelativeFolder = item.BuildtimeRelativeFolder;
            RuntimeRelativeFolder = item.RuntimeRelativeFolder;
            RequiredForExecution = item.RequiredForExecution;
            FileContentSize = item.FileContentSize;
            Created = item.Created;
            CreatedBy = item.CreatedBy;
            Modified = item.Modified;
            ModifiedBy = item.ModifiedBy;
        }

    }
}