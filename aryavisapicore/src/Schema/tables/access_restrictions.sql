DROP TABLE IF EXISTS access_restrictions;

CREATE TABLE `access_restrictions` (
   `id` int(11) NOT NULL AUTO_INCREMENT,
   `feature` varchar(45) NOT NULL,
   `entity_type` enum('ORG','ROLE','USER','DEFAULT') NOT NULL,
   `entity_id` int(11) NOT NULL,
   `allowed` bit(1) NOT NULL,
   PRIMARY KEY (`id`),
   UNIQUE KEY `UQ_feature_entity` (`feature`,`entity_type`,`entity_id`) USING BTREE,
   KEY `IDX_feature` (`feature`) USING BTREE,
   KEY `IDX_entity` (`entity_type`,`entity_id`) USING BTREE
 ) ENGINE=InnoDB AUTO_INCREMENT=4879 DEFAULT CHARSET=utf8mb4