DROP TABLE IF EXISTS user;
CREATE TABLE user (
  UserGuid				BINARY  (16)  NOT NULL,
  UserName				varchar(1000) CHARACTER SET utf8 NOT NULL,
  VendorId				varchar(100) NOT NULL,
  FirstName				varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  LastName				varchar(500) CHARACTER SET utf8 DEFAULT NULL,
  LoginName				varchar(500) CHARACTER SET utf8 NOT NULL,
  Password				varchar(255) NOT NULL,
  Email					varchar(1000) CHARACTER SET utf8 NOT NULL,
  RoleGroupId			int(11) NOT NULL,
  IsActive				bit(1) NOT NULL DEFAULT b'1',
  OrgGuid				BINARY  (16)  DEFAULT NULL,
  HomePhone				varchar(500) DEFAULT NULL,
  WorkPhone				varchar(500) DEFAULT NULL,
  MobilePhone			varchar(500) DEFAULT NULL,
  Address1				text,
  Address2				text,
  City					varchar(1000) DEFAULT NULL,
  State					varchar(1000) DEFAULT NULL,
  ZipCode				int(11) DEFAULT NULL,
  Country				varchar(1000) DEFAULT NULL,
  
  
  EmailSignature		longtext CHARACTER SET utf8,
  ProfilePic			varchar(45) DEFAULT NULL,
  LinkedINURl			varchar(2000) DEFAULT NULL,
  FacebookURL			varchar(2000) DEFAULT NULL,
  TwitterURL			varchar(2000) DEFAULT NULL,
  VideoURL				varchar(2000) DEFAULT NULL,
  AboutRecruiter		text,
  CandidateId			int(11) DEFAULT '0',
  ReportingTo			int(11) DEFAULT NULL,
  HashedPassword		varchar(255) DEFAULT 'ty',
  SSO varchar(255)		DEFAULT NULL,
  Preferedlanguage		varchar(45) DEFAULT NULL,
  IsFirstLogin			bit(1) NOT NULL DEFAULT b'0',
   
   -- Audit details
   CreatedByGuid		BINARY  (16) DEFAULT NULL,
   CreatedDate			datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
   ModifiedByGuid		BINARY  (16) DEFAULT NULL,
   ModifiedDate			datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,   

   PRIMARY KEY (UserGuid),
   UNIQUE KEYIDX_UNIQUE_vendor_org (VendorId,OrgGuid),  
    KEY `PKIndex` (`UserGuId`,`UserName`(255),`FirstName`(255),`LastName`(255),`RoleGroupID`,`OrgGuid`),
   KEY vendorid (VendorId),
   KEY IDX_Organization (OrgGuid),
   KEY Email (Email(255)),
   KEY IDX_vendor_org (VendorId,OrgGuid),
   KEY IDX_USER_SSO (SSO),
   KEY IDX_USER_LOGIN_NAME (LoginName(255)),
   KEY IDX_USERNAME (UserName)
 ) ENGINE=InnoDB AUTO_INCREMENT=14715 DEFAULT CHARSET=latin1
 
 