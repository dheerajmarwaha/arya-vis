using Arya.Storage;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Enums;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Utils;
using Arya.Vis.Repository.Cache;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Repository
{
    public class AccessRepository : BaseRepository, IAccessRepository
    {
        private readonly IFeaturesAccessCache _cache;
        public AccessRepository(ISqlProvider sqlProvider, IFeaturesAccessCache cache) : base(sqlProvider) =>
            _cache = cache;

        public async Task<IEnumerable<Feature>> FindAllFeaturesAsync(Guid userGuid)
        {
            if (_cache == null)
            {
                return await FetchAllFeaturesAsync(userGuid);
            }
            var features = _cache.GetFeatures(userGuid);
            if (features == null)
            {
                features = await FetchAllFeaturesAsync(userGuid);
                _cache.SetFeatures(userGuid, features);
            }
            return features;
        }

        private async Task<IEnumerable<Feature>> FetchAllFeaturesAsync(Guid userGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetAllowedFeatures);
            using (command)
            {
                SqlProvider.AddParameterWithValue(command, "vUserGuid", userGuid);
                return await ReadAllowedFeaturesAsync(await SqlProvider.ExecuteReaderAsync(command));
            }
        }

        private async Task<IEnumerable<Feature>> ReadAllowedFeaturesAsync(DbDataReader reader)
        {
            var allowedFeatures = new List<Feature>();
            using (reader)
            {
                while (await SqlProvider.ReadAsync(reader))
                {
                    var feature = new Feature();
                    feature.Name = EnumUtils.GetEnum<FeatureName>(await SqlProvider.GetFieldValueAsync<string>(reader, "FeatureName"));
                    feature.IsEnabled = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "IsEnabled"));
                    feature.IsAllowed = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "IsAllowed"));
                    allowedFeatures.Add(feature);
                }
            }
            return allowedFeatures;
        }
    }
}
