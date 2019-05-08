CREATE TABLE `WorkflowDefinitions` (
	`Id`									varchar(64)						CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `Name`									varchar(255) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `Description`							varchar(1024) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    `DefaultAssociationData`				longtext 						CHARACTER SET latin1 COLLATE latin1_general_ci default NULL,
    `allow_manual_start`					bit not null,
    `start_on_create`						bit not null,
    `start_on_modify`						bit not null,
    `is_enabled`							bit not null,
    
    `allow_event_farm_created`				bit								NOT NULL	DEFAULT 0,
	`allow_event_farm_modified`				bit								NOT NULL	DEFAULT 0,
	`allow_event_farm_deleted`				bit								NOT NULL	DEFAULT 0,

	`allow_event_site_created`				bit								NOT NULL	DEFAULT 0,
	`allow_event_site_modified`				bit								NOT NULL	DEFAULT 0,
	`allow_event_site_deleted`				bit								NOT NULL	DEFAULT 0,

	`allow_event_directory_created`			bit								NOT NULL	DEFAULT 0,
	`allow_event_directory_modified`		bit								NOT NULL	DEFAULT 0,
	`allow_event_directory_deleted`			bit								NOT NULL	DEFAULT 0,

	`allow_event_file_created`				bit								NOT NULL	DEFAULT 0,
	`allow_event_file_modified`				bit								NOT NULL	DEFAULT 0,
	`allow_event_file_deleted`				bit								NOT NULL	DEFAULT 0,

	`allow_event_catch_bubbledevents`		bit								NOT NULL	DEFAULT 0,
    
    `all_events_switch`						bit								GENERATED ALWAYS AS ( `allow_event_farm_created` & `allow_event_farm_modified` & `allow_event_farm_deleted` &
																					`allow_event_site_created` & `allow_event_site_modified` & `allow_event_site_deleted` & 
																						`allow_event_directory_created` & `allow_event_directory_modified` & `allow_event_directory_deleted` & 
																							`allow_event_file_created` & `allow_event_file_modified` & `allow_event_file_deleted` ) STORED,
                                                                                            
	`Created` 								datetime 						NOT NULL,
	`CreatedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
	`Modified` 								datetime 						NOT NULL,
	`ModifiedBy` 							varchar(64) 					CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
    
    PRIMARY KEY (`Id`),
	UNIQUE KEY `Id` (`Id`)
    
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;