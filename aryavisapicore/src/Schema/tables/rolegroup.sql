DROP TABLE IF EXISTS rolegroup;

CREATE TABLE rolegroup (
	RoleGroupID			int(11) NOT NULL AUTO_INCREMENT,
	RoleName			varchar(4000) NOT NULL,
	OrgGuId				BINARY  (16) NOT NULL,
	IsActive			bit(1) NOT NULL DEFAULT b'1',
	IsSuperAdmin		bit(1) NOT NULL DEFAULT b'0',
	IsOrgAdmin			bit(1) DEFAULT b'0',
	IsOrgTeamLeader	bit(1) DEFAULT b'0',
	IsCandidate		bit(1) DEFAULT b'0',
	IsVendor			bit(1) DEFAULT b'0',

	Description		text,
	-- Audit details
	CreatedByGuId		BINARY  (16) DEFAULT NULL,
	CreatedDate			datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
	ModifiedByGuId		BINARY  (16) DEFAULT NULL,
	ModifiedDate		datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,  

	PRIMARY KEY (RoleGroupID)
 ) ENGINE=InnoDB AUTO_INCREMENT=4990 DEFAULT CHARSET=latin1