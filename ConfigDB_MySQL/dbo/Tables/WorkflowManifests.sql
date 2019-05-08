CREATE TABLE `WorkflowManifests` (
	`Id`									varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowDefinitionId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,

	`WorkflowEngine`						char(4)							CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,

	`OutputAssemblyName`					varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`WorkflowClassName`						varchar(1024)					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,

	`build_assembly_title`					varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_description`			varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_company`				varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_product`				varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_copyright`				varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_trademark`				varchar(255)					CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_version`				varchar(16)						CHARACTER SET latin1 COLLATE latin1_general_ci NULL,
	`build_assembly_fileversion`			varchar(16)						CHARACTER SET latin1 COLLATE latin1_general_ci NULL,

	`always_compile`						bit								NOT NULL,
	`cache_compile_results`					bit								NOT NULL,

	`last_compiled_datetime`				datetime						NULL,
    
	`Created` 								datetime 						NOT NULL,
	`CreatedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`Modified` 								datetime 						NOT NULL,
	`ModifiedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    
    PRIMARY KEY (`Id`),
	UNIQUE KEY `Id` (`Id`)
    
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;