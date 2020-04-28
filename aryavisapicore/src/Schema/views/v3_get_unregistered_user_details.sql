
DROP procedure IF EXISTS `v3_get_unregistered_user_details`;

DELIMITER $$
CREATE PROCEDURE `v3_get_unregistered_user_details` 
(
	IN vUserGuid VARCHAR(36)
)
BEGIN

 SET @user_guid_bin = UUID_TO_BIN(vUserGuid);

  SELECT
      BIN_TO_UUID(uu.UserGuId) AS UserGuId,
      uu.FullName,
      uu.Email,
      uu.Phone,
      uu.City,
      uu.StateCode,
      uu.PostalCode,
      uu.CountryCode,
      uu.Company,
      uu.ProfileType,
      GROUP_CONCAT(uim.industry) as Industries
    FROM unregistered_user uu
    LEFT JOIN user_industry_mapping uim
      ON uu.UserGuId = uim.UserGuId
	WHERE uu.UserGuId = @user_guid_bin;

END
$$

DELIMITER ;

