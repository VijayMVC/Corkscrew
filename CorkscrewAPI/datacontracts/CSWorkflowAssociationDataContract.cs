using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// Data contract representing a CSWorkflowAssociation instance
    /// </summary>
    [DataContract]
    public class CSWorkflowAssociationDataContract
    {

        #region Properties

        /// <summary>
        /// Guid of the association
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        } = Guid.Empty;

        /// <summary>
        /// Name of the workflow (as shown in "Installed/Available workflows" lists)
        /// </summary>
        public string Name
        {
            get { return _name; }
            set {
                // we dont have a way here to get the definition name like the CSWorkflowAssociation class does. 
                // so generate a dirty name using a Guid to ensure uniqueness.
                _name = value.SafeString(255, false, true, nameof(Name), "Workflow Association " + Guid.NewGuid().ToString("N"));
            }
        }
        private string _name = null;

        /// <summary>
        /// This is copied from WorkflowDefinition.DefaultAssociationData during association. 
        /// </summary>
        /// <seealso cref="CSWorkflowDefinition.DefaultAssociationData"/>
        public string CustomAssociationInformation
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Guid of the site associated with
        /// </summary>
        public Guid SiteId
        {
            get;
            set;
        }

        /// <summary>
        /// Guid of the entity associated with this workflow
        /// </summary>
        public Guid AssociatedEntityId
        {
            get;
            set;
        }

        /// <summary>
        /// SiteId should be used only if this is false
        /// </summary>
        public bool IsFarmAssociation
        {
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
        /// Is the workflow association enabled
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        } = false;  // is only enabled when a manifest exists


        /// <summary>
        /// Date and time of creation. 
        /// Set by constructor.
        /// </summary>
        public DateTime Created { get; internal set; } = DateTime.Now;

        /// <summary>
        /// User who created.
        /// Set by constructor.
        /// </summary>
        public CSUser CreatedBy { get; internal set; } = null;

        /// <summary>
        /// Date and time of modification. 
        /// Set internally or by persistence.
        /// </summary>
        public DateTime Modified { get; internal set; } = DateTime.Now;

        /// <summary>
        /// User who modified. 
        /// Set internally or by persistence.
        /// </summary>
        public CSUser ModifiedBy { get; internal set; } = null;

        /// <summary>
        /// If set, the workflow will be triggered on changes in child objects that are not caught at a lower level.
        /// </summary>
        public bool AllowProcessingBubbledTriggers
        {
            get;
            set;
        }

        /// <summary>
        /// Returns if the authenticated user can modify this association
        /// </summary>
        public bool CanUserModifyWorkflowAssociation
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initialize the data contract with a definition instance
        /// </summary>
        /// <param name="assoc">Workflow association to initialize with</param>
        public CSWorkflowAssociationDataContract(CSWorkflowAssociation assoc)
        {
            if (assoc == null)
            {
                return;
            }

            Id = assoc.Id;
            Name = assoc.Name;
            CustomAssociationInformation = assoc.CustomAssociationInformation;
            AssociatedEntityId = Guid.Empty;
            if (assoc.Site == null)
            {
                IsFarmAssociation = true;
                SiteId = Guid.Empty;
            }
            else
            {
                if (assoc.Site.IsConfigSite)
                {
                    IsFarmAssociation = true;
                    SiteId = Guid.Empty;
                }
                else
                {
                    IsFarmAssociation = false;
                    SiteId = assoc.Site.Id;
                }
            }

            if (assoc.AssociatedEntity != null)
            {
                AssociatedEntityId = assoc.AssociatedEntity.Id;
            }

            AllowManualStart = assoc.AllowManualStart;
            StartOnCreate = assoc.StartOnCreate;
            StartOnModify = assoc.StartOnModify;
            IsEnabled = assoc.IsEnabled;
            Created = assoc.Created;
            CreatedBy = assoc.CreatedBy;
            Modified = assoc.Modified;
            ModifiedBy = assoc.ModifiedBy;
            AllowProcessingBubbledTriggers = assoc.AllowProcessingBubbledTriggers;
            CanUserModifyWorkflowAssociation = assoc.CanUserModifyWorkflowAssociation;

        }

    }
}