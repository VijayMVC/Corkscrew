using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// The data contract representation of a CSWorkflowManifest
    /// </summary>
    [DataContract]
    public class CSWorkflowManifestDataContract
    {

        #region Properties

        /// <summary>
        /// Id of the workflow manifest
        /// </summary>
        public Guid Id
        {
            get;
            set;
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
        private string _workflowClassName = null;

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
        public string BuildAssemblyVersion
        {
            get;
            set;
        } = (new Version(1, 0, 0, 0)).ToString(4);

        /// <summary>
        /// Assembly file version to set on build
        /// </summary>
        public string BuildAssemblyFileVersion
        {
            get;
            set;
        } = (new Version(1, 0, 0, 0)).ToString(4);

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

        /// <summary>
        /// Initialize the contract with the given manifest
        /// </summary>
        /// <param name="manifest">Manifest to initialize with</param>
        public CSWorkflowManifestDataContract(CSWorkflowManifest manifest)
        {
            if (manifest == null)
            {
                return;
            }

            Id = manifest.Id;
            WorkflowEngine = manifest.WorkflowEngine;
            OutputAssemblyName = manifest.OutputAssemblyName;
            WorkflowClassName = manifest.WorkflowClassName;
            BuildAssemblyCompany = manifest.BuildAssemblyCompany;
            BuildAssemblyCopyright = manifest.BuildAssemblyCopyright;
            BuildAssemblyDescription = manifest.BuildAssemblyDescription;
            BuildAssemblyFileVersion = manifest.BuildAssemblyFileVersion.ToString(4);
            BuildAssemblyProduct = manifest.BuildAssemblyProduct;
            BuildAssemblyTitle = manifest.BuildAssemblyTitle;
            BuildAssemblyTrademark = manifest.BuildAssemblyTrademark;
            BuildAssemblyVersion = manifest.BuildAssemblyVersion.ToString(4);
            AlwaysCompile = manifest.AlwaysCompile;
            CacheCompileResults = manifest.CacheCompileResults;
            LastCompiled = manifest.LastCompiled;
            Created = manifest.Created;
            CreatedBy = manifest.CreatedBy;
            Modified = manifest.Modified;
            ModifiedBy = manifest.ModifiedBy;

        }
    }
}