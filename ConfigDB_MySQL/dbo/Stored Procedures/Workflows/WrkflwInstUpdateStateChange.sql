DELIMITER ;;
CREATE PROCEDURE `WrkflwInstUpdateStateChange`(
	IN p_Id					varchar(64),
    IN p_State				int,
    IN p_StateInfo			longtext,
    IN p_Reason				int,
    IN p_ErrorMessage		longtext,
    IN p_SqlPersistenceId	varchar(64),
    IN p_IsLoadedInRuntime	bit
)
BEGIN

	declare	v_RightNow	datetime;

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    set v_RightNow = now();
    
    set p_Reason = if((p_State in (4, 5)), p_Reason, 0);
    
    start transaction;
    
		UPDATE `WorkflowInstances` 
			SET 
				`CurrentState` = p_State, 
				`AssociationData` = p_StateInfo,
				`CompletedReason` = p_Reason, 
				`ErrorMessage` = p_ErrorMessage, 
				`SqlPersistenceId` = p_SqlPersistenceId,
				`IsLoadedInRuntime` = p_IsLoadedInRuntime,
				`Modified` = v_RightNow, 
				`ModifiedBy` = `SystemUserGuid`()
		WHERE (`Id` = p_Id);
		
		INSERT INTO `WorkflowHistory` 
			(
				`Id`, `WorkflowAssociationId`, `WorkflowInstanceId`, 
					`AssociationData`, 
							`State`, `CompletedReason`, `ErrorMessage`, 
								`Created`, `CreatedBy`
			)
			SELECT 
				NEWID(), `WorkflowAssociationId`, p_Id, 
				  `AssociationData`, 
						p_State, p_Reason, p_ErrorMessage, 
							v_RightNow, `SystemUserGuid`()
			FROM `WorkflowInstances` 
			WHERE (`Id` = p_Id);
    
    commit;
        
END ;;
DELIMITER ;