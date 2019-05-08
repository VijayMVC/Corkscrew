﻿CREATE PROCEDURE [WrkflwAssnDeleteByDefinitionId]
	@WorkflowDefinitionId		uniqueidentifier
AS
BEGIN

	BEGIN TRANSACTION

	BEGIN TRY 

		--- abort the instances for the association
		UPDATE [WorkflowInstances] 
			SET 
				[CurrentState] = 2,		--- completed
				[CompletedReason] = 5,	--- aborted
				[ErrorMessage] = N'Workflow association was deleted',
				[Modified] = GETDATE(), 
				[ModifiedBy] = dbo.SYSTEMUSERGUID() 
		WHERE ([WorkflowDefinitionId] = @WorkflowDefinitionId)

		DELETE FROM [WorkflowAssociations] WHERE ([WorkflowDefinitionId] = @WorkflowDefinitionId)

		COMMIT TRANSACTION 
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION 
		RAISERROR ( N'Unable to delete Workflow associations.', 16, 1);
	END CATCH
	
	RETURN @@ROWCOUNT;
	
END