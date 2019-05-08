CREATE TABLE `WorkflowHistory` (
	`Id`									varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowAssociationId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowInstanceId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `AssociationData`						longtext 						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    
    `State`									int								not null default 0,
    `CompletedReason`						int								not null default 0,
    `ErrorMessage`							longtext						default null,
    
	`Created` 								datetime 						NOT NULL,
	`CreatedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
   
    PRIMARY KEY (`Id`),
	UNIQUE KEY `Id` (`Id`)
    
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;