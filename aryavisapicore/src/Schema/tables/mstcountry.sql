DROP TABLE IF EXISTS mstcountry;

CREATE TABLE `mstcountry` (
   `CountryId` int(11) NOT NULL AUTO_INCREMENT,
   `CountryCode` varchar(2000) DEFAULT NULL,
   `CountryCode3` varchar(45) DEFAULT NULL,
   `CountryName` varchar(2000) DEFAULT NULL,
   `CountryDesc` varchar(2000) DEFAULT NULL,
   `Region` int(11) DEFAULT NULL,
   `OrgGuid` BINARY(16) DEFAULT NULL,
   `IsActive` bit(1) DEFAULT b'1',
   `CreatedById` int(11) DEFAULT '1',
   `NationalCode` varchar(45) DEFAULT NULL,
   `CreatedDate` datetime DEFAULT NULL,
   PRIMARY KEY (`CountryId`),
   UNIQUE KEY `IDX_COUNTRYID_NAME` (`CountryId`,`CountryName`)
 ) ENGINE=InnoDB AUTO_INCREMENT=192 DEFAULT CHARSET=latin1