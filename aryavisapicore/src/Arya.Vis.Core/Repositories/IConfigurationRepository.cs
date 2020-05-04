using Arya.Vis.Core.Entities;
using Arya.Vis.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Repositories
{
    public interface IConfigurationRepository
    {
        Task UpdateUserConfigurationAsync(UserConfiguration configuration, Guid userGuid);
        //Task UpdateUserPortalConfigurationAsync(Guid userGuid, SourceConfiguration sourceConfiguration);
        //Task UpdateOrganizationConfigurationAsync(int orgId, SourceConfiguration sourceConfiguration);
        //Task UpdateOrganizationSourcingTypeConfigurationAsync(Guid orgGuid, StackRankType? sourcingType);
        Task PatchUserConfigurationAsync(UserConfiguration configuration, Guid userGuid);
        Task<UserConfiguration> GetUserConfigurationAsync(Guid userGuid);
        Task<OrganizationConfiguration> GetOrganizationConfigurationAsync(Guid orgGuid);
        //! TODO : Implementation to be implemented
        //Task<SearchOptions> GetConfiguredSearchOptionsAsync(Guid userGuid);
        //Task EnableAllSupportedSourcesAsync(Guid orgGuid);
    }
}
