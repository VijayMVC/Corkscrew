CREATE PROCEDURE [DropTableDef]
	@Id		uniqueidentifier
AS
BEGIN

	DECLARE @Sql	nvarchar(max) = N''
		,	@Msg	nvarchar(max);

	BEGIN TRANSACTION

	BEGIN TRY
		
		--- Drop the actual table
		SELECT @Sql = CONCAT(@Sql, 'DROP TABLE [' + [UniqueName] + ']; ') 
			FROM [TabularDataTableDefs] WITH (NOLOCK)
		WHERE ([Id] = @Id) 

		EXEC(@Sql)

		--- Delete the column usage info
		DELETE TDCDU 
		FROM [TabularDataTableColumns] TDCDU 
		INNER JOIN [TabularDataTableDefs] TDTD 
			ON (TDTD.[Id] = TDCDU.[TableId]) 
		WHERE (TDTD.[Id] = @Id);

		--- Delete the def from the defs table
		DELETE FROM [TabularDataTableDefs] WHERE ([Id] = @Id);

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		SET @Msg = ERROR_MESSAGE();
		rollback transaction;

		SET @Msg = CONCAT(N'Error dropping tabledef. Error: ', @Msg, '. SQL Query was: ', @Sql);
		RAISERROR (@Msg, 16, 1);
		return -1;
	END CATCH
	
END