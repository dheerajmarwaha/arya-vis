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
        Task<Organization> GetOrganizationAsync(Guid orgGuid);
        
        Task<Organization> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand);
        
        Task<Guid> UpdateAsync(Guid orgGuid, OrganizationCreateCommand organizationCreateCommand);
        
        Task<OrganizationMetadataSearchResult> GetOrganizationsMetadataAsync(OrganizationsMetadataQuery organizationMetadataQuery);
        
        Task<IDictionary<Guid, OrganizationStat>> GetBulkOrganizationStatsAsync(IEnumerable<Guid> organizationIds);
        
        Task DeleteOrganizationAsync(Guid orgGuid);
    }
}
