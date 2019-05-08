CREATE PROCEDURE [SiteDeleteById]
	@SiteId			uniqueidentifier, 
	@DeletedById	uniqueidentifier
AS
BEGIN

	DECLARE @WorkflowInstanceId		uniqueidentifier	= NEWID();
	DECLARE @RightNow				datetime			= getdate();
	DECLARE	@Sql					nvarchar(max)		= N'';

	BEGIN TRAN

	BEGIN TRY

		INSERT INTO [SitesChangeLog] 
			( [SiteId], [IsLocked], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
				[PrevName], [PrevDescription], [PrevDBServer], [PrevDBName], [PrevQuotaBytes], [PrevModified], [PrevModifiedBy] ) 
			SELECT 
				@SiteId, 0, 0, 'D', GETDATE(), @DeletedById,
				[Name], [Description], [ContentDBServerName], [ContentDBName], [QuotaBytes], [Modified], [ModifiedBy]
			FROM [Sites] WHERE 
			(
				[Id] = @SiteId
			)

		BEGIN TRANSACTION SITEDELWFTRANS

		BEGIN TRY
			IF (EXISTS (SELECT 1 FROM sys.tables WHERE (name='WorkflowInstances'))) 
			BEGIN
				INSERT INTO [WorkflowInstances] 
					(
						[Id], [WorkflowAssociationId], [WorkflowDefinitionId], 
							[AssociationData], 
								[FarmId], [SiteId], [DirectoryId], [FileId], 
									[CurrentState], [CompletedReason], [ErrorMessage], [SqlPersistenceId], [IsLoadedInRuntime], [InstanceStartEvent], 
										[Created], [CreatedBy], [Modified], [ModifiedBy]
					)
					SELECT
						@WorkflowInstanceId, [Id], [WorkflowDefinitionId], 
							[AssociationData], 
								[FarmId], @SiteId, NULL, NULL, 
									0, 0, NULL, NULL, 0, 'site_deleted', 
										@RightNow, @DeletedById, @RightNow, @DeletedById
					FROM [WorkflowAssociations] WITH (NOLOCK) 
					WHERE (
						([is_farm_scope] = 1) 
						AND ([start_on_create] = 1)
						AND (
								([subscribe_event_site_deleted] = 1) 
								OR (([subscribe_event_site_deleted] = 0) AND ([subscribe_event_catch_bubbledevents] = 1))
						) 
						AND ([is_enabled] = 1) 
						AND ([prevent_new_instances] = 0)
					)

				IF (@@ROWCOUNT > 0)
				BEGIN
					IF (EXISTS (SELECT 1 FROM sys.tables WHERE (name='WorkflowHistory'))) 
					BEGIN
						INSERT INTO [WorkflowHistory] 
						(
							[Id], [WorkflowAssociationId], [WorkflowInstanceId], 
								[AssociationData], [State], [CompletedReason], [ErrorMessage], 
									[Created], [CreatedBy]
						)
						SELECT
							NEWID(), [WorkflowAssociationId], [Id], 
								[AssociationData], [CurrentState], [CompletedReason], [ErrorMessage],
									[Modified], [ModifiedBy]
						FROM [WorkflowInstances] WITH (NOLOCK) 
						WHERE ([Id] = @WorkflowInstanceId)
					END
				END
			END

			COMMIT TRANSACTION SITEDELWFTRANS
		END TRY
		BEGIN CATCH 
			PRINT ERROR_MESSAGE()
			ROLLBACK TRANSACTION SITEDELWFTRANS		-- basically rolls back creating the workflow instance
		END CATCH

		SELECT @Sql = CONCAT(N'DROP DATABASE [', [ContentDBName], N'];') FROM [Sites] WITH (NOLOCK) 
				WHERE ([Id] = @SiteId);

		DELETE FROM [Sites] 
			WHERE ([Id] = @SiteId) 

		COMMIT TRAN

		--- This must be done outside the above transaction
		begin try
			EXEC(@Sql);
		end try
		begin catch
			print @Sql
			print error_message()
			print N'Could not drop database';
		end catch

	END TRY
	BEGIN CATCH
		DECLARE @Message	NVARCHAR(MAX) = ERROR_MESSAGE()
		PRINT 'Error: ' + @Message
		ROLLBACK TRAN 
	END CATCH

END
