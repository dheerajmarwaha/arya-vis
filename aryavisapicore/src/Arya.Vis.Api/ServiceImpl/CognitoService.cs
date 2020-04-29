//using Arya.Vis.Api.Services;
//using Arya.Vis.Core.Services;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Arya.Vis.Api.ServiceImpl
//{
//    public class CognitoService : IIdentityService
//    {
//        private readonly ILogger<CognitoService> _logger;
//        private readonly IAryaIdentityProviderClient _client;
//        private readonly IUserService _userService;
//        private readonly IOrganizationService _organizationService;
//        private readonly CognitoIamCredentials _cognitoIamCredentials;
//        private readonly CognitoClientConfiguration _cognitoClientConfiguration;
//        private const string PulseOrgType = "Pulse";

//        public CognitoService(
//            ILogger<CognitoService> logger,
//            IAryaIdentityProviderClient client,
//            IUserService userService,
//            IOrganizationService organizationService,
//            IOptions<CognitoIamCredentials> cognitoIamCredentials,
//            IOptions<CognitoClientConfiguration> cognitoClientConfiguration
//        )
//        {
//            _logger = logger;
//            _client = client;
//            _userService = userService;
//            _organizationService = organizationService;
//            _cognitoIamCredentials = cognitoIamCredentials.Value;
//            _cognitoClientConfiguration = cognitoClientConfiguration.Value;
//        }

//        public async Task<Organization> CreateOrganizationAsync(OrganizationCreateCommand organizatonCreateCommand)
//        {
//            ValidationUtil.NotNull(organizatonCreateCommand);
//            ValidationUtil.NotEmptyString(organizatonCreateCommand.Name, nameof(organizatonCreateCommand.Name));
//            if (string.IsNullOrWhiteSpace(_cognitoIamCredentials.UserPoolId))
//            {
//                throw new ArgumentException("User Pool Id is not Available");
//            }
//            var currentUser = _userService.GetCurrentUser();
//            var trimmedOrgName = organizatonCreateCommand.Name.Replace(" ", "");
//            organizatonCreateCommand.IdentityProviderIdentifier = trimmedOrgName;
//            var request = new CreateGroupRequest
//            {
//                UserPoolId = _cognitoIamCredentials.UserPoolId,
//                GroupName = trimmedOrgName
//            };
//            _logger.LogInformation("Creating group in cognito with groupname as {@TrimedOrgName} by user {@UserId}", trimmedOrgName, currentUser.Id);
//            await _client.CreateGroupAsync(request);
//            _logger.LogInformation("Created group in cognito with groupname as {@TrimedOrgName} by user {@UserId}", trimmedOrgName, currentUser.Id);
//            var response = await _organizationService.CreateOrganizationAsync(organizatonCreateCommand);
//            return response;
//        }

//        private async Task ValidateCreateUserRequest(int orgId, User user, string password)
//        {
//            ValidationUtil.NotZero(orgId, nameof(orgId));
//            ValidationUtil.NotNull(user);
//            ValidationUtil.NotEmptyString(user.Email, nameof(user.Email));
//            if (!EmailUtil.IsValidEmailFormat(user.Email))
//            {
//                throw new InvalidArgumentException(nameof(user.Email), user.Email, "Email should be in proper format");
//            }
//            ValidationUtil.NotEmptyString(password, nameof(password));
//            try
//            {
//                var existingUser = await _userService.GetUserAsync(user.Email.ToLower());
//                if (existingUser != null)
//                {
//                    throw new ConflictingEntityException("User", existingUser.UserGuid.ToString(), existingUser, "An account with the same email already exists");
//                }
//            }
//            catch (EntityNotFoundException)
//            {
//                return;
//            }
//        }

