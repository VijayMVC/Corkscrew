CREATE PROCEDURE [WrkflwHistGetByAssociation]
	@WorkflowAssociationId		uniqueidentifier
AS
BEGIN

	IF (NOT EXISTS(SELECT 1 FROM [WorkflowAssociations] WITH (NOLOCK) WHERE ([Id] = @WorkflowAssociationId)))
	BEGIN
		RAISERROR ( N'Workflow association does not exist.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS(SELECT 1 FROM [WorkflowInstances] WITH (NOLOCK) WHERE ([WorkflowAssociationId] = @WorkflowAssociationId)))
	BEGIN
		RAISERROR ( N'Workflow instance does not exist.', 16, 1);
		RETURN -1;
	END


	SELECT 
		[Id], [WorkflowAssociationId], [WorkflowInstanceId], 
			[AssociationData], 
					[State], [CompletedReason], [ErrorMessage], 
						[Created], [CreatedBy] 
	FROM [WorkflowHistory] WITH (NOLOCK) 
	WHERE (
		[WorkflowAssociationId] = @WorkflowAssociationId
	) 
	ORDER BY [Created] DESC;


	RETURN @@ROWCOUNT;

END