CREATE PROCEDURE [GetColumnsAddedToTable]
	@TableId		uniqueidentifier
AS
BEGIN
	
SELECT 
	[Id], [TableId], [ColDefId], [LocalName], [Created], [CreatedBy]
	FROM [TabularDataTableColumns] WITH (NOLOCK) 
WHERE ([TableId] = @TableId);

END