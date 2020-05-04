DROP procedure IF EXISTS `v3_update_organization_configuration`;

DELIMITER $$
CREATE PROCEDURE `v3_update_organization_configuration`(
IN vOrgGuid 		VARCHAR(36),
IN vSourceLimit 	INT(11),
IN vIsEnabled 		BIT(1),
IN vPortal 			  VARCHAR(100)
)
BEGIN
   SET @OrgGuid = UUID_TO_BIN(vOrgGuid);
   SET @correspondingPortalSourceCountEntryName = NULL,@correspondingFeatureName=NULL;
   
   IF(vPortal='CareerBuilder') THEN
      SET @correspondingPortalSourceCountEntryName = 'CareerBuilderSourceCount',@correspondingFeatureName='CAREER_BUILDER_JOBBOARD';
   ELSEIF(vPortal='Nexxt') THEN
      SET @correspondingPortalSourceCountEntryName = 'NexxtSourceCount',@correspondingFeatureName='NEXXT_JOBBOARD';
   ELSEIF(vPortal='ResumeLibrary') THEN
      SET @correspondingPortalSourceCountEntryName = 'ResumeLibrarySourceCount',@correspondingFeatureName='RESUMELIBRARY_JOBBOARD';
   ELSEIF(vPortal='EFinancial') THEN
	  SET @correspondingPortalSourceCountEntryName = 'EFinancialSourceCount',@correspondingFeatureName='EFINANCIAL_JOBBOARD';
   ELSEIF(vPortal='Monster') THEN
	  SET @correspondingPortalSourceCountEntryName = 'MonsterSourceCount',@correspondingFeatureName='MONSTER_JOBBOARD';
   ELSEIF(vPortal='Dice') THEN
      SET @correspondingPortalSourceCountEntryName = 'DiceSourceCount',@correspondingFeatureName='DICE_JOBBOARD';
   ELSEIF(vPortal='Indeed') THEN
      SET @correspondingPortalSourceCountEntryName = 'IndeedSourceCount',@correspondingFeatureName='INDEED_JOBBOARD';
   ELSEIF(vPortal='Internal') THEN
      SET @correspondingPortalSourceCountEntryName='InternalSourceCount',@correspondingFeatureName='INTERNAL_PORTAL';
   ELSEIF(vPortal='CVLibrary') THEN
      SET @correspondingPortalSourceCountEntryName='CVLibrarySourceCount',@correspondingFeatureName='CVLIBRARY_JOBBOARD';
   ELSEIF(vPortal='CWJobs') THEN
      SET @correspondingPortalSourceCountEntryName='CWJobsSourceCount',@correspondingFeatureName='CWJOBS_JOBBOARD';
   ELSEIF(vPortal='TotalJobs') THEN
      SET @correspondingPortalSourceCountEntryName='TotalJobsSourceCount',@correspondingFeatureName='TOTALJOBS_JOBBOARD';
   ELSEIF(vPortal='JobSite') THEN
      SET @correspondingPortalSourceCountEntryName='JobSiteSourceCount',@correspondingFeatureName='JOBSITE_JOBBOARD';
   ELSEIF(vPortal='Social') THEN
      SET @correspondingFeatureName='SOCIAL_PORTAL';
      UPDATE mstorganization mstorg SET mstorg.OrgSourceCount=vSourceLimit WHERE OrgGuid=UUID_TO_BIN(vOrgGuid);
   END IF;

   DELETE FROM access_restrictions WHERE entity_type='USER' AND feature=@correspondingFeatureName AND entity_guid IN (SELECT UserGuid FROM user WHERE OrgGuid = @OrgGuid);
   IF NOT EXISTS (SELECT 1 FROM access_restrictions WHERE entity_type='ORG' AND entity_guid = @OrgGuid AND feature = @correspondingFeatureName)
       THEN
       INSERT INTO access_restrictions
       (
        `feature`,
        `entity_type`,
        `entity_guid`,
        `allowed`
       )
       VALUES
       (
        @correspondingFeatureName,
        'ORG',
        @OrgGuid,
        vIsEnabled
       );
    ELSE
      UPDATE access_restrictions
        SET `allowed` = vIsEnabled
        WHERE entity_type='ORG'
        AND entity_id = @OrgGuid
        AND feature=@correspondingFeatureName;
    END IF;


  IF @correspondingPortalSourceCountEntryName IS NOT NULL
     THEN
       SET @SourceLimit = vSourceLimit;
       
	   SET @sqlText = CONCAT('UPDATE orgconfigsettings SET ' , @correspondingPortalSourceCountEntryName , ' = ? WHERE OrgId = ? ;');
	   PREPARE stmt FROM @sqlText;
	   EXECUTE stmt using @SourceLimit, @OrgGuid;
  END IF;
END$$

DELIMITER ;

