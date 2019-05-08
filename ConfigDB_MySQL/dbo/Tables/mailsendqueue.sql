CREATE TABLE `MailSendQueue` (
  `Id` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `From` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Recipient` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `InternalCopyTo` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `Subject` varchar(512) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `ContentHtml` longtext CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `WasSent` tinyint(1) NOT NULL,
  `Created` datetime NOT NULL,
  `CreatedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `LastModified` datetime NOT NULL,
  `LastModifiedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  UNIQUE KEY `CreatedBy` (`CreatedBy`),
  UNIQUE KEY `LastModifiedBy` (`LastModifiedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
