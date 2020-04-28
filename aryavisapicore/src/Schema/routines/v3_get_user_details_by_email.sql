DROP PROCEDURE IF EXISTS `v3_get_user_details_by_email`;
DELIMITER $$
CREATE PROCEDURE `v3_get_user_details_by_email`
(
		vLoginName varchar(500)
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
      WHERE LoginName=vLoginName OR Email=vLoginName;  
END$$
DELIMITER ;
