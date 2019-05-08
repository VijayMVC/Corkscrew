CREATE PROCEDURE [WrkflwHistGetByInstance]
	@WorkflowInstanceId		uniqueidentifier
AS
BEGIN

	IF (NOT EXISTS(SELECT 1 FROM [WorkflowInstances] WITH (NOLOCK) WHERE ([Id] = @WorkflowInstanceId)))
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
		[WorkflowInstanceId] = @WorkflowInstanceId
	) 
	ORDER BY [Created] DESC;

	RETURN @@ROWCOUNT;

END