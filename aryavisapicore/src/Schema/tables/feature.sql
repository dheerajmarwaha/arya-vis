DROP TABLE IF EXISTS feature;

 CREATE TABLE `feature` (
   `FeatureGuid` binary(16) NOT NULL,
   `FeatureName` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
   PRIMARY KEY (`FeatureGuid`),
   UNIQUE KEY `name_UNIQUE` (`FeatureName`)
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci