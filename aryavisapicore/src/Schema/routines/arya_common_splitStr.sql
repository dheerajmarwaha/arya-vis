
DROP procedure IF EXISTS `arya_common_splitStr`;

DELIMITER $$
CREATE PROCEDURE `arya_common_splitStr`(
vStr TEXT,
vSplitBy CHAR
)
BEGIN
  -- @author Mouli Kalakota
  -- converting job ids to a list
  DECLARE delim INT DEFAULT 0;
  DECLARE mylist TEXT DEFAULT vStr;
  DECLARE TEMP TEXT DEFAULT '';
  DECLARE strlen INT DEFAULT LENGTH(vStr);

  DROP TEMPORARY TABLE IF EXISTS temp_strs;

  CREATE TEMPORARY TABLE temp_strs (id INTEGER NOT NULL AUTO_INCREMENT PRIMARY KEY, split_str TEXT);

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

  -- read from temporary table temp_strs
END$$

DELIMITER ;

