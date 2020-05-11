DROP PROCEDURE IF EXISTS `arya_common_splitStr_collated_utf8mb4`;

DELIMITER $$
CREATE PROCEDURE `arya_common_splitStr_collated_utf8mb4`(
vStr TEXT charset utf8mb4 collate utf8mb4_unicode_520_ci,
vSplitBy CHAR
)
BEGIN
  DECLARE delim INT DEFAULT 0;
  DECLARE mylist TEXT charset utf8mb4 collate utf8mb4_unicode_520_ci DEFAULT vStr;
  DECLARE TEMP TEXT charset utf8mb4 collate utf8mb4_unicode_520_ci DEFAULT '';
  DECLARE strlen INT DEFAULT LENGTH(vStr);

  DROP TEMPORARY TABLE IF EXISTS temp_strs;

  CREATE TEMPORARY TABLE temp_strs (id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, split_str TEXT charset utf8mb4 collate utf8mb4_unicode_520_ci) COLLATE utf8mb4_unicode_520_ci;

  SET delim = LOCATE(vSplitBy, mylist);

  WHILE strlen > 0 DO
    IF delim = 0 THEN
      SET TEMP = TRIM(mylist);
      SET mylist = '';
      SET strlen = 0;
    ELSE
      SET TEMP = TRIM(SUBSTRING(mylist, 1, delim - 1));
      SET mylist = TRIM(SUBSTRING(mylist FROM delim + 1));
      SET strlen = LENGTH(mylist);
    END IF;

    IF LENGTH(TEMP) > 0 THEN
      INSERT INTO temp_strs(split_str) VALUES(TEMP);
    END IF;

    SET delim = LOCATE(vSplitBy, mylist);
  END WHILE;


END$$

DELIMITER ;

