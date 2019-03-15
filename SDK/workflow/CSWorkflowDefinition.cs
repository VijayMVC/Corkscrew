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
    /// Represents the definition of a single workflow. Each workflow can be defined only once, a farm may contain multiple workflow definitions.
    /// </summary>
    public class CSWorkflowDefinition : IDisposable
    {
        
        #region Properties

        /// <summary>
        /// Reference to the Farm
        /// </summary>
        public CSFarm Farm
        {
            get;
            internal set;
        }

        /// <summary>
        /// Workflow definition Id
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
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
            internal set;
        } = true;

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
                    (new OdmWorkflow(Farm)).GetWorkflowTriggers(this, _allowedTriggers);
                }

                return _allowedTriggers;
            }
        }
        private CSWorkflowTriggerEvents _allowedTriggers = null;

        /// <summary>
        /// Returns if the authenticated user is allowed to modify this workflow definition
        /// </summary>
        internal bool AllowUserToModify
        {
            get
            {
                if (Farm != null)
                {
                    return Farm.IsAuthenticatedUserFarmAdministrator;
                }

                return false;
            }
        }

        /// <summary>
        /// If set, the workflow will be triggered on changes in child objects that are not caught at a lower level.
        /// </summary>
        public bool AllowProcessingBubbledTriggers
        {
            get;
            set;
        }

        /// <summary>
        /// Associations of this definition
        /// </summary>
        public CSWorkflowAssociationCollection Associations
        {
            get
            {
                if (_associations == null)
                {
                    _associations = new CSWorkflowAssociationCollection(Farm, Farm.IsAuthenticatedUserFarmAdministrator);
                }

                return _associations;
            }
        }
        private CSWorkflowAssociationCollection _associations = null;

        /// <summary>
        /// Returns all the available instances for this workflow
        /// </summary>
        public CSWorkflowInstanceCollection AllInstances
        {
            get
            {
                if (_instances == null)
                {
                    _instances = CSWorkflowInstanceCollection.GetInstances(this);
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
                    _runnableInstances = CSWorkflowInstanceCollection.GetInstances(this, true);
                }
                return _runnableInstances;
            }
        }
        private CSWorkflowInstanceCollection _runnableInstances = null;

        #endregion

        #region Constructors

        // default internal constructor
        internal CSWorkflowDefinition()
        {
            DateTime now = DateTime.Now;
            Created = now;
            Modified = now;
            Farm = null;
        }

        /// <summary>
        /// Get a definition by name
        /// </summary>
        /// <param name="farm">The farm to get the definition for</param>
        /// <param name="name">Name of workflow</param>
        /// <returns>Workflow definition</returns>
        public static CSWorkflowDefinition Get(CSFarm farm, string name)
        {
            return (new OdmWorkflow(farm)).GetWorkflowDefinition(name);
        }

        /// <summary>
        /// Get a definition by id
        /// </summary>
        /// <param name="farm">The farm to get the definition for</param>
        /// <param name="id">Guid of workflow</param>
        /// <returns>Workflow definition</returns>
        public static CSWorkflowDefinition Get(CSFarm farm, Guid id)
        {
            return (new OdmWorkflow(farm)).GetWorkflowDefinition(id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register a workflow trigger
        /// </summary>
        /// <param name="trigger">The event trigger</param>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public void RegisterTrigger(WorkflowTriggerEventNamesEnum trigger)
        {
            ThrowIfNotAllowedToModify();

            AllowedTriggers.Add(trigger, false);

            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;
            (new OdmWorkflow(Farm)).UpdateEventSubscriptions(this, AllowedTriggers);
        }

        /// <summary>
        /// De-register a workflow trigger
        /// </summary>
        /// <param name="trigger">The event trigger</param>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public void DeregisterTrigger(WorkflowTriggerEventNamesEnum trigger)
        {
            ThrowIfNotAllowedToModify();

            AllowedTriggers.Remove(trigger, false);

            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;
            (new OdmWorkflow(Farm)).UpdateEventSubscriptions(this, AllowedTriggers);
        }

        /// <summary>
        /// Returns if there is already a registration for the given trigger event
        /// </summary>
        /// <param name="trigger">Trigger to check</param>
        /// <returns>True if trigger is registered</returns>
        public bool IsTriggerRegistered(WorkflowTriggerEventNamesEnum trigger)
        {
            return AllowedTriggers.Has(trigger);
        }

        /// <summary>
        /// Returns if the definition contains any enabled events for given scope
        /// </summary>
        /// <param name="scope">Scope to check (only Farm, Site and Directory are valid)</param>
        /// <returns>True if scope has events</returns>
        /// <remarks>This function will also return true if AllowProcessingBubbledTriggers is set and child-item events are allowed</remarks>
        public bool HasEventsForScope(ScopeEnum scope)
        {
            switch (scope)
            {
                case ScopeEnum.Farm:
                    if (AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.farm_created) 
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.farm_modified) 
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.farm_deleted)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.site_created)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.site_modified)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.site_deleted)
                        || AllowProcessingBubbledTriggers)
                    {
                        return true;
                    }
                    break;

                case ScopeEnum.Site:
                    if (AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.directory_created)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.directory_modified)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.directory_deleted)
                        || AllowProcessingBubbledTriggers)
                    {
                        return true;
                    }
                    break;

                case ScopeEnum.Directory:
                    if (AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.file_created)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.file_modified)
                        || AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.file_deleted))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// Creates a workflow manifest
        /// </summary>
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
        public CSWorkflowManifest CreateManifest(WorkflowEngineEnum engine, string assemblyName, string className, bool alwaysCompile, bool cacheCompileResults)
        {
            ThrowIfNotAllowedToModify();
            return CSWorkflowManifest.CreateManifest(this, engine, assemblyName, className, alwaysCompile, cacheCompileResults);
        }

        /// <summary>
        /// Get the workflow manifest for this definition
        /// </summary>
        /// <returns>The workflow manifest or NULL</returns>
        /// <remarks>There can be only one manifest for a workflow</remarks>
        public CSWorkflowManifest GetManifest()
        {
            return (new OdmWorkflow(Farm)).GetWorkflowManifestForDefinition(this);
        }


        /// <summary>
        /// Save changes made to the definition
        /// </summary>
        /// <returns>Success of saving the changes</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public bool Save()
        {
            ThrowIfNotAllowedToModify();

            // validate!
            List<CSCompilerValidationError> errors = ValidateDefinition();
            if (errors.Count > 0)
            {
                throw new CSWorkflowException("Cannot commit workflow definition. One or more errors.", CSExceptionHelper.GetExceptionFromValidationErrors(errors));
            }

            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;
            return (new OdmWorkflow(Farm)).SaveChanges(this);
        }

        /// <summary>
        /// Enable the workflow definition
        /// </summary>
        /// <returns>Success of the operation</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public bool Enable()
        {
            if (IsEnabled)
            {
                return true;
            }

            ThrowIfNotAllowedToModify();

            IsEnabled = true;
            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;

            return (new OdmWorkflow(Farm)).SaveChanges(this);
        }

        /// <summary>
        /// Disable the workflow definition
        /// </summary>
        /// <returns>Success of the operation</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public bool Disable()
        {
            if (!IsEnabled)
            {
                return true;
            }

            ThrowIfNotAllowedToModify();

            IsEnabled = false;
            Modified = DateTime.Now;
            ModifiedBy = Farm.AuthenticatedUser;

            return (new OdmWorkflow(Farm)).SaveChanges(this);
        }

        /// <summary>
        /// Deletes this definition
        /// </summary>
        /// <returns>Success of deletion</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to delete the workflow definition</exception>
        public bool Delete()
        {
            ThrowIfNotAllowedToModify();

            return (new OdmWorkflow(Farm)).DeleteWorkflowDefinition(this);
        }

        /// <summary>
        /// Delete all associations for definition
        /// </summary>
        /// <returns>Success of deletion</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public bool DeleteAllAssociations()
        {
            ThrowIfNotAllowedToModify();

            return (new OdmWorkflow(Farm)).DeleteAssociationsForWorkflowDefinition(this);
        }

        /// <summary>
        /// Validates if the definition is good
        /// </summary>
        /// <returns>A list of the errors found during validation</returns>
        public List<CSCompilerValidationError> ValidateDefinition()
        {
            List<CSCompilerValidationError> result = new List<CSCompilerValidationError>();

            if ((!AllowManualStart) && (!StartOnCreate) && (!StartOnModify) && (! AllowedTriggers.Has(WorkflowTriggerEventNamesEnum.None)))
            {
                result.Add(
                    new CSCompilerValidationError(
                        "Either startup type has not been set, or no events have been allowed.",
                        200,
                        false,  // error, not warning
                        "Startup Types and Allowed Events"
                    )
                );
            }

            return result;
        }

        #region Create Associations

        /// <summary>
        /// Create a farm level workflow association
        /// </summary>
        /// <param name="name">Name for the association</param>
        /// <returns>The created association or NULL</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public CSWorkflowAssociation CreateFarmAssociation(string name)
        {
            ThrowIfNotAllowedToModify();

            return CSWorkflowAssociation.CreateFarmAssociation(this, name);
        }

        /// <summary>
        /// Create a site level workflow association
        /// </summary>
        /// <param name="name">Name for the association</param>
        /// <param name="site">Site to associate to</param>
        /// <returns>The created association or NULL</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public CSWorkflowAssociation CreateSiteAssociation(string name, CSSite site)
        {
            ThrowIfNotAllowedToModify();

            return CSWorkflowAssociation.CreateSiteAssociation(this, name, site);
        }

        /// <summary>
        /// Create a directory level workflow association
        /// </summary>
        /// <param name="name">Name for the association</param>
        /// <param name="directory">Directory to associate to</param>
        /// <returns>The created association or NULL</returns>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        public CSWorkflowAssociation CreateDirectoryAssociation(string name, CSFileSystemEntryDirectory directory)
        {
            ThrowIfNotAllowedToModify();

            return CSWorkflowAssociation.CreateDirectoryAssociation(this, name, directory);
        }

        #endregion

        /// <summary>
        /// Throws an UnauthorizedAccessException if the user cannot modify the workflow (i.e., authenticated user is not a Farm admin)
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">If the user does not have permissions to modify the workflow definition</exception>
        internal void ThrowIfNotAllowedToModify()
        {
            if (! AllowUserToModify)
            {
                throw new UnauthorizedAccessException("Cannot modify workflow definition. User is not authorized.");
            }
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose the definition
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_runnableInstances != null) _runnableInstances.Dispose();
                    if (_instances != null) _instances.Dispose();
                    if (_associations != null) _associations.Dispose();
                    if (_allowedTriggers != null) _allowedTriggers.Dispose();
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
    /// Collection of workflow definitions
    /// </summary>
    public class CSWorkflowDefinitionCollection : CSBaseCollection<CSWorkflowDefinition>
    {

        #region Properties
        /// <summary>
        /// Returns reference to the farm this workflow collection belongs to
        /// </summary>
        public CSFarm Farm
        {
            get;
            internal set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a blank collection of definitions
        /// </summary>
        /// <param name="isReadonly">Flag to indicate whether to open it in readonly mode</param>
        private CSWorkflowDefinitionCollection(bool isReadonly) : base(isReadonly)
        {
            Farm = null;
        }

        /// <summary>
        /// Open a collection of definitions
        /// </summary>
        /// <param name="farm">The farm this collection belongs to</param>
        /// <param name="isReadonly">Flag to indicate whether to open it in readonly mode</param>
        public CSWorkflowDefinitionCollection(CSFarm farm, bool isReadonly)
            : base(
                  ((farm.IsWorkflowEnabled ? (new OdmWorkflow(farm)).GetAllWorkflowDefinitions() : new List<CSWorkflowDefinition>())),
                  (((farm == null) || (! farm.IsAuthenticatedUserFarmAdministrator) || (!farm.IsWorkflowEnabled)) ? true : isReadonly)
              )
        {
            if (farm == null)
            {
                throw new ArgumentNullException("farm");
            }

            Farm = farm;
        }

        /// <summary>
        /// Returns a collection from provided enumeration. Collection is readonly.
        /// </summary>
        /// <param name="list">Enumeration of definitions to populate</param>
        public CSWorkflowDefinitionCollection(IEnumerable<CSWorkflowDefinition> list)
            : base(list, true)
        {
            Farm = null;
        }

        #endregion

        /// <summary>
        /// Creates a new workflow definition.
        /// </summary>
        /// <param name="name">Name of the workflow - must be unique in the Farm</param>
        /// <param name="description">Description of the workflow</param>
        /// <param name="defaultAssociationData">Default association data (serialized)</param>
        /// <param name="startWorkflowOnCreate">Allow workflow to automatically start on new item creation</param>
        /// <param name="startWorkflowOnModify">Allow workflow to automatically start on item modification</param>
        /// <param name="allowStartWorkflowManually">Allow workflow to be manually started by an administrator or the item owner</param>
        /// <returns>Workflow definition, persisted. If creation fails, returns NULL.</returns>
        /// <exception cref="IsReadonlyException">If collection is readonly</exception>
        /// <exception cref="UnauthorizedAccessException">If user is not a Farm administrator</exception>
        /// <exception cref="CSWorkflowException">If definition could not be saved</exception>
        /// <exception cref="ArgumentException">If another definition with the same name already exists</exception>
        public CSWorkflowDefinition Add(string name, string description, string defaultAssociationData, bool startWorkflowOnCreate, bool startWorkflowOnModify, bool allowStartWorkflowManually)
        {
            ThrowIfReadonly();

            if (! Farm.IsWorkflowEnabled)
            {
                throw new InvalidOperationException("Workflows are disabled in this farm.");
            }

            if (! Farm.IsAuthenticatedUserFarmAdministrator)
            {
                throw new UnauthorizedAccessException("Cannot create workflow definition. User is not authorized to modify this collection.");
            }

            if (Find(name) != null)
            {
                throw new ArgumentException("Farm already has a Workflow definition with the same name.");
            }

            CSWorkflowDefinition definition = new CSWorkflowDefinition()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DefaultAssociationData = defaultAssociationData,

                IsEnabled = true,

                StartOnCreate = startWorkflowOnCreate,
                StartOnModify = startWorkflowOnModify,
                AllowManualStart = allowStartWorkflowManually,

                Farm = Farm,

                CreatedBy = Farm.AuthenticatedUser,
                ModifiedBy = Farm.AuthenticatedUser
            };

            List<CSCompilerValidationError> errors = definition.ValidateDefinition();
            if (errors.Count > 0)
            {
                throw new CSWorkflowException("Cannot commit workflow definition. One or more errors.", CSExceptionHelper.GetExceptionFromValidationErrors(errors));
            }

            // There is no practical need to lock the farm to add a definition.

            if (! (new OdmWorkflow(Farm)).CreateWorkflowDefinition(definition))
            {
                return null;
            }

            AddInternal(definition);

            return definition;
        }

        /// <summary>
        /// Find the definition with the given Guid
        /// </summary>
        /// <param name="id">Guid of the workflow to search for</param>
        /// <returns>Workflow definition or null</returns>
        public CSWorkflowDefinition Find(Guid id)
        {
            foreach(CSWorkflowDefinition def in base.Collection)
            {
                if (def.Id.Equals(id))
                {
                    return def;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the definition with the given Guid
        /// </summary>
        /// <param name="name">Name of the workflow to search for</param>
        /// <returns>Workflow definition or null</returns>
        public CSWorkflowDefinition Find(string name)
        {
            foreach (CSWorkflowDefinition def in base.Collection)
            {
                if (def.Name.Equals(name))
                {
                    return def;
                }
            }

            return null;
        }

    }

}
