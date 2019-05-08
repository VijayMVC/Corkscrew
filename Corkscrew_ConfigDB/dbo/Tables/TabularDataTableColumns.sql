CREATE TABLE [TabularDataTableColumns]
(
	[Id]			uniqueidentifier				not null,
	[TableId]		uniqueidentifier				not null,
	[ColDefId]		uniqueidentifier				not null,
	[LocalName]		nvarchar(255)					not null,

	[Created]		datetime						not null,
	[CreatedBy]		uniqueidentifier				not null,

	constraint		[PK_TabularDataTableColumns]		primary key		([Id], [TableId], [LocalName])
)
