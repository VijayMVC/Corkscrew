CREATE TABLE `DNSSites` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `DnsName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `SiteId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `SiteId` (`SiteId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
