CREATE TABLE `SignaturePanel` (
  `Id` varchar(64) COLLATE latin1_general_ci NOT NULL,
  `PanelType` int(11) NOT NULL,
  `IsTimelimited` tinyint(1) NOT NULL,
  `Deadline` datetime NOT NULL,
  `ResponsesInProgress` tinyint(1) NOT NULL,
  `CurrentState` int(11) NOT NULL,
  `Created` datetime NOT NULL,
  `CreatedBy` varchar(64) COLLATE latin1_general_ci NOT NULL,
  `Modified` datetime NOT NULL,
  `ModifiedBy` varchar(64) COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  UNIQUE KEY `CreatedBy` (`CreatedBy`),
  UNIQUE KEY `ModifiedBy` (`ModifiedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;