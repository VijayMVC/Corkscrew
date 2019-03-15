using Corkscrew.SDK.exceptions;
using Corkscrew.SDK.odm;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Workflow Manifest. A manifest is a list of items. In this case, this is a list of files, source code, etc that are necessary 
    /// to build and run the workflow. Each CSWorkflowDefinition must contain exactly one manifest to enable it to be executed.
    /// </summary>
    public class CSWorkflowManifest
    {

        #region Properties

        /// <summary>
        /// Id of the workflow manifest
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Workflow definition this manifest belongs to
        /// </summary>
        public CSWorkflowDefinition WorkflowDefinition
        {
            get;
            internal set;
        }

        /// <summary>
        /// Engine for the workflow
        /// </summary>
        public WorkflowEngineEnum WorkflowEngine
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the output assembly. Either generated at compilation time or present in the manifest.
        /// </summary>
        public string OutputAssemblyName
        {
            get { return _outputAssemblyName; }
            set
            {
                _outputAssemblyName = value.SafeString(255, false, true, nameof(OutputAssemblyName));
            }
        }
        private string _outputAssemblyName = null;

        /// <summary>
        /// Name of the workflow class (fully qualified, including namespace). This class will be instantiated and 
        /// executed by the CSWorkflowManager executive.
        /// </summary>
        public string WorkflowClassName
        {
            get { return _workflowClassName; }
            set
            {
                _workflowClassName = value.SafeString(1024, false, true, nameof(WorkflowClassName));
            }
        }
        private string _workflowClassName= null;

        /// <summary>
        /// Assembly title to set on build
        /// </summary>
        public string BuildAssemblyTitle
        {
            get { return _buildAssemblyTitle; }
            set
            {
                _buildAssemblyTitle = value.SafeString(255, true, true, nameof(BuildAssemblyTitle), null);
            }
        }
        private string _buildAssemblyTitle = null;

        /// <summary>
        /// Assembly description to set on build
        /// </summary>
        public string BuildAssemblyDescription
        {
            get { return _buildAssemblyDescription; }
            set
            {
                _buildAssemblyDescription = value.SafeString(255, true, true, nameof(BuildAssemblyDescription), null);
            }
        }
        private string _buildAssemblyDescription = null;

        /// <summary>
        /// Assembly company to set on build
        /// </summary>
        public string BuildAssemblyCompany
        {
            get { return _buildAssemblyCompany; }
            set
            {
                _buildAssemblyCompany = value.SafeString(255, true, true, nameof(BuildAssemblyCompany), null);
            }
        }
        private string _buildAssemblyCompany = null;

        /// <summary>
        /// Assembly product to set on build
        /// </summary>
        public string BuildAssemblyProduct
        {
            get { return _buildAssemblyProduct; }
            set
            {
                _buildAssemblyProduct = value.SafeString(255, true, true, nameof(BuildAssemblyProduct), null);
            }
        }
        private string _buildAssemblyProduct = null;

        /// <summary>
        /// Assembly copyright to set on build
        /// </summary>
        public string BuildAssemblyCopyright
        {
            get { return _buildAssemblyCopyright; }
            set
            {
                _buildAssemblyCopyright = value.SafeString(255, true, true, nameof(BuildAssemblyCopyright), null);
            }
        }
        private string _buildAssemblyCopyright = null;

        /// <summary>
        /// Assembly trademark to set on build
        /// </summary>
        public string BuildAssemblyTrademark
        {
            get { return _buildAssemblyTrademark; }
            set
            {
                _buildAssemblyTrademark = value.SafeString(255, true, true, nameof(BuildAssemblyTrademark), null);
            }
        }
        private string _buildAssemblyTrademark = null;

        /// <summary>
        /// Assembly version to set on build
        /// </summary>
        public Version BuildAssemblyVersion
        {
            get;
            set;
        } = new Version(1, 0, 0, 0);

        /// <summary>
        /// Assembly file version to set on build
        /// </summary>
        public Version BuildAssemblyFileVersion
        {
            get;
            set;
        } = new Version(1, 0, 0, 0);

        /// <summary>
        /// If set, will always compile the output. 
        /// If this is set, CacheCompileResults is ignored.
        /// </summary>
        /// <seealso cref="CacheCompileResults"/>
        public bool AlwaysCompile
        {
            get;
            set;
        }

        /// <summary>
        /// If set, results of the compile are persisted for reuse. 
        /// This is used only if AlwaysCompile is FALSE.
        /// </summary>
        /// <seealso cref="AlwaysCompile"/>
        public bool CacheCompileResults
        {
            get;
            set;
        }

        /// <summary>
        /// Date/time of the last successful compilation
        /// </summary>
        public DateTime LastCompiled
        {
            get;
            internal set;
        }

        /// <summary>
        /// Date and time of creation. 
        /// Set by constructor.
        /// </summary>
        public DateTime Created { get; internal set; }

        /// <summary>
        /// User who created.
        /// Set by constructor.
        /// </summary>
        public CSUser CreatedBy { get; internal set; }

        /// <summary>
        /// Date and time of modification. 
        /// Set internally or by persistence.
        /// </summary>
        public DateTime Modified { get; internal set; }

        /// <summary>
        /// User who modified. 
        /// Set internally or by persistence.
        /// </summary>
        public CSUser ModifiedBy { get; internal set; }


        #endregion

        #region Constructors

        // internal constructor for ODM
        internal CSWorkflowManifest() { }

        /// <summary>
        /// Creates a workflow manifest
        /// </summary>
        /// <param name="definition">The workflow definition to create the manifest for</param>
        /// <param name="engine">Workflow engine version</param>
        /// <param name="assemblyName">Assembly file name of the compiled result</param>
        /// <param name="className">Name of class (fully qualified, including namespace) that contains the required workflow</param>
        /// <param name="alwaysCompile">If set, workflow is always compiled (mutually exclusive with cacheCompileResults)</param>
        /// <param name="cacheCompileResults">If set, compilation result is cached to backend (mutually exclusive with alwaysCompile)</param>
        /// <returns>The created manifest object or NULL</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        /// <exception cref="ArgumentNullException">If definition, assemblyName or className are null</exception>
        /// <exception cref="CSWorkflowException">If the definition is disabled or already has a manifest attached</exception>
        /// <exception cref="ArgumentException">If alwaysCompile and cacheCompileResults are both set.</exception>
        public static CSWorkflowManifest CreateManifest(CSWorkflowDefinition definition, WorkflowEngineEnum engine, string assemblyName, string className, bool alwaysCompile, bool cacheCompileResults)
        {

            CSExceptionHelper.ThrowIfNull(definition, assemblyName, className);

            if (!definition.IsEnabled)
            {
                throw new CSWorkflowException("Cannot create manifest. Workflow definition is disabled.");
            }

            definition.ThrowIfNotAllowedToModify();

            // these two flags are mutually exclusive
            if (alwaysCompile && cacheCompileResults)
            {
                throw new ArgumentException("AlwaysCompile and CacheCompileResults cannot be both true.");
            }

            if ((!alwaysCompile) && (!cacheCompileResults))
            {
                // both are false. Optimize
                cacheCompileResults = true;
            }

            if (definition.GetManifest() != null)
            {
                throw new CSWorkflowException("Workflow Definition already has a manifest. Cannot create another manifest.");
            }

            CSWorkflowManifest manifest = new CSWorkflowManifest()
            {
                Id = Guid.NewGuid(),
                WorkflowDefinition = definition,
                WorkflowEngine = engine,
                OutputAssemblyName = assemblyName,
                WorkflowClassName = className,
                AlwaysCompile = alwaysCompile,
                CacheCompileResults = cacheCompileResults,

                Created = DateTime.Now,
                Modified = DateTime.Now,
                CreatedBy = definition.Farm.AuthenticatedUser,
                ModifiedBy = definition.Farm.AuthenticatedUser,

                LastCompiled = DateTime.MinValue
            };

            if (! (new OdmWorkflow(definition.Farm)).CreateWorkflowManifest(manifest))
            {
                return null;
            }

            return manifest;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves any changes made to the manifest
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        public void Save()
        {
            if (! WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }

            Modified = DateTime.Now;
            ModifiedBy = WorkflowDefinition.Farm.AuthenticatedUser;
            (new OdmWorkflow(WorkflowDefinition.Farm)).SaveChanges(this, false);
        }

        /// <summary>
        /// Updates the workflow manifest compilation results to the backend. This function should typically not be called 
        /// by anything other than a compiler engine.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        public void UpdateCompileResults()
        {
            if (!WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }

            LastCompiled = DateTime.Now;
            Modified = DateTime.Now;
            ModifiedBy = WorkflowDefinition.Farm.AuthenticatedUser;
            (new OdmWorkflow(WorkflowDefinition.Farm)).SaveChanges(this, true);
        }

        /// <summary>
        /// Deletes this manifest (will also cause deletion of all manifest items)
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        public void Delete()
        {
            if (!WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }

            (new OdmWorkflow(WorkflowDefinition.Farm)).DeleteWorkflowManifest(this);
        }

        /// <summary>
        /// Create a new manifest item
        /// </summary>
        /// <param name="fileName">Filename of file</param>
        /// <param name="filenameExtension">File extension name of file</param>
        /// <param name="type">Type of file</param>
        /// <param name="isRequiredForExecution">Whether required for execution</param>
        /// <param name="content">Content of the file as a byte array</param>
        /// <param name="buildRelativeFolder">Folder to place file in during build. Must be a relative path (file will be hosted in a temporary folder).</param>
        /// <param name="runtimeRelativeFolder">Folder to place file when running the workflow. Must be a relative path (file will be hosted in a temporary folder).</param>
        /// <returns>Created manifest item</returns>
        /// <seealso cref="CSWorkflowManifestItem.CreateManifestItem(CSWorkflowManifest, string, string, WorkflowManifestItemTypeEnum, bool, byte[], string, string)"/>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        /// <exception cref="ArgumentNullException">If manifest is null, if both fileName and filenameExtension are empty, if the content is empty for an item that is not a PrimaryAssembly</exception>
        /// <exception cref="ArgumentOutOfRangeException">If type is Unknown</exception>
        /// <exception cref="ArgumentException">If type is a Xaml file but the manifest.WorkflowEngine is not a Xaml-type, or filenameExtension does not match the item type, 
        /// if the buildRelativeFolder or runtimeRelativeFolder are rooted paths, an item with the same filename and extension already exists</exception>
        public CSWorkflowManifestItem AddItem(string fileName, string filenameExtension, WorkflowManifestItemTypeEnum type, bool isRequiredForExecution, byte[] content, string buildRelativeFolder = null, string runtimeRelativeFolder = null)
        {
            return CSWorkflowManifestItem.CreateManifestItem(this, fileName, filenameExtension, type, isRequiredForExecution, content, buildRelativeFolder, runtimeRelativeFolder);
        }

        /// <summary>
        /// Deletes the manifest item from this manifest
        /// </summary>
        /// <param name="item">Manifest item to delete</param>
        /// <exception cref="UnauthorizedAccessException">If the user is not a Farm administrator</exception>
        public void RemoveItem(CSWorkflowManifestItem item)
        {
            if (!WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }

            (new OdmWorkflow(WorkflowDefinition.Farm)).DeleteWorkflowManifestItem(item);
        }

        /// <summary>
        /// Returns the manifest items for this manifest
        /// </summary>
        /// <returns>Readonly list of manifest items</returns>
        public IReadOnlyList<CSWorkflowManifestItem> GetItems()
        {
            return (new OdmWorkflow(WorkflowDefinition.Farm)).GetManifestItemsForManifest(this).AsReadOnly();
        }

        #endregion
    }

    /// <summary>
    /// Workflow engine names
    /// </summary>
    public enum WorkflowEngineEnum
    {

        /// <summary>
        /// Not defined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Corkscrew coded workflow
        /// </summary>
        CS1C,

        /// <summary>
        /// Coded v4.0 workflow
        /// </summary>
        WF4C,

        /// <summary>
        /// Xaml v4.0 workflow
        /// </summary>
        WF4X,

        /// <summary>
        /// Coded v3.0 workflow
        /// </summary>
        WF3C,

        /// <summary>
        /// Xaml v3.0 workflow
        /// </summary>
        WF3X

    }
}
