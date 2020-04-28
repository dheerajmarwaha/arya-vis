DROP procedure IF EXISTS `v3_get_user_guids_by_org_guid`;

DELIMITER $$
CREATE PROCEDURE `v3_get_user_guids_by_org_guid`(
	IN vOrgGuid varchar(36)  
)
BEGIN
 SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	SELECT
		UserGuId
	FROM  user
	WHERE OrgGuid = UUID_TO_BIN(vOrgGuid);
END$$

DELIMITER ;

