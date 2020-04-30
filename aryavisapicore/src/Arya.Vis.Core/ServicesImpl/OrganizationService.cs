using Arya.Exceptions;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Core.ServicesImpl
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrganizationRepository _organizationRepository;
        //private readonly IEventBus _eventBus;

        public OrganizationService(
            ILogger<OrganizationService> logger,
            IUserService userService,
            IUnitOfWork unitOfWork,
            IOrganizationRepository organizationRepository
            //IEventBus eventBus

        )
        {
            _logger = logger;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _organizationRepository = organizationRepository;
            //_eventBus = eventBus;

        }

        public async Task<Organization> GetOrganizationAsync(Guid orgGuid)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var org = await _organizationRepository.GetOrganizationAsync(orgGuid);
                    await _unitOfWork.CompleteAsync();
                    return org;
                }
                catch (RepositoryException e)
                {
                    _logger.LogError(e, "Error occurred while fetching organization information for orgId: {@OrgId}", orgGuid);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<Organization> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    _logger.LogInformation("Creating organization with the following details {@OrganizationCreateCommand}", organizationCreateCommand);
                    var createdOrgGuid = await _organizationRepository.CreateOrganizationAsync(organizationCreateCommand);
                    var createdOrg = await _organizationRepository.GetOrganizationAsync(createdOrgGuid);
                    //await _eventBus.Publish(new OrganizationCreatedEvent
                    //{
                    //    Organization = createdOrg
                    //});
                    await _unitOfWork.CompleteAsync();
                    return createdOrg;
                }
                catch (RepositoryException e)
                {
                    _logger.LogError(e, "Error occurred while creating org with command: {@OrganizationCreateCommand}", organizationCreateCommand);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<Organization> UpdateAsync(Guid orgGuid, OrganizationCreateCommand organizationCreateCommand)
        {
            var user = _userService.GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    _logger.LogInformation("Updating organization with the following details {@OrganizationCreateCommand} by user {@UserGuid}", organizationCreateCommand, user.UserGuid);
                    var updatedOrgGuid = await _organizationRepository.UpdateAsync(orgGuid, organizationCreateCommand);
                    var updatedOrg = await _organizationRepository.GetOrganizationAsync(updatedOrgGuid);
                    _logger.LogInformation("Updated organization with the following details {@UpdatedOrg} by user {@UserGuid}", updatedOrg, user.UserGuid);
                    await _unitOfWork.CompleteAsync();
                    return updatedOrg;
                }
                catch (RepositoryException e)
                {
                    _logger.LogError(e, "Error occurred while updating organization information for org {@OrgId} with command:{@OrganizationCreateCommand}", orgGuid, organizationCreateCommand);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<OrganizationMetadataSearchResult> GetOrganizationsMetadataAsync(OrganizationsMetadataQuery organizationMetadataQuery)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var organizationsSearchResult = await _organizationRepository.GetOrganizationsMetadataAsync(organizationMetadataQuery);
                    await _unitOfWork.CompleteAsync();
                    return organizationsSearchResult;
                }
                catch (RepositoryException e)
                {
                    _logger.LogError(e, "Error occurred while fetching organizations list with query {@Query}", organizationMetadataQuery);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<IDictionary<Guid, OrganizationStat>> GetBulkOrganizationStatsAsync(IEnumerable<Guid> organizationIds)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var organizationStats = await _organizationRepository.GetBulkOrganizationStatsAsync(organizationIds);
                    await _unitOfWork.CompleteAsync();
                    return organizationStats;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while getting bulk organization stats. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteOrganizationAsync(Guid orgGuid)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    await _organizationRepository.DeleteOrganizationAsync(orgGuid);
                    await _unitOfWork.CompleteAsync();
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occurred while updating the organization status to inactive with orgid {@OrgGuid}", orgGuid);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
