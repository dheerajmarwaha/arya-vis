using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Services
{
    public interface ISystemConfigurationService
    {
        Task<string> GetValueAsync(string key);
        Task LoadAsync();
    }
}
