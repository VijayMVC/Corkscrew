CREATE TABLE `TabularDataColDefs` (
	`Id` 		VARCHAR(64) 		NOT NULL,
	`Name`		VARCHAR(255)		NOT NULL,
    `Type`		VARCHAR(255)		NOT NULL,
    `Nullable`	BIT					NOT NULL,
    `MaxLength`	BIGINT				NULL,
    `MinValue`	blob				NULL,
    `MaxValue`	blob				NULL,
    
    `Created`		datetime			not null,
    `CreatedBy`		varchar(64)			not null,

	PRIMARY KEY (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_general_ci;
