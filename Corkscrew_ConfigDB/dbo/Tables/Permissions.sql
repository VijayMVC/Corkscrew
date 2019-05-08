CREATE TABLE [Permissions]
(
	[Id]				uniqueidentifier				not null		default newsequentialid(),
	[PrincipalId]		uniqueidentifier				not null,
	[CorkscrewUri]		nvarchar(max)					not null,
	[IsRead]			bit								not null, 
	[IsContribute]		bit								not null, 
	[IsFullControl]		bit								not null, 
	[IsChildAccess]		bit								not null,

	constraint			[PK_Permissions]				primary key		( [Id], [PrincipalId] )
)
