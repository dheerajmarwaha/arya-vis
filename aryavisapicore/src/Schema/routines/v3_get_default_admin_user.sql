DROP procedure IF EXISTS `v3_get_default_admin_user`;

DELIMITER $$
CREATE PROCEDURE `v3_get_default_admin_user` 
(
	IN vOrgGuid varchar(36)
)
BEGIN

  SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;  
  SELECT
        UserGuId,
        FullName,
        VendorId,
        FirstName,
        LastName,
        HomePhone,
        WorkPhone,
        LoginName,
        Email,
        RoleGroupID,        
        RoleName,
        OrgGuid,
        OrgName,       
        IsActive,
        CreatedByGuId,
        CreatedDate,
        ModifiedByGuId,
        ModifiedDate,
		City,
        State,
        ZipCode,
        Country
      FROM v3_view_user_details
	  where u.OrgGuid = UUID_TO_BIN(vOrgGuid)
		and IsActive = 1		
		and (RoleName = 'God View' or rg.RoleName = 'Admin')
	  order by RoleName desc, u.UserGuId limit 1;

END$$

DELIMITER ;
