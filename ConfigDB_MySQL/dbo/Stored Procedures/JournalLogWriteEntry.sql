DELIMITER ;;
CREATE PROCEDURE `JournalLogWriteEntry`(
    IN	p_MachineName		varchar(255),
    in	p_LogType			tinyint,
    in	p_Timestamp			datetime,
    in	p_Message			varchar(1024),
    in	p_CorrelationId		varchar(64),
    in	p_SiteId			varchar(64),
    in	p_FileSystemEntryId	varchar(64),
    in 	p_UserId			varchar(64),
    in	p_ModuleName		varchar(255),
    in	p_StateInfo			varchar(4000),
    in	p_ExceptionStack	varchar(4000),
    in	p_EventClass		varchar(255),
    in	p_EventType			varchar(255),
    in	p_EventId			int
)
BEGIN

	declare	v_EmptyGuid		varchar(64);
    
    set v_EmptyGuid = `GuidDefault`();

	set p_Timestamp = ifnull(p_Timestamp, now());
	if (p_LogType not in (1, 2, 4, 5, 6)) then set p_LogType = 1; end if;
    if (p_EventId < 0) then set p_EventId = 0; end if;
    if (p_CorrelationId = v_EmptyGuid) then set p_CorrelationId = null; end if;
    if (p_SiteId = v_EmptyGuid) then set p_SiteId = null; end if;
    if (p_FileSystemEntryId = v_EmptyGuid) then set p_FileSystemEntryId = null; end if;
    if (p_UserId = v_EmptyGuid) then set p_UserId = null; end if;
    
    insert into `JournalLog` 
		( `MachineName`, `LogType`, `Timestamp`, `Message`, `CorrelationId`, 
			`SiteId`, `FilesystemEntryId`, `UserId`, 
				`ModuleName`, `StateInfo`, `ExceptionStack`, 
					`EventClass`, `EventType`, `EventId` ) 
		values 
        (
			p_MachineName, p_LogType, p_Timestamp, 
				p_Message,
					p_CorrelationId,
						p_SiteId, p_FileSystemEntryId, p_UserId,
							p_ModuleName, p_StateInfo, p_ExceptionStack,
								p_EventClass, p_EventType, p_EventId
        );
        
END ;;
DELIMITER ;