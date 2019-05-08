DELIMITER ;;
CREATE PROCEDURE `WrkflwInstGetAllRunnable`(
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    SELECT 
		`Id`, `WorkflowAssociationId`, `WorkflowInstanceId`, 
			`AssociationData`, 
					`State`, `CompletedReason`, `ErrorMessage`, 
						`Created`, `CreatedBy` 
	FROM `WorkflowHistory` 
	WHERE (
		`WorkflowInstanceId` = p_WorkflowInstanceId
	) 
	ORDER BY `Created` DESC;
        
END ;;
DELIMITER ;