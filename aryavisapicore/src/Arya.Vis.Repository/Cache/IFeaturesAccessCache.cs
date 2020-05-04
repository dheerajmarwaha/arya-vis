using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Repository.Cache
{
    public interface IFeaturesAccessCache : IRepositoryCache
    {
        IEnumerable<Feature> GetFeatures(Guid userGuid);
        void SetFeatures(Guid userGuid, IEnumerable<Feature> features);
    }
}
