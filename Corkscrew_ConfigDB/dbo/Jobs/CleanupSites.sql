CREATE PROCEDURE [CleanupSites]
AS
BEGIN

	--- any sites that got dropped would have records inserted into the 
	--- DatabaseCleanupQueue table 
	--- Drop these marked databases
	DECLARE @Database			NVARCHAR(255) = NULL
	DECLARE	@Sql				NVARCHAR(MAX) = NULL
	DECLARE @EmptyGuid			uniqueidentifier = dbo.GUIDDEFAULT()

	BEGIN TRANSACTION

	BEGIN TRY

		--- drop FS items that dont belong here
		DELETE FS 
		FROM [FileSystem] FS
			LEFT JOIN [Sites] S WITH (NOLOCK) 
				ON (S.[Id] = FS.[SiteId]) 
		WHERE (
			(FS.[SiteId] <> @EmptyGuid)
			AND (S.[Id] IS NULL) 
		)
		
	
		SELECT @Sql = 
			COALESCE(@Sql, '') +  
				'IF (EXISTS (SELECT 1 FROM master.sys.databases WHERE ([name]=''' + [DatabaseName] + '''))) DROP DATABASE [' + [DatabaseName] + ']; ' + 
					'IF (NOT EXISTS (SELECT 1 FROM master.sys.databases WHERE ([name]=''' + [DatabaseName] + '''))) DELETE FROM [DatabaseCleanupQueue] WHERE ([DatabaseName]=''' + [DatabaseName] + ''');'
			FROM [DatabaseCleanupQueue] 
			WHERE (
				(ISNULL([DatabaseName], '') <> '')
			)

		IF (ISNULL(@Sql, '') <> '')
		BEGIN
			EXEC sp_executesql 
				@stmt = @Sql 
		END


		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

END