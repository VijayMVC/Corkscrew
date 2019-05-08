CREATE PROCEDURE [SiteSave]
	@Id					uniqueidentifier,
	@Name				nvarchar(255),
	@Description		nvarchar(512),
	@Created			datetime,
	@CreatedBy			uniqueidentifier,
	@Modified			datetime,
	@ModifiedBy			uniqueidentifier, 
	@ContentDBServer	SYSNAME,
	@ContentDBName		SYSNAME,
	@QuotaBytes			bigint			= NULL
AS
BEGIN

	DECLARE	@CreateSiteDbSql		nvarchar(max)		= null;
	DECLARE @NewSiteRootFolderId	uniqueidentifier	= null;
	DECLARE @WorkflowInstanceId		uniqueidentifier	= NEWID();

	--- If we are running on SQL Azure, we cannot have different databases
	IF (@@VERSION LIKE '%Azure%') 
	BEGIN
		SET @ContentDBName = db_name();
	END

	IF (NOT EXISTS (SELECT 1 FROM [Sites] WITH (NOLOCK) WHERE ([Id] = @Id)))
	BEGIN

		BEGIN TRANSACTION TRNSITESAVE

		BEGIN TRY
			SET @NewSiteRootFolderId = NEWID();

			INSERT INTO [EntityRegistry] 
					([EntityId], [EntityClass]) 
				VALUES 
				(
					@Id, 'site'
				);

			INSERT INTO [Sites] 
				([Id], [Name], [Description], [ContentDBServerName], [ContentDBName], [QuotaBytes], [Created], [CreatedBy], [Modified], [ModifiedBy] ) 
					VALUES ( @Id, @Name, @Description, @ContentDBServer, @ContentDBName, @QuotaBytes, @Created, @CreatedBy, @Modified, @ModifiedBy ) 

			INSERT INTO [SitesChangeLog] 
				( [SiteId], [IsLocked], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
					[PrevName], [PrevDescription], [PrevDBServer], [PrevDBName], [PrevQuotaBytes], [PrevModified], [PrevModifiedBy] ) 
				VALUES ( @Id, 0, 0, 'I', @Modified, @ModifiedBy, 
							null, null, null, null, null, null, null )

			--- Install the filesystem in the ConfigDB filesystem as well
			INSERT INTO [FileSystem] 
				( [Id], [SiteId], [Filename], [FilenameExtension], [DirectoryName], 
					[Created], [CreatedBy], [Modified], [ModifiedBy], [LastAccessed], [LastAccessedBy], 
						[is_directory], [is_readonly], [is_archive], [is_system],
							[ContentStream] )
			VALUES 
			(
				@NewSiteRootFolderId, @Id, '/', NULL, NULL, 
					@Created, @CreatedBy, @Modified, @ModifiedBy, @Modified, @ModifiedBy, 
						1, 0, 0, 0, NULL 
			)

			COMMIT TRANSACTION TRNSITESAVE

		END TRY
		BEGIN CATCH
			PRINT ERROR_MESSAGE()
			ROLLBACK TRANSACTION TRNSITESAVE
		END CATCH

		IF (EXISTS (SELECT 1 FROM sys.tables WHERE (name='WorkflowInstances'))) 
		BEGIN

			BEGIN TRANSACTION SITEINSWFTRANS

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
									0, 0, NULL, NULL, 0, 'site_created', 
										@Modified, @ModifiedBy, @Modified, @ModifiedBy
					FROM [WorkflowAssociations] WITH (NOLOCK) 
					WHERE (
						([is_farm_scope] = 1) 
						AND ([start_on_create] = 1)
						AND (
								([subscribe_event_site_created] = 1) 
								OR (([subscribe_event_site_created] = 0) AND ([subscribe_event_catch_bubbledevents] = 1))
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

				COMMIT TRANSACTION SITEINSWFTRANS
			END TRY
			BEGIN CATCH 
				PRINT ERROR_MESSAGE()
				ROLLBACK TRANSACTION SITEINSWFTRANS		-- basically rolls back creating the workflow instance
			END CATCH

		END

	END
	ELSE
		BEGIN 
			INSERT INTO [SitesChangeLog] 
				( [SiteId], [IsLocked], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
					[PrevName], [PrevDescription], [PrevDBServer], [PrevDBName], [PrevQuotaBytes], [PrevModified], [PrevModifiedBy] ) 
				SELECT @Id, 0, 0, 'U', @Modified, @ModifiedBy, 
						[Name], [Description], [ContentDBServerName], [ContentDBName], [QuotaBytes], [Modified], [ModifiedBy] 
				FROM [Sites] WITH (NOLOCK) 
				WHERE ([Id] = @Id)


			UPDATE [Sites] 
			SET 
				[Name] = @Name, 
				[Description] = @Description, 
				[QuotaBytes] = @QuotaBytes,
				[ContentDBServerName] = @ContentDBServer, 
				[ContentDBName] = @ContentDBName,
				[Modified] = @Modified,
				[ModifiedBy] = @ModifiedBy
			WHERE ([Id] = @Id)

			BEGIN TRANSACTION SITEUPDWFTRANS

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
										0, 0, NULL, NULL, 0, 'site_modified', 
											@Modified, @ModifiedBy, @Modified, @ModifiedBy
						FROM [WorkflowAssociations] WITH (NOLOCK) 
						WHERE (
							([is_farm_scope] = 1) 
							AND ([start_on_create] = 1)
							AND (
									([subscribe_event_site_modified] = 1) 
									OR (([subscribe_event_site_modified] = 0) AND ([subscribe_event_catch_bubbledevents] = 1))
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

				COMMIT TRANSACTION SITEUPDWFTRANS
			END TRY
			BEGIN CATCH 
				PRINT ERROR_MESSAGE()
				ROLLBACK TRANSACTION SITEUPDWFTRANS		-- basically rolls back creating the workflow instance
			END CATCH
		END

END
