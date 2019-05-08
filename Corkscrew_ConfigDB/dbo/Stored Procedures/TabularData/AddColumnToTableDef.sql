CREATE PROCEDURE [AddColumnToTableDef] 
	@Id				uniqueidentifier,
	@ColDefId		uniqueidentifier,
	@TableDefId		uniqueidentifier,
	@LocalName		nvarchar(255)	= NULL,
	@Created		datetime,
	@CreatedBy		uniqueidentifier
AS
BEGIN

	DECLARE @Msg			nvarchar(max) = N''
		,	@Sql			nvarchar(max) = N''
		,	@TableName		nvarchar(max) = N''
		,	@Type			nvarchar(255) = N''
		,	@MaxLength		bigint		  = 0
		,	@AllowNull		bit			  = 1
		,	@DefaultValue	nvarchar(max) = N'' 
		,	@ColSiteId		uniqueidentifier = NULL 
		,	@TableSiteId	uniqueidentifier = NULL

	--- verify table exists
	IF (NOT EXISTS(SELECT 1 FROM [TabularDataTableDefs] WITH (NOLOCK) WHERE ([Id] = @TableDefId))) 
	BEGIN
		SET @Msg = CONCAT(N'Table (Id: ', cast(@TableDefId as varchar(64)), ') does not exist.');
		RAISERROR (@Msg, 16, 1);
		RETURN -1;
	END
	ELSE 
		BEGIN
			SELECT @TableName = [UniqueName] FROM [TabularDataTableDefs] WITH (NOLOCK) WHERE ([Id] = @TableDefId);
		END

	--- verify coldef exists
	IF (NOT EXISTS(SELECT 1 FROM [TabularDataColDefs] WITH (NOLOCK) WHERE ([Id] = @ColDefId))) 
	BEGIN
		SET @Msg = CONCAT(N'Column (Id: ', cast(@ColDefId as varchar(64)), ') does not exist.');
		RAISERROR (@Msg, 16, 1);
		RETURN -1;
	END

	SELECT @ColSiteId = [SiteId] FROM [TabularDataColDefs] WITH (NOLOCK) WHERE ([Id] = @ColDefId);
	SELECT @TableSiteId = [SiteId] FROM [TabularDataTableDefs] WITH (NOLOCK) WHERE ([Id] = @TableDefId);

	IF (@ColSiteId <> @TableSiteId) 
	BEGIN
		RAISERROR (N'Table and column definitions do not belong to the same site', 16, 1);
		RETURN -1;
	END

	IF (@LocalName = '') SET @LocalName = NULL;

	SELECT 
		@LocalName = ISNULL(@LocalName, [Name]), 
		@Type = [Type], 
		@AllowNull = [Nullable], 
		@MaxLength = [MaxLength] 
	FROM [TabularDataColDefs] WITH (NOLOCK) WHERE ([Id] = @ColDefId)

	IF (UPPER(@LocalName) = 'CORKSCREW_ROWID') 
	BEGIN 
		SET @LocalName = CONCAT(@LocalName, '_1')
	END

	IF (EXISTS(SELECT 1 FROM [TabularDataTableColumns] WITH (NOLOCK) WHERE (([ColDefId] = @ColDefId) AND ([TableId] = @TableDefId) AND ([LocalName] = @LocalName)))) 
	BEGIN
		SET @Msg = CONCAT(N'Column ', @LocalName, ' is already added to this table (Id: ', cast(@TableDefId as varchar(64)), ') with this local name.');
		RAISERROR (@Msg, 16, 1);
		RETURN -1;
	END

	-- select a decent default value if AllowNull is false, otherwise SQL Server will reject our ALTER statement
	IF (@AllowNull = 0) 
	BEGIN
		SET @DefaultValue = CASE @Type 
								WHEN 'varbinary' THEN '0x' 
								WHEN 'ntext' THEN ''''''
								WHEN 'nvarchar' THEN '''''' 
								WHEN 'bigint' THEN '0' 
								WHEN 'float' THEN '0.00' 
								WHEN 'datetime' THEN 'dbo.[DATETIMEDEFAULT]()' 
								WHEN 'bit' THEN '0'
								WHEN 'uniqueidentifier' THEN 'dbo.[GUIDDEFAULT]()'
							END
	END

	IF (@Type = 'nvarchar')
	BEGIN
		--- append the length
		SET @Type = CONCAT('nvarchar(', cast(@MaxLength as nvarchar(64)), ')')
	END
	ELSE IF (@Type = 'varbinary') 
			BEGIN
				--- append the length
				SET @Type = 'varbinary(max)'
			END

	SET @Sql = CONCAT(
					N'ALTER TABLE [', @TableName, '] ADD [', @LocalName, '] ', @Type, 
					CASE @AllowNull 
						WHEN 1 THEN ' NULL' 
						WHEN 0 THEN ' NOT NULL DEFAULT(' + @DefaultValue + ')' 
					END
			   )


	BEGIN TRAN

	BEGIN TRY

		EXEC(@Sql)

		INSERT INTO [TabularDataTableColumns] 
			( [Id], [TableId], [ColDefId], [LocalName], [Created], [CreatedBy] ) 
			VALUES 
			(
				@Id, @TableDefId, @ColDefId, @LocalName, @Created, @CreatedBy
			);

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		SET @Msg = ERROR_MESSAGE() 
		SET @Msg = CONCAT(@Msg, ' SQL Statement was: ', @Sql)
		ROLLBACK TRAN
		RAISERROR (@Msg, 16, 1);
		RETURN -1;
	END CATCH

	RETURN 0;
END