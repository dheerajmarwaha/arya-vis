DROP TABLE IF EXISTS org_feature_toggles;
CREATE TABLE `org_feature_toggles` (
   FeatureGuid 	binary(16) NOT NULL,
   OrgGuId		BINARY  (16) NOT NULL,
   `is_allowed` bit(1) NOT NULL DEFAULT b'0',
   `is_enabled` bit(1) DEFAULT b'1',
   PRIMARY KEY (`FeatureGuid`,`OrgGuId`),
   UNIQUE KEY `unique_org_feature` (`OrgGuId`,`FeatureGuid`),
   KEY `idx_org` (`OrgGuId`),
   CONSTRAINT `fk_org_feature_toggle` FOREIGN KEY (`FeatureGuid`) REFERENCES `feature` (`FeatureGuid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT `fk_org_feature_toggle_org` FOREIGN KEY (`OrgGuId`) REFERENCES `mstorganization` (`OrgGuId`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci