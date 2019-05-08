CREATE PROCEDURE [CreateTableDef]
	@Id					uniqueidentifier,
	@SiteId				uniqueidentifier,
	@Name				nvarchar(255),
	@UniqueName			nvarchar(255),
	@FriendlyName		nvarchar(255)	= NULL,
	@Description		nvarchar(1024)	= NULL,
	@Modified			datetime,
	@ModifiedBy			uniqueidentifier
AS
BEGIN

	DECLARE @Sql	nvarchar(max)
		,	@Msg	nvarchar(max);

	IF (NOT EXISTS (SELECT 1 FROM [TabularDataTableDefs] WITH (NOLOCK) WHERE (([Id] = @Id) OR (UPPER([Name]) = UPPER(@Name))))) 
	BEGIN

		--- There should not be a table already with this name
		IF (EXISTS (select 1 from sys.tables where ([name]=@UniqueName)))
		begin
			RAISERROR (N'A system table of that name already exists in the database.', 16, 1);
			RETURN -1;
		end
		
		begin transaction

		begin try

			INSERT INTO [EntityRegistry] 
					([EntityId], [EntityClass]) 
				VALUES 
				(
					@Id, 'tabulardata'
				);

			INSERT INTO [TabularDataTableDefs] 
				( [Id], [SiteId], [Name], [UniqueName], [FriendlyName], [Description], [Created], [CreatedBy], [Modified], [ModifiedBy] ) 
				VALUES 
				(
					@Id, @SiteId, @Name, @UniqueName, @FriendlyName, @Description, @Modified, @ModifiedBy, @Modified, @ModifiedBy
				);

			SET @Sql = CONCAT(N'CREATE TABLE [', @UniqueName, ']( [Corkscrew_RowId] uniqueidentifier not null, constraint [PK_', @UniqueName, '] primary key ([Corkscrew_RowId]) );');
			EXEC(@Sql);

			commit transaction;

		end try
		begin catch
			SET @Msg = ERROR_MESSAGE();
			rollback transaction;

			SET @Msg = CONCAT(N'Error creating tabledef. Error: ', @Msg, '. SQL Query was: ', @Sql);
			RAISERROR (@Msg, 16, 1);
			return -1;
		end catch

	END
	ELSE 
		BEGIN
			UPDATE [TabularDataTableDefs] 
			SET 
				[FriendlyName] = @FriendlyName,
				[Description] = @Description,
				[Modified] = @Modified,
				[ModifiedBy] = @ModifiedBy 
			WHERE ([Id] = @Id);
		END

	return 0;
END