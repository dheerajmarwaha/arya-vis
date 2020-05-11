
DROP procedure IF EXISTS `v3_create_unregistered_user`;

DELIMITER $$
CREATE PROCEDURE `v3_create_unregistered_user`(
	IN vUserGuid 		VARCHAR(36),
	IN vName 			VARCHAR(255) charset utf8mb4,
	IN vEmail 			VARCHAR(255) charset utf8mb4 COLLATE utf8mb4_unicode_520_ci,
	IN vPhone 			VARCHAR(255) charset utf8mb4,
	IN vCity 			VARCHAR(255) charset utf8mb4,
	IN vStateCode 		VARCHAR(20) charset utf8mb4,
	IN vPostalCode 		VARCHAR(20) charset utf8mb4,
	IN vCountryCode		CHAR(2) charset utf8mb4,
	IN vCompany 		VARCHAR(500) charset utf8mb4,
	IN vProfileType 	VARCHAR(50) charset utf8mb4,
	IN vIndustries 		TEXT charset utf8mb4
)
BEGIN
  IF vUserGuid IS NULL THEN
	  SET vUserGuid = uuid();
  END IF;

  SET @user_guid_bin = UUID_TO_BIN(vUserGuid);

  CALL arya_common_splitStr_collated_utf8mb4(vIndustries, ',');

  IF NOT EXISTS ( SELECT 1  FROM unregistered_user   WHERE Email = vEmail )  THEN
    INSERT INTO unregistered_user
    (
		UserGuid,
		FullName,
		Email,
		Phone,
		City,
		StateCode,
		PostalCode,
		CountryCode,
		Company,
		ProfileType		
    )
    VALUES (
		@user_guid_bin,
		vName,
		vEmail,
		vPhone,
		vCity,
		vStateCode,
		vPostalCode,
		vCountryCode,
		vCompany,
		vProfileType
    );


    INSERT INTO user_industry_mapping (
    UserGuid,
    Industry
    )
    SELECT
        @user_guid_bin as UserGuId,
        split_str AS industry
      FROM temp_strs;
  END IF;
END
$$
DELIMITER ;

