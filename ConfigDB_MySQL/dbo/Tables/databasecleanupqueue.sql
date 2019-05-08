CREATE TABLE `DatabaseCleanupQueue` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ServerName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `DatabaseName` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
