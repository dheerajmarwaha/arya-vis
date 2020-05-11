DROP TABLE IF EXISTS unregistered_user;

CREATE TABLE `unregistered_user` (  
	UserGuid		BINARY  (16)  NOT NULL,
    FullName 		varchar(255)  DEFAULT NULL,
   `Email` 			varchar(255)  DEFAULT NULL,
   `Phone` 			varchar(255)  DEFAULT NULL,
   `City` 			varchar(255)  DEFAULT NULL,
   `StateCode` 			varchar(20)  DEFAULT NULL,
   `PostalCode` 		varchar(20)  DEFAULT NULL,
   `CountryCode`		char(2)  DEFAULT NULL,
   `Company` 		varchar(500)  DEFAULT NULL,
   `ProfileType` 	enum('AryaVis')  DEFAULT NULL,
   `CreatedDate` 	datetime DEFAULT CURRENT_TIMESTAMP,
   PRIMARY KEY (`UserGuid`),
   KEY `IDX_EMAIL` (`Email`),
   KEY `IDX_COUNTRY` (`CountryCode`)
 ) ENGINE=InnoDB AUTO_INCREMENT=205 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci