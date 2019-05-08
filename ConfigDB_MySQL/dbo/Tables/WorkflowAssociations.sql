CREATE TABLE `WorkflowAssociations` (
	`Id`									varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `WorkflowDefinitionId`					varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `Name`									varchar(255) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `AssociationData`						longtext 						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    
    `FarmId`								varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    `SiteId`								varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    `DirectoryId`							varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    
    `subscribe_manual_start`				bit not null,
    `start_on_create`						bit not null,
    `start_on_modify`						bit not null,
    `prevent_new_instances`					bit not null,
    `is_enabled`							bit not null,
    
    `subscribe_event_farm_created`			bit								NOT NULL	DEFAULT 0,
	`subscribe_event_farm_modified`			bit								NOT NULL	DEFAULT 0,
	`subscribe_event_farm_deleted`			bit								NOT NULL	DEFAULT 0,

	`subscribe_event_site_created`			bit								NOT NULL	DEFAULT 0,
	`subscribe_event_site_modified`			bit								NOT NULL	DEFAULT 0,
	`subscribe_event_site_deleted`			bit								NOT NULL	DEFAULT 0,

	`subscribe_event_directory_created`		bit								NOT NULL	DEFAULT 0,
	`subscribe_event_directory_modified`	bit								NOT NULL	DEFAULT 0,
	`subscribe_event_directory_deleted`		bit								NOT NULL	DEFAULT 0,

	`subscribe_event_file_created`			bit								NOT NULL	DEFAULT 0,
	`subscribe_event_file_modified`			bit								NOT NULL	DEFAULT 0,
	`subscribe_event_file_deleted`			bit								NOT NULL	DEFAULT 0,

	`subscribe_event_catch_bubbledevents`	bit								NOT NULL	DEFAULT 0,
    
	`Created` 								datetime 						NOT NULL,
	`CreatedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`Modified` 								datetime 						NOT NULL,
	`ModifiedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    
    `is_farm_scope`							bit								generated always as ( if((`SiteId` is null), 1, 0) ) stored,
    `is_site_scope`							bit								generated always as ( if((`DirectoryId` is null), 1, 0) ) stored,
    `is_directory_scope`					bit								generated always as ( if((`DirectoryId` is not null), 1, 0) ) stored,
    
    PRIMARY KEY (`Id`),
	UNIQUE KEY `Id` (`Id`)
    
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;