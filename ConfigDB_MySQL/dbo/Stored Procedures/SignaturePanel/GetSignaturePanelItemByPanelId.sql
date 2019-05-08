DELIMITER ;;
CREATE PROCEDURE `GetSignaturePanelItemByPanelId`(
	IN	p_PanelId		varchar(64)
)
BEGIN

	select 
		`Id`, `PanelId`, `RespondentId`, `IsFinalDecider`, `IsTieBreaker`, `IsMandatory`, 
			`Response`, `Comment`, `RespondedOn`, `SentToResponder`,
				`Created`, `CreatedBy`, `Modified`, `ModifiedBy`
	from `SignaturePanelItem` 
	where (
		`PanelId` = p_PanelId
	);
        
END ;;
DELIMITER ;