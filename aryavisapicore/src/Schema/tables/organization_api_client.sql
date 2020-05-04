CREATE TABLE `organization_api_client` (
   `OrgGuid` binary(16) NOT NULL,
   `client_id` char(26) COLLATE utf8mb4_unicode_520_ci NOT NULL,
   PRIMARY KEY (`OrgGuid`),
   UNIQUE KEY `client_id_UNIQUE` (`client_id`),
   CONSTRAINT `organization_api_client_organization` FOREIGN KEY (`OrgGuid`) REFERENCES `mstorganization` (`OrgGuid`) 
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci