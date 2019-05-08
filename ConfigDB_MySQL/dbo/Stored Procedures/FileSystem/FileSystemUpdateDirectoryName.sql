DELIMITER ;;
CREATE PROCEDURE `FileSystemUpdateDirectoryName`(
	IN p_SiteId					varchar(64), 
    IN p_OldDirectoryName		longtext,
    IN p_NewDirectoryName		longtext,
	IN p_Modified				datetime, 
	IN p_ModifiedBy				varchar(64)
)
BEGIN

	DECLARE v_DNWhereString	longtext;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION 
	BEGIN
		rollback;
	END;
    
    SET v_DNWhereString = p_OldDirectoryName;
	
    IF ((SELECT count(*) FROM `FileSystem` WHERE ((`SiteId` = p_SiteId) and (`DirectoryName` = p_NewDirectoryName))) = 0) THEN
		call raise_error;
	END IF;
    
    IF (right(v_DNWhereString, 1) <> '/') THEN 
		SET v_DNWhereString = concat(v_DNWhereString, '/');
	END IF;
    
    SET v_DNWhereString = concat(v_DNWhereString, '%');
    
    IF ((right(p_OldDirectoryName, 1) = '/') and (right (p_NewDirectoryName, 1) <> '/')) THEN 
		SET p_NewDirectoryName = concat(p_NewDirectoryName, '/');
    ELSE IF ((right(p_OldDirectoryName, 1) <> '/') and (right (p_NewDirectoryName, 1) = '/')) THEN 
			SET p_NewDirectoryName = left(p_NewDirectoryName, (length(p_NewDirectoryName) - 1)); 
		  END IF; 
	END IF;
    
    start transaction;
    
	INSERT INTO `FileSystemChangeLog` 
		( `Id`, `FileSystemId`, `SiteId`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
			`PreviousData`, `PreviousFilename`, `PreviousFilenameExtension`, `PreviousDirectoryName`,  
				`PreviousCreated`, `PreviousCreatedBy`, `PreviousModified`, `PreviousModifiedBy`,
					`was_directory`, `was_readonly`, `was_archive`, `was_system` )
		SELECT
			UUID(), `Id`, `SiteId`, 0, 'UP', p_Modified, p_ModifiedBy, 
				`ContentStream`, `Filename`, `FileExtension`, `DirectoryName`, 
					`Created`, `CreatedBy`, `Modified`, `ModifiedBy`, 
						`is_directory`, `is_readonly`, `is_archive`, `is_system`
			from `filesystem` 
			where (
				(`SiteId` = p_SiteId) 
				and (`DirectoryName` like v_DNWhereString)
			);

	UPDATE `FileSystem` 
		SET 
			`DirectoryName`		= REPLACE(`DirectoryName`, p_OldDirectoryName, p_NewDirectoryName),
			`Modified`			= p_Modified, 
			`ModifiedBy`		= p_ModifiedBy, 
			`LastAccessed`		= p_Modified, 
			`LastAccessedBy`	= p_ModifiedBy
	WHERE (
		(`SiteId` = p_SiteId) 
        and (`DirectoryName` like v_DNWhereString)
	);
    
    commit;
    
END ;;
DELIMITER ;