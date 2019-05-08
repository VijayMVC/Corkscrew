DELIMITER ;;
CREATE PROCEDURE `GetUserMemberships`(
	IN p_UserId		varchar(64)
)
BEGIN
	
	select grp.`Id`, grp.`Username`, grp.`DisplayName`, grp.`EmailAddress`, grp.`IsWinADGroup`, ugm.`IsWinADMembership`
        from `UserGroups` grp 
			inner join `UserGroupMembers` ugm on (ugm.`GroupId` = grp.`Id`)
	where (ugm.`UserId` = p_UserId);
    
END ;;
DELIMITER ;