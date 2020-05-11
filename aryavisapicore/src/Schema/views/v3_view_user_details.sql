DROP view IF EXISTS `v3_view_user_details`;
DELIMITER $$
CREATE VIEW v3_view_user_details AS  
SELECT
        BIN_TO_UUID(u.UserGuid) AS UserGuid,
        u.UserName as FullName,
        u.VendorId,
        u.FirstName,
        u.LastName,
        u.HomePhone,
        u.WorkPhone,
        u.LoginName, 
        u.Email,
        u.RoleGroupID,        
        rg.RoleName,
        BIN_TO_UUID(org.OrgGuid) AS OrgGuid,
        org.OrganizationName,  
        org.OrgCode,
        u.IsActive AND org.IsActive AND org.SubscriptionEndDate >= NOW() AS IsActive,
        BIN_TO_UUID(u.CreatedByGuid) AS CreatedByGuid,
        u.CreatedDate,
        BIN_TO_UUID(u.ModifiedByGuid) AS ModifiedByGuid,
        u.ModifiedDate,
		u.City,
        u.State,
        u.ZipCode,
        u.Country
      FROM user u
      LEFT JOIN mstorganization org
        ON u.OrgGuid=org.OrgGuid
      LEFT JOIN rolegroup rg
        ON u.RoleGroupID = rg.RoleGroupID AND rg.OrgGuid = u.OrgGuid