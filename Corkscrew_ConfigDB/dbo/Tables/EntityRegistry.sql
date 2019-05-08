CREATE TABLE [EntityRegistry]
(
	[EntityId]				uniqueidentifier		NOT NULL,
	[EntityClass]			nvarchar(1024)			NOT NULL,

	constraint				[PK_EntityRegistry]		primary key ([EntityId])
)
