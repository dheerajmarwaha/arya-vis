DROP TABLE IF EXISTS user_feature_toggles;

CREATE TABLE `user_feature_toggles` (
   `FeatureGuid` binary(16) NOT NULL,
   `UserGuid` binary(16) NOT NULL,
   `IsAllowed` bit(1) NOT NULL DEFAULT b'0',
   `IsEnabled` bit(1) DEFAULT b'1',
   PRIMARY KEY (`FeatureGuid`,`UserGuid`),
   UNIQUE KEY `unique_user_feature` (`UserGuid`,`FeatureGuid`),
   KEY `idx_user` (`UserGuid`),
   CONSTRAINT `fk_user_feature_toggle` FOREIGN KEY (`FeatureGuid`) REFERENCES `feature` (`FeatureGuid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT `fk_user_feature_toggle_user` FOREIGN KEY (`UserGuid`) REFERENCES `user` (`UserGuid`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci