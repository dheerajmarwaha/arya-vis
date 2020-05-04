using Arya.Exceptions;
using Arya.Vis.Container.Web.Services;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.ServiceImpl
{
    public class OrgApiClientService : IOrgApiClientService
    {

        private readonly IOrganizationService _organizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrgApiClientRepository _orgApiClientRepository;

        public OrgApiClientService(
            IOrganizationService organizationService,
            IUnitOfWork unitOfWork,
            IOrgApiClientRepository orgApiClientRepository
        )
        {
            _organizationService = organizationService;
            _unitOfWork = unitOfWork;
            _orgApiClientRepository = orgApiClientRepository;
        }
        public async Task<Organization> GetAsync(string clientId)
        {
            ValidationUtil.NotEmptyString(clientId, nameof(clientId));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var orgGuid = await _orgApiClientRepository.GetOrgGuidAsync(clientId);
                    if (orgGuid == null)
                    {
                        await _unitOfWork.CompleteAsync();
                        throw new EntityNotFoundException(nameof(Organization), clientId);
                    }
                    var organization = await _organizationService.GetOrganizationAsync(orgGuid.Value);
                    await _unitOfWork.CompleteAsync();
                    return organization;
                }
                catch (RepositoryException)
                {
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
