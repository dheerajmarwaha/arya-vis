DROP procedure IF EXISTS `v3_get_user_configurations`;

DELIMITER $$
CREATE PROCEDURE `v3_get_user_configurations`
(
	IN vUserGuid VARCHAR(36)
)
BEGIN

  SET @UserGuid = UUID_TO_BIN(vUserGuid);
  SET @jobLimit = NULL;
  SET @managementUserRoleId = NULL;

  SET @jobLimit = (SELECT NULLIF(JobCountLimit, 0)
                  FROM mstorganization o
                  JOIN user u ON u.Organization = o.OrgGuid AND u.UserGuid = @UserGuid);

  SELECT
    muser.RoleGroupId INTO @managementUserRoleGroupId
  FROM user u
  JOIN management_user_role muser
    ON u.UserGuid = @UserGuid AND muser.UserGuid = u.UserGuid
  LIMIT 1;

  SELECT
    OrgLevelMiles AS distance,
    Preferedlanguage AS language,
    TRUE AS is_auto_logout_enabled,
    (@managementUserRoleGroupId IS NOT NULL) AS is_management_user,
    ShowCandidateScore AS is_candidate_score_visible,
    30 AS inactive_minutes,
    @jobLimit AS max_job_limit,
    IFNULL(
      CASE WHEN MAX(us.`30SourceLimit`) = 0 THEN NULL Else MAX(us.`30SourceLimit`) End,
      CASE WHEN MAX(o.`30SourceLimit`) = 0 THEN NULL Else MAX(o.`30SourceLimit`) End
    ) AS default_source_limit,
    IFNULL(o.MaxAllowed30SourceLimit, 3 * o.`30SourceLimit`) AS max_source_limit,
    o.IsAutoSourcingEnabled AS is_auto_sourcing_enabled,
    o.WhitelabelId AS white_label_id
  FROM orgconfigsettings o
  JOIN user u ON u.OrgGuid = o.OrgGuid
  LEFT JOIN usersettings us ON u.UserGuid = us.UserGuid
  WHERE u.UserGuid = @UserGuid;

END$$

DELIMITER ;
