CREATE PROCEDURE [WrkflwInstUpdateStateChange]
	@Id						uniqueidentifier,
	@State					int,
	@StateInfo				nvarchar(max) = NULL,
	@Reason					int,
	@ErrorMessage			nvarchar(max) = NULL,
	@SqlPersistenceId		uniqueidentifier,
	@IsLoadedInRuntime		bit
AS
BEGIN

	DECLARE	@RightNow		datetime			= GETDATE()

	IF (ISNULL(@ErrorMessage, '') = '') SET @ErrorMessage = NULL 

	SET @Reason = 
		CASE 
			WHEN (@State IN (4, 5)) THEN @Reason		/* makes sense only for Completed and Errored events */
			ELSE 0
		END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowInstances] WITH (NOLOCK) WHERE ([Id] = @Id))) 
	BEGIN
		RAISERROR ( N'Workflow instance does not exist.', 16, 1);
	END

	IF (EXISTS (SELECT 1 FROM [WorkflowInstances] WITH (NOLOCK) WHERE (([Id] = @Id) AND ([CurrentState] IN (2, 5))))) 
	BEGIN
		RAISERROR ( N'Workflow instance has already completed its lifecycle. Cannot update state.', 16, 1);
	END

	BEGIN TRANSACTION

	BEGIN TRY

		UPDATE [WorkflowInstances] 
			SET 
				[CurrentState] = @State, 
				[AssociationData] = @StateInfo,
				[CompletedReason] = @Reason, 
				[ErrorMessage] = @ErrorMessage, 
				[SqlPersistenceId] = @SqlPersistenceId,
				[IsLoadedInRuntime] = @IsLoadedInRuntime,
				[Modified] = @RightNow, 
				[ModifiedBy] = dbo.SYSTEMUSERGUID()
		WHERE ([Id] = @Id)
		
		INSERT INTO [WorkflowHistory] 
			(
				[Id], [WorkflowAssociationId], [WorkflowInstanceId], 
					[AssociationData], 
							[State], [CompletedReason], [ErrorMessage], 
								[Created], [CreatedBy]
			)
			SELECT 
				NEWID(), [WorkflowAssociationId], @Id, 
				  [AssociationData], 
						@State, @Reason, @ErrorMessage, 
							@RightNow, dbo.SYSTEMUSERGUID()
			FROM [WorkflowInstances] WITH (NOLOCK) 
			WHERE ([Id] = @Id)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

	RETURN 0;

END