CREATE TABLE `Permissions` (
  `Id` varchar(64) COLLATE latin1_general_ci NOT NULL,
  `PrincipalId` varchar(64) COLLATE latin1_general_ci not null,
  `CorkscrewUri` longtext collate latin1_general_ci not null,
  `IsRead` bit NOT NULL,
  `IsContribute` bit NOT NULL,
  `IsFullControl` bit NOT NULL,
  `IsChildAccess` bit NOT NULL,
  PRIMARY KEY (`Id`, `PrincipalId` ),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
