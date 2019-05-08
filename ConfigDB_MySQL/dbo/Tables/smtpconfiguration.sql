CREATE TABLE `SMTPConfiguration` (
  `ServerDNS` varchar(1024) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `ServerPort` int(11) NOT NULL DEFAULT '25',
  `SSLRequired` tinyint(1) NOT NULL DEFAULT '0',
  `Username` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `PasswordEncrypted` varchar(1024) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `DefaultSender` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Created` datetime NOT NULL,
  `CreatedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `LastModified` datetime NOT NULL,
  `LastModifiedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  UNIQUE KEY `CreatedBy` (`CreatedBy`),
  UNIQUE KEY `LastModifiedBy` (`LastModifiedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
