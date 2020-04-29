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
        Task<Organization> GetOrganizationAsync(Guid orgGuid);
        
        Task<Guid> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand);
        
        Task<Guid> UpdateAsync(Guid orgGuid, OrganizationCreateCommand organizationCreateCommand);
        
        Task<OrganizationMetadataSearchResult> GetOrganizationsMetadataAsync(OrganizationsMetadataQuery organizationsMetadataQuery);
        Task<IDictionary<Guid, OrganizationStat>> GetBulkOrganizationStatsAsync(IEnumerable<Guid> orgGuids);
        
        Task DeleteOrganizationAsync(Guid orgGuid);
    }
}
