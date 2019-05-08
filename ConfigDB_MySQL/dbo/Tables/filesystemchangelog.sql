CREATE TABLE `FileSystemChangeLog` (
  `Id` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `FileSystemId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `SiteId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `IsProcessed` bit not null default 0,
  `ChangeType` char(2) character set latin1 collate latin1_general_ci,
  `ChangeTimeStamp` datetime not null,
  `ChangedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  
  `PreviousFilename` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `PreviousFilenameExtension` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `PreviousDirectoryName` longtext CHARACTER SET latin1 COLLATE latin1_general_ci ,
  `PreviousCreated` datetime NOT NULL,
  `PreviousCreatedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `PreviousModified` datetime NOT NULL,
  `PreviousModifiedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `was_directory` tinyint(1) NOT NULL,
  `was_readonly` tinyint(1) NOT NULL,
  `was_archive` tinyint(1) NOT NULL,
  `was_system` tinyint(1) NOT NULL,
  `PreviousData` longblob,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
