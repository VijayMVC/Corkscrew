DELIMITER ;;
CREATE PROCEDURE `DeleteSignaturePanelItemById`(
	IN	p_Id		varchar(64)
)
BEGIN

	delete from `SignaturePanelItem` 
		where (
			`Id` = p_Id
        );
        
END ;;
DELIMITER ;