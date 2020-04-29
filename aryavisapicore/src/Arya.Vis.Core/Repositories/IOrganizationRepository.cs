using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Repositories
{
    public interface IOrganizationRepository
    {
        Task<Guid> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand);
        Task<Guid> UpdateAsync(Guid OrgGuid, OrganizationCreateCommand organizationCreateCommand);
        Task<Organization> FetchOrganizationAsync(Guid OrgGuid);
        Task DeleteOrganizationAsync(Guid OrgGuid);
        Task<OrganizationMetadataSearchResult> FetchOrganizationsMetadataAsync(OrganizationsMetadataQuery organizationsMetadataQuery);
        Task<IDictionary<Guid, OrganizationStat>> GetBulkOrganizationStatsAsync(IEnumerable<Guid> orgGuids);
    }
}
