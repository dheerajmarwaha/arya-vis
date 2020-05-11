DROP procedure IF EXISTS `v3_get_organization`;

DELIMITER $$
CREATE  PROCEDURE `v3_get_organization`(
  IN vOrgGuid VARCHAR(36)
)
BEGIN
	-- =================================================================================================
	-- Author				    :	
	-- Create date			:	
	-- Modified date		:
	-- Description			:	
	-- Tables Used			: 
	-- Parameters			  :	
	-- Changes History	:
	-- Exececution Step	:	
	-- =================================================================================================
	SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	SET @OrgGuid = UUID_TO_BIN(vOrgGuid);
    
	SELECT AuthenticatedCountries  INTO @AuthenticatedCountryIDs
	FROM mstorganization  WHERE OrgGuid = @OrgGuid;

	CALL arya_common_splitStr(@AuthenticatedCountryIDs, ',');

	SELECT GROUP_CONCAT(CountryName)
	INTO @AuthenticatedCountries
	FROM mstcountry A
		JOIN temp_strs B ON A.CountryId = B.split_str;

  SELECT    
		vOrgGuid,
		OrganizationName,
		ContactPerson,
		CPEmail AS ContactEmail,
		OrgHomePageLink,
		IdentityProviderIdentifier,
		Org.IsActive AND Org.SubscriptionEndDate >= NOW() AS IsActive,
		Address,
		OrgType,
		OrgCode,
		SubscriptionEndDate,
		OrgConfig.`30SourceLimit` AS SourceLimit,
		JobCountLimit,
		OrgLevelMiles,
		Org.AuthenticatedCountries AS AuthenticatedCountriesID,
		@AuthenticatedCountries AS AuthenticatedCountries
  FROM mstorganization Org
    JOIN orgconfigsettings OrgConfig ON Org.OrgGuid = OrgConfig.OrgGuid
  WHERE Org.OrgGuid = @OrgGuid;

  SELECT
    RoleGroupID,
    RoleName
  FROM rolegroup
  WHERE OrgGuid = @OrgGuid;
END$$

DELIMITER ;

