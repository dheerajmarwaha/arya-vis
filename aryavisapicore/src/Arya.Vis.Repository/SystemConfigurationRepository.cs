using Arya.Storage;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Repository.Cache;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Repository
{
    public class SystemConfigurationRepository : BaseRepository, ISystemConfigurationRepository
    {
        private readonly ILogger<SystemConfigurationRepository> _logger;
        private readonly ISystemConfigurationCache _cache;
        public SystemConfigurationRepository(
            ILogger<SystemConfigurationRepository> logger,
            ISqlProvider sqlProvider,
            ISystemConfigurationCache cache
        ) : base(sqlProvider)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<string> GetValueAsync(string key)
        {
            if (_cache == null)
            {
                var configuration = await FetchConfigurationAsync();
                configuration.TryGetValue(key, out var value);
                return value;
            }
            if (!_cache.IsLoaded() || _cache.GetValue(key) == null)
            {
                _logger.LogInformation("Cache is not loaded, or cache hit is empty for key '{@Key}'", key);
                var configuration = await FetchConfigurationAsync();
                _cache.Set(configuration);
                _logger.LogInformation("Cache is reset from database");
            }
            var cachedValue = _cache.GetValue(key);
            return cachedValue;
        }

        public async Task LoadAsync()
        {
            if (_cache == null)
            {
                return;
            }
            var configuration = await FetchConfigurationAsync();
            _cache.Clear();
            _cache.Set(configuration);
        }

        private async Task<IDictionary<string, string>> FetchConfigurationAsync()
        {
            var command = SqlProvider.CreateCommand(Routines.GetSystemConfiguration);
            using (command)
            {
                return await ReadConfigurationAsync(await SqlProvider.ExecuteReaderAsync(command));
            }
        }

        private async Task<IDictionary<string, string>> ReadConfigurationAsync(DbDataReader reader)
        {
            var configuration = new Dictionary<string, string>();
            using (reader)
            {
                while (await SqlProvider.ReadAsync(reader))
                {
                    var key = await SqlProvider.GetFieldValueAsync<string>(reader, "key");
                    var value = await SqlProvider.GetFieldValueAsync<string>(reader, "value");
                    if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                    {
                        _logger.LogWarning("Invalid key: {@Key} or value: {@Value} is provided", key, value);
                        continue;
                    }
                    configuration[key] = value;
                }
            }
            return configuration;
        }
    }
}
