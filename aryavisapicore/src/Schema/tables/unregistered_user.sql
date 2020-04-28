DROP TABLE IF EXISTS unregistered_user;
CREATE TABLE `unregistered_user` (  
	UserGuId		BINARY  (16)  NOT NULL,
    FullName 		varchar(255) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `Email` 			varchar(255) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `Phone` 			varchar(255) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `City` 			varchar(255) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `StateCode` 			varchar(20) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `PostalCode` 		varchar(20) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `CountryCode`		char(2) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `Company` 		varchar(500) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `ProfileType` 	enum('AryaVis') COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
   `CreatedDate` 	datetime DEFAULT CURRENT_TIMESTAMP,
   PRIMARY KEY (`UserGuId`),
   KEY `IDX_EMAIL` (`Email`),
   KEY `IDX_COUNTRY` (`CountryCode`)
 ) ENGINE=InnoDB AUTO_INCREMENT=205 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci