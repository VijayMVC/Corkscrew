DELIMITER ;;
CREATE PROCEDURE `GetSignaturePanelById`(
	IN	p_Id		varchar(64)
)
BEGIN

	select 
		`Id`, `PanelType`, `IsTimeLimited`, `Deadline`, `CurrentState`, `ResponsesInProgress`, 
			`Created`, `CreatedBy`, `Modified`, `ModifiedBy`
	from `SignaturePanel`
	where (
		`Id` = p_Id
	);
        
END ;;
DELIMITER ;