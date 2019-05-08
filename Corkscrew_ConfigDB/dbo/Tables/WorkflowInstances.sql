CREATE TABLE [WorkflowInstances]
(
	[Id]									uniqueidentifier				NOT NULL,
	[WorkflowAssociationId]					uniqueidentifier				NOT NULL,

	[WorkflowDefinitionId]					uniqueidentifier				NOT NULL,

	[AssociationData]						nvarchar(max)					NULL,

	[FarmId]								uniqueidentifier				NOT NULL,
	[SiteId]								uniqueidentifier				NULL,
	[DirectoryId]							uniqueidentifier				NULL,
	[FileId]								uniqueidentifier				NULL,

	[CurrentState]							int								NOT NULL,
	/*
		0 - undefined
		1 - Started
		2 - Completed
		3 - Paused 
		4 - Continued
		5 - Errored
	*/

	[CompletedReason]						int								NOT NULL,
	/*
		0 - undefined 
		1 - Successful
		2 - Error on start
		3 - Error processing 
		4 - Terminated by user
		5 - Aborted
	*/

	[ErrorMessage]							nvarchar(max)					NULL,

	[IsLoadedInRuntime]						bit								NOT NULL,
	/*
		Set to 1 if the instance gets loaded into a workflow runtime for actual execution. 
		Used to differentiate between whether we are just working with the instance data or if the instance is being run and managed.
	*/

	[SqlPersistenceId]						uniqueidentifier				NULL,

	[InstanceStartEvent]					nvarchar(255)					NOT NULL,
	/*
		Name of the event that spun up this instance
	*/

	[Created]								datetime						NOT NULL,
	[CreatedBy]								uniqueidentifier				NOT NULL,
	[Modified]								datetime						NOT NULL,
	[ModifiedBy]							uniqueidentifier				NOT NULL,

	constraint								[PK_WorkflowInstances]			primary key ( [Id], [WorkflowAssociationId], [WorkflowDefinitionId], [CurrentState] )
)
