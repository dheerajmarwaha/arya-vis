using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Repository
{
    public static class Routines
    {
        /* User */
        public const string CreateInterview = "v3_create_interview";

        public const string GetInterview = "v3_get_interview";

        public const string GetUserDetailsByEmail = "v3_get_user_details_by_email";

        public const string GetUserDetailsByGuid = "v3_get_user_details_by_guid";

        public const string CreateUser = "v3_insert_user";

        public const string UpdateUser = "v3_update_user";

        public const string GetUsersByOrgGuid = "v3_get_users_by_org_guid";

        public const string GetUserGuidsByOrgGuid = "v3_get_user_guids_by_org_guid";

        public const string GetAllUsers = "v3_get_all_users";

        public const string GetUserCount = "v3_get_user_count_for_org";

        public const string GetDefaultAdminUser = "v3_get_default_admin_user";

        public const string DeleteUser = "v3_delete_user";

        public const string CreateUnregisteredUser = "v3_create_unregistered_user";

        public const string GetUnregisteredUserDetails = "v3_get_unregistered_user_details";

        /* User Configuration */
        public const string GetUserConfigurations = "v3_get_user_configurations";

        public const string PatchUserConfiguration = "v3_patch_user_configuration";

        public const string UpdateUserConfiguration = "v3_update_user_configuration";


        /* Organization */
        public const string CreateOrganization = "v3_insert_organization";

        public const string UpdateOrganization = "v3_update_organization";

        public const string GetOrganization = "v3_get_organization";

        public const string DeleteOrganization = "v3_delete_organization";

        public const string GetOrganizations = "v3_get_organizations_list";

        public const string GetBulkOrganizationStats = "v3_get_bulk_organization_stats";

        /* System Configuration */
        public const string GetSystemConfiguration = "v3_get_system_configuration";

        public const string GetOrganizationConfiguration = "v3_get_organization_configuration";

        public const string UpdateOrganizationConfiguration = "v3_update_organization_configuration";



        /* Features */
        public const string GetAllowedFeatures = "v3_get_allowed_features";
    }
}
