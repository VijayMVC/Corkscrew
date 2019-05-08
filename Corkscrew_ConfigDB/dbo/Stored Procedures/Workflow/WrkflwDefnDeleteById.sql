CREATE PROCEDURE [WrkflwDefnDeleteById]
	@Id				uniqueidentifier
AS
BEGIN

	BEGIN TRANSACTION

	BEGIN TRY 

		DELETE FROM [WorkflowManifestItems] WHERE ([WorkflowDefinitionId] = @Id)

		DELETE FROM [WorkflowManifests] WHERE ([WorkflowDefinitionId] = @Id)

		--- abort the instances for the association
		UPDATE [WorkflowInstances] 
			SET 
				[CurrentState] = 2,		--- completed
				[CompletedReason] = 5,	--- aborted
				[ErrorMessage] = N'Workflow definition was deleted',
				[Modified] = GETDATE(), 
				[ModifiedBy] = dbo.SYSTEMUSERGUID() 
		WHERE ([WorkflowDefinitionId] = @Id)
			
		DELETE FROM [WorkflowAssociations] WHERE ([WorkflowDefinitionId] = @Id)

		DELETE FROM [WorkflowDefinitions] WHERE ([Id] = @Id)

		COMMIT TRANSACTION 
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION 
		RAISERROR ( N'Unable to delete Workflow associations.', 16, 1);
	END CATCH

	

	RETURN @@ROWCOUNT;
END