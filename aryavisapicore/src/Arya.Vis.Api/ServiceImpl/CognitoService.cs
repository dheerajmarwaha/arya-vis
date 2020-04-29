using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Arya.Exceptions;
using Arya.Vis.Api.Config;
using Arya.Vis.Api.IdentityProviders;
using Arya.Vis.Api.RequestModels;
using Arya.Vis.Api.Services;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.Utils;
using Arya.Vis.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Api.ServiceImpl
{
    public class CognitoService : IIdentityService
    {
        private readonly ILogger<CognitoService> _logger;
        private readonly IAryaIdentityProviderClient _client;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly CognitoIAMCredentials _cognitoIAMCredentials;
        private readonly CognitoClientConfiguration _cognitoClientConfiguration;
        private const string OrgType = "AryaVis";

        public CognitoService(
            ILogger<CognitoService> logger,
            IAryaIdentityProviderClient client,
            IUserService userService,
            IOrganizationService organizationService,
            IOptions<CognitoIAMCredentials> cognitoIamCredentials,
            IOptions<CognitoClientConfiguration> cognitoClientConfiguration
        )
        {
            _logger = logger;
            _client = client;
            _userService = userService;
            _organizationService = organizationService;
            _cognitoIAMCredentials = cognitoIamCredentials.Value;
            _cognitoClientConfiguration = cognitoClientConfiguration.Value;
        }

        public async Task<Organization> CreateOrganizationAsync(OrganizationCreateCommand organizatonCreateCommand)
        {
            ValidationUtil.NotNull(organizatonCreateCommand);
            ValidationUtil.NotEmptyString(organizatonCreateCommand.OrganizationName, nameof(organizatonCreateCommand.OrganizationName));
            if (string.IsNullOrWhiteSpace(_cognitoIAMCredentials.UserPoolId))
            {
                throw new ArgumentException("User Pool Id is not Available");
            }
            
            var currentUser = _userService.GetCurrentUser();
            //Set Audit Details
            organizatonCreateCommand.CreatedByGuId = currentUser.CreatedByGuId;

            var trimmedOrgName = organizatonCreateCommand.OrganizationName.Replace(" ", "");
            organizatonCreateCommand.IdentityProviderIdentifier = trimmedOrgName;

            var request = new CreateGroupRequest
            {
                UserPoolId = _cognitoIAMCredentials.UserPoolId,
                GroupName = trimmedOrgName
            };
            _logger.LogInformation("Creating group in cognito with groupname as {@TrimedOrgName} by user {@UserGuid}", trimmedOrgName, currentUser.UserGuid);
            await _client.CreateGroupAsync(request);
            _logger.LogInformation("Created group in cognito with groupname as {@TrimedOrgName} by user {@UserGuid}", trimmedOrgName, currentUser.UserGuid);
            var response = await _organizationService.CreateOrganizationAsync(organizatonCreateCommand);
            return response;
        }

        public async Task<User> CreateUserAsync(Guid orgGuid, User user, string password)
        {
            var currentUser = _userService.GetCurrentUser();
            await ValidateCreateUserRequest(orgGuid, user, password);
            user.Email = user.Email.ToLower();
            user.CreatedByGuId = currentUser.CreatedByGuId;

            var createUserRequest = new AdminCreateUserRequest
            {
                UserPoolId          = _cognitoIAMCredentials.UserPoolId,
                MessageAction       = MessageActionType.SUPPRESS,
                Username            = user.Email,
                TemporaryPassword   = password,
                UserAttributes      = new List<AttributeType> {
                                                                    new AttributeType {
                                                                        Name = "email_verified",
                                                                        Value = "True"
                                                                    },
                                                                    new AttributeType {
                                                                        Name = "email",
                                                                        Value = user.Email
                                                                    }
                                                                }
            };
            _logger.LogInformation("[@time]: Creating user in cognito with following emailId {@EmailId} by user {@UserId}", DateTime.Now, user.Email, currentUser.UserGuid);
            var cognitoResponse = await _client.AdminCreateUserAsync(createUserRequest);
            _logger.LogInformation("[@time]: Created user in cognito with following emailId {@EmailId} by user {@UserId}", DateTime.Now, user.Email, currentUser.UserGuid);

            var userGuid = cognitoResponse.User.Attributes.Where(x => x.Name.Equals("sub")).FirstOrDefault().Value;
            var organizationDetails = await _organizationService.GetOrganizationAsync(orgGuid);
            var identityProviderIdentifier = organizationDetails.IdentityProviderIdentifier;
            var addUserToGroupRequest = new AdminAddUserToGroupRequest
            {
                UserPoolId = _cognitoIAMCredentials.UserPoolId,
                Username = user.Email,
                GroupName = identityProviderIdentifier
            };
            _logger.LogInformation("Mapping user to {@Group} group cognito with following emailId {@EmailId} by user {@UserId}", identityProviderIdentifier, user.Email, currentUser.UserGuid);
            await _client.AdminAddUserToGroupAsync(addUserToGroupRequest);
            _logger.LogInformation("Mapped user to {@Group} group cognito with following emailId {@EmailId} by user {@UserId}", identityProviderIdentifier, user.Email, currentUser.UserGuid);
            if (!user.IsActive)
            {
                var disableUserRequest = new AdminDisableUserRequest
                {
                    UserPoolId = _cognitoIAMCredentials.UserPoolId,
                    Username = user.Email,
                };
                _logger.LogInformation("Inactive user creation under group {@Group} with following email {@EmailId} in cognito by user {@UserId}", identityProviderIdentifier, user.Email, currentUser.UserGuid);
                await _client.AdminDisableUserAsync(disableUserRequest);
            }
            user.UserGuid = Guid.Parse(userGuid);
            var response = await _userService.CreateUserAsync(user);
            return response;
        }

        private async Task ValidateCreateUserRequest(Guid orgGuid, User user, string password)
        {
            ValidationUtil.NotEmptyGuid(orgGuid, nameof(orgGuid));
            ValidationUtil.NotNull(user);
            ValidationUtil.NotEmptyString(user.Email, nameof(user.Email));
            if (!EmailUtil.IsValidEmailFormat(user.Email))
            {
                throw new InvalidArgumentException(nameof(user.Email), user.Email, "Email should be in proper format");
            }
            ValidationUtil.NotEmptyString(password, nameof(password));
            try
            {
                var existingUser = await _userService.GetUserAsync(user.Email.ToLower());
                if (existingUser != null)
                {
                    throw new ConflictingEntityException("User", existingUser.UserGuid.ToString(), existingUser, "An account with the same email already exists");
                }
            }
            catch (EntityNotFoundException)
            {
                return;
            }
        }

        private async Task ValidateUpdateUserRequestAsync(Guid userGuid, User user)
        {
            ValidationUtil.NotEmptyGuid(userGuid, nameof(userGuid));
            ValidationUtil.NotNull(user);
            ValidationUtil.NotEmptyString(user.Email, nameof(user.Email));

            var existingUser = await _userService.GetUserAsync(userGuid);
            if (!string.Equals(user.Email, existingUser.Email, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Email update is not allowed");
            }
        }

        public async Task<User> UpdateUserAsync(Guid userGuid, User user)
        {
            await ValidateUpdateUserRequestAsync(userGuid, user);

            var currentUser = _userService.GetCurrentUser();
            if (user.IsActive)
            {
                var enableUserRequest = new AdminEnableUserRequest
                {
                    UserPoolId = _cognitoIAMCredentials.UserPoolId,
                    Username = user.Email,
                };
                _logger.LogInformation("Updating user status to active in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.UserGuid);
                await _client.AdminEnableUserAsync(enableUserRequest);
                _logger.LogInformation("Updated user status to active in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.UserGuid);
            }
            else
            {
                var disableUserRequest = new AdminDisableUserRequest
                {
                    UserPoolId = _cognitoIAMCredentials.UserPoolId,
                    Username = user.Email,
                };
                _logger.LogInformation("Updating user status to inactive in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.UserGuid);
                await _client.AdminDisableUserAsync(disableUserRequest);
                _logger.LogInformation("Updating user status to inactive in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.UserGuid);
            }
            var response = await _userService.UpdateAsync(userGuid, user);
            return response;
        }

        private void ValidatePasswordResetRequest(Guid userGuid, ResetUserPasswordRequest passwordResetRequest)
        {
            ValidationUtil.NotEmptyGuid(userGuid, nameof(userGuid));
            ValidationUtil.NotNull(passwordResetRequest);
            ValidationUtil.NotEmptyString(passwordResetRequest.Username, nameof(passwordResetRequest.Username));
            ValidationUtil.NotEmptyString(passwordResetRequest.Password, nameof(passwordResetRequest.Password));
        }

        public async Task ResetUserPasswordAsync(Guid userGuid, ResetUserPasswordRequest passwordResetRequest)
        {
            ValidatePasswordResetRequest(userGuid, passwordResetRequest);

            var currentUser = _userService.GetCurrentUser();
            var setUserPassowrdRequest = new AdminSetUserPasswordRequest
            {
                Password = passwordResetRequest.Password,
                Permanent = true,
                Username = passwordResetRequest.Username,
                UserPoolId = _cognitoIAMCredentials.UserPoolId
            };

            _logger.LogInformation("Updating user password in cognito with following userId {@UserId} in cognito by user {@UserId}", userGuid, currentUser.UserGuid);
            await _client.AdminSetUserPasswordAsync(setUserPassowrdRequest);
            _logger.LogInformation("Updated user password in cognito with following userId {@UserId} in cognito by user {@UserId}", userGuid, currentUser.UserGuid);
        }

        public async Task VerifyUserAsync(UserVerificationRequest request)
        {
            // ? AWS ConfirmSignup Docs https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_ConfirmSignUp.html

            ValidationUtil.NotNull(request);
            ValidationUtil.NotEmptyString(request.VerificationCode, nameof(request.VerificationCode));
            ValidationUtil.NotEmptyGuid(request.UserGuid, nameof(request.UserGuid));
            try
            {
                var secretBytes = Encoding.UTF8.GetBytes(_cognitoClientConfiguration.Secret);
                var hashMessage = Encoding.UTF8.GetBytes(request.UserGuid + _cognitoClientConfiguration.Id);
                var hmacSha256Base64 = Convert.ToBase64String((new HMACSHA256(secretBytes)).ComputeHash(hashMessage));
                await _client.ConfirmSignUpAsync(new ConfirmSignUpRequest
                {
                    ClientId = _cognitoClientConfiguration.Id,
                    SecretHash = hmacSha256Base64,
                    ConfirmationCode = request.VerificationCode,
                    Username = Convert.ToString(request.UserGuid)
                });
                var unregisteredUser = await _userService.GetUnregisteredUserDetailsAsync(request.UserGuid);
                await CreateAryaVisUserAndOrganizationAsync(unregisteredUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Confirm signup failed with message", ex.Message);
                throw;
            }
        }

        private async Task CreateAryaVisUserAndOrganizationAsync(UnregisteredUser unregisteredUser)
        {
            var newOrganization = await _organizationService.CreateOrganizationAsync(new OrganizationCreateCommand
            {
                OrganizationName = unregisteredUser.Company,
                IdentityProviderIdentifier = unregisteredUser.Company.Replace(" ", ""),
                ContactPerson = unregisteredUser.FullName,
                ContactEmail = unregisteredUser.Email,
                OrgType = OrgType,
                IsActive = true,
                SubscriptionEndDate = DateTime.UtcNow.AddYears(10)
            });

            var nameArray = unregisteredUser.FullName.Trim().Split(new[] { ' ' }, 2);
            await _userService.CreateUserAsync(new User
            {
                UserGuid = unregisteredUser.UserGuid.Value,
                FullName = unregisteredUser.FullName,
                FirstName = nameArray.Length > 0 ? nameArray.First() : null,
                LastName = nameArray.Length > 1 ? nameArray.Skip(1).First().Trim() : null,
                Email = unregisteredUser.Email,
                WorkPhone = unregisteredUser.Phone,
                City = unregisteredUser.City,
                State = unregisteredUser.StateCode,
                ZipCode = unregisteredUser.PostalCode,
                Country = unregisteredUser.CountryCode,
                OrganizationName = newOrganization.OrganizationName,
                OrgGuid = newOrganization.OrgGuid,
                RoleGroupID = newOrganization.Roles.FirstOrDefault(x => x.RoleName.Equals("Recruiter", StringComparison.OrdinalIgnoreCase)).RoleGroupID,
                IsActive = true
            });
        }
    }
}
