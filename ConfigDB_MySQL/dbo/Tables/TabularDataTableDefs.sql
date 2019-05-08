CREATE TABLE `TabularDataTableDefs` (
	`Id` 			VARCHAR(64) 		NOT NULL,
	`Name`			VARCHAR(255)		NOT NULL,
    `FriendlyName`	VARCHAR(255)		NULL,
    `Description`	VARCHAR(1024)		NULL,
    
    `Created`		datetime			not null,
    `CreatedBy`		varchar(64)			not null,
    `Modified`		datetime			not null,
    `ModifiedBy`	varchar(64)			not null,

	PRIMARY KEY (`Id`, `Name`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_general_ci;
