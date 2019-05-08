CREATE TABLE `Sites` (
  `Id` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Description` varchar(512) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `ContentDBServerName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `ContentDBName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `QuotaBytes` bigint(20) DEFAULT NULL,
  `Created` datetime NOT NULL,
  `CreatedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Modified` datetime NOT NULL,
  `ModifiedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `IDX_Sites` (`ContentDBServerName`,`ContentDBName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
