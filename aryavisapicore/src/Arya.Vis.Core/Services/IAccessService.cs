using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.Services
{
    public interface IAccessService
    {
        /// <summary>
        /// Verifies the given feature is allowed or not.
        /// </summary>
        /// <param name="feature">The feature to verify.</param>
        /// <returns>true/false</returns>
        Task<bool> IsAllowedAsync(FeatureName feature);
        /// <summary>
        /// Verifies the given collection of features are allowed or not and returns the allowed features.
        /// </summary>
        /// <param name="features">The collection of features to verify.</param>
        /// <returns>Collection of allowed features</returns>
        Task<IEnumerable<FeatureName>> AreAllowedAsync(IEnumerable<FeatureName> features);
        Task<IEnumerable<Feature>> ListAsync();
    }
}
