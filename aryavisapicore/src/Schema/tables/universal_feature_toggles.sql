DROP TABLE IF EXISTS universal_feature_toggles;

CREATE TABLE `universal_feature_toggles` (
   `FeatureGuid` binary(16) NOT NULL,
   `IsAllowed` bit(1) NOT NULL DEFAULT b'0',
   `IsEnabled` bit(1) DEFAULT b'1',
   PRIMARY KEY (`FeatureGuid`),
   CONSTRAINT `fk_universal_feature_toggle` FOREIGN KEY (`FeatureGuid`) REFERENCES `feature` (`FeatureGuid`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci