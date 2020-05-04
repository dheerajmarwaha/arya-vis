DROP procedure IF EXISTS `v3_patch_user_configuration`;

DELIMITER $$
CREATE PROCEDURE `v3_patch_user_configuration`(
  IN vUserGuid VARCHAR(36),
  IN vDistance INT,
  IN vUnit VARCHAR(10),
  IN vLanguage VARCHAR(35),
  IN vIsAutoLogoutEnabled BOOL,
  IN vInactiveMinutes INT)
BEGIN
  -- Swarali Joshi

  UPDATE orgconfigsettings o
	JOIN user u ON u.Organization = o.OrgId AND u.UserId = UUID_TO_BIN(vUserGuid)
	SET OrgLevelMiles =    CASE
							  WHEN vDistance IS NOT NULL THEN vDistance 
							  ELSE OrgLevelMiles 
							END,
	Preferedlanguage =    CASE
							  WHEN vLanguage IS NOT NULL THEN vLanguage 
							  ELSE Preferedlanguage 
						  END;

END$$

DELIMITER ;
