CREATE PROCEDURE [GetTableData]
	@Id			uniqueidentifier
AS
BEGIN

	DECLARE @TableName	nvarchar(255) = N''
		,	@Sql		nvarchar(max) = N''

	SELECT @TableName = [Name] FROM [TabularDataTableDefs] WITH (NOLOCK) 
		WHERE ([Id] = @Id);

	IF (ISNULL(@TableName, '') = '') 
	BEGIN
		SET @Sql = CONCAT(N'No table was found for Id ', cast(@Id as varchar(64)))
		RAISERROR (@Sql, 16, 1);
		RETURN -1;
	END

	SET @Sql = 'SELECT * FROM [' + @TableName + '] WITH (NOLOCK);'
	EXEC(@Sql)

END