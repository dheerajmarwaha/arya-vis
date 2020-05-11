DROP PROCEDURE IF EXISTS `v3_insert_user`;

DELIMITER $$
CREATE  PROCEDURE `v3_insert_user`(
  IN vUserGuid VARCHAR(36),
  IN vFirstName VARCHAR(1000),
  IN vLastName VARCHAR(1000),
  IN vEmail VARCHAR(1000),
  IN vHomePhone VARCHAR(500),
  IN vWorkPhone VARCHAR(500),
  IN vRoleGroupID INT,
  IN vIsActive BOOL,
  IN vOrgGuid VARCHAR(36),
  IN vVendorID VARCHAR(100),
  IN vCity VARCHAR(25),
  IN vState VARCHAR(25),
  IN vCountry VARCHAR(25),
  IN vZipCodeId VARCHAR(25),
  IN vCreatedByGuId		VARCHAR(36)
)
BEGIN

  SET @Cnt = 0;
  SET @LogInName = vEmail;
  SET @Username = CONCAT(IFNULL(vFirstName,'')," ",IFNULL(vLastName,''));
  SET @Passwords = 'N/A';
  SET @CreatedDate = UTC_TIMESTAMP();
  SET @ModifiedDate = UTC_TIMESTAMP();
  SET @RoleName=NULL;
  SET @orgCode = NULL;
  SET @BINUserGuid = UUID_TO_BIN(vUserGuid);  
  SET @BINCreatedByGuid = UUID_TO_BIN(vCreatedByGuId);
  SET @BinOrgGuid = UUID_TO_BIN(vOrgGuid);


  SELECT Rolename into @RoleName from rolegroup WHERE RoleGroupId=vRoleGroupID;
  SELECT OrgCode  INTO @orgCode  FROM mstorganization WHERE OrgGuid = @BinOrgGuid;

  IF @RoleName='Super Admin' THEN

     INSERT INTO management_user_role(UserGuId, RoleGroupID )
     VALUES(@BINUserGuId,  vRoleGroupID);
     -- ON duplicate KEY UPDATE role_id = 1;

   ELSEIF EXISTS(SELECT 1 FROM management_user_role where UserGuId=@BINUserGuId) THEN

      DELETE FROM management_user_role WHERE UserGuId=@BINUserGuId;
   
   END IF;

  SET @Cnt = (SELECT MAX(cast(VendorId AS UNSIGNED)) FROM user WHERE OrgGuid = @BinOrgGuid);
  SET @Cnt = IF (@Cnt IS NULL, 1, @Cnt+1);
  SET @VendorID = IF (vVendorID IS NULL,@Cnt,vVendorID);

  START TRANSACTION;   
      INSERT INTO user
      (
        `UserGuid`,
        `UserName`,
        `FirstName`,
        `LastName`,
        `VendorID`,
        `Email`,
        `LogInName`,
        `HomePhone`,
        `WorkPhone`,
        `Password`,
        `RoleGroupID`,
        `IsActive`,
        `OrgGuid`,        
        `CreatedByGuid`,
        `CreatedDate`,        
        `City`,
        `State`,
        `Country`,
        `ZipCode`
      )
      VALUES
      (
        @BINUserGuid ,
        @Username,
        vFirstName,
        vLastName,
        vVendorID,
        vEmail,
        @LogInName,
        vHomePhone,
        vWorkPhone,
        @Passwords,
        vRoleGroupID,
        vIsActive,
        @BinOrgGuid,
        @BINCreatedByGuid,
        @CreatedDate,        
        vCity,
        vState,
        vCountry,
        vZipCodeId
      );      

		-- IF(@orgCode = 'Pulse') THEN
		-- --  Career Site urls
		-- SET @joblistBaseUrl = (SELECT CONCAT(ConfigValue, '?OrgId=', @vOrgGuId, '&UserId=', vUserId)
		-- 						FROM mstaryaconfigparameter
		-- 						WHERE ConfigKey = 'leoforce.careersite.joblist.base.url');

		-- SET @jobDetailsUrl = (SELECT ConfigValue
		-- 					   FROM mstaryaconfigparameter
		-- 					   WHERE ConfigKey = 'leoforce.careersite.jobdetails.base.url');

		-- UPDATE mstorganization
		-- SET OrgJobsPageLink = @joblistBaseUrl, OrgJobViewURL = @jobDetailsUrl
		-- WHERE orgId = @vOrgGuId AND OrgJobsPageLink IS NULL AND OrgJobViewURL IS NULL;

		-- END IF;

      SELECT @BINUserGuid	AS UserGuid, 'User Successfully Created!!!'	AS ErrMsg;

  COMMIT;
END$$

DELIMITER ;

