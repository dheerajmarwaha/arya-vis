DROP TABLE IF EXISTS management_user_role;

CREATE TABLE `management_user_role` (
   `UserGuId` binary(16) NOT NULL,
   `RoleGroupID`  binary(16) NOT NULL,
   PRIMARY KEY (`UserGuId`)
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci