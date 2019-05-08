CREATE TABLE `WorkflowManifestItems` (
	`Id`									varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowDefinitionId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowManifestId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,

	`Filename`								varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`FilenameExtension`						varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`ItemType`								int								NOT NULL,

	`build_relative_folder`					varchar(1024)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`runtime_folder`						varchar(1024)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	
	`required_for_execution`				bit								NOT NULL, 

	`ContentStream`							longblob						NOT NULL,
	`ContentSize`							bigint							not null default 0,
    
	`Created` 								datetime 						NOT NULL,
	`CreatedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`Modified` 								datetime 						NOT NULL,
	`ModifiedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    
    PRIMARY KEY (`Id`),
	UNIQUE KEY `Id` (`Id`)
    
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;