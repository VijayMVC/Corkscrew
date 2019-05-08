using Corkscrew.API.business;
using Corkscrew.API.datacontracts;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace Corkscrew.API.services
{

    /// <summary>
    /// This service allows interaction with the workflow system in Corkscrew. 
    /// </summary>
    public class Workflows : IWorkflows
    {
        private CSFarm GetFarm(string token)
        {
            CSUser user = Tools.GetCurrentlyAuthenticatedUser(token);
            if (user == null)
            {
                throw new FaultException("Token is not valid.");
            }

            CSFarm farm = CSFarm.Open(user);
            if (farm == null)
            {
                throw new FaultException("Farm could not be opened.");
            }

            return farm;
        }

        /// <summary>
        /// Returns all the workflow definitions in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of workflow definitions</returns>
        public CSWorkflowDefinitionDataContract[] GetAllDefinitions(string token)
        {
            List<CSWorkflowDefinitionDataContract> result = new List<CSWorkflowDefinitionDataContract>();
            foreach (CSWorkflowDefinition def in GetFarm(token).AllWorkflowDefinitions)
            {
                result.Add(new CSWorkflowDefinitionDataContract(def));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns a single workflow definition given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <returns>A single workflow definition</returns>
        public CSWorkflowDefinitionDataContract GetDefinitionById(string token, Guid id)
        {
            return new CSWorkflowDefinitionDataContract(GetFarm(token).AllWorkflowDefinitions.Find(id));
        }

        /// <summary>
        /// Returns all the associations for a given workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <returns>Array of workflow associations</returns>
        public CSWorkflowAssociationDataContract[] GetAllAssociationsForDefinition(string token, Guid definitionId)
        {
            List<CSWorkflowAssociationDataContract> result = new List<CSWorkflowAssociationDataContract>();

            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def != null)
            {
                foreach (CSWorkflowAssociation assoc in def.Associations)
                {
                    result.Add(new CSWorkflowAssociationDataContract(assoc));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns a single workflow association given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <returns>A single workflow association</returns>
        public CSWorkflowAssociationDataContract GetAssociationById(string token, Guid id)
        {
            CSWorkflowDefinitionCollection definitions = GetFarm(token).AllWorkflowDefinitions;
            foreach (CSWorkflowDefinition def in definitions)
            {
                foreach (CSWorkflowAssociation assoc in def.Associations)
                {
                    if (assoc.Id.Equals(id))
                    {
                        return new CSWorkflowAssociationDataContract(assoc);
                    }
                }
            }

            return new CSWorkflowAssociationDataContract(null);
        }

        /// <summary>
        /// Returns the manifest for a workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <returns>A single workflow manifest (there can only be one manifest for a definition)</returns>
        public CSWorkflowManifestDataContract GetManifestForDefinition(string token, Guid definitionId)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def != null)
            {
                return new CSWorkflowManifestDataContract(def.GetManifest());
            }

            return new CSWorkflowManifestDataContract(null);
        }

        /// <summary>
        /// Returns all the items in a manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="workflowDefinitionId">Guid of the workflow definition</param>
        /// <returns>Array of manifest items</returns>
        public CSWorkflowManifestItemDataContract[] GetManifestItemsForManifest(string token, Guid workflowDefinitionId)
        {
            List<CSWorkflowManifestItemDataContract> result = new List<CSWorkflowManifestItemDataContract>();

            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(workflowDefinitionId);
            if (def != null)
            {
                CSWorkflowManifest manifest = def.GetManifest();
                if (manifest != null)
                {
                    foreach (CSWorkflowManifestItem item in manifest.GetItems())
                    {
                        result.Add(new CSWorkflowManifestItemDataContract(item));
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets the file data for the manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="workflowDefinitionId">Guid of the workflow definition</param>
        /// <param name="manifestItemId">Id of the manifest item to get the data for</param>
        /// <returns>Byte stream of data</returns>
        public byte[] GetManifestItemData(string token, Guid workflowDefinitionId, Guid manifestItemId)
        {
            CSFarm farm = GetFarm(token);
            CSWorkflowDefinition def = farm.AllWorkflowDefinitions.Find(workflowDefinitionId);
            if (def != null)
            {
                CSWorkflowManifest manifest = def.GetManifest();
                if (manifest != null)
                {
                    foreach (CSWorkflowManifestItem item in manifest.GetItems())
                    {
                        if (item.Id.Equals(manifestItemId))
                        {
                            CSMIMEType type = farm.AllContentTypes.Find(item.FilenameExtension);
                            WebOperationContext.Current.OutgoingResponse.Headers.Add("X-Corkscrew-Content-Type", type.KnownMimeType);
                            WebOperationContext.Current.OutgoingResponse.Headers.Add("X-Corkscrew-Content-Length", item.FileContentSize.ToString());

                            return item.FileContent;
                        }
                    }
                }
            }

            WebOperationContext.Current.OutgoingResponse.Headers.Add("X-Corkscrew-Content-Type", CSMIMEType.DEFAULT_MIME_TYPE);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("X-Corkscrew-Content-Length", "0");
            return new byte[0];
        }


        /// <summary>
        /// Creates a new workflow definition in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="name">Name of the workflow - must be unique in the Farm</param>
        /// <param name="description">Description of the workflow</param>
        /// <param name="defaultAssociationData">Default association data (serialized)</param>
        /// <param name="startWorkflowOnCreate">Allow workflow to automatically start on new item creation</param>
        /// <param name="startWorkflowOnModify">Allow workflow to automatically start on item modification</param>
        /// <param name="allowStartWorkflowManually">Allow workflow to be manually started by an administrator or the item owner</param>
        /// <returns>Workflow definition, persisted. If creation fails, returns NULL.</returns>
        public Guid CreateWorkflowDefinition(string token, string name, string description, string defaultAssociationData, bool startWorkflowOnCreate, bool startWorkflowOnModify, bool allowStartWorkflowManually)
        {
            CSFarm farm = GetFarm(token);
            CSWorkflowDefinition def = farm.AllWorkflowDefinitions.Add(name, description, defaultAssociationData, startWorkflowOnCreate, startWorkflowOnModify, allowStartWorkflowManually);
            if (def == null)
            {
                return Guid.Empty;
            }

            return def.Id;
        }

        /// <summary>
        /// Updates the workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definition">Workflow definition</param>
        /// <returns>True if updated successfully</returns>
        public bool UpdateWorkflowDefinition(string token, CSWorkflowDefinitionDataContract definition)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definition.Id);
            if (def == null)
            {
                return false;
            }

            CSWorkflowDefinitionDataContract persistedDefContract = new CSWorkflowDefinitionDataContract(def);
            if (persistedDefContract != definition)
            {
                def.AllowManualStart = definition.AllowManualStart;
                def.AllowProcessingBubbledTriggers = definition.AllowProcessingBubbledTriggers;
                def.DefaultAssociationData = definition.DefaultAssociationData;
                def.Description = definition.Description;
                def.Name = definition.Name;

                bool result = def.Save();
                if (result)
                {

                    // we need to implement the enable/disable toggle here based on property value 
                    // because the service does not provide seperate methods to do this.
                    if (definition.IsEnabled != def.IsEnabled)
                    {
                        if (definition.IsEnabled)
                        {
                            result = def.Enable();
                        }
                        else
                        {
                            result = def.Disable();
                        }
                    }

                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes the workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <returns>True if deleted successfully</returns>
        public bool DeleteWorkflowDefinition(string token, Guid id)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(id);
            if (def == null)
            {
                return false;
            }

            return def.Delete();
        }

        /// <summary>
        /// Deletes all the associations for the given workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <returns>True if deleted successfully</returns>
        public bool DeleteAssociationsForDefinition(string token, Guid id)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(id);
            if (def == null)
            {
                return false;
            }

            return def.DeleteAllAssociations();
        }

        /// <summary>
        /// Create a farm level workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <returns>Guid of the created association or NULL</returns>
        public Guid CreateWorkflowAssociationFarm(string token, Guid definitionId, string name)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return Guid.Empty;
            }

            CSWorkflowAssociation assoc = def.CreateFarmAssociation(name);
            if (assoc == null)
            {
                return Guid.Empty;
            }

            return assoc.Id;
        }

        /// <summary>
        /// Create a site level workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <param name="site">Guid of the site</param>
        /// <returns>Guid of the created association or NULL</returns>
        public Guid CreateWorkflowAssociationSite(string token, Guid definitionId, string name, Guid site)
        {
            CSFarm farm = GetFarm(token);

            CSWorkflowDefinition def = farm.AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return Guid.Empty;
            }

            CSSite siteInstance = farm.AllSites.Find(site);
            if (siteInstance == null)
            {
                return Guid.Empty;
            }

            CSWorkflowAssociation assoc = def.CreateSiteAssociation(name, siteInstance);
            if (assoc == null)
            {
                return Guid.Empty;
            }

            return assoc.Id;
        }

        /// <summary>
        /// Create a directory level workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <param name="site">Guid of the site</param>
        /// <param name="directory">Guid of the directory</param>
        /// <returns>Guid of the created association or NULL</returns>
        public Guid CreateWorkflowAssociationDirectory(string token, Guid definitionId, string name, Guid site, Guid directory)
        {
            CSFarm farm = GetFarm(token);

            CSWorkflowDefinition def = farm.AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return Guid.Empty;
            }

            CSSite siteInstance = farm.AllSites.Find(site);
            if (siteInstance == null)
            {
                return Guid.Empty;
            }

            CSFileSystemEntryDirectory directoryInstance = siteInstance.GetDirectory(directory);
            if (directoryInstance == null)
            {
                return Guid.Empty;
            }

            CSWorkflowAssociation assoc = def.CreateDirectoryAssociation(name, directoryInstance);
            if (assoc == null)
            {
                return Guid.Empty;
            }

            return assoc.Id;
        }

        /// <summary>
        /// Updates the workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="association"></param>
        /// <returns>True if updated successfully</returns>
        public bool UpdateWorkflowAssociation(string token, CSWorkflowAssociationDataContract association)
        {
            CSWorkflowAssociation persistedAssociation = null;

            CSWorkflowDefinitionCollection definitions = GetFarm(token).AllWorkflowDefinitions;
            foreach (CSWorkflowDefinition def in definitions)
            {
                foreach (CSWorkflowAssociation assoc in def.Associations)
                {
                    if (assoc.Id.Equals(association.Id))
                    {
                        persistedAssociation = assoc;
                        break;
                    }
                }
            }

            if (persistedAssociation == null)
            {
                return false;
            }

            CSWorkflowAssociationDataContract persistedAssocContract = new CSWorkflowAssociationDataContract(persistedAssociation);
            if (persistedAssocContract != association)
            {
                persistedAssociation.AllowManualStart = association.AllowManualStart;
                persistedAssociation.AllowProcessingBubbledTriggers = association.AllowProcessingBubbledTriggers;
                persistedAssociation.Name = association.Name;
                persistedAssociation.CustomAssociationInformation = association.CustomAssociationInformation;
                persistedAssociation.StartOnCreate = association.StartOnCreate;
                persistedAssociation.StartOnModify = association.StartOnModify;

                bool result = persistedAssociation.Save();
                if (result)
                {

                    // we need to implement the enable/disable toggle here based on property value 
                    // because the service does not provide seperate methods to do this.
                    if (association.IsEnabled != persistedAssociation.IsEnabled)
                    {
                        if (association.IsEnabled)
                        {
                            result = persistedAssociation.Enable();
                        }
                        else
                        {
                            result = persistedAssociation.Disable();
                        }
                    }

                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes the given workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <returns>True if deleted successfully</returns>
        public bool DeleteWorkflowAssociation(string token, Guid id)
        {
            CSWorkflowAssociation persistedAssociation = null;

            CSWorkflowDefinitionCollection definitions = GetFarm(token).AllWorkflowDefinitions;
            foreach (CSWorkflowDefinition def in definitions)
            {
                foreach (CSWorkflowAssociation assoc in def.Associations)
                {
                    if (assoc.Id.Equals(id))
                    {
                        persistedAssociation = assoc;
                        break;
                    }
                }
            }

            if (persistedAssociation == null)
            {
                return false;
            }

            return persistedAssociation.Delete();
        }

        /// <summary>
        /// Register a trigger to a workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <param name="triggerName">Name of the trigger to register</param>
        /// <returns>True if registration was successful</returns>
        public bool RegisterTrigger(string token, Guid id, WorkflowTriggerEventNamesEnum triggerName)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(id);
            if (def == null)
            {
                return false;
            }

            def.RegisterTrigger(triggerName);
            return true;
        }

        /// <summary>
        /// Remove the registration of a trigger for a workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <param name="triggerName">Name of the trigger to unregister</param>
        /// <returns>True if unregistration was successful</returns>
        public bool UnregisterTrigger(string token, Guid id, WorkflowTriggerEventNamesEnum triggerName)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(id);
            if (def == null)
            {
                return false;
            }

            def.DeregisterTrigger(triggerName);
            return true;
        }

        /// <summary>
        /// Add subscription to a trigger event for a workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <param name="eventName">Name of the event to subscribe to</param>
        /// <returns>True if subscription was successful</returns>
        public bool SubscribeEvent(string token, Guid id, WorkflowTriggerEventNamesEnum eventName)
        {
            CSWorkflowAssociation persistedAssociation = null;

            CSWorkflowDefinitionCollection definitions = GetFarm(token).AllWorkflowDefinitions;
            foreach (CSWorkflowDefinition def in definitions)
            {
                foreach (CSWorkflowAssociation assoc in def.Associations)
                {
                    if (assoc.Id.Equals(id))
                    {
                        persistedAssociation = assoc;
                        break;
                    }
                }
            }

            if (persistedAssociation == null)
            {
                return false;
            }

            persistedAssociation.SubscribeWorkflowEvent(eventName);
            return true;
        }

        /// <summary>
        /// Remove the subscription to a workflow event for a workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <param name="eventName">Name of the event to unsubscribe from</param>
        /// <returns>True if unsubscribe was successful</returns>
        public bool UnsubscribeEvent(string token, Guid id, WorkflowTriggerEventNamesEnum eventName)
        {
            CSWorkflowAssociation persistedAssociation = null;

            CSWorkflowDefinitionCollection definitions = GetFarm(token).AllWorkflowDefinitions;
            foreach (CSWorkflowDefinition def in definitions)
            {
                foreach (CSWorkflowAssociation assoc in def.Associations)
                {
                    if (assoc.Id.Equals(id))
                    {
                        persistedAssociation = assoc;
                        break;
                    }
                }
            }

            if (persistedAssociation == null)
            {
                return false;
            }

            persistedAssociation.UnsubscribeWorkflowEvent(eventName);
            return true;
        }

        /// <summary>
        /// Creates a workflow manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the Workflow definition</param>
        /// <param name="engine">Workflow engine version</param>
        /// <param name="assemblyName">Assembly file name of the compiled result</param>
        /// <param name="className">Name of class (fully qualified, including namespace) that contains the required workflow</param>
        /// <param name="alwaysCompile">If set, workflow is always compiled (mutually exclusive with cacheCompileResults)</param>
        /// <param name="cacheCompileResults">If set, compilation result is cached to backend (mutually exclusive with alwaysCompile)</param>
        /// <returns>The created manifest object or NULL</returns>
        public CSWorkflowManifestDataContract CreateManifest(string token, Guid definitionId, WorkflowEngineEnum engine, string assemblyName, string className, bool alwaysCompile, bool cacheCompileResults)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return new CSWorkflowManifestDataContract(null);
            }

            CSWorkflowManifest manifest = def.CreateManifest(engine, assemblyName, className, alwaysCompile, cacheCompileResults);
            return new CSWorkflowManifestDataContract(manifest);
        }

        /// <summary>
        /// Updates a workflow manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifest">The manifest data to update</param>
        /// <returns>True if operation succeeded</returns>
        public bool UpdateManifest(string token, Guid definitionId, CSWorkflowManifestDataContract manifest)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return false;
            }

            CSWorkflowManifest persistedManifest = def.GetManifest();
            if (persistedManifest == null)
            {
                return false;
            }

            CSWorkflowManifestDataContract persistedManifestContract = new CSWorkflowManifestDataContract(persistedManifest);
            if (persistedManifestContract != manifest)
            {
                persistedManifest.WorkflowEngine = manifest.WorkflowEngine;
                persistedManifest.OutputAssemblyName = manifest.OutputAssemblyName;
                persistedManifest.WorkflowClassName = manifest.WorkflowClassName;
                persistedManifest.AlwaysCompile = manifest.AlwaysCompile;
                persistedManifest.CacheCompileResults = manifest.CacheCompileResults;

                persistedManifest.BuildAssemblyCompany = manifest.BuildAssemblyCompany;
                persistedManifest.BuildAssemblyCopyright = manifest.BuildAssemblyCopyright;
                persistedManifest.BuildAssemblyDescription = manifest.BuildAssemblyDescription;
                persistedManifest.BuildAssemblyFileVersion = new Version(manifest.BuildAssemblyFileVersion);
                persistedManifest.BuildAssemblyProduct = manifest.BuildAssemblyProduct;
                persistedManifest.BuildAssemblyTitle = manifest.BuildAssemblyTitle;
                persistedManifest.BuildAssemblyTrademark = manifest.BuildAssemblyTrademark;
                persistedManifest.BuildAssemblyVersion = new Version(manifest.BuildAssemblyVersion);

                persistedManifest.Save();
            }

            return true;
        }

        /// <summary>
        /// Deletes a workflow manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <returns>True if operation succeeded</returns>
        public bool DeleteManifest(string token, Guid definitionId, Guid manifestId)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return false;
            }

            CSWorkflowManifest persistedManifest = def.GetManifest();
            if ((persistedManifest == null) || (persistedManifest.Id != manifestId))
            {
                return false;
            }

            persistedManifest.Delete();
            return true;
        }

        /// <summary>
        /// Create a new manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="fileName">Filename of file</param>
        /// <param name="filenameExtension">File extension name of file</param>
        /// <param name="type">Type of file</param>
        /// <param name="isRequiredForExecution">Whether required for execution</param>
        /// <param name="content">Content of the file as a byte array</param>
        /// <param name="buildRelativeFolder">Folder to place file in during build. Must be a relative path (file will be hosted in a temporary folder).</param>
        /// <param name="runtimeRelativeFolder">Folder to place file when running the workflow. Must be a relative path (file will be hosted in a temporary folder).</param>
        /// <returns>Created manifest item</returns>
        public CSWorkflowManifestItemDataContract CreateManifestItem(string token, Guid definitionId, Guid manifestId, string fileName, string filenameExtension, WorkflowManifestItemTypeEnum type, bool isRequiredForExecution, string buildRelativeFolder, string runtimeRelativeFolder, byte[] content)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return new CSWorkflowManifestItemDataContract(null);
            }

            CSWorkflowManifest persistedManifest = def.GetManifest();
            if ((persistedManifest != null) && (persistedManifest.Id == manifestId))
            {
                CSWorkflowManifestItem item = persistedManifest.AddItem(fileName, filenameExtension, type, isRequiredForExecution, content, buildRelativeFolder, runtimeRelativeFolder);
                if (item != null)
                {
                    return new CSWorkflowManifestItemDataContract(item);
                }
            }

            return new CSWorkflowManifestItemDataContract(null);
        }

        /// <summary>
        /// Updates a workflow manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="item">The workflow manifest item to update</param>
        /// <returns>True if operation succeeded</returns>
        public bool UpdateManifestItem(string token, Guid definitionId, Guid manifestId, CSWorkflowManifestItemDataContract item)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return false;
            }

            CSWorkflowManifest persistedManifest = def.GetManifest();
            if ((persistedManifest != null) && (persistedManifest.Id == manifestId))
            {
                CSWorkflowManifestItem persistedItem = null;
                foreach(CSWorkflowManifestItem t in persistedManifest.GetItems())
                {
                    if (t.Id.Equals(item.Id))
                    {
                        persistedItem = t;
                        break;
                    }
                }

                if (persistedItem != null)
                {
                    persistedItem.Filename = item.Filename;
                    persistedItem.FilenameExtension = item.FilenameExtension;
                    persistedItem.ItemType = item.ItemType;
                    persistedItem.RequiredForExecution = item.RequiredForExecution;
                    persistedItem.BuildtimeRelativeFolder = item.BuildtimeRelativeFolder;
                    persistedItem.RuntimeRelativeFolder = item.RuntimeRelativeFolder;

                    return persistedItem.Save();
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the content for a manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="manifestItemId">Guid of the manifest item</param>
        /// <param name="data">The data as a byte stream</param>
        /// <returns>True if operation succeeded</returns>
        public bool UpdateManifestItemData(string token, Guid definitionId, Guid manifestId, Guid manifestItemId, byte[] data)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return false;
            }

            CSWorkflowManifest persistedManifest = def.GetManifest();
            if ((persistedManifest != null) && (persistedManifest.Id == manifestId))
            {
                CSWorkflowManifestItem persistedItem = null;
                foreach (CSWorkflowManifestItem t in persistedManifest.GetItems())
                {
                    if (t.Id.Equals(manifestItemId))
                    {
                        persistedItem = t;
                        break;
                    }
                }

                if (persistedItem != null)
                {
                    persistedItem.FileContent = data;
                    return persistedItem.Save();
                }
            }

            return false;
        }


        /// <summary>
        /// Deletes a workflow manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="manifestItemId">Guid of the manifest item</param>
        /// <returns>True if operation succeeded</returns>
        public bool DeleteManifestItem(string token, Guid definitionId, Guid manifestId, Guid manifestItemId)
        {
            CSWorkflowDefinition def = GetFarm(token).AllWorkflowDefinitions.Find(definitionId);
            if (def == null)
            {
                return false;
            }

            CSWorkflowManifest persistedManifest = def.GetManifest();
            if ((persistedManifest != null) && (persistedManifest.Id == manifestId))
            {
                CSWorkflowManifestItem persistedItem = null;
                foreach (CSWorkflowManifestItem t in persistedManifest.GetItems())
                {
                    if (t.Id.Equals(manifestItemId))
                    {
                        persistedItem = t;
                        break;
                    }
                }

                if (persistedItem != null)
                {
                    persistedManifest.RemoveItem(persistedItem);
                }
            }

            return false;
        }

    }
}
