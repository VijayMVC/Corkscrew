CREATE TABLE `TabularDataColDefUsage` (
	`Id` 		bigint 				NOT NULL	AUTO_INCREMENT,
	`TableName`	VARCHAR(255)		NOT NULL,
    `ColDefId`	VARCHAR(64)			NOT NULL,

	PRIMARY KEY (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_general_ci;
