DROP procedure IF EXISTS `v3_get_users_by_org_guid`;

DELIMITER $$
CREATE PROCEDURE `v3_get_users_by_org_guid`(
  IN vOrgGuid varchar(36)  
)
BEGIN
-- =================================================================================================
-- Author				      :	
-- Create date			  :	
-- Modified date		  :
-- Description			  :	
-- Tables Used			  : 
-- Parameters			    :	
-- Changes History		:
-- Exececution Step		:	
-- =================================================================================================
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
    WHERE 	UserGuId = UUID_TO_BIN(vOrgGuid);
   
END$$

DELIMITER ;
