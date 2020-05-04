using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Arya.Vis.Repository.Cache
{
    public class SystemConfigurationCache : ISystemConfigurationCache
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<SystemConfigurationCache> _logger;
        private const string IsCacheLoaded = "IsCacheLoaded";
        private const string True = "true";

        private CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        public SystemConfigurationCache(IMemoryCache cache, ILogger<SystemConfigurationCache> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public string GetValue(string key)
        {
            _cache.TryGetValue<string>(key, out var value);
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogInformation($"Requested value fetched from System config cache for key : {key} is null/empty");
            }
            return value;
        }

        public void SetValue(string key, string value)
        {
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10)) // an hour? who decides?
                .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token)); // to allow manual reset
            _cache.Set<string>(key, value, options);
            _cache.Set<string>(IsCacheLoaded, True, options);
        }

        public void Set(IEnumerable<KeyValuePair<string, string>> configuration)
        {
            if (configuration == null) return;
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10)) // an hour? who decides?
                .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token)); // to allow manual reset
            options.SetSize(configuration.Count());
            foreach (var entry in configuration)
            {
                _cache.Set<string>(entry.Key, entry.Value, options);
            }
            _cache.Set<string>(IsCacheLoaded, True, options);
        }

        public void Clear()
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();
        }

        public bool IsLoaded()
        {
            // if cache is empty, load cache. Otherwise, return from cache
            if (!_cache.TryGetValue<string>(IsCacheLoaded, out var loaded))
            {
                return false;
            }
            return !string.IsNullOrWhiteSpace(loaded);
        }
    }
}
