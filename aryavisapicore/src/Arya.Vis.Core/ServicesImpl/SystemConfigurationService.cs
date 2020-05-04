using Arya.Exceptions;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.ServicesImpl
{
    public class SystemConfigurationService : ISystemConfigurationService
    {
        private readonly ILogger<SystemConfigurationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        public SystemConfigurationService(ILogger<SystemConfigurationService> logger, IUnitOfWork unitOfWork, ISystemConfigurationRepository systemConfigurationRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _systemConfigurationRepository = systemConfigurationRepository;
        }

        public async Task<string> GetValueAsync(string key)
        {
            ValidateConfigurationKey(key);
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var value = await _systemConfigurationRepository.GetValueAsync(key);
                    await _unitOfWork.CompleteAsync();
                    return value;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting system configuration value. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task LoadAsync()
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    await _systemConfigurationRepository.LoadAsync();
                    await _unitOfWork.CompleteAsync();
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while loading system configuartion. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private void ValidateConfigurationKey(string configurationKey)
        {
            if (string.IsNullOrWhiteSpace(configurationKey))
            {
                throw new InvalidArgumentException(nameof(configurationKey), configurationKey, "non null or non empty configuration key");
            }
        }
    }
}
