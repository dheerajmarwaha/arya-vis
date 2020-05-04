using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Repositories
{
    public interface IOrgApiClientRepository
    {
        Task<Guid?> GetOrgGuidAsync(string clientId);
    }
}
