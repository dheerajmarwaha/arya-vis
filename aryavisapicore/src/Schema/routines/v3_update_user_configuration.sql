DROP procedure IF EXISTS `v3_update_user_configuration`;

DELIMITER $$
CREATE PROCEDURE `v3_update_user_configuration`(
  IN vUserGuid VARCHAR(36),
  IN vDistance INT,
  IN vUnit VARCHAR(10),
  IN vLanguage VARCHAR(35),
  IN vIsAutoLogoutEnabled BOOL,
  IN vInactiveMinutes INT)
BEGIN
  -- Swarali Joshi

  UPDATE orgconfigsettings o
	JOIN user u ON u.OrgGuid = o.OrgGuid AND u.UserId = UUID_TO_BIN(vUserGuid)
    SET OrgLevelMiles = vDistance,
      Preferedlanguage = vLanguage;

END$$

DELIMITER ;
