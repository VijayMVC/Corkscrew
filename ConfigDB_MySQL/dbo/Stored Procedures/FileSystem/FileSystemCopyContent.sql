DELIMITER ;;
CREATE PROCEDURE `FileSystemCopyContent`(
	IN p_SourceId		varchar(64),
    IN p_DestinationId	varchar(64),
    IN p_Modified		datetime,
    IN p_ModifiedBy		varchar(64)
)
BEGIN
	
    declare v_RightNow			datetime;
    declare v_ContentStream		longblob;
    
    declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    set v_RightNow = now();
    set v_ContentStream = null;
    
    if ((select count(*) from `FileSystem` where (`Id` = p_DestinationId)) = 0) then 
		call raise_error;
	end if;
    
    if ((select count(*) from `FileSystem` where (`Id` = p_SourceId)) = 0) then 
		call raise_error;
	end if;
    
    start transaction;
    
		select `ContentStream` from `FileSystem` 
			where (`Id` = p_SourceId) 
				into v_ContentStream;
    
		update `FileSystem` 
			set 
				`ContentStream` = v_ContentStream,
                `Modified` = p_Modified,
                `ModifiedBy` = p_ModifiedBy, 
                `LastAccessed` = p_Modified,
                `LastAccessedBy` = p_ModifiedBy
		where (`Id` = p_DestinationId);
        
        update `FileSystem` 
			set 
                `LastAccessed` = p_Modified,
                `LastAccessedBy` = p_ModifiedBy
		where (`Id` = p_SourceId);
        
	commit;
    
END ;;
DELIMITER ;