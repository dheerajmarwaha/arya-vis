DROP procedure IF EXISTS `v3_get_organizations_list`;

DELIMITER $$
CREATE PROCEDURE `v3_get_organizations_list`
(
	vSearchKeyword VARCHAR(100) CHARSET utf8mb4, 
	vCount INT, 
	vSkip INT,
	vSortBy VARCHAR(30),
	vSortOrder VARCHAR(30)
)
BEGIN
  SET vSortBy = LOWER(vSortBy);
  SET vSortOrder= LOWER(vSortOrder);
  SELECT    
	   o.OrgGuid,
      o.OrganizationName,     
      o.SubscriptionEndDate,
      o.IsActive AND o.SubscriptionEndDate >= NOW() AS Is_active,
      o.OrgType,
      o.OrgCode,
      o.CreatedDate,
      o.ModifiedDate
    FROM mstorganization o
    JOIN orgconfigsettings os
      ON o.OrgGuid = os.OrgGuid   AND os.Is30Enabled IS TRUE
    WHERE o.OrgGuid IS NOT NULL
      AND (
			vSearchKeyword IS NULL
			OR vSearchKeyword = ''
			OR o.OrganizationName LIKE CONCAT('%', vSearchKeyword, '%')
		  )
    ORDER BY
     CASE WHEN vSortOrder = 'ascending' THEN 1
     ELSE
      CASE vSortBy
        WHEN 'modifieddate' THEN o.ModifiedDate
        WHEN 'createddate'  THEN o.CreatedDate
        WHEN 'name' THEN o.OrganizationName
      END
	END
    DESC,
    CASE WHEN vSortOrder != 'ascending' THEN 1
    ELSE
       CASE vSortBy
        WHEN 'modifieddate' THEN o.ModifiedDate
        WHEN 'createddate'  THEN o.CreatedDate
        WHEN 'name' THEN o.OrganizationName
      END
	END
    ASC
    LIMIT vSkip, vCount;

  SELECT COUNT(1) AS total
	  FROM mstorganization o
    JOIN orgconfigsettings os
      ON o.OrgGuid = os.OrgGuid
      AND os.Is30Enabled IS TRUE
     WHERE o.OrgGuid IS NOT NULL
      AND (
			vSearchKeyword IS NULL
			OR vSearchKeyword = ''
			OR o.OrganizationName LIKE CONCAT('%', vSearchKeyword, '%')
		  );
END$$

DELIMITER ;

