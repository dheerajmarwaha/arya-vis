DROP procedure IF EXISTS `v3_get_all_users`;

DELIMITER $$
CREATE PROCEDURE `v3_get_all_users`(
  IN vOrgGuId VARCHAR(36),
  IN vFrom INT,
  IN vSize INT,
  IN vSearchTerm VARCHAR(200) CHARSET utf8mb4,
  IN vVendorIds TEXT,
  IN vNames TEXT  CHARSET utf8mb4,
  IN vEmails TEXT CHARSET utf8mb4,
  IN vUserGuids TEXT)
BEGIN
-- @Author Prashanth, Swarali
  -- 04/05/2018
  -- @Modified by Naveen Chandra (added IsActive = true condition)
  -- @Modified Date: 19th Mar 2019
  -- @Modified Date: 27th Mar 2019 Rishi

  SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

  SET @IsServiceRep = FALSE;
  SELECT 
	  CASE WHEN is_enabled IS TRUE AND is_allowed IS TRUE THEN TRUE ELSE FALSE END 
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
		u.UserGuId,
        u.UserName as FullName,
        u.VendorId,
        u.FirstName,
        u.LastName,
        u.HomePhone,
        u.WorkPhone,
        u.LoginName, 
        u.Email,
        u.RoleGroupID,        
        rg.RoleName,
        org.OrgGuid,
        org.OrganizationName,       
        u.IsActive AND org.IsActive AND org.SubscriptionEndDate >= NOW() AS IsActive,
        u.CreatedByGuId,
        u.CreatedDate,
        u.ModifiedByGuId,
        u.ModifiedDate,
		u.City,
        u.State,
        u.ZipCode,
        u.Country
    FROM user u
    JOIN rolegroup rg
      ON rg.RoleGroupId = u.Role
    JOIN mstorganization o
      ON o.OrgGuId = u.OrgGuId
    LEFT JOIN vendorId_temp vt
      ON u.VendorId = vt.vendorId
    LEFT JOIN names_temp nt
      ON u.UserName = nt.`name`
    LEFT JOIN emails_temp et
      ON (u.Email = et.email OR u.LoginName = et.email)
    LEFT JOIN guids_temp gt
      ON u.UserGuid = gt.`guid`
    WHERE (OrgGuId = vOrgGuId OR (@IsServiceRep AND (o.OrgCode = 'WhiteGloveService')))
      AND (vSearchTerm = '' OR vSearchTerm IS NULL    OR u.UserName LIKE CONCAT('%', vSearchTerm, '%')
			OR u.LoginName LIKE CONCAT('%', vSearchTerm, '%') OR u.Email LIKE CONCAT('%', vSearchTerm, '%'))
      AND (vVendorIds IS NULL  OR vt.vendorId IS NOT NULL)
      AND (vNames IS NULL      OR nt.`name` IS NOT NULL)
      AND (vEmails IS NULL     OR et.email IS NOT NULL)
      AND (vUserGuids IS NULL  OR gt.`guid` IS NOT NULL)
    ORDER BY u.IsActive DESC, UserName
    LIMIT VFROM , VSIZE;

  SELECT COUNT(DISTINCT UserId) AS count
  FROM user u
    JOIN rolegroup rg
      ON rg.RoleGroupId = u.Role
    JOIN mstorganization o
      ON o.OrgGuId = u.OrgGuId
    LEFT JOIN vendorId_temp vt
      ON u.VendorId = vt.vendorId
    LEFT JOIN names_temp nt
      ON u.UserName = nt.`name`
    LEFT JOIN emails_temp et
      ON (u.Email = et.email OR u.LoginName = et.email)
    LEFT JOIN guids_temp gt
      ON u.UserGuid = gt.`guid`
    WHERE (OrgGuId = vOrgGuId OR (@IsServiceRep AND (o.OrgCode = 'WhiteGloveService')))
      AND (vSearchTerm = '' OR vSearchTerm IS NULL    OR u.UserName LIKE CONCAT('%', vSearchTerm, '%')
			OR u.LoginName LIKE CONCAT('%', vSearchTerm, '%') OR u.Email LIKE CONCAT('%', vSearchTerm, '%'))
      AND (vVendorIds IS NULL  OR vt.vendorId IS NOT NULL)
      AND (vNames IS NULL      OR nt.`name` IS NOT NULL)
      AND (vEmails IS NULL     OR et.email IS NOT NULL)
      AND (vUserGuids IS NULL  OR gt.`guid` IS NOT NULL);
END$$

DELIMITER ;
