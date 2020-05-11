DROP procedure IF EXISTS `v3_get_all_users`;

DELIMITER $$
CREATE PROCEDURE `v3_get_all_users`(
  IN vOrgGuid VARCHAR(36),
  IN vFrom INT,
  IN vSize INT,
  IN vSearchTerm VARCHAR(200) CHARSET utf8mb4,
  IN vVendorIds TEXT,
  IN vNames TEXT  CHARSET utf8mb4,
  IN vEmails TEXT CHARSET utf8mb4,
  IN vUserGuids TEXT)
BEGIN
  SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

  SET @IsServiceRep = FALSE;
  SELECT 
	  CASE WHEN IsEnabled IS TRUE AND IsAllowed IS TRUE THEN TRUE ELSE FALSE END 
  INTO @IsServiceRep
  FROM feature f
  JOIN org_feature_toggles oft ON oft.OrgGuId = vOrgGuid AND f.FeatureGuid = oft.FeatureGuid AND f.FeatureName = 'WhiteGloveServiceProgress';


  DROP TEMPORARY TABLE IF EXISTS vendorId_temp;
  CREATE TEMPORARY TABLE vendorId_temp (vendorId VARCHAR(150) UNIQUE);
  CALL arya_common_splitStr(vVendorIds, ',');
  INSERT IGNORE INTO vendorId_temp (vendorId) SELECT split_str from temp_strs;

  DROP TEMPORARY TABLE IF EXISTS names_temp;
  CREATE TEMPORARY TABLE names_temp (`name` VARCHAR(200) UNIQUE);
  CALL arya_common_splitStr(vNames, ',');
  INSERT IGNORE INTO names_temp (`name`) SELECT split_str from temp_strs;

  DROP TEMPORARY TABLE IF EXISTS emails_temp;
  CREATE TEMPORARY TABLE emails_temp (email VARCHAR(200) UNIQUE);
  CALL arya_common_splitStr(vEmails, ',');
  INSERT IGNORE INTO emails_temp (email) SELECT split_str from temp_strs;

  DROP TEMPORARY TABLE IF EXISTS guids_temp;
  CREATE TEMPORARY TABLE guids_temp (`guid` CHAR(36) UNIQUE);
  CALL arya_common_splitStr(vUserGuids, ',');
  INSERT IGNORE INTO guids_temp (`guid`) SELECT split_str from temp_strs;

  SELECT
        UserGuid,
        FullName,
        vu.VendorId,
        FirstName,
        LastName,
        HomePhone,
        WorkPhone,
        LoginName,
        vu.Email,
        RoleGroupID,        
        RoleName,
        OrgGuid,
        OrganizationName,       
        IsActive,
        CreatedByGuid,
        CreatedDate,
        ModifiedByGuid,
        ModifiedDate,
		City,
        State,
        ZipCode,
        Country
	FROM v3_view_user_details vu
    LEFT JOIN vendorId_temp vt
      ON vu.VendorId = vt.vendorId
    LEFT JOIN names_temp nt
      ON vu.FullName = nt.`name`
    LEFT JOIN emails_temp et
      ON (vu.Email = et.email OR vu.LoginName = et.email)
    LEFT JOIN guids_temp gt
      ON vu.UserGuid = gt.`guid`
    WHERE (vu.OrgGuid = vOrgGuid OR (@IsServiceRep AND (vu.OrgCode = 'WhiteGloveService')))
      AND (vSearchTerm = '' OR vSearchTerm IS NULL    OR vu.FullName LIKE CONCAT('%', vSearchTerm, '%')
			OR vu.LoginName LIKE CONCAT('%', vSearchTerm, '%') OR vu.Email LIKE CONCAT('%', vSearchTerm, '%'))
      AND (vVendorIds IS NULL  OR vt.vendorId IS NOT NULL)
      AND (vNames IS NULL      OR nt.`name` IS NOT NULL)
      AND (vEmails IS NULL     OR et.email IS NOT NULL)
      AND (vUserGuids IS NULL  OR gt.`guid` IS NOT NULL)
    ORDER BY vu.IsActive DESC, FullName
    LIMIT VFROM , VSIZE;

  SELECT COUNT(DISTINCT vu.UserGuid) AS count
  FROM v3_view_user_details vu
    LEFT JOIN vendorId_temp vt
      ON vu.VendorId = vt.vendorId
    LEFT JOIN names_temp nt
      ON vu.FullName = nt.`name`
    LEFT JOIN emails_temp et
      ON (vu.Email = et.email OR vu.LoginName = et.email)
    LEFT JOIN guids_temp gt
      ON vu.UserGuid = gt.`guid`
    WHERE (vu.OrgGuid = vOrgGuid OR (@IsServiceRep AND (vu.OrgCode = 'WhiteGloveService')))
      AND (vSearchTerm = '' OR vSearchTerm IS NULL    OR vu.FullName LIKE CONCAT('%', vSearchTerm, '%')
			OR vu.LoginName LIKE CONCAT('%', vSearchTerm, '%') OR vu.Email LIKE CONCAT('%', vSearchTerm, '%'))
      AND (vVendorIds IS NULL  OR vt.vendorId IS NOT NULL)
      AND (vNames IS NULL      OR nt.`name` IS NOT NULL)
      AND (vEmails IS NULL     OR et.email IS NOT NULL)
      AND (vUserGuids IS NULL  OR gt.`guid` IS NOT NULL);
END$$

DELIMITER ;
