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
        Task ResetUserPasswordAsync(int userId, ResetUserPasswordRequest passwordResetRequest);
        Task<User> CreateUserAsync(int orgId, User user, string password);
        Task<User> UpdateUserAsync(int userId, User user);
        Task VerifyUserAsync(UserVerificationRequest request);
    }
}
