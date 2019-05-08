CREATE PROCEDURE [FileSystemSave]
	@Id				uniqueidentifier, 
	@SiteId			uniqueidentifier,
	@Filename		nvarchar(255),
	@FileExtension	nvarchar(255)		= NULL, 
	@DirectoryName	nvarchar(max)		= NULL,
	@Created		datetime, 
	@CreatedBy		uniqueidentifier, 
	@Modified		datetime, 
	@ModifiedBy		uniqueidentifier, 
	@is_directory	bit, 
	@is_readonly	bit,
	@is_archive		bit,
	@is_system		bit, 
	@DoNotUpdate	bit					= 0
AS
BEGIN

	DECLARE @RootFolder			nvarchar(max)		= CONCAT('corkscrew://', CAST(@SiteId as nvarchar(max)), '/')
	DECLARE	@WorkflowInstanceId	uniqueidentifier	= NEWID()

	IF (NOT EXISTS(SELECT 1 FROM [FileSystem] WHERE ([FullPath] = @RootFolder)))
	BEGIN
		RAISERROR ( N'Filesystem is not initialized.', 16, 1);
		RETURN;
	END

	--- Force all non-pathed items to be children of the root folder
	SET @DirectoryName = ISNULL(@DirectoryName, @RootFolder)

	IF (NOT EXISTS(SELECT 1 FROM [FileSystem] WHERE ([FullPath] = @DirectoryName))) 
	BEGIN 
		RAISERROR (N'Parent directory does not exist!', 16, 1); 
		RETURN;
	END

	IF (ISNULL(@Filename, '') = '') SET @Filename = NULL
	IF (ISNULL(@FileExtension, '') = '') SET @FileExtension = NULL

	BEGIN TRANSACTION FSSAVETRN

	BEGIN TRY
		IF (NOT EXISTS (SELECT 1 FROM [FileSystem] WITH (NOLOCK) WHERE ([Id] = @Id))) 
		BEGIN 
			INSERT INTO [EntityRegistry] 
					([EntityId], [EntityClass]) 
				VALUES 
				(
					@Id, 
						CASE @is_directory
							WHEN 1 THEN 'directory' 
							ELSE 'file'
						END
				);

			INSERT INTO [FileSystem] 
				( [Id], [SiteId], [Filename], [FilenameExtension], [DirectoryName], 
					[Created], [CreatedBy], [Modified], [ModifiedBy], [LastAccessed], [LastAccessedBy], 
						[is_directory], [is_readonly], [is_archive], [is_system],
							[ContentStream] )
			VALUES 
			(
				@Id, @SiteId, @Filename, @FileExtension, @DirectoryName, 
					@Created, @CreatedBy, @Modified, @ModifiedBy, @Modified, @ModifiedBy, 
						@is_directory, @is_readonly, @is_archive, @is_system, 
								/* [ContentStream] */
								CASE 
									WHEN @is_directory = 1
										THEN NULL 
									ELSE 
										0x
								END
			)

			INSERT INTO [FileSystemChangeLog] 
				( [Id], [FileSystemId], [SiteId], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
						[PreviousData], [PreviousFilename], [PreviousFilenameExtension], [PreviousDirectoryName],  
							[PreviousCreated], [PreviousCreatedBy], [PreviousModified], [PreviousModifiedBy],
								[was_directory], [was_readonly], [was_archive], [was_system] )
				SELECT 
					newid(), @Id, @SiteId, 0, 'I', @Modified, @ModifiedBy, 
						null, null, null, null,
							null, null, null, null,
								null, null, null, null;

			IF (EXISTS (SELECT 1 FROM sys.tables WHERE (name='WorkflowInstances'))) 
			BEGIN

				BEGIN TRANSACTION FSINSWFTRANS

				BEGIN TRY
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
												WHEN 1 THEN 'directory_created' 
												ELSE 'file_created' 
											END,
											@Modified, @ModifiedBy, @Modified, @ModifiedBy
						FROM [WorkflowAssociations] WITH (NOLOCK) 
						WHERE (
							(
								(
									([is_directory_scope] = 1) 
									AND (
											((@is_directory = 1) AND ([subscribe_event_directory_created] = 1)) 
											OR ((@is_directory = 0) AND ([subscribe_event_file_created] = 1))
									)
								)
								OR (
										([is_site_scope] = 1) 
										AND (
												((@is_directory = 1) AND ([subscribe_event_directory_created] = 0) AND ([subscribe_event_catch_bubbledevents] = 1)) 
												OR ((@is_directory = 0) AND ([subscribe_event_file_created] = 0) AND ([subscribe_event_catch_bubbledevents] = 1))
										)
								)
							)
							AND ([start_on_create] = 1) 
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

					COMMIT TRANSACTION FSINSWFTRANS
				END TRY
				BEGIN CATCH 
					PRINT ERROR_MESSAGE()
					ROLLBACK TRANSACTION FSINSWFTRANS		-- basically rolls back creating the workflow instance
				END CATCH

			END

		END
		ELSE IF (@DoNotUpdate = 0)
			BEGIN 

				INSERT INTO [FileSystemChangeLog] 
					( [Id], [FileSystemId], [SiteId], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
							[PreviousData], [PreviousFilename], [PreviousFilenameExtension], [PreviousDirectoryName],  
								[PreviousCreated], [PreviousCreatedBy], [PreviousModified], [PreviousModifiedBy],
									[was_directory], [was_readonly], [was_archive], [was_system] )
					SELECT 
						newid(), @Id, @SiteId, 0, 'U', @Modified, @ModifiedBy, 
							[ContentStream], [Filename], [FilenameExtension], [DirectoryName], 
								[Created], [CreatedBy], [Modified], [ModifiedBy], 
									[is_directory], [is_readonly], [is_archive], [is_system] 
					FROM [FileSystem] WITH (NOLOCK) 
					WHERE (([Id] = @Id) AND ([SiteId] = @SiteId))

				UPDATE [FileSystem] 
					SET 
						[Filename]			= @Filename, 
						[FilenameExtension]	= @FileExtension,
						[DirectoryName]		= @DirectoryName, 
						[Modified]			= @Modified, 
						[ModifiedBy]		= @ModifiedBy, 
						[LastAccessed]		= @Modified, 
						[LastAccessedBy]	= @ModifiedBy, 
						[is_readonly]		= @is_readonly, 
						[is_archive]		= @is_archive
						/* 
							We should never change the is_directory and is_system flags
						*/
				WHERE (
					([Id] = @Id) 
					AND ([SiteId] = @SiteId)
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

	COMMIT TRAN FSSAVETRN

	END TRY
	BEGIN CATCH
		PRINT ERROR_MESSAGE()
		ROLLBACK TRANSACTION FSSAVETRN
	END CATCH

END
