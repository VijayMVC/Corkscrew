CREATE PROCEDURE [FileSystemSaveContent]
	@Id				uniqueidentifier,
	@Modified		datetime,
	@ModifiedBy		uniqueidentifier, 
	@Content		varbinary(max)
AS
BEGIN 

	DECLARE	@WorkflowInstanceId	uniqueidentifier	= NEWID()
	DECLARE	@is_directory		bit					= 0

	--- Ensure destination exists
	IF (NOT EXISTS (SELECT 1 FROM [FileSystem] WHERE ([Id] = @Id)))
	BEGIN
		RAISERROR ( N'Item does not exist.', 16, 1);
		RETURN -1;
	END

	SELECT @is_directory = [is_directory] FROM [FileSystem] WITH (NOLOCK) WHERE ([Id] = @Id);

	INSERT INTO [FileSystemChangeLog] 
		( [Id], [FileSystemId], [SiteId], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
				[PreviousData], [PreviousFilename], [PreviousFilenameExtension], [PreviousDirectoryName],  
					[PreviousCreated], [PreviousCreatedBy], [PreviousModified], [PreviousModifiedBy],
						[was_directory], [was_readonly], [was_archive], [was_system] )
		SELECT 
			newid(), @Id, [SiteId], 0, 'U', @Modified, @ModifiedBy, 
				[ContentStream], [Filename], [FilenameExtension], [DirectoryName], 
					[Created], [CreatedBy], [Modified], [ModifiedBy], 
						[is_directory], [is_readonly], [is_archive], [is_system] 
		FROM [FileSystem] WITH (NOLOCK) 
		WHERE ([Id] = @Id)

	UPDATE [FileSystem] 
		SET 
			[ContentStream] = @Content,
			[Modified] = @Modified,
			[ModifiedBy] = @ModifiedBy, 
			[LastAccessed] = @Modified,
			[LastAccessedBy] = @ModifiedBy
	WHERE (([Id] = @Id) AND ([is_directory] = 0))


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
										WHEN 1 THEN 'directory_modified' 
										ELSE 'file_modified' 
									END,
									@Modified, @ModifiedBy, @Modified, @ModifiedBy
				FROM [WorkflowAssociations] WITH (NOLOCK) 
				WHERE (
					(
						(
							([is_directory_scope] = 1) 
							AND (
									((@is_directory = 1) AND ([subscribe_event_directory_modified] = 1)) 
									OR ((@is_directory = 0) AND ([subscribe_event_file_modified] = 1))
							)
						)
						OR (
								([is_site_scope] = 1) 
								AND (
										((@is_directory = 1) AND ([subscribe_event_directory_modified] = 0) AND ([subscribe_event_catch_bubbledevents] = 1)) 
										OR ((@is_directory = 0) AND ([subscribe_event_file_modified] = 0) AND ([subscribe_event_catch_bubbledevents] = 1))
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