using Corkscrew.SDK.odm;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.IO;

namespace Corkscrew.SDK.workflow
{
    /// <summary>
    /// Represents a single item in the Workflow Manifest
    /// </summary>
    public class CSWorkflowManifestItem
    {

        #region Properties

        /// <summary>
        /// Item Id
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        } = Guid.Empty;

        /// <summary>
        /// Workflow manifest this item belongs to. Each manifest item can belong to only one manifest (that is, items cannot be shared among different manifests).
        /// </summary>
        public CSWorkflowManifest WorkflowManifest
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Gets the Workflow definition (equal to accessing WorkflowManifest.WorkflowDefinition)
        /// </summary>
        public CSWorkflowDefinition WorkflowDefinition
        {
            get
            {
                return ((WorkflowManifest == null) ? null : WorkflowManifest.WorkflowDefinition);
            }
        }

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
        /// Returns the filename with extension. 
        /// </summary>
        public string FilenameWithExtension
        {
            get
            {
                return Utility.SafeString
                    (
                        string.Format(
                            "{0}{1}",
                            Utility.SafeString(Filename, ""),
                            Utility.SafeString(FilenameExtension, "", expectStart: ".")
                        )
                        , removeAtEnd: "."      // full filename cannot end with a ".", but it can start with a "." if the filename is missing.
                    );
            }
        }

        /// <summary>
        /// Type of item
        /// </summary>
        public WorkflowManifestItemTypeEnum ItemType
        {
            get;
            set;
        } = WorkflowManifestItemTypeEnum.Unknown;

