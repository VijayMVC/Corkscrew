CREATE TABLE [TabularDataColDefs]
(
	[Id]				uniqueidentifier		not null,
	[SiteId]			uniqueidentifier		not null,
	[Name]				nvarchar(255)			not null,
	[Type]				nvarchar(255)			not null,
	[Nullable]			bit						not null,
	[MaxLength]			int						null,

	[Created]			datetime				not null,
	[CreatedBy]			uniqueidentifier		not null,

	constraint			[PK_TabularDataColDefs]	primary key ([Id], [SiteId], [Name])
)
