DROP procedure IF EXISTS `v3_delete_user`;

DELIMITER $$
CREATE PROCEDURE `v3_delete_user` 
(		
        vUserGuid varchar(36)
)
BEGIN
  UPDATE `user` SET IsActive = 0 WHERE UserGuId = UUID_TO_BIN(vUserGuid);
END
$$

DELIMITER ;

