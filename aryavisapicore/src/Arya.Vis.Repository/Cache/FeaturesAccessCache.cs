using Arya.Vis.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Arya.Vis.Repository.Cache
{
    public class FeaturesAccessCache : IFeaturesAccessCache
    {
        private const string IsCacheLoaded = "IsCacheLoaded";
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _options;

        private CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        public FeaturesAccessCache(IMemoryCache cache)
        {
            _cache = cache;
            _options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)) // an hour? who decides?
                .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token)); // to allow manual reset
        }

        public IEnumerable<Feature> GetFeatures(Guid userGuid)
        {
            _cache.TryGetValue(userGuid, out IEnumerable<Feature> features);
            return features;
        }

        public void SetFeatures(Guid userGuid, IEnumerable<Feature> features)
        {
            _cache.Set(userGuid, features, _options);
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
