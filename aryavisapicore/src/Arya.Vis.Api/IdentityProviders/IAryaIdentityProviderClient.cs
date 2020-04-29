using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.IdentityProviders
{
    public interface IAryaIdentityProviderClient
    {
        Task<CreateGroupResponse> CreateGroupAsync(CreateGroupRequest request);
        Task<AdminCreateUserResponse> AdminCreateUserAsync(AdminCreateUserRequest request);
        Task<AdminAddUserToGroupResponse> AdminAddUserToGroupAsync(AdminAddUserToGroupRequest request);
        Task<AdminSetUserPasswordResponse> AdminSetUserPasswordAsync(AdminSetUserPasswordRequest request);
        Task<AdminEnableUserResponse> AdminEnableUserAsync(AdminEnableUserRequest request);
        Task<AdminDisableUserResponse> AdminDisableUserAsync(AdminDisableUserRequest request);
        Task<ConfirmSignUpResponse> ConfirmSignUpAsync(ConfirmSignUpRequest request);
    }
}
