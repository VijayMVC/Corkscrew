using Corkscrew.SDK.constants;
using Corkscrew.SDK.exceptions;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Association of a Corkscrew workflow to a target
    /// </summary>
    public class CSWorkflowAssociation : IDisposable
    {

        #region Properties

        /// <summary>
        /// Guid of the association
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        } = Guid.Empty;

        /// <summary>
        /// Name of the workflow (as shown in "Installed/Available workflows" lists)
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value.SafeString(255, false, true, nameof(Name), string.Format("{0} Workflow Association", WorkflowDefinition.Name)); }
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
        /// Workflow definition
        /// </summary>
        public CSWorkflowDefinition WorkflowDefinition
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Associated farm, will always return the current farm.
        /// </summary>
        public CSFarm Farm
        {
            get { return ((WorkflowDefinition == null) ? null : WorkflowDefinition.Farm); }
        }

        /// <summary>
        /// Associated Site 
        /// (will be NULL only for Farm level workflows)
        /// </summary>
        public CSSite Site
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// The entity associated with the workflow. Is never NULL.
        /// </summary>
        public IWorkflowAssociable AssociatedEntity
        {
            get;
            internal set;
        } = null;

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
            internal set;
        } = false;  // is only enabled when a manifest exists


        /// <summary>
        /// Set when there are workflows in progress and someone modifies or 
        /// deletes the association. 
        /// 
        /// When this is set, no new instances are startable.
        /// </summary>
        internal bool PreventStartingNewInstances
        {
            get;
            set;
        } = false;

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
        /// Event triggers that are allowed by this Workflow Definition
        /// </summary>
        internal CSWorkflowTriggerEvents AllowedTriggers
        {
            get
            {
                if (_allowedTriggers == null)
                {
                    _allowedTriggers = new CSWorkflowTriggerEvents();
                    (new OdmWorkflow(Farm, Site)).GetAssociationTriggers(this, _allowedTriggers);
                }

                return _allowedTriggers;
            }
        }
        private CSWorkflowTriggerEvents _allowedTriggers = null;

        /// <summary>
        /// If set, the workflow will be triggered on changes in child objects that are not caught at a lower level.
        /// </summary>
        public bool AllowProcessingBubbledTriggers
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the scope set in the association
        /// </summary>
        public ScopeEnum WorkflowScope
        {
            get
            {
                if ((AssociatedEntity != null) && (!(AssociatedEntity is CSSite)) && (!(AssociatedEntity is CSFarm)))
                {
                    // could be any number of things, but let's call it a directory (a container)
                    return ScopeEnum.Directory;
                }

                if (Site != null)
                {
                    return ScopeEnum.Site;
                }

                return ScopeEnum.Farm;
            }
        }

        /// <summary>
        /// Returns all the available instances for this workflow
        /// </summary>
        public CSWorkflowInstanceCollection AllInstances
        {
            get
            {
                if (_instances == null)
                {
                    _instances = CSWorkflowInstanceCollection.GetInstances(this, false);
                }
                return _instances;
            }
        }
        private CSWorkflowInstanceCollection _instances = null;

        /// <summary>
        /// Returns the runnable instances for this workflow
        /// </summary>
        public CSWorkflowInstanceCollection AllRunnableInstances
        {
            get
            {
                if (_runnableInstances == null)
                {
                    _instances = CSWorkflowInstanceCollection.GetInstances(this, true);
                }
                return _runnableInstances;
            }
        }
        private CSWorkflowInstanceCollection _runnableInstances = null;

        /// <summary>
        /// Returns if the authenticated user can modify this association
        /// </summary>
        public bool CanUserModifyWorkflowAssociation
        {
            get
            {
                CSPermission acl = CSPermission.TestAccess(AssociatedEntity.FullPath, Farm.AuthenticatedUser);
                if (acl.CanFullControl)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region Constructors

        // blank constructor
        internal CSWorkflowAssociation()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            Modified = DateTime.Now;
        }

        /// <summary>
        /// Create a farm level workflow association
        /// </summary>
        /// <param name="definition">Workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <returns>The created association or NULL</returns>
        /// <exception cref="ArgumentNullException">If definition or name are null</exception>
        /// <exception cref="UnauthorizedAccessException">If user is not a farm administrator</exception>
        /// <exception cref="CSWorkflowException">If definition is not enabled or it does not have a manifest</exception>
        public static CSWorkflowAssociation CreateFarmAssociation(CSWorkflowDefinition definition, string name)
        {
            CSExceptionHelper.ThrowIfNull(definition, name);

            if (! definition.Farm.IsWorkflowEnabled)
            {
                throw new InvalidOperationException("Workflows are disabled in this farm.");
            }

            // user must be a farm admin
            if (! definition.Farm.IsAuthenticatedUserFarmAdministrator)
            {
                throw new UnauthorizedAccessException("Only farm administrators may create farm workflows.");
            }

            if (! definition.IsEnabled)
            {
                throw new CSWorkflowException("Cannot create association. Workflow definition is disabled.");
            }

            if (definition.GetManifest() == null)
            {
                throw new CSWorkflowException("Cannot create association. Workflow definition has no manifest.");
            }

            CSWorkflowAssociation assoc = new CSWorkflowAssociation()
            {
                WorkflowDefinition = definition,
                Name = name,
                CreatedBy = definition.Farm.AuthenticatedUser,
                ModifiedBy = definition.Farm.AuthenticatedUser
            };

            if (!assoc.CreateAssociationHelper())
            {
                return null;
            }

            return assoc;
        }

        /// <summary>
        /// Create a site level workflow association
        /// </summary>
        /// <param name="definition">Workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <param name="site">Site to associate to</param>
        /// <returns>The created association or NULL</returns>
        /// <exception cref="ArgumentNullException">If definition or name or site are null</exception>
        /// <exception cref="UnauthorizedAccessException">If user is not a farm or site administrator</exception>
        /// <exception cref="CSWorkflowException">If definition is not enabled or it does not have a manifest</exception>
        public static CSWorkflowAssociation CreateSiteAssociation(CSWorkflowDefinition definition, string name, CSSite site)
        {

            CSExceptionHelper.ThrowIfNull(definition, name, site);

            if (!definition.Farm.IsWorkflowEnabled)
            {
                throw new InvalidOperationException("Workflows are disabled in this farm.");
            }

            // user must be a site admin
            if (! site.IsAuthenticatedUserSiteAdministrator)
            {
                throw new UnauthorizedAccessException("Only farm or site administrators may create site workflows.");
            }

            if (!definition.IsEnabled)
            {
                throw new CSWorkflowException("Cannot create association. Workflow definition is disabled.");
            }

            if (definition.GetManifest() == null)
            {
                throw new CSWorkflowException("Cannot create association. Workflow definition has no manifest.");
            }

            CSWorkflowAssociation assoc = new CSWorkflowAssociation()
            {
                WorkflowDefinition = definition,
                Name = name,
                Site = site,
                CreatedBy = site.AuthenticatedUser,
                ModifiedBy = site.AuthenticatedUser
            };

            if (!assoc.CreateAssociationHelper())
            {
                return null;
            }

            return assoc;
        }

        /// <summary>
        /// Create a directory level workflow association
        /// </summary>
        /// <param name="definition">Workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <param name="directory">Directory to associate to</param>
        /// <returns>The created association or NULL</returns>
        /// <exception cref="ArgumentNullException">If definition or name or directory are null</exception>
        /// <exception cref="UnauthorizedAccessException">If user is not an administrator or does not have FullControl permission on the directory</exception>
        /// <exception cref="CSWorkflowException">If definition is not enabled or it does not have a manifest</exception>
        public static CSWorkflowAssociation CreateDirectoryAssociation(CSWorkflowDefinition definition, string name, CSFileSystemEntryDirectory directory)
        {

            CSExceptionHelper.ThrowIfNull(definition, name, directory);

            if (!definition.Farm.IsWorkflowEnabled)
            {
                throw new InvalidOperationException("Workflows are disabled in this farm.");
            }

            // user must be have full control on the entry
            CSPermission acl = CSPermission.TestAccess(directory.Site, directory, directory.Site.AuthenticatedUser);
            if (!acl.CanFullControl)
            {
                throw new UnauthorizedAccessException("Only farm, site administrators or users with full control permissions on a directory may create workflows at this level.");
            }

            if (!definition.IsEnabled)
            {
                throw new CSWorkflowException("Cannot create association. Workflow definition is disabled.");
            }

            if (definition.GetManifest() == null)
            {
                throw new CSWorkflowException("Cannot create association. Workflow definition has no manifest.");
            }

            CSWorkflowAssociation assoc = new CSWorkflowAssociation()
            {
                WorkflowDefinition = definition,
                Name = name,
                Site = directory.Site,
                AssociatedEntity = directory,
                CreatedBy = directory.Site.AuthenticatedUser,
                ModifiedBy = directory.Site.AuthenticatedUser
            };

            if (!assoc.CreateAssociationHelper())
            {
                return null;
            }

            return assoc;
        }

        #endregion

        #region Methods
        private void ThrowIfNotAllowedToModify()
        {
            if (! CanUserModifyWorkflowAssociation)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow association. User is not authorized.");
            }
        }
        
        // this is called from one of the CreateAssociation* constructors, rights check is done before this method is called!
        // helper to create the association record in the database
        private bool CreateAssociationHelper()
        {
            if (string.IsNullOrEmpty(CustomAssociationInformation) && (! string.IsNullOrEmpty(WorkflowDefinition.DefaultAssociationData)))
            {
                CustomAssociationInformation = WorkflowDefinition.DefaultAssociationData;
            }

            // enable the association only if definition already has a manifest
            IsEnabled = (WorkflowDefinition.GetManifest() != null);

            return (new OdmWorkflow(Farm, Site)).CreateWorkflowAssociation(this);
        }

        /// <summary>
        /// Subscribe to a workflow event
        /// </summary>
        /// <param name="eventName">The event trigger name</param>
        /// <exception cref="UnauthorizedAccessException">If the user does not have sufficient privileges to modify the association</exception>
        public void SubscribeWorkflowEvent(WorkflowTriggerEventNamesEnum eventName)
        {
            if (!WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow association. User is not authorized.");
            }

            AllowedTriggers.Add(eventName, false);

            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;
            (new OdmWorkflow(Farm, Site)).UpdateEventSubscriptions(this, AllowedTriggers);
        }

        /// <summary>
        /// Unsubscribe from a workflow event
        /// </summary>
        /// <param name="eventName">The event trigger name</param>
        /// <exception cref="UnauthorizedAccessException">If the user does not have sufficient privileges to modify the association</exception>
        public void UnsubscribeWorkflowEvent(WorkflowTriggerEventNamesEnum eventName)
        {
            if (!WorkflowDefinition.AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow association. User is not authorized.");
            }

            AllowedTriggers.Remove(eventName, false);

            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;
            (new OdmWorkflow(Farm, Site)).UpdateEventSubscriptions(this, AllowedTriggers);
        }

        /// <summary>
        /// Returns if the given event is subscribed to
        /// </summary>
        /// <param name="eventName">Trigger to check</param>
        /// <returns>True if registered</returns>
        public bool IsEventSubscribed(WorkflowTriggerEventNamesEnum eventName)
        {
            return AllowedTriggers.Has(eventName);
        }

        /// <summary>
        /// Save changes made to the workflow association
        /// </summary>
        /// <returns>Success of the save operation</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have sufficient privileges to modify the association</exception>
        public bool Save()
        {
            ThrowIfNotAllowedToModify();

            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;

            return (new OdmWorkflow(Farm, Site)).SaveChanges(this);
        }

        /// <summary>
        /// Delete the association
        /// </summary>
        /// <returns>Success of deletion</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have sufficient privileges to delete the association</exception>
        public bool Delete()
        {
            ThrowIfNotAllowedToModify();

            return (new OdmWorkflow(Farm, Site)).DeleteWorkflowAssociation(this);
        }

        /// <summary>
        /// Enable the workflow association
        /// </summary>
        /// <returns>Success of the operation</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have sufficient privileges to modify the association</exception>
        public bool Enable()
        {

            if (IsEnabled)
            {
                return true;
            }

            ThrowIfNotAllowedToModify();

            IsEnabled = true;
            return Save();
        }

        /// <summary>
        /// Disable the workflow association
        /// </summary>
        /// <returns>Success of the operation</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have sufficient privileges to modify the association</exception>
        public bool Disable()
        {

            if (! IsEnabled)
            {
                return true;
            }

            ThrowIfNotAllowedToModify();

            IsEnabled = false;
            return Save();
        }

        /// <summary>
        /// Get the history for this association
        /// </summary>
        /// <returns>Workflow history chain (linked list)</returns>
        public CSWorkflowHistoryChain GetHistory()
        {
            return (new OdmWorkflow(Farm, Site)).GetHistoryByAssociation(this);
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose the definition
        /// </summary>
        /// <param name="disposing">True if really disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_allowedTriggers != null) _allowedTriggers.Dispose();
                    if (AssociatedEntity != null) AssociatedEntity = null;
                    if (_instances != null) _instances.Dispose();
                    if (_runnableInstances != null) _runnableInstances.Dispose();
                    if (Site != null) Site.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose the definition
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    /// <summary>
    /// Collection of workflow associations.
    /// </summary>
    public class CSWorkflowAssociationCollection : CSBaseCollection<CSWorkflowAssociation>
    {

        #region Constructors

        // internal blank constructor
        internal CSWorkflowAssociationCollection(bool isReadonly = false) : base(isReadonly) { }

        /// <summary>
        /// Creates a collection of associations, populated with Farm associated workflows
        /// </summary>
        /// <param name="farm">Farm to populate collection</param>
        /// <param name="isReadonly">Flag to set the collection as readonly</param>
        /// <exception cref="ArgumentNullException">If farm is null</exception>
        public CSWorkflowAssociationCollection(CSFarm farm, bool isReadonly = false)
            : base((((farm == null) || (!farm.IsAuthenticatedUserFarmAdministrator) || (!farm.IsWorkflowEnabled)) ? true : isReadonly))
        {
            if (farm == null)
            {
                throw new ArgumentNullException();
            }

            ImportItemsFromListHelper((new OdmWorkflow(farm, null)).GetAllWorkflowAssociationsForFarm());
        }

        /// <summary>
        /// Creates a collection of associations, populated with workflows associated to the given Site
        /// </summary>
        /// <param name="site">Site to open for</param>
        /// <param name="isReadonly">Flag to set the collection as readonly</param>
        /// <exception cref="ArgumentNullException">If site is null</exception>
        public CSWorkflowAssociationCollection(CSSite site, bool isReadonly = false)
            : base((((site == null) || (!site.Farm.IsAuthenticatedUserFarmAdministrator) || (!site.Farm.IsWorkflowEnabled)) ? true : isReadonly))
        {
            if (site == null)
            {
                throw new ArgumentNullException();
            }

            ImportItemsFromListHelper((new OdmWorkflow(site.Farm, site)).GetAllWorkflowAssociationsForSite(site));
        }

        /// <summary>
        /// Creates a collection of associations, populated with workflows associated to the given Directory
        /// </summary>
        /// <param name="directory">Directory to populate for</param>
        /// <param name="isReadonly">Flag to set the collection as readonly</param>
        /// <exception cref="ArgumentNullException">If directory is null</exception>
        public CSWorkflowAssociationCollection(CSFileSystemEntryDirectory directory, bool isReadonly = false)
            : base((((directory == null) || (!directory.Farm.IsAuthenticatedUserFarmAdministrator) || (!directory.Farm.IsWorkflowEnabled)) ? true : isReadonly))
        {
            if (directory == null)
            {
                throw new ArgumentNullException();
            }

            ImportItemsFromListHelper((new OdmWorkflow(directory.Site.Farm, directory.Site)).GetAllWorkflowAssociationsForDirectory(directory.Site, directory));
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Add association to collection
        /// </summary>
        /// <param name="association">Association to add</param>
        /// <exception cref="ArgumentNullException">If association is null</exception>
        public void Add(CSWorkflowAssociation association)
        {
            CSExceptionHelper.ThrowIfNull(association);

            if (! association.WorkflowDefinition.Farm.IsWorkflowEnabled)
            {
                throw new InvalidOperationException("Workflows are disabled in this farm.");
            }

            if (! association.WorkflowDefinition.Farm.IsWorkflowEnabled)
            {
                throw new InvalidOperationException("Workflows are disabled in this farm.");
            }

            AddInternal(association, true);
        }

        /// <summary>
        /// Remove association from collection
        /// </summary>
        /// <param name="association">Association to remove</param>
        /// <exception cref="ArgumentNullException">If association is null</exception>
        public void Remove(CSWorkflowAssociation association)
        {
            CSExceptionHelper.ThrowIfNull(association);
            RemoveInternal(association);
        }

        /// <summary>
        /// Find an association with its Guid
        /// </summary>
        /// <param name="id">Guid of the association to find</param>
        /// <returns>Association found or NULL</returns>
        public CSWorkflowAssociation Find(Guid id)
        {
            foreach (CSWorkflowAssociation assn in base.Collection)
            {
                if (assn.Id.Equals(id))
                {
                    return assn;
                }
            }

            return null;
        }

        /// <summary>
        /// Find associations created for the given definition
        /// </summary>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <returns>IEnumerable of workflow associations found or empty list.</returns>
        public IEnumerable<CSWorkflowAssociation> FindByDefinition(Guid definitionId)
        {
            List<CSWorkflowAssociation> list = new List<CSWorkflowAssociation>();
            foreach(CSWorkflowAssociation assn in base.Collection)
            {
                if (assn.WorkflowDefinition.Id.Equals(definitionId))
                {
                    list.Add(assn);
                }
            }

            return list;
        }

        /// <summary>
        /// Find associations created for the associated entity
        /// </summary>
        /// <param name="entityId">Guid of the entity</param>
        /// <returns>IEnumerable of workflow associations found or empty list.</returns>
        public IEnumerable<CSWorkflowAssociation> FindByAssociatedEntityId(Guid entityId)
        {
            List<CSWorkflowAssociation> list = new List<CSWorkflowAssociation>();
            foreach (CSWorkflowAssociation assn in base.Collection)
            {
                if ((assn.WorkflowScope == ScopeEnum.Directory) && (assn.AssociatedEntity != null) && (assn.AssociatedEntity.Id.Equals(entityId)))
                {
                    list.Add(assn);
                }
            }

            return list;
        }

        #endregion


        private void ImportItemsFromListHelper(IEnumerable<CSWorkflowAssociation> list)
        {
            foreach (CSWorkflowAssociation item in list)
            {
                base.AddInternal(item, false);
            }
        }

    }
}
