CREATE PROCEDURE [WrkflwInstGetAllRunnable] 
AS
BEGIN
	SELECT 
		WI.[Id], WI.[WorkflowAssociationId], WI.[WorkflowDefinitionId], 
			WI.[AssociationData], 
				WI.[FarmId], WI.[SiteId], WI.[DirectoryId], WI.[FileId], 
					WI.[CurrentState], WI.[CompletedReason], WI.[ErrorMessage], WI.[SqlPersistenceId], WI.[IsLoadedInRuntime],
						WI.[Created], WI.[CreatedBy], WI.[Modified], WI.[ModifiedBy] 
	FROM [WorkflowInstances] WI WITH (NOLOCK) 
		INNER JOIN [WorkflowAssociations] WA WITH (NOLOCK) 
			ON (WA.[Id] = WI.[WorkflowAssociationId]) 
	WHERE (
		(WI.[CurrentState] IN (0, 1, 3, 4)) 
		AND (WA.[is_enabled] = 1) 
	)

	RETURN @@ROWCOUNT;

END