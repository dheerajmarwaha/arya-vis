DROP PROCEDURE IF EXISTS `v3_get_user_details_by_guid`;
DELIMITER $$
CREATE PROCEDURE `v3_get_user_details_by_guid`
(		
        vUserGuid varchar(36)
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
      WHERE UserGuid=vUserGuid;  
END$$
DELIMITER ;
