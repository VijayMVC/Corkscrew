﻿CREATE PROCEDURE [WrkflwInstGetByAssociation]
	@WorkflowAssociationId			uniqueidentifier,
	@only_runnable					bit					= 1			/* if set, we do not return any "Completed" instances */
AS
BEGIN
	
	IF (@only_runnable = 1)
	BEGIN
		SELECT 
			[Id], [WorkflowAssociationId], [WorkflowDefinitionId], 
				[AssociationData], 
					[FarmId], [SiteId], [DirectoryId], [FileId], 
						[CurrentState], [CompletedReason], [ErrorMessage], [SqlPersistenceId], [IsLoadedInRuntime],
							[Created], [CreatedBy], [Modified], [ModifiedBy] 
		FROM [WorkflowInstances] WITH (NOLOCK) 
		WHERE (
			([WorkflowAssociationId] = @WorkflowAssociationId) 
			AND ([CurrentState] IN (0, 1, 3, 4))
		)
	END
	ELSE 
		BEGIN
			SELECT 
				[Id], [WorkflowAssociationId], [WorkflowDefinitionId], 
					[AssociationData], 
						[FarmId], [SiteId], [DirectoryId], [FileId], 
							[CurrentState], [CompletedReason], [ErrorMessage], [SqlPersistenceId], [IsLoadedInRuntime],
								[Created], [CreatedBy], [Modified], [ModifiedBy] 
			FROM [WorkflowInstances] WITH (NOLOCK) 
			WHERE ([WorkflowAssociationId] = @WorkflowAssociationId) 
		END

	RETURN @@ROWCOUNT;

END