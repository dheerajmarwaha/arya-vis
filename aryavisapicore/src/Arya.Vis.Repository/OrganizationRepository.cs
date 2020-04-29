using Arya.Exceptions;
using Arya.Storage;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Repository
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        public OrganizationRepository(ISqlProvider sqlProvider) : base(sqlProvider) { }

        public async Task<Guid> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand)
        {
            var command = SqlProvider.CreateCommand(Routines.CreateOrganization);
            using (command)
            {
                organizationCreateCommand.OrgGuid = Guid.NewGuid();
                AddOrgParameters_Insert(command, organizationCreateCommand);
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                using (reader)
                {
                    if (await SqlProvider.ReadAsync(reader))
                    {
                        return await SqlProvider.GetFieldValueAsync<Guid>(reader, "OrgGuid");
                    }
                    throw new RepositoryException("OrganizationRepository", "No resultset returned on org creation");
                }
            }
        }

        public async Task<Guid> UpdateAsync(Guid OrgGuid, OrganizationCreateCommand organizationCreateCommand)
        {
            var command = SqlProvider.CreateCommand(Routines.UpdateOrganization);
            organizationCreateCommand.OrgGuid = OrgGuid;
            using (command)
            {
                AddOrgParameters_Update(command, organizationCreateCommand);
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                using (reader)
                {
                    if (await SqlProvider.ReadAsync(reader))
                    {
                        return await SqlProvider.GetFieldValueAsync<Guid>(reader, "OrgGuid");
                    }
                    throw new RepositoryException("OrganizationRepository", $"No resultset returned on org update for org:{OrgGuid}");
                }
            }
        }

        public async Task<Organization> GetOrganizationAsync(Guid OrgGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetOrganization);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", OrgGuid);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                using (reader)
                {
                    if (await SqlProvider.ReadAsync(reader))
                    {
                        var org = await ReadWithoutDisposingReaderAsync(reader);
                        if (await reader.NextResultAsync())
                        {
                            org.Roles = await ReadOrgRolesWithoutDisposingReaderAsync(reader);
                        }
                        return org;
                    }
                    throw new RepositoryException("OrganizationRepository", $"No resultset returned on fetch org: {OrgGuid}");
                }
            }
        }

        public async Task DeleteOrganizationAsync(Guid OrgGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.DeleteOrganization);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", OrgGuid);
            using (command)
            {
                await SqlProvider.ExecuteScalarAsync(command);
            }
        }

        public async Task<OrganizationMetadataSearchResult> GetOrganizationsMetadataAsync(OrganizationsMetadataQuery organizationsMetadataQuery)
        {
            var command = SqlProvider.CreateCommand(Routines.GetOrganizations);
            SqlProvider.AddParameterWithValue(command, "vSearchKeyword", organizationsMetadataQuery.SearchKeyword);
            SqlProvider.AddParameterWithValue(command, "vCount", organizationsMetadataQuery.Size);
            SqlProvider.AddParameterWithValue(command, "vSkip", organizationsMetadataQuery.From);
            SqlProvider.AddParameterWithValue(command, "vSortBy", organizationsMetadataQuery.SortBy);
            SqlProvider.AddParameterWithValue(command, "vSortOrder", organizationsMetadataQuery.SortOrder);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                return await ReadOrganizationsMetaDataAsync(reader);
            }
        }

        public async Task<IDictionary<Guid, OrganizationStat>> GetBulkOrganizationStatsAsync(IEnumerable<Guid> orgGuids)
        {
            var command = SqlProvider.CreateCommand(Routines.GetBulkOrganizationStats);
            using (command)
            {
                string commaSeperatedOrgIds = string.Join(",", orgGuids);
                SqlProvider.AddParameterWithValue(command, "vOrgGuids", commaSeperatedOrgIds);
                return await ReadBulkOrganizationStats(await SqlProvider.ExecuteReaderAsync(command));
            }
        }

        private async Task<Organization> ReadWithoutDisposingReaderAsync(DbDataReader reader)
        {
            var org = new Organization
            {
                OrgGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "OrgGuid"),
                OrganizationName = await SqlProvider.GetFieldValueAsync<string>(reader, "OrganizationName"),
                ContactPerson = await SqlProvider.GetFieldValueAsync<string>(reader, "ContactPerson"),
                ContactEmail = await SqlProvider.GetFieldValueAsync<string>(reader, "ContactEmail"),
                HomePageLink = await SqlProvider.GetFieldValueAsync<string>(reader, "OrgHomePageLink"),
                IdentityProviderIdentifier = await SqlProvider.GetFieldValueAsync<string>(reader, "IdentityProviderIdentifier"),
                IsActive = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "IsActive")),
                Address = await SqlProvider.GetFieldValueAsync<string>(reader, "Address"),
                OrgType = await SqlProvider.GetFieldValueAsync<string>(reader, "OrgType"),
                OrgCode = await SqlProvider.GetFieldValueAsync<string>(reader, "OrgCode"),
                SubscriptionEndDate = await SqlProvider.GetFieldValueAsync<DateTime>(reader, "SubscriptionEndDate"),
                SourceLimit = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "SourceLimit")),
                JobCountLimit = await SqlProvider.GetFieldValueAsync<int>(reader, "JobCountLimit"),
                OrgLevelMiles = await SqlProvider.GetFieldValueAsync<string>(reader, "OrgLevelMiles"),
                
            };
            return org;
        }

        private async Task<IEnumerable<Role>> ReadOrgRolesWithoutDisposingReaderAsync(DbDataReader reader)
        {
            var roles = new List<Role>();
            while (await SqlProvider.ReadAsync(reader))
            {
                var role = new Role
                {
                    RoleGroupID = await SqlProvider.GetFieldValueAsync<int>(reader, "RoleGroupID"),
                    RoleName = await SqlProvider.GetFieldValueAsync<string>(reader, "RoleName")
                };
                roles.Add(role);
            }
            return roles;
        }

        private void AddOrgParameters_Insert(DbCommand command, OrganizationCreateCommand organizationCreateCommand)
        {
            
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", organizationCreateCommand.OrgGuid);
            SqlProvider.AddParameterWithValue(command, "vOrganizationName", organizationCreateCommand.OrganizationName);
            SqlProvider.AddParameterWithValue(command, "vContactPerson", organizationCreateCommand.ContactPerson);
            SqlProvider.AddParameterWithValue(command, "vCPEmail", organizationCreateCommand.ContactEmail);
            SqlProvider.AddParameterWithValue(command, "vOrgHomePageLink", organizationCreateCommand.HomePageLink);            
            SqlProvider.AddParameterWithValue(command, "vIsActive", organizationCreateCommand.IsActive);
            SqlProvider.AddParameterWithValue(command, "vAddress", organizationCreateCommand.Address);
            SqlProvider.AddParameterWithValue(command, "vOrgType", organizationCreateCommand.OrgType);
            SqlProvider.AddParameterWithValue(command, "vSubscriptionEndDate", organizationCreateCommand.SubscriptionEndDate);
            SqlProvider.AddParameterWithValue(command, "vIdentityProviderIdentifier", organizationCreateCommand.IdentityProviderIdentifier);
            SqlProvider.AddParameterWithValue(command, "vOrgSourceCount", organizationCreateCommand.SourceLimit);
            SqlProvider.AddParameterWithValue(command, "vJobCountLimit", organizationCreateCommand.JobCountLimit);
            SqlProvider.AddParameterWithValue(command, "vOrgLevelMiles", organizationCreateCommand.OrgLevelMiles);
            SqlProvider.AddParameterWithValue(command, "v30StackRankType", organizationCreateCommand.StackRankType);
            SqlProvider.AddParameterWithValue(command, "vCreatedByGuid", null); //TO DO :Dheeraj
        }

        private void AddOrgParameters_Update(DbCommand command, OrganizationCreateCommand organizationCreateCommand)
        {
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", organizationCreateCommand.OrgGuid);
            SqlProvider.AddParameterWithValue(command, "vContactPerson", organizationCreateCommand.ContactPerson);
            SqlProvider.AddParameterWithValue(command, "vCPEmail", organizationCreateCommand.ContactEmail);
            SqlProvider.AddParameterWithValue(command, "vOrgHomePageLink", organizationCreateCommand.HomePageLink);
            SqlProvider.AddParameterWithValue(command, "vIsActive", organizationCreateCommand.IsActive);
            SqlProvider.AddParameterWithValue(command, "vAddress", organizationCreateCommand.Address);
            SqlProvider.AddParameterWithValue(command, "vOrgType", organizationCreateCommand.OrgType);
            SqlProvider.AddParameterWithValue(command, "vSubscriptionEndDate", organizationCreateCommand.SubscriptionEndDate);
            SqlProvider.AddParameterWithValue(command, "vOrgSourceCount", organizationCreateCommand.SourceLimit);
            SqlProvider.AddParameterWithValue(command, "vJobCountLimit", organizationCreateCommand.JobCountLimit);
            SqlProvider.AddParameterWithValue(command, "vOrgLevelMiles", organizationCreateCommand.OrgLevelMiles);
            SqlProvider.AddParameterWithValue(command, "vModifiedByGuId", null); //TO DO :Dheeraj
        }

        private async Task<IDictionary<Guid, OrganizationStat>> ReadBulkOrganizationStats(DbDataReader reader)
        {
            var stats = new Dictionary<Guid, OrganizationStat>();
            using (reader)
            {
                while (await SqlProvider.ReadAsync(reader))
                {
                    var orgGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "OrgGuid");
                    var stat = new OrganizationStat();
                    stat.Licenses = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "licenses"));
                    //stat.Portals = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "portals"));
                    //stat.Vaults = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "vaults"));
                    //stat.Clients = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "clients"));
                    stat.TotalInterviews = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "total_interviews"));
                    stat.ActivatedInteriviews = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "activated_interiviews"));
                    stats.Add(orgGuid, stat);
                }
            }
            return stats;
        }

        private async Task<OrganizationMetadataSearchResult> ReadOrganizationsMetaDataAsync(DbDataReader reader)
        {
            var orgs = new List<OrganizationMetadata>();
            int total = 0;
            using (reader)
            {
                while (await SqlProvider.ReadAsync(reader))
                {
                    var org = new OrganizationMetadata
                    {
                        OrgGuid = Guid.Parse(await SqlProvider.GetFieldValueAsync<string>(reader, "OrgGuid")),
                        OrganizationName = await SqlProvider.GetFieldValueAsync<string>(reader, "OrganizationName"),
                        SubscriptionEndDate = await SqlProvider.GetFieldValueAsync<DateTime>(reader, "SubscriptionEndDate"),
                        IsActive = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "Is_active")),
                        OrgType = await SqlProvider.GetFieldValueAsync<string>(reader, "OrgType"),
                        OrgCode = await SqlProvider.GetFieldValueAsync<string>(reader, "OrgCode"),
                        CreatedDate = await SqlProvider.GetFieldValueAsync<DateTime>(reader, "CreatedDate"),
                        ModifiedDate = await SqlProvider.GetFieldValueAsync<DateTime>(reader, "ModifiedDate")
                    };
                    orgs.Add(org);
                }
                if (await SqlProvider.NextResultAsync(reader) && await SqlProvider.ReadAsync(reader))
                {
                    total = (int)await SqlProvider.GetFieldValueAsync<Int64>(reader, "total");
                }
            }
            return new OrganizationMetadataSearchResult
            {
                Organizations = orgs,
                Total = total
            };
        }

        
    }
}
