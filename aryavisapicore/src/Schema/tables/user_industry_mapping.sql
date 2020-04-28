DROP TABLE IF EXISTS user_industry_mapping;

CREATE TABLE `user_industry_mapping` (
   UserGuId		BINARY  (16)  NOT NULL,
   Industry 	varchar(500) COLLATE utf8mb4_unicode_520_ci DEFAULT NULL
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci