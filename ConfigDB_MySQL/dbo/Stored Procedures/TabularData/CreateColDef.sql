DELIMITER ;;
CREATE PROCEDURE `CreateColDef` 
(
	IN p_Id				varchar(64),
	IN p_Name 			varchar(255),
    IN p_Type			varchar(255),
    IN p_MaxLength		bigint,
    IN p_MinValue		blob,
    IN p_MaxValue		blob,
    IN p_AllowNull		bit,
    IN p_Created		datetime,
    IN p_CreatedBy		varchar(64)
)
BEGIN
	
    if ((select count(*) from `TabularDataColDefs` where ((`Id` = p_Id) or (UPPER(`Name`) = UPPER(p_Name)))) > 0) then 
		call raise_error;
    end if;
    
    insert into `TabularDataColDefs` 
		( `Id`, `Name`, `Type`, `MaxLength`, `Nullable`, `MinValue`, `MaxValue`, `Created`, `CreatedBy` ) 
        values 
        (
			p_Id, p_Name, p_Type, p_MaxLength, p_AllowNull, p_MinValue, p_MaxValue, p_Created, p_CreatedBy
        );
    
END ;;
DELIMITER ;