//        public async Task<User> CreateUserAsync(int orgId, User user, string password)
//        {
//            var currentUser = _userService.GetCurrentUser();
//            await ValidateCreateUserRequest(orgId, user, password);
//            user.Email = user.Email.ToLower();
//            var createUserRequest = new AdminCreateUserRequest
//            {
//                UserPoolId = _cognitoIamCredentials.UserPoolId,
//                MessageAction = MessageActionType.SUPPRESS,
//                Username = user.Email,
//                TemporaryPassword = password,
//                UserAttributes = new List<AttributeType> {
//                new AttributeType {
//                Name = "email_verified",
//                Value = "True"
//                },
//                new AttributeType {
//                Name = "email",
//                Value = user.Email
//                }
//                }
//            };
//            _logger.LogInformation("Creating user in cognito with following emailId {@EmailId} by user {@UserId}", user.Email, currentUser.Id);
//            var cognitoResponse = await _client.AdminCreateUserAsync(createUserRequest);
//            _logger.LogInformation("Created user in cognito with following emailId {@EmailId} by user {@UserId}", user.Email, currentUser.Id);
//            var userGuid = cognitoResponse.User.Attributes.Where(x => x.Name.Equals("sub")).FirstOrDefault().Value;
//            var organizationDetails = await _organizationService.GetOrganizationAsync(orgId);
//            var identityProviderIdentifier = organizationDetails.IdentityProviderIdentifier;
//            var addUserToGroupRequest = new AdminAddUserToGroupRequest
//            {
//                UserPoolId = _cognitoIamCredentials.UserPoolId,
//                Username = user.Email,
//                GroupName = identityProviderIdentifier
//            };
//            _logger.LogInformation("Mapping user to {@Group} group cognito with following emailId {@EmailId} by user {@UserId}", identityProviderIdentifier, user.Email, currentUser.Id);
//            await _client.AdminAddUserToGroupAsync(addUserToGroupRequest);
//            _logger.LogInformation("Mapped user to {@Group} group cognito with following emailId {@EmailId} by user {@UserId}", identityProviderIdentifier, user.Email, currentUser.Id);
//            if (!user.IsActive)
//            {
//                var disableUserRequest = new AdminDisableUserRequest
//                {
//                    UserPoolId = _cognitoIamCredentials.UserPoolId,
//                    Username = user.Email,
//                };
//                _logger.LogInformation("Inactive user creation under group {@Group} with following email {@EmailId} in cognito by user {@UserId}", identityProviderIdentifier, user.Email, currentUser.Id);
//                await _client.AdminDisableUserAsync(disableUserRequest);
//            }
//            user.UserGuid = Guid.Parse(userGuid);
//            var response = await _userService.CreateUserAsync(user);
//            return response;
//        }

//        private async Task ValidateUpdateUserRequestAsync(int userId, User user)
//        {
//            ValidationUtil.NotZero(userId, nameof(userId));
//            ValidationUtil.NotNull(user);
//            ValidationUtil.NotEmptyString(user.Email, nameof(user.Email));
//            var existingUser = await _userService.GetUserAsync(userId);
//            if (!string.Equals(user.Email, existingUser.Email, StringComparison.OrdinalIgnoreCase))
//            {
//                throw new InvalidOperationException("Email update is not allowed");
//            }
//        }

//        public async Task<User> UpdateUserAsync(int userId, User user)
//        {
//            await ValidateUpdateUserRequestAsync(userId, user);
//            var currentUser = _userService.GetCurrentUser();
//            if (user.IsActive)
//            {
//                var enableUserRequest = new AdminEnableUserRequest
//                {
//                    UserPoolId = _cognitoIamCredentials.UserPoolId,
//                    Username = user.Email,
//                };
//                _logger.LogInformation("Updating user status to active in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.Id);
//                await _client.AdminEnableUserAsync(enableUserRequest);
//                _logger.LogInformation("Updated user status to active in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.Id);
//            }
//            else
//            {
//                var disableUserRequest = new AdminDisableUserRequest
//                {
//                    UserPoolId = _cognitoIamCredentials.UserPoolId,
//                    Username = user.Email,
//                };
//                _logger.LogInformation("Updating user status to inactive in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.Id);
//                await _client.AdminDisableUserAsync(disableUserRequest);
//                _logger.LogInformation("Updating user status to inactive in cognito with following email {@EmailId} in cognito by user {@UserId}", user.Email, currentUser.Id);
//            }
//            var response = await _userService.UpdateAsync(userId, user);
//            return response;
//        }

//        private void ValidatePasswordResetRequest(int userId, ResetUserPasswordRequest passwordResetRequest)
//        {
//            ValidationUtil.NotZero(userId, nameof(userId));
//            ValidationUtil.NotNull(passwordResetRequest);
//            ValidationUtil.NotEmptyString(passwordResetRequest.Username, nameof(passwordResetRequest.Username));
//            ValidationUtil.NotEmptyString(passwordResetRequest.Password, nameof(passwordResetRequest.Password));
//        }

