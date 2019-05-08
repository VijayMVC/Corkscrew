CREATE TABLE [TabularDataTableDefs]
(
	[Id]					uniqueidentifier			not null,
	[SiteId]				uniqueidentifier			not null,
	[Name]					nvarchar(255)				not null,
	[UniqueName]			nvarchar(255)				not null,
	[FriendlyName]			nvarchar(255)				null,
	[Description]			nvarchar(1024)				null,

	[Created]				datetime					not null,
	[CreatedBy]				uniqueidentifier			not null,
	[Modified]				datetime					not null,
	[ModifiedBy]			uniqueidentifier			not null,

	constraint				[PK_TabularDataTableDefs]	primary key ([Id], [SiteId], [Name])
)
