CREATE TABLE `UserGroupMembers` (
  `UserId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `GroupId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `IsWinADMembership` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`UserId`, `GroupId`),
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
