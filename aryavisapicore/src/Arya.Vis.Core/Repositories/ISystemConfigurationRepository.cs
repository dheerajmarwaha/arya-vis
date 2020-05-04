using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Repositories
{
    public interface ISystemConfigurationRepository
    {
        Task<string> GetValueAsync(string key);
        Task LoadAsync();
    }
}
