CREATE TABLE `UserGroups` (
  `Id` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Username` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `DisplayName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `EmailAddress` varchar(512) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `IsWinADGroup` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
