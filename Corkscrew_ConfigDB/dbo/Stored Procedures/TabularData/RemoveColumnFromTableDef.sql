CREATE PROCEDURE [RemoveColumnFromTableDef]
	@TableDefId			uniqueidentifier,
	@ColDefId			uniqueidentifier,
	@LocalName			nvarchar(255)
AS
BEGIN

	DECLARE @Msg			nvarchar(max) = N''
		,	@Sql			nvarchar(max) = N''
		,	@TableName		nvarchar(255) = N''
		,	@ColumnName		nvarchar(255) = N''

	IF (EXISTS (SELECT 1 FROM [TabularDataTableColumns] WITH (NOLOCK) WHERE (([ColDefId] = @ColDefId) AND ([TableId] = @TableDefId) AND ([LocalName] = @LocalName))))
	BEGIN
		SELECT @TableName = [Name] FROM [TabularDataTableDefs] WITH (NOLOCK) WHERE ([Id] = @TableDefId);

		SET @Sql = CONCAT(N'ALTER TABLE [', @TableName, '] DROP COLUMN [', @LocalName, '];')

		begin transaction

		begin try 

			--- drop the column from the actual table
			EXEC(@Sql)

			--- remove it from column references table
			DELETE FROM [TabularDataTableColumns] WHERE (([ColDefId] = @ColDefId) AND ([TableId] = @TableDefId) AND ([LocalName] = @LocalName));

			commit transaction;
		end try
		begin catch
			rollback transaction;
			return -1;
		end catch
	END

END