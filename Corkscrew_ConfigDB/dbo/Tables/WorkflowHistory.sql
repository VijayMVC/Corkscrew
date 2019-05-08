CREATE TABLE [WorkflowHistory]
(
	[Id]									uniqueidentifier				NOT NULL,
	[WorkflowAssociationId]					uniqueidentifier				NOT NULL,
	
	[WorkflowInstanceId]					uniqueidentifier				NULL,

	[AssociationData]						nvarchar(max)					NULL,

	[State]									int								NOT NULL,
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
	
	[Created]								datetime						NOT NULL,
	[CreatedBy]								uniqueidentifier				NOT NULL,

	constraint								[PK_WorkflowHistory]			primary key ( [Id], [WorkflowAssociationId], [State] )
)
