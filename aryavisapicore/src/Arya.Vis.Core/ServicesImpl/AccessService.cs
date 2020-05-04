using Arya.Exceptions;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Enums;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arya.Vis.Core.ServicesImpl
{
    public class AccessService : IAccessService
    {
        private readonly ILogger<AccessService> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessRepository _accessRepository;

        private CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        public AccessService(ILogger<AccessService> logger, IUserService userService, IUnitOfWork unitOfWork, IAccessRepository accessRepository)
        {
            _logger = logger;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _accessRepository = accessRepository;
        }

        public async Task<IEnumerable<FeatureName>> AreAllowedAsync(IEnumerable<FeatureName> features)
        {
            ValidationUtil.NotNull(features);
            ValidationUtil.NotEmptyCollection((ICollection<FeatureName>)features);
            var currentUser = _userService.GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var allFeatures = await _accessRepository.FindAllFeaturesAsync(currentUser.UserGuid);
                    var allowedFeatures = allFeatures
                                          .Where(feature => feature.IsAllowed && feature.IsEnabled && features.ToList().Contains(feature.Name))
                                          .Select(feature => feature.Name);

                    await _unitOfWork.CompleteAsync();
                    return allowedFeatures?.Intersect(features);
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting allowed features for user {currentUser.UserGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> IsAllowedAsync(FeatureName featureName)
        {
            var currentUser = _userService.GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var allFeatures = await _accessRepository.FindAllFeaturesAsync(currentUser.UserGuid);
                    var allowedFeature = allFeatures.FirstOrDefault(feature => feature.IsEnabled && feature.IsAllowed && (feature.Name == featureName));
                    await _unitOfWork.CompleteAsync();
                    return allowedFeature != null;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while checking feature is allowed for user {currentUser.UserGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Feature>> ListAsync()
        {
            var currentUser = _userService.GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var allFeatures = await _accessRepository.FindAllFeaturesAsync(currentUser.UserGuid);
                    await _unitOfWork.CompleteAsync();
                    return allFeatures;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while checking feature is allowed for user {currentUser.UserGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
