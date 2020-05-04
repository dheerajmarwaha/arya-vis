DROP procedure IF EXISTS `v3_get_allowed_features`;

DELIMITER $$
CREATE PROCEDURE `v3_get_allowed_features`(
	IN vUserGuid VARCHAR(36)
)
BEGIN
  -- @author Mouli Kalakota
  -- @created date February 02, 2019
  SELECT
      OrgGuid INTO @orgGuid
    FROM user
    WHERE UserGuid = UUID_TO_BIN(vUserGuid);

  DROP TEMPORARY TABLE IF EXISTS temp_features;
  CREATE TEMPORARY TABLE temp_features (FeatureGuid binary(16) NOT NULL, IsAllowed BIT, IsEnabled BIT, PRIMARY KEY(FeatureGuid));

  INSERT INTO temp_features
  SELECT t.FeatureGuid, t.IsAllowed, t.IsEnabled
    FROM user_feature_toggles t
    WHERE t.UserGuid = UUID_TO_BIN(vUserGuid);

  INSERT IGNORE INTO temp_features
  SELECT t.FeatureGuid, t.IsAllowed, t.IsEnabled
    FROM org_feature_toggles t
    WHERE t.org_id = @orgGuid;

  INSERT IGNORE INTO temp_features
  SELECT f.FeatureGuid, CAST(IFNULL(t.IsEnabled, FALSE) as signed) AS IsEnabled, CAST(IFNULL(t.IsAllowed, FALSE) as signed) AS IsAllowed
    FROM feature f
    LEFT JOIN universal_feature_toggles t
		ON f.FeatureGuid = t.FeatureGuid;


  SELECT f.`FeatureName` AS name, BIN_TO_UUID(t.FeatureGuid) AS FeatureGuid,t.IsEnabled AS IsEnabled, t.IsAllowed AS IsAllowed
    FROM temp_features t
    JOIN feature f
      ON t.FeatureGuid = f.id;
END$$

DELIMITER ;
