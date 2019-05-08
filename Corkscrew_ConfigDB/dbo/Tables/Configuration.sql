CREATE TABLE [Configuration]
(
	[Id]				int					NOT NULL		identity(1, 1),
	[Name]				nvarchar(255)		NOT NULL,
	[Value]				nvarchar(4000)		NULL,

	CONSTRAINT	[PK_Configuration_Id]	PRIMARY KEY ([Id])
)
