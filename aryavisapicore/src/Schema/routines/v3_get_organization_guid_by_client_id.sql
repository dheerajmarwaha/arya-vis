DROP PROCEDURE IF EXISTS `v3_get_organization_guid_by_client_id`;

DELIMITER $$
CREATE PROCEDURE `v3_get_organization_guid_by_client_id` (
  vClientId CHAR(26) CHARSET utf8mb4 COLLATE utf8mb4_unicode_520_ci
) BEGIN
  SELECT OrgGuid
    FROM organization_api_client
    WHERE client_id = vClientId;
END$$

DELIMITER ;