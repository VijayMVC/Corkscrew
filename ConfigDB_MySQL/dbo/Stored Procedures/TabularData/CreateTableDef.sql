DELIMITER ;;
CREATE PROCEDURE `CreateTableDef` 
(
	IN p_Id				varchar(64),
	IN p_Name 			varchar(255),
    IN p_FriendlyName	varchar(255),
    IN p_Description	varchar(1024),
    IN p_Modified		datetime,
    IN p_ModifiedBy		varchar(64)
)
BEGIN

    if ((select count(*) from `TabularDataTableDefs` where ((`Id` = p_Id) or (UPPER(`Name`) = UPPER(p_Name)))) = 0) then 
		INSERT INTO `TabularDataTableDefs` 
			( `Id`, `Name`, `FriendlyName`, `Description`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy` ) 
			VALUES 
			(
				p_Id, p_Name, p_FriendlyName, p_Description, p_Modified, p_ModifiedBy, p_Modified, p_ModifiedBy
			);

		SET @CreateTableSql = CONCAT(N'CREATE TABLE `', p_Name, '` ( `RowId` bigint not null auto_increment, primary key (`RowId`) ) ENGINE=InnoDB, DEFAULT CHARACTER SET=latin1, COLLATE=latin1_general_ci;');
        PREPARE DSQL01 FROM @CreateTableSql;
		EXECUTE DSQL01;
        DEALLOCATE PREPARE DSQL01;
	else 
		UPDATE `TabularDataTableDefs` 
        SET 
			`FriendlyName` = p_FriendlyName,
            `Description` = p_Description,
            `Modified` = p_Modified,
            `ModifiedBy` = p_ModifiedBy 
		WHERE (`Id` = p_Id);
    end if;

END ;;
DELIMITER ;
