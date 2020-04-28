DROP function IF EXISTS `BIN_TO_UUID`;
DELIMITER $$

CREATE FUNCTION `BIN_TO_UUID`
(
	vBinaryId BINARY(16)
) 
RETURNS char(36) CHARSET utf8mb4
DETERMINISTIC
BEGIN
  RETURN (
    INSERT(
      INSERT(
        INSERT(
          INSERT(
            HEX(
              CONCAT(
                SUBSTR(vBinaryId, 5, 4), SUBSTR(vBinaryId, 3, 2),
                SUBSTR(vBinaryId, 1, 2), SUBSTR(vBinaryId, 9, 8))
			), 9, 0, '-'),
		  14, 0, '-'),
		19, 0, '-'),
	  24, 0, '-')
	);
END$$

DELIMITER ;
