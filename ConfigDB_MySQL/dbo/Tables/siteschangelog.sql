CREATE TABLE `SitesChangeLog` (
  `Id` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `SiteId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `IsLocked` bit not null default 0,
  `IsProcessed` bit not null default 0,
  `ChangeType` char(1) character set latin1 collate latin1_general_ci,
  `ChangeTimeStamp` datetime not null,
  `ChangedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
  `PrevName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
  `PrevDescription` varchar(512) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `PrevDBServer` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
  `PrevDBName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
  `PrevQuotaBytes` bigint(20) DEFAULT NULL,
  `PrevModified` datetime NULL,
  `PrevModifiedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
