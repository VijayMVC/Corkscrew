DELIMITER ;;
CREATE PROCEDURE `GetUsersNotInGroupByGroupId`(
	IN p_GroupId		varchar(64)
)
BEGIN
	
	select usr.`Id`, usr.`Username`, usr.`SecretHash`, usr.`DisplayName`, usr.`EmailAddress`, usr.`IsWinADUser`, ugm.`IsWinADMembership`
        from `Users` usr 
			left join `UserGroupMembers` ugm on (ugm.`UserId` = usr.`Id`)
	where (
		(ugm.`GroupId` = p_GroupId) 
        and (ugm.`UserId` = null)
	);
    
END ;;
DELIMITER ;