//        public async Task ResetUserPasswordAsync(int userId, ResetUserPasswordRequest passwordResetRequest)
//        {
//            ValidatePasswordResetRequest(userId, passwordResetRequest);
//            var currentUser = _userService.GetCurrentUser();
//            var setUserPassowrdRequest = new AdminSetUserPasswordRequest
//            {
//                Password = passwordResetRequest.Password,
//                Permanent = true,
//                Username = passwordResetRequest.Username,
//                UserPoolId = _cognitoIamCredentials.UserPoolId
//            };
//            _logger.LogInformation("Updating user password in cognito with following userId {@UserId} in cognito by user {@UserId}", userId, currentUser.Id);
//            await _client.AdminSetUserPasswordAsync(setUserPassowrdRequest);
//            _logger.LogInformation("Updated user password in cognito with following userId {@UserId} in cognito by user {@UserId}", userId, currentUser.Id);
//        }

//        public async Task VerifyUserAsync(UserVerificationRequest request)
//        {
//            // ? AWS ConfirmSignup Docs https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_ConfirmSignUp.html

//            ValidationUtil.NotNull(request);
//            ValidationUtil.NotEmptyString(request.VerificationCode, nameof(request.VerificationCode));
//            ValidationUtil.NotEmptyGuid(request.Username, nameof(request.Username));
//            try
//            {
//                var secretBytes = Encoding.UTF8.GetBytes(_cognitoClientConfiguration.Secret);
//                var hashMessage = Encoding.UTF8.GetBytes(request.Username + _cognitoClientConfiguration.Id);
//                var hmacSha256Base64 = Convert.ToBase64String((new HMACSHA256(secretBytes)).ComputeHash(hashMessage));
//                await _client.ConfirmSignUpAsync(new ConfirmSignUpRequest
//                {
//                    ClientId = _cognitoClientConfiguration.Id,
//                    SecretHash = hmacSha256Base64,
//                    ConfirmationCode = request.VerificationCode,
//                    Username = Convert.ToString(request.Username)
//                });
//                var unregisteredUser = await _userService.GetUnregisteredUserDetailsAsync(request.Username);
//                await CreateAryaUserAndOrganizationAsync(unregisteredUser);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Confirm signup failed with message", ex.Message);
//                throw;
//            }
//        }

//        private async Task CreateAryaUserAndOrganizationAsync(UnregisteredUser unregisteredUser)
//        {
//            var newOrganization = await _organizationService.CreateOrganizationAsync(new OrganizationCreateCommand
//            {
//                Name = unregisteredUser.Company,
//                IdentityProviderIdentifier = unregisteredUser.Company.Replace(" ", ""),
//                ContactPerson = unregisteredUser.FullName,
//                ContactEmail = unregisteredUser.Email,
//                OrgType = PulseOrgType,
//                IsActive = true,
//                SubscriptionExpiry = DateTime.UtcNow.AddYears(10)
//            });

//            var nameArray = unregisteredUser.FullName.Trim().Split(new[] { ' ' }, 2);
//            await _userService.CreateUserAsync(new User
//            {
//                UserGuid = unregisteredUser.UserGuid.Value,
//                FullName = unregisteredUser.FullName,
//                FirstName = nameArray.Length > 0 ? nameArray.First() : null,
//                LastName = nameArray.Length > 1 ? nameArray.Skip(1).First().Trim() : null,
//                Email = unregisteredUser.Email,
//                OfficePhone = unregisteredUser.Phone,
//                City = unregisteredUser.City,
//                State = unregisteredUser.StateCode,
//                ZipCode = unregisteredUser.PostalCode,
//                Country = unregisteredUser.CountryCode,
//                OrgName = newOrganization.Name,
//                OrgGuid = Convert.ToString(newOrganization.OrgGuid),
//                OrgId = newOrganization.Id,
//                RoleId = newOrganization.Roles.FirstOrDefault(x => x.RoleName.Equals("Recruiter", StringComparison.OrdinalIgnoreCase)).RoleId,
//                IsActive = true
//            });
//        }
//    }
//}
