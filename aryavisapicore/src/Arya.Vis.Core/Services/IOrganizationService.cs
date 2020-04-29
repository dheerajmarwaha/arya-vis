using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Services
{
    public interface IOrganizationService
    {
        Task<Organization> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand);
        Task<Organization> GetOrganizationAsync(Guid OrgGuid);
        Task<Organization> UpdateAsync(Guid OrgGuid, OrganizationCreateCommand organizationCreateCommand);
        Task<OrganizationMetadataSearchResult> GetOrganizationsMetadataAsync(OrganizationsMetadataQuery organizationMetadataQuery);
        Task<IDictionary<int, OrganizationStat>> GetBulkOrganizationStatsAsync(IEnumerable<int> organizationIds);
        Task DeleteOrganizationAsync(Guid OrgGuid);
    }
}
