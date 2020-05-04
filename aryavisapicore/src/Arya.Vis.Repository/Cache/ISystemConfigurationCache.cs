using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Repository.Cache
{
    public interface ISystemConfigurationCache : IRepositoryCache
    {
        string GetValue(string key);
        void SetValue(string key, string value);
        void Set(IEnumerable<KeyValuePair<string, string>> configuration);
    }
}
