using Arya.Vis.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Services
{
    public interface IConfigurationService
    {
        Task<UserConfiguration> GetUserConfigurationAsync();
        Task UpdateUserConfigurationAsync(UserConfiguration configuration);
        //Task UpdateOrganizationConfigurationAsync(Guid orgGuid, OrganizationConfiguration organizationConfiguration);
        Task PatchUserConfigurationAsync(UserConfiguration configuration);
        Task<SearchOptions> GetConfiguredSearchOptionsAsync();
        Task<OrganizationConfiguration> GetOrganizationConfigurationAsync(Guid orgGuid);
    }
}
