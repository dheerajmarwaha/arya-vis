using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Arya.Exceptions;
using Arya.Vis.Container.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.IdentityProviders
{
    public class AryaCognitoIdentityProviderClient : IAryaIdentityProviderClient
    {
        private readonly AmazonCognitoIdentityProviderClient _client;

        public AryaCognitoIdentityProviderClient(AmazonCognitoIdentityProviderClient client)
        {
            _client = client;
        }
        public async Task<AdminAddUserToGroupResponse> AdminAddUserToGroupAsync(AdminAddUserToGroupRequest request)
        {
            return await _client.AdminAddUserToGroupAsync(request);
        }

        public async Task<AdminCreateUserResponse> AdminCreateUserAsync(AdminCreateUserRequest request)
        {
            try
            {
                return await _client.AdminCreateUserAsync(request);
            }
            catch (UsernameExistsException ex)
            {
                throw new ConflictingEntityException("User", request.Username, null, ex.Message);
            }
            catch (InvalidPasswordException e)
            {
                throw new InvalidArgumentException("Password", request.TemporaryPassword, e.Message);
            }
        }

        public async Task<AdminDisableUserResponse> AdminDisableUserAsync(AdminDisableUserRequest request)
        {
            try
            {
                return await _client.AdminDisableUserAsync(request);
            }
            catch (Amazon.CognitoIdentityProvider.Model.UserNotFoundException)
            {
                throw new Arya.Exceptions.UserNotFoundException();
            }
        }

        public async Task<AdminEnableUserResponse> AdminEnableUserAsync(AdminEnableUserRequest request)
        {
            try
            {
                return await _client.AdminEnableUserAsync(request);
            }
            catch (Amazon.CognitoIdentityProvider.Model.UserNotFoundException)
            {
                throw new Arya.Exceptions.UserNotFoundException();
            }
        }

        public async Task<AdminSetUserPasswordResponse> AdminSetUserPasswordAsync(AdminSetUserPasswordRequest request)
        {
            try
            {
                return await _client.AdminSetUserPasswordAsync(request);
            }
            catch (InvalidPasswordException e)
            {
                throw new InvalidArgumentException("Password", request.Password, e.Message);
            }
            catch (Amazon.CognitoIdentityProvider.Model.UserNotFoundException)
            {
                throw new Arya.Exceptions.UserNotFoundException();
            }
        }

        public async Task<ConfirmSignUpResponse> ConfirmSignUpAsync(ConfirmSignUpRequest request)
        {
            try
            {
                return await _client.ConfirmSignUpAsync(request);
            }
            catch (CodeMismatchException ex)
            {
                throw new IdentityProviderCodeConfirmationFailedException("CODE_MISMATCH", ex);
            }
            catch (ExpiredCodeException ex)
            {
                throw new IdentityProviderCodeConfirmationFailedException("CODE_EXPIRED", ex);
            }
            catch (Amazon.CognitoIdentityProvider.Model.LimitExceededException ex)
            {
                throw new IdentityProviderCodeConfirmationFailedException("LIMIT_EXCEEDED", ex);
            }
            catch (TooManyFailedAttemptsException ex)
            {
                throw new IdentityProviderCodeConfirmationFailedException("TOO_MANY_FAILED_ATTEMPTS", ex);
            }
            catch (TooManyRequestsException ex)
            {
                throw new IdentityProviderCodeConfirmationFailedException("TOO_MANY_REQUESTS", ex);
            }
            catch (Amazon.CognitoIdentityProvider.Model.UserNotFoundException ex)
            {
                throw new IdentityProviderCodeConfirmationFailedException("USER_NOT_FOUND", ex);
            }
            catch (NotAuthorizedException ex)
            {
                // 💩 Since Cognito doesn't provide specific exception for confirmed user, so checking based on message
                if (ex.Message.Contains("Current status is CONFIRMED", StringComparison.OrdinalIgnoreCase))
                {
                    throw new IdentityProviderCodeConfirmationFailedException("USER_ALREADY_CONFIRMED", ex);
                }
                throw;
            }
        }

        public async Task<CreateGroupResponse> CreateGroupAsync(CreateGroupRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
