﻿CREATE PROCEDURE [SiteGetHistoryById]
	@Id		bigint
AS
	SELECT 
		[Id], [SiteId], [IsLocked], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
			[PrevName], [PrevDescription], [PrevDBServer], [PrevDBName], [PrevQuotaBytes], [PrevModified], [PrevModifiedBy]
	FROM [SitesChangeLog] WITH (NOLOCK) 
	WHERE ([Id] = @Id)
