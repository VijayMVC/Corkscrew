using Corkscrew.API.datacontracts;
using Corkscrew.SDK.workflow;
using System;
using System.ServiceModel;

namespace Corkscrew.API.services
{
    
    /// <summary>
    /// This service allows the remote client to add, remove and manage workflow definitions and associations. 
    /// This service does not contain features to execute or change execution of workflow instances.
    /// </summary>
    [ServiceContract]
    public interface IWorkflows
    {

        /// <summary>
        /// Returns all the workflow definitions in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of workflow definitions</returns>
        [OperationContract]
        CSWorkflowDefinitionDataContract[] GetAllDefinitions(string token);

        /// <summary>
        /// Returns a single workflow definition given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <returns>A single workflow definition</returns>
        [OperationContract]
        CSWorkflowDefinitionDataContract GetDefinitionById(string token, Guid id);

        /// <summary>
        /// Returns all the associations for a given workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <returns>Array of workflow associations</returns>
        [OperationContract]
        CSWorkflowAssociationDataContract[] GetAllAssociationsForDefinition(string token, Guid definitionId);

        /// <summary>
        /// Returns a single workflow association given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <returns>A single workflow association</returns>
        [OperationContract]
        CSWorkflowAssociationDataContract GetAssociationById(string token, Guid id);

        /// <summary>
        /// Returns the manifest for a workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <returns>A single workflow manifest (there can only be one manifest for a definition)</returns>
        [OperationContract]
        CSWorkflowManifestDataContract GetManifestForDefinition(string token, Guid definitionId);

        /// <summary>
        /// Returns all the items in a manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="workflowDefinitionId">Guid of the workflow definition</param>
        /// <returns>Array of manifest items</returns>
        [OperationContract]
        CSWorkflowManifestItemDataContract[] GetManifestItemsForManifest(string token, Guid workflowDefinitionId);

        /// <summary>
        /// Gets the file data for the manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="workflowDefinitionId">Guid of the workflow definition</param>
        /// <param name="manifestItemId">Id of the manifest item to get the data for</param>
        /// <returns>Byte stream of data</returns>
        [OperationContract]
        byte[] GetManifestItemData(string token, Guid workflowDefinitionId, Guid manifestItemId);

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
        [OperationContract]
        Guid CreateWorkflowDefinition(string token, string name, string description, string defaultAssociationData, bool startWorkflowOnCreate, bool startWorkflowOnModify, bool allowStartWorkflowManually);

        /// <summary>
        /// Updates the workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definition">Workflow definition</param>
        /// <returns>True if updated successfully</returns>
        [OperationContract]
        bool UpdateWorkflowDefinition(string token, CSWorkflowDefinitionDataContract definition);

        /// <summary>
        /// Deletes the workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <returns>True if deleted successfully</returns>
        [OperationContract]
        bool DeleteWorkflowDefinition(string token, Guid id);

        /// <summary>
        /// Deletes all the associations for the given workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <returns>True if deleted successfully</returns>
        [OperationContract]
        bool DeleteAssociationsForDefinition(string token, Guid id);

        /// <summary>
        /// Create a farm level workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <returns>Guid of the created association or NULL</returns>
        [OperationContract]
        Guid CreateWorkflowAssociationFarm(string token, Guid definitionId, string name);

        /// <summary>
        /// Create a site level workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <param name="site">Guid of the site</param>
        /// <returns>Guid of the created association or NULL</returns>
        [OperationContract]
        Guid CreateWorkflowAssociationSite(string token, Guid definitionId, string name, Guid site);

        /// <summary>
        /// Create a directory level workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="name">Name for the association</param>
        /// <param name="site">Guid of the site</param>
        /// <param name="directory">Guid of the directory</param>
        /// <returns>Guid of the created association or NULL</returns>
        [OperationContract]
        Guid CreateWorkflowAssociationDirectory(string token, Guid definitionId, string name, Guid site, Guid directory);

        /// <summary>
        /// Updates the workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="association"></param>
        /// <returns>True if updated successfully</returns>
        [OperationContract]
        bool UpdateWorkflowAssociation(string token, CSWorkflowAssociationDataContract association);

        /// <summary>
        /// Deletes the given workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <returns>True if deleted successfully</returns>
        [OperationContract]
        bool DeleteWorkflowAssociation(string token, Guid id);

        /// <summary>
        /// Register a trigger to a workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <param name="triggerName">Name of the trigger to register</param>
        /// <returns>True if registration was successful</returns>
        [OperationContract]
        bool RegisterTrigger(string token, Guid id, WorkflowTriggerEventNamesEnum triggerName);

        /// <summary>
        /// Remove the registration of a trigger for a workflow definition
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow definition</param>
        /// <param name="triggerName">Name of the trigger to unregister</param>
        /// <returns>True if unregistration was successful</returns>
        [OperationContract]
        bool UnregisterTrigger(string token, Guid id, WorkflowTriggerEventNamesEnum triggerName);

        /// <summary>
        /// Add subscription to a trigger event for a workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <param name="eventName">Name of the event to subscribe to</param>
        /// <returns>True if subscription was successful</returns>
        [OperationContract]
        bool SubscribeEvent(string token, Guid id, WorkflowTriggerEventNamesEnum eventName);

        /// <summary>
        /// Remove the subscription to a workflow event for a workflow association
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the workflow association</param>
        /// <param name="eventName">Name of the event to unsubscribe from</param>
        /// <returns>True if unsubscribe was successful</returns>
        [OperationContract]
        bool UnsubscribeEvent(string token, Guid id, WorkflowTriggerEventNamesEnum eventName);


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
        [OperationContract]
        CSWorkflowManifestDataContract CreateManifest(string token, Guid definitionId, WorkflowEngineEnum engine, string assemblyName, string className, bool alwaysCompile, bool cacheCompileResults);

        /// <summary>
        /// Updates a workflow manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifest">The manifest data to update</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool UpdateManifest(string token, Guid definitionId, CSWorkflowManifestDataContract manifest);

        /// <summary>
        /// Deletes a workflow manifest
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool DeleteManifest(string token, Guid definitionId, Guid manifestId);

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
        [OperationContract]
        CSWorkflowManifestItemDataContract CreateManifestItem(string token, Guid definitionId, Guid manifestId, string fileName, string filenameExtension, WorkflowManifestItemTypeEnum type, bool isRequiredForExecution, string buildRelativeFolder, string runtimeRelativeFolder, byte[] content);

        /// <summary>
        /// Updates a workflow manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="item">The workflow manifest item to update</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool UpdateManifestItem(string token, Guid definitionId, Guid manifestId, CSWorkflowManifestItemDataContract item);


        /// <summary>
        /// Deletes a workflow manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="manifestItemId">Guid of the manifest item</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool DeleteManifestItem(string token, Guid definitionId, Guid manifestId, Guid manifestItemId);


        /// <summary>
        /// Updates the content for a manifest item
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="definitionId">Guid of the workflow definition</param>
        /// <param name="manifestId">Guid of the manifest</param>
        /// <param name="manifestItemId">Guid of the manifest item</param>
        /// <param name="data">The data as a byte stream</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool UpdateManifestItemData(string token, Guid definitionId, Guid manifestId, Guid manifestItemId, byte[] data);

    }
}
