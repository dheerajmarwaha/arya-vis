DROP procedure IF EXISTS `v3_delete_organization`;

DELIMITER $$
CREATE PROCEDURE `v3_delete_organization` 
(
	IN vOrgGuid VARCHAR(36)
)
BEGIN
  UPDATE mstorganization SET IsActive = 0 WHERE OrgGuid = UUID_TO_BIN(vOrgGuid);
END
$$

DELIMITER ;

