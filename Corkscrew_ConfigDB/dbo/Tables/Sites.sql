CREATE TABLE [Sites]
(
	[Id]					uniqueidentifier		not null, 
    [Name]					nvarchar(255)			not null,
	[Description]			nvarchar(512)			null,

	[ContentDBServerName]	nvarchar(255)			not null,
	[ContentDBName]			nvarchar(255)			not null,

	[QuotaBytes]			bigint					null,

	[Created]				datetime				not null,
	[CreatedBy]				uniqueidentifier		not null,
	[Modified]				datetime				not null,
	[ModifiedBy]			uniqueidentifier		not null

	CONSTRAINT [PK_Sites_Id] PRIMARY KEY ([Id])
)
