DROP procedure IF EXISTS `v3_get_system_configuration`;

DELIMITER $$
CREATE PROCEDURE `v3_get_system_configuration`()
BEGIN
  -- @author Mouli Kalakota
  -- @created January 16, 2019
  SELECT
      ConfigKey AS `key`,
      ConfigValue AS `value`
	FROM mstaryaconfigparameter;
END$$

DELIMITER ;