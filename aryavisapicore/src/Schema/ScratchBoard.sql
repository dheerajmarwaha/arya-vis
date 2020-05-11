/*
	OrgGuid - 946c481b-8eb9-11ea-b04b-00090faa0001
    UserGuid - 80f93d84-8eba-11ea-b04b-00090faa0001
    CreateByGuid  - 8a715352-8eba-11ea-b04b-00090faa0001
    
    SET @orgGuid = UUID_TO_BIN('946c481b-8eb9-11ea-b04b-00090faa0001')
    SET @userGuid = UUID_TO_BIN('80f93d84-8eba-11ea-b04b-00090faa0001')
	SET @createdByGuid = UUID_TO_BIN('8a715352-8eba-11ea-b04b-00090faa0001')

SET @orgGuid = UUID_TO_BIN('946c481b-8eb9-11ea-b04b-00090faa0001');
    SET @userGuid = UUID_TO_BIN('80f93d84-8eba-11ea-b04b-00090faa0001');
	SET @createdByGuid = UUID_TO_BIN('8a715352-8eba-11ea-b04b-00090faa0001');
-- delete from mstorganization;
-- insert into mstorganization (OrgGuid, OrganizationName, OrgCode, IsActive, SubscriptionEndDate, CreatedByGuid) values (@orgGuid, 'Leoforce, LLC', 'ORG-01', 1, '9999-01-01', @createdByGuid);
-- select * from mstorganization;
-- delete from rolegroup;
-- insert into rolegroup(RoleName,OrgGuid,IsActive,IsSuperAdmin,IsOrgAdmin,CreatedByGuid ) values( 'SuperAdmin',@orgGuid,1,1,1, @createdByGuid);
-- select * from rolegroup;

-- call v3_insert_user ('80f93d84-8eba-11ea-b04b-00090faa0001', 'Dheeraj','Marwaha','dheeraj.marwaha@leoforce.com','9945988000',null, 4991, 1, '946c481b-8eb9-11ea-b04b-00090faa0001', 'Dumy_123', 'Hyderabad', 'Telangana','India','500075', '8a715352-8eba-11ea-b04b-00090faa0001');

-- call v3_get_user_details_by_email ('dheeraj.marwaha@leoforce.com');
-- call v3_get_user_details_by_guid ('80f93d84-8eba-11ea-b04b-00090faa0001')
-- call v3_get_all_users ('946c481b-8eb9-11ea-b04b-00090faa0001',0,10,null,null,null,null,null)
*/
 select Bin_to_uuid(UserGuid) from unregistered_user;






