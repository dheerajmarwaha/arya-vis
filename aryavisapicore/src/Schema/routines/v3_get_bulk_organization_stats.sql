DROP procedure IF EXISTS `v3_get_bulk_organization_stats`;

DELIMITER $$
CREATE PROCEDURE `v3_get_bulk_organization_stats`
(
	IN vOrgGuids VARCHAR(4000)
)
BEGIN
	
	CALL arya_common_splitStr(vOrgGuids, ',');

  DROP TEMPORARY TABLE IF EXISTS temp_org_stats;
  -- CREATE TEMPORARY TABLE temp_org_stats (`OrgGuid` VARCHAR(36), `licenses` INT,`portals` INT, `clients` INT, `vaults` INT, `total_interviews` INT, `activated_interiviews` INT);
  CREATE TEMPORARY TABLE temp_org_stats (`OrgGuid` VARCHAR(36), `licenses` INT, `clients` INT,`total_interviews` INT, `activated_interiviews` INT);
  INSERT INTO temp_org_stats (`OrgGuid`)
  SELECT split_str FROM temp_strs;

   UPDATE temp_org_stats A
   SET licenses = (SELECT COUNT(1) from user WHERE OrgGuid = A.OrgGuid);

   UPDATE temp_org_stats A
   SET clients = (SELECT COUNT(CompanyId) from mstcompany WHERE OrgGuid = A.OrgGuid);

	/*
   UPDATE temp_org_stats tos
   SET activated_jobs= (SELECT
     COUNT(distinct(CASE WHEN (B.JobId IS NOT NULL
     OR C.jobId IS NOT NULL
     OR D.JobID IS NOT NULL) THEN  A.JobId
     ELSE
     NULL END)) AS ActivatedJobs
   FROM job A
     LEFT JOIN socialaryapublishedjob B ON A.JobId = B.JobId
     LEFT JOIN aryadetails C ON A.jobId = C.JobID
     LEFT JOIN suriautosorce D ON A.jobId = D.JobID
   WHERE A.Organization = tos.org_id);
	*/
    
   UPDATE temp_org_stats A
   SET total_jobs = (SELECT COUNT(1) from interview WHERE OrgGuid = A.OrgGuid);
/*
   UPDATE temp_org_stats A
   SET vaults = (SELECT COUNT(1) from mstatsconfiguration WHERE OrgGuid = A.OrgGuid);

	CALL arya_bulk_organization_access_isAllowed__noresult(
    vOrgIds,
    'INTERNAL_PORTAL,SOCIAL_PORTAL,INDEED_JOBBOARD,CAREER_BUILDER_JOBBOARD,DICE_JOBBOARD,MONSTER_JOBBOARD,NEXXT_JOBBOARD,EFINANCIAL_JOBBOARD,RESUMELIBRARY_JOBBOARD,CVLIBRARY_JOBBOARD,CWJOBS_JOBBOARD,TOTALJOBS_JOBBOARD,JOBSITE_JOBBOARD'
    );

   UPDATE temp_org_stats A
   SET portals = (select count(1) from temp_restrictions tr where tr.org_id = A.org_id AND tr.is_allowed IS TRUE);
*/
  SELECT * FROM temp_org_stats;
END$$

DELIMITER ;

