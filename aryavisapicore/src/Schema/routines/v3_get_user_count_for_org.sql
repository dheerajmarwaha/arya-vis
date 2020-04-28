DROP procedure IF EXISTS `v3_get_user_count_for_org`;

DELIMITER $$
CREATE PROCEDURE `v3_get_user_count_for_org`(
  IN vOrgGuid VARCHAR(36))
BEGIN

  SELECT COUNT(DISTINCT UserId)
    FROM user
    WHERE OrgGuId = UUID_TO_BIN(vOrgGuid);

END$$

DELIMITER ;
