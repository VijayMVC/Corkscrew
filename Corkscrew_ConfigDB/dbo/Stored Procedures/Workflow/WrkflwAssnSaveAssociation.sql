CREATE PROCEDURE [WrkflwAssnSaveAssociation]
	@Id							uniqueidentifier,
	@Name						nvarchar(255),
	@AssociationData			nvarchar(max)		= NULL,
	@allow_manual_start			bit,
	@start_on_create			bit,
	@start_on_modify			bit,
	@is_enabled					bit,
	@prevent_new_instances		bit,
	@ModifiedBy					uniqueidentifier,
	@Modified					datetime
AS
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowAssociations] WITH (NOLOCK) WHERE ([Id] = @Id)))
	BEGIN
		RAISERROR ( N'Workflow with that Id does not exist.', 16, 1);
		RETURN -1;
	END

	BEGIN TRANSACTION

	BEGIN TRY

		--- if association is disabled or deleted, also abort any running instances.
		IF (((@prevent_new_instances & @is_enabled) = 1) OR (@is_enabled = 0))
		BEGIN
			UPDATE [WorkflowInstances] 
				SET 
					[CurrentState] = 2,		--- completed
					[CompletedReason] = 5,	--- aborted
					[ErrorMessage] = N'Workflow association was disabled or deleted',
					[Modified] = @Modified,  
					[ModifiedBy] = dbo.SYSTEMUSERGUID()		--- always system, not admin user
			WHERE ([WorkflowAssociationId] = @Id)
		END

		UPDATE [WorkflowAssociations] 
			SET
				[Name] = @Name, 
				[AssociationData] = @AssociationData, 
				[allow_manual_start] = @allow_manual_start, 
				[start_on_create] = @start_on_create, 
				[start_on_modify] = @start_on_modify, 
				[prevent_new_instances] = @prevent_new_instances, 
				[is_enabled] = @is_enabled, 
				[Modified] = @Modified, 
				[ModifiedBy] = @ModifiedBy 
		WHERE ([Id] = @Id)
		
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

	RETURN 0;

END
