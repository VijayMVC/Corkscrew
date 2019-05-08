CREATE TABLE `WorkflowInstances` (
	`Id`									varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowAssociationId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowDefinitionId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `AssociationData`						longtext 						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    
    `FarmId`								varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `SiteId`								varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    `DirectoryId`							varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    `FileId`								varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    
    `CurrentState`							int								not null default 0,
    `CompletedReason`						int								not null default 0,
    `ErrorMessage`							longtext						default null,
    `IsLoadedInRuntime`						bit								not null default 0,
    `SqlPersistenceId`						varchar(64)						null,
    `InstanceStartEvent`					varchar(255)					not null,
    
	`Created` 								datetime 						NOT NULL,
	`CreatedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`Modified` 								datetime 						NOT NULL,
	`ModifiedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    
    PRIMARY KEY (`Id`),
	UNIQUE KEY `Id` (`Id`)
    
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;