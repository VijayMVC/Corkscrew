CREATE TABLE [DatabaseCleanupQueue]
(
	[Id]			INT					NOT NULL		IDENTITY(1, 1), 
	[ServerName]	NVARCHAR(255)		NOT NULL,
	[DatabaseName]	NVARCHAR(255)		NOT NULL,

	constraint		[PK_DatabaseCleanupQueue_Id]		PRIMARY KEY ([Id], [DatabaseName])
)
