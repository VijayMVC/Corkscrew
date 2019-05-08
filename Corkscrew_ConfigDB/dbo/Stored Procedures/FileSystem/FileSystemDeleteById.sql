CREATE PROCEDURE [FileSystemDeleteById]
	@Id				uniqueidentifier, 
	@DeleteUserId	uniqueidentifier
AS
BEGIN 

	DECLARE @DeletedItemFullPath	nvarchar(max)

	DECLARE	@WorkflowInstanceId	uniqueidentifier	= NEWID()
	DECLARE	@is_directory		bit					= 0
	DECLARE @RightNow			datetime			= getdate()
	
	--- Ensure destination exists
	IF (NOT EXISTS (SELECT 1 FROM [FileSystem] WHERE ([Id] = @Id)))
	BEGIN
		RAISERROR ( N'Cannot delete item. Target does not exist.', 16, 1);
		RETURN -1;
	END

	SELECT @is_directory = [is_directory], @DeletedItemFullPath = [FullPath] FROM [FileSystem] WITH (NOLOCK) 
		WHERE ([Id] = @Id) 

	IF (@DeletedItemFullPath IS NOT NULL) 
	BEGIN

		INSERT INTO [FileSystemChangeLog] 
		( [Id], [FileSystemId], [SiteId], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
				[PreviousData], [PreviousFilename], [PreviousFilenameExtension], [PreviousDirectoryName],  
					[PreviousCreated], [PreviousCreatedBy], [PreviousModified], [PreviousModifiedBy],
						[was_directory], [was_readonly], [was_archive], [was_system] )
			SELECT 
				newid(), @Id, [SiteId], 0, 'D', @RightNow, @DeleteUserId, 
					[ContentStream], [Filename], [FilenameExtension], [DirectoryName], 
						[Created], [CreatedBy], [Modified], [ModifiedBy], 
							[is_directory], [is_readonly], [is_archive], [is_system] 
			FROM [FileSystem] WITH (NOLOCK) 
			WHERE (
					(
						([FullPath] = @DeletedItemFullPath)					--- Path itself
						OR ([FullPath] LIKE @DeletedItemFullPath + '/%')	--- child items if any
					)
			)

		DELETE FROM [FileSystem] 
			WHERE (
					(
						([FullPath] = @DeletedItemFullPath)					--- Path itself
						OR ([FullPath] LIKE @DeletedItemFullPath + '/%')	--- child items if any
					)
			)

		BEGIN TRANSACTION FSUPDWFTRANS

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
								[FarmId], @Id, NULL, NULL, 
									0, 0, NULL, NULL, 0, 
										CASE @is_directory 
											WHEN 1 THEN 'directory_deleted' 
											ELSE 'file_deleted' 
										END,
										@RightNow, @DeleteUserId, @RightNow, @DeleteUserId
					FROM [WorkflowAssociations] WITH (NOLOCK) 
					WHERE (
						(
							(
								([is_directory_scope] = 1) 
								AND (
										((@is_directory = 1) AND ([subscribe_event_directory_deleted] = 1)) 
										OR ((@is_directory = 0) AND ([subscribe_event_file_deleted] = 1))
								)
							)
							OR (
									([is_site_scope] = 1) 
									AND (
											((@is_directory = 1) AND ([subscribe_event_directory_deleted] = 0) AND ([subscribe_event_catch_bubbledevents] = 1)) 
											OR ((@is_directory = 0) AND ([subscribe_event_file_deleted] = 0) AND ([subscribe_event_catch_bubbledevents] = 1))
									)
							)
						) 
						AND ([start_on_modify] = 1)
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

			COMMIT TRANSACTION FSUPDWFTRANS
		END TRY
		BEGIN CATCH 
			PRINT ERROR_MESSAGE()
			ROLLBACK TRANSACTION FSUPDWFTRANS		-- basically rolls back creating the workflow instance
		END CATCH
	END

	RETURN 0;

END
