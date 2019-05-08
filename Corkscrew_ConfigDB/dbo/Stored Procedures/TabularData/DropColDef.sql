CREATE PROCEDURE [DropColDef]
	@Id			uniqueidentifier
AS
BEGIN

	DECLARE @Sql	nvarchar(max) = N'';

	BEGIN TRANSACTION

	BEGIN TRY
		
		SELECT @Sql = CONCAT(@Sql, 'ALTER TABLE [' + TDTD.[Name] + '] DROP COLUMN [' + TDCD.[Name] + ']; ') 
			FROM [TabularDataColDefs] TDCD WITH (NOLOCK) 
				INNER JOIN [TabularDataTableColumns] TDCDU WITH (NOLOCK) ON (TDCDU.ColDefId = TDCD.[Id]) 
					INNER JOIN [TabularDataTableDefs] TDTD WITH (NOLOCK) ON (TDTD.[Id] = TDCDU.[TableId]) 
		WHERE (TDCD.[Id] = @Id) 

		EXEC(@Sql)

		DELETE FROM [TabularDataColDefs] 
			WHERE ([Id] = @Id);

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		RETURN -1;
	END CATCH

END