using System;
using System.Threading.Tasks;

namespace Arya.Vis.Gateway.EmailProvider.Repositories
{
    public interface IManualEmailProviderRepository
    {
         Task DeleteConfigurationAsync(Guid providerId, bool isAdmin);
    }
}