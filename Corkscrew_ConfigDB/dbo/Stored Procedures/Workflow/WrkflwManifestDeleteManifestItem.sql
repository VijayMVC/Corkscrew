CREATE PROCEDURE [WrkflwManifestDeleteManifestItem]
	@ManifestItemId			uniqueidentifier
AS
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowManifestItems] WITH (NOLOCK) WHERE ([Id] = @ManifestItemId)))
	BEGIN
		RAISERROR ( N'Workflow manifest item with that Id does not exist.', 16, 1);
		RETURN -1;
	END

	BEGIN TRANSACTION

	BEGIN TRY

		/*
			deleting manifest items of types 1-5 are "breaking" to any running workflow instances. 
			So, update -> instances to abort.

			ItemType:
				1 - primary assembly
				2 - dependency assembly 
				3 - source file
				4 - xaml file
				5 - config file

		*/

		UPDATE WI 
		SET 
			WI.[CurrentState] = 2,		--- completed
			WI.[CompletedReason] = 5,	--- aborted
			WI.[ErrorMessage] = N'Workflow manifest was deleted',
			WI.[Modified] = GETDATE(), 
			WI.[ModifiedBy] = dbo.SYSTEMUSERGUID() 
		FROM [WorkflowInstances] WI 
		INNER JOIN [WorkflowManifestItems] WMI ON (WMI.[WorkflowDefinitionId] = WI.[WorkflowDefinitionId]) 
		WHERE (
			(WMI.[Id] = @ManifestItemId) 
			AND (WMI.[ItemType] IN (1, 2, 3, 4, 5)) 
		);

		UPDATE WD 
		SET 
			WD.[Modified] = GETDATE(), 
			WD.[ModifiedBy] = dbo.SYSTEMUSERGUID()
		FROM [WorkflowDefinitions] WD 
		INNER JOIN [WorkflowManifestItems] WMI 
			ON (WMI.[WorkflowDefinitionId] = WD.[Id]) 
		WHERE (
			(WMI.[Id] = @ManifestItemId) 
		)

		DELETE FROM [WorkflowManifestItems] 
			WHERE ([Id] = @ManifestItemId)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

	

	RETURN @@ROWCOUNT;

END