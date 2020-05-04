using Arya.Exceptions;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Enums;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.Utils;
using Arya.Vis.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.ServicesImpl
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ILogger<ConfigurationService> _logger;
        private readonly IUserService _userService;
        private readonly IAccessService _accessService;
        //private readonly ISourceConfigurationService _sourceConfigurationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IList<string> SuperAdminRoleNames = new List<string> { "SuperAdmin", "Super Admin" };

        public ConfigurationService(
            ILogger<ConfigurationService> logger,
            IUserService userService,
            IAccessService accessService,
            //ISourceConfigurationService sourceConfigurationService,
            IUnitOfWork unitOfWork,
            IConfigurationRepository configurationRepository
        )
        {
            _logger = logger;
            _userService = userService;
            _accessService = accessService;
           // _sourceConfigurationService = sourceConfigurationService;
            _unitOfWork = unitOfWork;
            _configurationRepository = configurationRepository;
        }

        public async Task<UserConfiguration> GetUserConfigurationAsync()
        {
            var user = _userService.GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var userConfiguration = await _configurationRepository.GetUserConfigurationAsync(user.UserGuid);
                    userConfiguration.Role = user.RoleName;

                    //var sources = await _sourceConfigurationService.SearchAsync(new SourceConfigurationQuery
                    //{
                    //    IsAuthorized = null
                    //});
                    //userConfiguration.Sources = await GroupSourceConfigurations(sources);
                    
                    userConfiguration.Features = await _accessService.ListAsync();
                    await _unitOfWork.CompleteAsync();
                    return userConfiguration;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while getting user configuration for userId : `{@UserId}`. Rolling back.", user.UserGuid);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<OrganizationConfiguration> GetOrganizationConfigurationAsync(Guid orgGuid)
        {
            ValidationUtil.NotEmptyGuid(orgGuid, nameof(orgGuid));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var organizationConfiguration = await _configurationRepository.GetOrganizationConfigurationAsync(orgGuid);
                    //organizationConfiguration.Sources = await _sourceConfigurationService.AdminSearchAsync(orgGuid);
                    await _unitOfWork.CompleteAsync();
                    return organizationConfiguration;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while getting organization configuration for orgGuid : `{@OrgId}`. Rolling back.", orgGuid);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        //public Task<IEnumerable<SourceConfiguration>> GroupSourceConfigurations(IEnumerable<SourceConfiguration> sourceConfigurations)
        //{
        //    var user = _userService.GetCurrentUser();
        //    try
        //    {
        //        var sourceGroups = new Dictionary<string, List<SourceConfiguration>>();
        //        var newSourceConfigurations = new List<SourceConfiguration>();
        //        var unGroupedSourceConfigurations = new List<SourceConfiguration>();
        //        foreach (var sourceConfiguration in sourceConfigurations)
        //        {
        //            var groupName = sourceConfiguration.Source.Group;
        //            if (groupName != null)
        //            {
        //                if (sourceGroups.ContainsKey(groupName))
        //                {
        //                    sourceGroups[groupName].Add(sourceConfiguration);
        //                }
        //                else
        //                {
        //                    sourceGroups[groupName] = new List<SourceConfiguration> { sourceConfiguration };
        //                }
        //            }
        //            else
        //            {
        //                unGroupedSourceConfigurations.Add(sourceConfiguration);
        //            }
        //        }
        //        foreach (var entry in sourceGroups)
        //        {
        //            var sourceConfig = new SourceConfiguration
        //            {
        //                Source = new Source { Group = entry.Key }
        //            };
        //            foreach (var sourceConfiguration in entry.Value)
        //            {
        //                sourceConfig.IsEnabled = sourceConfig.IsEnabled || sourceConfiguration.IsEnabled;
        //                sourceConfig.IsAuthorized = sourceConfig.IsAuthorized || sourceConfiguration.IsAuthorized;
        //                sourceConfig.Credits = sourceConfiguration.Credits;
        //                sourceConfig.IsCandidateDownloadOnViewEnabled = sourceConfiguration.IsCandidateDownloadOnViewEnabled;
        //                sourceConfig.SourceGroupId = sourceConfiguration.SourceGroupId;
        //            }
        //            newSourceConfigurations.Add(sourceConfig);
        //        }
        //        newSourceConfigurations.AddRange(unGroupedSourceConfigurations);
        //        return Task.FromResult(newSourceConfigurations.AsEnumerable());
        //    }
        //    catch (RepositoryException ex)
        //    {
        //        _logger.LogError(ex, "Error occured while grouping sources for userId : `{@UserId}`. Rolling back.", user.Id);
        //        throw;
        //    }
        //}

        public async Task<SearchOptions> GetConfiguredSearchOptionsAsync()
        {
            var user = _userService.GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);

                    await _unitOfWork.CompleteAsync();
                    return null;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting configured search options for userId : `{user.UserGuid}`. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        //public async Task UpdateOrganizationConfigurationAsync(Guid orgGuid, OrganizationConfiguration organizationConfiguration)
        //{
        //    ValidationUtil.NotEmptyGuid(orgGuid, nameof(orgGuid));
        //    ValidateOrganizationConfiguration(organizationConfiguration);
        //    var currentUser = _userService.GetCurrentUser();
        //    using (_unitOfWork)
        //    {
        //        try
        //        {
        //            await _unitOfWork.StartAsync();
        //            if (organizationConfiguration.StackRankType != null)
        //            {
        //                _logger.LogInformation("Updating sourcing type configuration for organization with orgGuid {@OrgId} by userId {@UserId}", orgGuid, currentUser.Id);
        //                await _configurationRepository.UpdateOrganizationSourcingTypeConfigurationAsync(orgGuid, organizationConfiguration.StackRankType);
        //                _logger.LogInformation("Updated sourcing type configuration for organization with orgGuid {@OrgId} by userId {@UserId}", orgGuid, currentUser.Id);
        //            }
        //            if (organizationConfiguration.Sources?.Any() == true)
        //            {
        //                foreach (var sourceConfiguration in organizationConfiguration.Sources)
        //                {
        //                    _logger.LogInformation("Updating {@Portal} active status for organization with orgGuid {@OrgId} by userId {@UserId}", sourceConfiguration.Source.Name, orgGuid, currentUser.Id);
        //                    await _configurationRepository.UpdateOrganizationConfigurationAsync(orgGuid, sourceConfiguration);
        //                    _logger.LogInformation("Updated {@Portal} active status for organization with orgGuid {@OrgId} by userId {@UserId}", sourceConfiguration.Source.Name, orgGuid, currentUser.Id);
        //                }
        //            }
        //            await _unitOfWork.CompleteAsync();
        //            return;
        //        }
        //        catch (RepositoryException ex)
        //        {
        //            _logger.LogError(ex, "Error occured while updating organization configuration for orgGuid : `{@OrgId}`. Rolling back.", orgGuid);
        //            _unitOfWork.Rollback();
        //            throw;
        //        }
        //    }
        //}

        private void ValidateOrganizationConfiguration(OrganizationConfiguration organizationConfiguration)
        {
            ValidationUtil.NotNull(organizationConfiguration);
            //if (organizationConfiguration.Sources?.Any() == true)
            //{
            //    foreach (var sourceConfiguration in organizationConfiguration.Sources)
            //    {
            //        ValidationUtil.NotNull(sourceConfiguration);
            //        ValidationUtil.NotNull(sourceConfiguration.Source);
            //        ValidationUtil.NotEmptyString(sourceConfiguration.Source.Portal, nameof(sourceConfiguration.Source.Portal));
            //    }
            //}
        }

        public async Task UpdateUserConfigurationAsync(UserConfiguration configuration)
        {
            ValidateUserConfiguration(configuration);
            using (_unitOfWork)
            {
                User user = null;
                try
                {
                    await _unitOfWork.StartAsync();
                    user = _userService.GetCurrentUser();
                    ValidateUserAuthorization(configuration, user);
                    _logger.LogInformation($"Open DB connection to update configuration for `userId` : `{user.UserGuid}`");
                    await _configurationRepository.UpdateUserConfigurationAsync(configuration, user.UserGuid);
                    await _unitOfWork.CompleteAsync();
                    return;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while updating user configuration for user {user.UserGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task PatchUserConfigurationAsync(UserConfiguration configuration)
        {
            ValidateUserConfiguration(configuration);
            using (_unitOfWork)
            {
                await _unitOfWork.StartAsync();
                var user = _userService.GetCurrentUser();
                ValidateUserAuthorization(configuration, user);
                _logger.LogInformation($"Open DB connection to patch configuration for `userId` : `{user.UserGuid}`");
                await _configurationRepository.PatchUserConfigurationAsync(configuration, user.UserGuid);
                await _unitOfWork.CompleteAsync();
                return;
            }
        }

        private void ValidateUserConfiguration(UserConfiguration configuration)
        {
            if (configuration.Distance != null && configuration.Distance.Distance <= 0)
                throw new InvalidArgumentException(nameof(configuration.Distance.Distance), configuration.Distance.Distance.ToString(), "Positive Integer");
        }

        private void ValidateUserAuthorization(UserConfiguration configuration, User user)
        {
            if ((configuration.Distance != null || 
                configuration.Features.FirstOrDefault(feature => feature.Name == FeatureName.ShareJob) != null || configuration.Logout != null) && (!user.IsAdmin && !user.HasGodView && !SuperAdminRoleNames.Contains(user.RoleName, StringComparer.OrdinalIgnoreCase)))
            {
                throw new UnauthorizedOperationException("distance, job sharing and auto-logout setting", "update");
            }
        }
    }
}
