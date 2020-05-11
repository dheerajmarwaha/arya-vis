DROP procedure IF EXISTS `v3_insert_organization`;

DELIMITER $$
CREATE PROCEDURE `v3_insert_organization`
(
	  IN vOrgGuid VARCHAR(36),
	  IN vOrganizationName VARCHAR(2000),
	  IN vContactPerson VARCHAR(2000),
	  IN vCPEmail VARCHAR(2000),
	  IN vOrgHomePageLink VARCHAR(1000),  
	  IN vIsActive BOOL,
	  IN vAddress TEXT,
	  IN vOrgType VARCHAR(2000),
	  IN vSubscriptionEndDate DATETIME,
	  IN vIdentityProviderIdentifier VARCHAR(255),
	  IN vOrgSourceCount INT,
	  IN vJobCountLimit INT,
	  IN vOrgLevelMiles VARCHAR(45),
	  IN v30StackRankType VARCHAR(50),
	  IN vCreatedByGuid		VARCHAR(36)
)
BEGIN

		SET @MoversEnabled = 0;
		SET @CandidateScoreEnabled = 0;		
		SET @Is30Enabled = 1;
		SET @JobBoardAllow = 0;
        SET @CreatedDate = UTC_TIMESTAMP();
        SET @OrgGuid = UUID_TO_BIN(vOrgGuid);
        SET @CreatedByGuid = UUID_TO_BIN(vCreatedByGuid);

  START TRANSACTION;
		INSERT INTO mstorganization
		(
			OrgGuid,
			OrganizationName,
			ContactPerson,
			CPEmail,
			OrgHomePageLink,
			IdentityProviderIdentifier,
			IsActive,
			Address,
			OrgType,
			OrgCode,
			SubscriptionEndDate,
			MoversEnabled,
			CandidateScoreEnabled,
			OrgSourceCount,
			JobCountLimit,			
			CreatedByGuId,
			CreatedDate						
		)
		VALUES
		(
			@OrgGuid,
			vOrganizationName,
			vContactPerson,
			vCPEmail,
			vOrgHomePageLink,
			vIdentityProviderIdentifier,
			vIsActive,
			vAddress,			
			vOrgType,
            "WOATS",
			vSubscriptionEndDate,
			@MoversEnabled,
			@CandidateScoreEnabled,
			null,
			vJobCountLimit,	
            @CreatedByGuid,
			@CreatedDate							
		);      

		INSERT INTO orgconfigsettings
		(
			OrgGuid,
			JobBoardAllow,
			OrgLevelMiles,
			Is30Enabled,
			30StackRankType,
			CreatedbyGuid,
			CreatedDate,			
			30SourceLimit
		)
		VALUES
		(
			@OrgGuid,
			@JobBoardAllow,
			vOrgLevelMiles,
			@Is30Enabled,
			v30StackRankType,
			@CreatedByGuid,
			@CreatedDate,			
			vOrgSourceCount
		);

		INSERT INTO access_restrictions
		(
			feature,
			entity_type,
			entity_id,
			allowed
		)
		VALUES
		(
			'MASTERVIEW_SOCIAL',
			'ORG',
			@OrgGuid,
			1
		);

		INSERT INTO rolegroup
		(
			RoleName,
			OrgGuId,
			IsActive,
			IsSuperAdmin,
			IsOrgAdmin,
			IsOrgTeamLeader,
			IsCandidate,
			IsVendor,
			CreatedByGuId,
			CreatedDate
		)
		SELECT
			RoleName,
			OrgGuid,
			IsActive,
			IsSuperAdmin,
			IsOrgAdmin,
			IsOrgTeamLeader,
			IsCandidate,
			IsVendor,
			CreatedByGuid,
			CreatedDate
		FROM
		(
		SELECT
			'Admin' RoleName,
			@OrgGuid,
			1 IsActive,
			0 IsSuperAdmin,
			1 IsOrgAdmin,
			0 IsOrgTeamLeader,
			0 IsCandidate,
			0 IsVendor,
			@CreatedByGuid CreatedByGuid,
			@CreatedDate CreatedDate
		UNION
		SELECT
			'Recruiter' RoleName,
			@OrgGuid,
			1 IsActive,
			0 IsSuperAdmin,
			0 IsOrgAdmin,
			0 IsOrgTeamLeader,
			0 IsCandidate,
			0 IsVendor,
			@CreatedByGuid CreatedByGuid,
			@CreatedDate CreatedDate
		)X;

  SELECT vOrgGuid	AS OrgGuiD, 'Organization Successfully Created!!!'	AS ErrMsg;

  COMMIT;
END$$

DELIMITER ;

