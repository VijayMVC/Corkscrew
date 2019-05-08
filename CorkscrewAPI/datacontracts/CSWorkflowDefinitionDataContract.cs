using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// Data contract representing a CSWorkflowDefinition instance
    /// </summary>
    [DataContract]
    public class CSWorkflowDefinitionDataContract
    {
        #region Properties

        /// <summary>
        /// Workflow definition Id
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        } = Guid.Empty;

        /// <summary>
        /// Name of the workflow (as shown in "Installed/Available workflows" lists). 
        /// The name must be unique in a farm, checked at the time of saving the definition.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value.SafeString(255, false, true, nameof(Name)); }
        }
        private string _name = null;

        /// <summary>
        /// Description of the workflow. Displayed on the UI.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value.SafeString(1024, true, true, nameof(Description)); }
        }
        private string _description = null;

        /// <summary>
        /// Default association data
        /// </summary>
        public string DefaultAssociationData
        {
            // this is defined as nvarchar(max) in the db. no need for safestring!

            get;
            set;
        }


        /// <summary>
        /// Allow starting the workflow manually. 
        /// Only Administrator users and the user owning the item the workflow is attached to can start a workflow manually.
        /// </summary>
        public bool AllowManualStart
        {
            get;
            set;
        } = true;

        /// <summary>
        /// If set, automatically fires the workflow when a new item is created.
        /// </summary>
        public bool StartOnCreate
        {
            get;
            set;
        } = true;

        /// <summary>
        /// If set, automatically fires the workflow when the item is modified in someway (property and content).
        /// </summary>
        public bool StartOnModify
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Is the workflow definition enabled
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Date and time of creation. 
        /// Set by constructor.
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// User who created.
        /// Set by constructor.
        /// </summary>
        public CSUser CreatedBy { get; private set; }

        /// <summary>
        /// Date and time of modification. 
        /// Set internally or by persistence.
        /// </summary>
        public DateTime Modified { get; private set; }

        /// <summary>
        /// User who modified. 
        /// Set internally or by persistence.
        /// </summary>
        public CSUser ModifiedBy { get; private set; }

        /// <summary>
        /// Returns if the authenticated user is allowed to modify this workflow definition
        /// </summary>
        public bool AllowUserToModify
        {
            get;
            private set;
        }

        /// <summary>
        /// If set, the workflow will be triggered on changes in child objects that are not caught at a lower level.
        /// </summary>
        public bool AllowProcessingBubbledTriggers
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initialize the data contract with a definition instance
        /// </summary>
        /// <param name="def">Workflow definition to initialize with</param>
        public CSWorkflowDefinitionDataContract(CSWorkflowDefinition def)
        {
            if (def == null)
            {
                return;
            }

            Id = def.Id;
            Name = def.Name;
            Description = def.Description;
            DefaultAssociationData = def.DefaultAssociationData;
            AllowManualStart = def.AllowManualStart;
            StartOnCreate = def.StartOnCreate;
            StartOnModify = def.StartOnModify;
            AllowUserToModify = (((def.Farm != null) && (def.Farm.IsAuthenticatedUserFarmAdministrator)) ? true : false);
            IsEnabled = def.IsEnabled;
            Created = def.Created;
            CreatedBy = def.CreatedBy;
            Modified = def.Modified;
            ModifiedBy = def.ModifiedBy;
        }
    }
}