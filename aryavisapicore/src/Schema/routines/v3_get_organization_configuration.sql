DROP procedure IF EXISTS `v3_get_organization_configuration`;

DELIMITER $$
CREATE PROCEDURE `v3_get_organization_configuration`(
 IN vOrgGuid VARCHAR(36)
)
BEGIN
   SELECT 30StackRankType FROM orgconfigsettings WHERE OrgGuid=UUID_TO_BIN(vOrgGuid);
END$$

DELIMITER ;
