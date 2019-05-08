CREATE TABLE `MIMETypes` (
  `FileExtension` varchar(24) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `KnownMimeType` varchar(255) CHARACTER SET latin1 NOT NULL COLLATE latin1_general_ci DEFAULT 'application/octet-stream',
  PRIMARY KEY (`FileExtension`),
  UNIQUE KEY `FileExtension` (`FileExtension`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
