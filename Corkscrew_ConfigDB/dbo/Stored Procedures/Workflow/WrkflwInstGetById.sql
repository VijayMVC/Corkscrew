CREATE PROCEDURE [WrkflwInstGetById]
	@Id				uniqueidentifier
AS
BEGIN
	
	SELECT 
		[Id], [WorkflowAssociationId], [WorkflowDefinitionId], 
			[AssociationData], 
				[FarmId], [SiteId], [DirectoryId], [FileId], 
					[CurrentState], [CompletedReason], [ErrorMessage], [SqlPersistenceId], [IsLoadedInRuntime],
						[Created], [CreatedBy], [Modified], [ModifiedBy] 
	FROM [WorkflowInstances] WITH (NOLOCK) 
	WHERE ([Id] = @Id) 

	RETURN @@ROWCOUNT;

END