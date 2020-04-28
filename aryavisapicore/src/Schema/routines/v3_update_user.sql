DROP PROCEDURE IF EXISTS v3_update_user;

DELIMITER $$
CREATE  PROCEDURE v3_update_user(
  IN vUserGuid VARCHAR(36),
  IN vFirstName VARCHAR(1000),
  IN vLastName VARCHAR(1000),
  IN vEmail VARCHAR(1000),
  IN vHomePhone VARCHAR(500),
  IN vWorkPhone VARCHAR(500),
  IN vRoleGroupID INT,
  IN vIsActive BOOL,
  IN vOrgGuId VARCHAR(255),
  IN vModifiedByGuId	VARCHAR(36)   
)
BEGIN

  SET @LogInName = vEmail;
  SET @Username = CONCAT(IFNULL(vFirstName,'')," ",IFNULL(vLastName,''));
  SET @ModifiedDate = UTC_TIMESTAMP();
  SET @BINUserGuId = UUID_TO_BIN(vUserGuid);
  SET @BINRoleGroupID = UUID_TO_BIN(vRoleGroupID);
  SET @BINModifiedByGuId = UUID_TO_BIN(vModifiedByGuId);


  START TRANSACTION;   
      UPDATE user
      SET
        UserName		= @Username,
        FirstName		= vFirstName,
        LastName		= vLastName,
        Email			= vEmail,
        LogInName		= @LogInName,
        HomePhone		= vHomePhone,
        WorkPhone		= vWorkPhone,
        IsActive		= vIsActive,
        RoleGroupID		= @BINRoleGroupID,
        OrgGuId			= @vOrgGuId,
        ModifiedByGuId	= @BINModifiedByGuId,
        ModifiedDate	= @ModifiedDate
      WHERE UserGuId		= @BINUserGuId;

      SELECT vUserGuid	AS UserGuId, 'User Successfully Updated!!!'	AS ErrMsg;

  COMMIT;
END$$

DELIMITER ;