        /// <summary>
        /// Returns true if this item needs to be passed to the compiler engine
        /// </summary>
        public bool IsCompilerInput
        {
            get
            {
                switch (ItemType)
                {
                    case WorkflowManifestItemTypeEnum.DependencyAssembly:
                    case WorkflowManifestItemTypeEnum.ResourceFile:
                    case WorkflowManifestItemTypeEnum.SourceCodeFile:
                    case WorkflowManifestItemTypeEnum.XamlFile:                 // Xaml file should be present in both places
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns true if this item needs to be present at runtime
        /// </summary>
        public bool IsRuntimeComponent
        {
            get
            {
                if (RequiredForExecution)
                {
                    return true;
                }

                switch (ItemType)
                {
                    case WorkflowManifestItemTypeEnum.ConfigurationFile:
                    case WorkflowManifestItemTypeEnum.CustomDataFile:
                    case WorkflowManifestItemTypeEnum.DependencyAssembly:
                    case WorkflowManifestItemTypeEnum.MediaResourceFile:
                    case WorkflowManifestItemTypeEnum.PrimaryAssembly:
                    case WorkflowManifestItemTypeEnum.Stylesheet:
                    case WorkflowManifestItemTypeEnum.XamlFile:                 // Xaml file should be present in both places
                        return true;
                }

                return false;
            }
        }

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
        /// The file content of this manifest item
        /// </summary>
        public byte[] FileContent
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Gets the size of the FileContent byte array
        /// </summary>
        public int FileContentSize
        {
            get
            {
                return ((FileContent == null) ? 0 : FileContent.Length);
            }
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

        /// <summary>
        /// Returns true if item must be compiled (if it is a compiler input)
        /// </summary>
        public bool MustCompile
        {
            get
            {
                if (!IsCompilerInput)
                {
                    return false;
                }

                if (Modified > WorkflowManifest.LastCompiled)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region Constructors

        // blank for ODM
        internal CSWorkflowManifestItem() { }

        /// <summary>
        /// Create a new manifest item
        /// </summary>
        /// <param name="manifest">Manifest to add item to</param>
        /// <param name="fileName">Filename of file</param>
        /// <param name="filenameExtension">File extension name of file</param>
        /// <param name="type">Type of file</param>
        /// <param name="isRequiredForExecution">Whether required for execution</param>
        /// <param name="content">Content of the file as a byte array</param>
        /// <param name="buildRelativeFolder">Folder to place file in during build. Must be a relative path (file will be hosted in a temporary folder).</param>
        /// <param name="runtimeRelativeFolder">Folder to place file when running the workflow. Must be a relative path (file will be hosted in a temporary folder).</param>
        /// <returns>Created manifest item</returns>
        /// <seealso cref="CSWorkflowManifest.AddItem(string, string, WorkflowManifestItemTypeEnum, bool, byte[], string, string)"/>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        /// <exception cref="ArgumentNullException">If manifest is null, if both fileName and filenameExtension are empty, if the content is empty for an item that is not a PrimaryAssembly</exception>
        /// <exception cref="ArgumentOutOfRangeException">If type is Unknown</exception>
        /// <exception cref="ArgumentException">If type is a Xaml file but the manifest.WorkflowEngine is not a Xaml-type, or filenameExtension does not match the item type, 
        /// if the buildRelativeFolder or runtimeRelativeFolder are rooted paths, an item with the same filename and extension already exists</exception>
        public static CSWorkflowManifestItem CreateManifestItem(CSWorkflowManifest manifest, string fileName, string filenameExtension, WorkflowManifestItemTypeEnum type, bool isRequiredForExecution, byte[] content, string buildRelativeFolder = null, string runtimeRelativeFolder = null)
        {

            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            if (!manifest.WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }

            if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(filenameExtension))
            {
                throw new ArgumentNullException("Filename and File extension cannot both be null or empty.");
            }

            if (type == WorkflowManifestItemTypeEnum.Unknown)
            {
                throw new ArgumentOutOfRangeException("type");
            }

            // validate if type being added is valid for type of workflow engine
            if ((type == WorkflowManifestItemTypeEnum.XamlFile) && ((manifest.WorkflowEngine != WorkflowEngineEnum.WF3X) && (manifest.WorkflowEngine != WorkflowEngineEnum.WF4X)))
            {
                throw new ArgumentException("Xaml files may be added only to manifests with WorkflowEngine set to WF3X and WF4X.");
            }

            // validate type and extension for some known types
            if (!string.IsNullOrEmpty(filenameExtension))
            {

                filenameExtension = filenameExtension.ToLower();

                switch (type)
                {
                    case WorkflowManifestItemTypeEnum.ConfigurationFile:
                        if ((!filenameExtension.EndsWith(".config")) && (!filenameExtension.EndsWith(".ini")) && (!filenameExtension.EndsWith(".ini")) && (!filenameExtension.EndsWith(".inf")))
                        {
                            throw new ArgumentException("Filename Extension [" + filenameExtension + "] is not a valid configuration file type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.DependencyAssembly:
                    case WorkflowManifestItemTypeEnum.PrimaryAssembly:
                        if (!filenameExtension.EndsWith(".dll"))
                        {
                            throw new ArgumentException("Filename Extension [" + filenameExtension + "] is not a valid assembly type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.ResourceFile:
                        if ((!filenameExtension.EndsWith(".res")) && (!filenameExtension.EndsWith(".resx")) && (!filenameExtension.EndsWith(".resources")))
                        {
                            throw new ArgumentException("Filename Extension [" + filenameExtension + "] is not a valid resource file type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.SourceCodeFile:
                        if ((!filenameExtension.EndsWith(".cs")) && (!filenameExtension.EndsWith(".vb")))
                        {
                            throw new ArgumentException("Filename Extension [" + filenameExtension + "] is not a valid source code file type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.XamlFile:
                        if ((!filenameExtension.EndsWith(".xaml")) && (!filenameExtension.EndsWith(".xoml")))
                        {
                            throw new ArgumentException("Filename Extension [" + filenameExtension + "] is not a valid Xaml file type.");
                        }
                        break;
                }
            }

            if ((type != WorkflowManifestItemTypeEnum.PrimaryAssembly) && ((content == null) || (content.Length < 1)))
            {
                throw new ArgumentNullException("content");
            }

            if ((!string.IsNullOrEmpty(buildRelativeFolder)) && (Path.IsPathRooted(buildRelativeFolder)))
            {
                throw new ArgumentException("buildRelativeFolder must be a relative path.");
            }

            if ((!string.IsNullOrEmpty(runtimeRelativeFolder)) && (Path.IsPathRooted(runtimeRelativeFolder)))
            {
                throw new ArgumentException("runtimeRelativeFolder must be a relative path.");
            }

            string fileNameWithExtension = Utility.SafeString
                    (
                        string.Format(
                            "{0}{1}",
                            Utility.SafeString(fileName, ""),
                            Utility.SafeString(filenameExtension, "", expectStart: ".")
                        )
                        , removeAtEnd: "."      // full filename cannot end with a ".", but it can start with a "." if the filename is missing.
                    );

            IReadOnlyList<CSWorkflowManifestItem> existingItems = manifest.GetItems();
            foreach (CSWorkflowManifestItem existingItem in existingItems)
            {
                if (fileNameWithExtension.Equals(existingItem.FilenameWithExtension))
                {
                    throw new ArgumentException("Item with the same filename already exists.");
                }
            }

            CSWorkflowManifestItem item = new CSWorkflowManifestItem()
            {
                Id = Guid.NewGuid(),
                WorkflowManifest = manifest,
                ItemType = type,
                Filename = fileName,
                FilenameExtension = filenameExtension,
                BuildtimeRelativeFolder = buildRelativeFolder,
                RuntimeRelativeFolder = runtimeRelativeFolder,
                FileContent = content,
                RequiredForExecution = isRequiredForExecution,

                Created = DateTime.Now,
                Modified = DateTime.Now,
                CreatedBy = manifest.WorkflowDefinition.Farm.AuthenticatedUser,
                ModifiedBy = manifest.WorkflowDefinition.Farm.AuthenticatedUser,
            };

            if (!(new OdmWorkflow(manifest.WorkflowDefinition.Farm)).CreateWorkflowManifestItem(item))
            {
                return null;
            }

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save changes to this item
        /// </summary>
        /// <returns>Success or failure</returns>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        /// <exception cref="ArgumentNullException">If manifest is null, if both fileName and filenameExtension are empty, if the content is empty for an item that is not a PrimaryAssembly</exception>
        /// <exception cref="ArgumentOutOfRangeException">If type is Unknown</exception>
        /// <exception cref="ArgumentException">If type is a Xaml file but the manifest.WorkflowEngine is not a Xaml-type, or filenameExtension does not match the item type, 
        /// if the buildRelativeFolder or runtimeRelativeFolder are rooted paths, an item with the same filename and extension already exists</exception>
        public bool Save()
        {

            if (!WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }


            if (string.IsNullOrEmpty(Filename) && string.IsNullOrEmpty(FilenameExtension))
            {
                throw new ArgumentNullException("Filename and File extension cannot both be null or empty.");
            }

            if (ItemType == WorkflowManifestItemTypeEnum.Unknown)
            {
                throw new ArgumentOutOfRangeException("type");
            }

            // validate if type being added is valid for type of workflow engine
            if ((ItemType == WorkflowManifestItemTypeEnum.XamlFile) && ((WorkflowManifest.WorkflowEngine != WorkflowEngineEnum.WF3X) && (WorkflowManifest.WorkflowEngine != WorkflowEngineEnum.WF4X)))
            {
                throw new ArgumentException("Xaml files may be added only to manifests with WorkflowEngine set to WF3X and WF4X.");
            }

            // validate type and extension for some known types
            if (!string.IsNullOrEmpty(FilenameExtension))
            {

                FilenameExtension = FilenameExtension.ToLower();

                switch (ItemType)
                {
                    case WorkflowManifestItemTypeEnum.ConfigurationFile:
                        if ((!FilenameExtension.EndsWith(".config")) && (!FilenameExtension.EndsWith(".ini")) && (!FilenameExtension.EndsWith(".ini")) && (!FilenameExtension.EndsWith(".inf")))
                        {
                            throw new ArgumentException("Filename Extension [" + FilenameExtension + "] is not a valid configuration file type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.DependencyAssembly:
                    case WorkflowManifestItemTypeEnum.PrimaryAssembly:
                        if (!FilenameExtension.EndsWith(".dll"))
                        {
                            throw new ArgumentException("Filename Extension [" + FilenameExtension + "] is not a valid assembly type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.ResourceFile:
                        if ((!FilenameExtension.EndsWith(".res")) && (!FilenameExtension.EndsWith(".resx")) && (!FilenameExtension.EndsWith(".resources")))
                        {
                            throw new ArgumentException("Filename Extension [" + FilenameExtension + "] is not a valid resource file type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.SourceCodeFile:
                        if ((!FilenameExtension.EndsWith(".cs")) && (!FilenameExtension.EndsWith(".vb")))
                        {
                            throw new ArgumentException("Filename Extension [" + FilenameExtension + "] is not a valid source code file type.");
                        }
                        break;

                    case WorkflowManifestItemTypeEnum.XamlFile:
                        if ((!FilenameExtension.EndsWith(".xaml")) && (!FilenameExtension.EndsWith(".xoml")))
                        {
                            throw new ArgumentException("Filename Extension [" + FilenameExtension + "] is not a valid Xaml file type.");
                        }
                        break;
                }
            }

            if ((ItemType != WorkflowManifestItemTypeEnum.PrimaryAssembly) && ((FileContent == null) || (FileContentSize < 1)))
            {
                throw new ArgumentNullException("FileContent");
            }

            if ((!string.IsNullOrEmpty(BuildtimeRelativeFolder)) && (Path.IsPathRooted(BuildtimeRelativeFolder)))
            {
                throw new ArgumentException("BuildtimeRelativeFolder must be a relative path.");
            }

            if ((!string.IsNullOrEmpty(RuntimeRelativeFolder)) && (Path.IsPathRooted(RuntimeRelativeFolder)))
            {
                throw new ArgumentException("RuntimeRelativeFolder must be a relative path.");
            }

            IReadOnlyList<CSWorkflowManifestItem> existingItems = WorkflowManifest.GetItems();
            foreach (CSWorkflowManifestItem existingItem in existingItems)
            {
                if ((!Id.Equals(existingItem.Id)) && (FilenameWithExtension.Equals(existingItem.FilenameWithExtension)))
                {
                    throw new ArgumentException("Another item with the same filename already exists.");
                }
            }

            Modified = DateTime.Now;
            ModifiedBy = WorkflowDefinition.Farm.AuthenticatedUser;

            return (new OdmWorkflow(WorkflowDefinition.Farm)).SaveChanges(this);
        }

        /// <summary>
        /// Returns the full path this item would exist at for the compiler
        /// </summary>
        /// <param name="tempFolderPath">Absolute path on the disk storage where the temporary folder is hosted</param>
        /// <returns>Full path string</returns>
        public string GetFullPathForCompiler(string tempFolderPath)
        {
            return Path.Combine(tempFolderPath, Utility.SafeString(BuildtimeRelativeFolder, string.Empty));
        }

        /// <summary>
        /// Returns the full path this item would exist at for the runtime
        /// </summary>
        /// <param name="tempFolderPath">Absolute path on the disk storage where the temporary folder is hosted</param>
        /// <returns>Full path string</returns>
        public string GetFullPathForRuntime(string tempFolderPath)
        {
            return Path.Combine(tempFolderPath, Utility.SafeString(RuntimeRelativeFolder, string.Empty));
        }

        /// <summary>
        /// Writes the file content to the disk, using the Build folder
        /// </summary>
        /// <param name="diskFolderPath">Absolute path on the disk storage to write to</param>
        /// <returns>The full path of the file that was created</returns>
        public string WriteToDiskForCompiler(string diskFolderPath)
        {
            return WriteToDiskHelper(GetFullPathForCompiler(diskFolderPath));
        }

        /// <summary>
        /// Writes the file content to the disk, using the Runtime folder
        /// </summary>
        /// <param name="diskFolderPath">Absolute path on the disk storage to write to</param>
        /// <returns>The full path of the file that was created</returns>
        public string WriteToDiskForRuntime(string diskFolderPath)
        {
            return WriteToDiskHelper(GetFullPathForRuntime(diskFolderPath));
        }


        // this method should not throw exceptions because callers will not expect that
        private string WriteToDiskHelper(string fullFolderPath)
        {
            if (FileContentSize >= 1)
            {
                string outputFilePath = Path.Combine(fullFolderPath, FilenameWithExtension);
                try
                {
                    if (!Directory.Exists(fullFolderPath))
                    {
                        Directory.CreateDirectory(fullFolderPath);
                    }

                    File.WriteAllBytes(outputFilePath, FileContent);
                }
                catch
                {
                    return null;
                }

                return outputFilePath;
            }

            return null;
        }

        #endregion

    }

    ////// NOTE: Only add items to the *_END_* of this list.
    /// <summary>
    /// Type of workflow manifest item. 
    /// </summary>
    public enum WorkflowManifestItemTypeEnum
    {

        /// <summary>
        /// Not known
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Primary assembly (contains the entry point class and function)
        /// </summary>
        PrimaryAssembly,

        /// <summary>
        /// A dependency or satellite assembly that is used by the PrimaryAssembly
        /// </summary>
        DependencyAssembly,

        /// <summary>
        /// A code-behind or code file (.cs or .vb)
        /// </summary>
        SourceCodeFile,

        /// <summary>
        /// A Xaml file (.xaml)
        /// </summary>
        XamlFile,

        /// <summary>
        /// A .config file
        /// </summary>
        ConfigurationFile,

        /// <summary>
        /// A media resource (pictures, audio, video)
        /// </summary>
        MediaResourceFile,

        /// <summary>
        /// CSS stylesheet
        /// </summary>
        Stylesheet,

        /// <summary>
        /// A custom data file (can be .txt, .xml, etc)
        /// </summary>
        CustomDataFile,

        /// <summary>
        /// A resource file (.resx, .resources, etc)
        /// </summary>
        ResourceFile

    }


}
