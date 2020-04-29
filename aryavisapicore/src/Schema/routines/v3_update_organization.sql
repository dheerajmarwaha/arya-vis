DROP procedure IF EXISTS `v3_update_organization`;

DELIMITER $$
CREATE PROCEDURE `v3_update_organization`(
  IN vOrgGuid VARCHAR(36),
  IN vContactPerson VARCHAR(2000),
  IN vCPEmail VARCHAR(2000),
  IN vOrgHomePageLink VARCHAR(1000),  
  IN vIsActive BOOL,
  IN vAddress TEXT,
  IN vOrgType VARCHAR(2000),
  IN vSubscriptionEndDate DATETIME,
  IN vOrgSourceCount INT,
  IN vJobCountLimit INT,
  IN vOrgLevelMiles VARCHAR(45),  
  IN vModifiedByGuId	VARCHAR(36) 
)
BEGIN

	SET @OrgGuid = UUID_TO_BIN(vOrgGuid);
	SET @ModifiedByGuId = UUID_TO_BIN(vModifiedByGuId);
	SET @ModifiedDate = UTC_TIMESTAMP();

	START TRANSACTION;    
		  UPDATE mstorganization
		  SET
			ContactPerson		= vContactPerson,
			CPEmail				= vCPEmail,
			OrgHomePageLink		= vOrgHomePageLink,
			IsActive			= vIsActive,
			OrgCode				=vOrgType,
			Address				= vAddress,
			SubscriptionEndDate = vSubscriptionEndDate,
			JobCountLimit		= vJobCountLimit,
			ModifiedById		= @ModifiedByGuId,
			ModifiedDate		= @ModifiedDate
		  WHERE OrgGuid			= @OrgGuid;

		  UPDATE 	orgconfigsettings
		  SET
			OrgLevelMiles = vOrgLevelMiles,
			30SourceLimit = vOrgSourceCount
		  WHERE OrgGuid			= @OrgGuid;

		  SELECT @OrgGuid	AS OrgGuid, 'Organization Successfully Updated!!!' AS ErrMsg;
  COMMIT;
END$$

DELIMITER ;

