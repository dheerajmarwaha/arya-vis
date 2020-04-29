using Arya.Vis.Api.RequestModels;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Services
{
    public interface IIdentityService
    {
        Task<Organization> CreateOrganizationAsync(OrganizationCreateCommand organizationCreateCommand);
        Task ResetUserPasswordAsync(Guid userGuid, ResetUserPasswordRequest passwordResetRequest);
        Task<User> CreateUserAsync(Guid OrgGuid, User user, string password);
        Task<User> UpdateUserAsync(Guid userGuid, User user);
        Task VerifyUserAsync(UserVerificationRequest request);
    }
}
