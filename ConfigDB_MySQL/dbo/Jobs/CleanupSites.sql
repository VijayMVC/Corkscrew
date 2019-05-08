DELIMITER ;;
CREATE PROCEDURE `CleanupSites`()
BEGIN
	DECLARE var_database	nvarchar(255);
    DECLARE var_emptyid		varchar(64);
   
	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    SELECT var_emptyid = `corkscrew_configdb`.`GuidDefault`();
   
	start transaction;
    
		delete fs 
		from `FileSystem` fs 
			left join `sites` s 
				on (s.`Id` = fs.`SiteId`) 
		where (
			(fs.`SiteId` <> var_emptyid) 
            and (s.`Id` is null)
        );
        
	commit;
    
END ;;
DELIMITER ;