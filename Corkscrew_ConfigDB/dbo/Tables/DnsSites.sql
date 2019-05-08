CREATE TABLE [DnsSites]
(
	[Id]		bigint				not null		identity(1, 1),
	[DnsName]	nvarchar(255)		not null,
	[SiteId]	uniqueidentifier	not null,

	constraint	[PK_DnsSites_Id]	primary key		([Id], [DnsName])
)
