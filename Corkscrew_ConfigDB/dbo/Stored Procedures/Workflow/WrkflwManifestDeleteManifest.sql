CREATE PROCEDURE [WrkflwManifestDeleteManifest]
	@WorkflowManifestId				uniqueidentifier
AS 
BEGIN 
	IF (NOT EXISTS (SELECT 1 FROM [WorkflowManifests] WITH (NOLOCK) WHERE ([Id] = @WorkflowManifestId)))
	BEGIN
		RAISERROR ( N'Workflow manifest with that Id does not exist.', 16, 1);
		RETURN -1;
	END

	BEGIN TRANSACTION

	BEGIN TRY

		UPDATE WI 
		SET 
			WI.[CurrentState] = 2,		--- completed
			WI.[CompletedReason] = 5,	--- aborted
			WI.[ErrorMessage] = N'Workflow manifest was deleted',
			WI.[Modified] = GETDATE(), 
			WI.[ModifiedBy] = dbo.SYSTEMUSERGUID() 
		FROM [WorkflowInstances] WI 
		INNER JOIN [WorkflowManifests] WM ON (WM.[WorkflowDefinitionId] = WI.[WorkflowDefinitionId])
		WHERE (WM.[Id] = @WorkflowManifestId)
		
		UPDATE WD 
		SET 
			WD.[Modified] = GETDATE(), 
			WD.[ModifiedBy] = dbo.SYSTEMUSERGUID()
		FROM [WorkflowDefinitions] WD 
		INNER JOIN [WorkflowManifests] WM 
			ON (WM.[WorkflowDefinitionId] = WD.[Id]) 
		WHERE (
			(WM.[Id] = @WorkflowManifestId) 
		)

		DELETE FROM [WorkflowManifestItems] 
			WHERE ([WorkflowManifestId] = @WorkflowManifestId)

		DELETE FROM [WorkflowManifests] 
			WHERE ([Id] = @WorkflowManifestId) 


		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH	

	RETURN @@ROWCOUNT;
END
