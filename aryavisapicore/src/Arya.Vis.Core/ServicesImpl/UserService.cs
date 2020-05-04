using Arya.Exceptions;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.Utils;
using Arya.Vis.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Arya.Vis.Core.ServicesImpl
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
       // private readonly IEventBus _eventBus;
        private User _user;
        public UserService(
            ILogger<UserService> logger,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
            //IEventBus eventBus
            )
        {
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            //_eventBus = eventBus;
        }

        public User GetCurrentUser()
        {
            if (_user == null)
            {
                throw new UserNotFoundException();
            }
            return _user;
        }

        public async Task<UserSearchResult> GetAllUsersAsync(UserSearchQuery query, Guid? orgGuid)
        {
            orgGuid = orgGuid ?? GetCurrentUser().OrgGuid;
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var userSearchResult = await _userRepository.GetAllUsersAsync(query, orgGuid.Value);
                    await _unitOfWork.CompleteAsync();
                    return userSearchResult;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting getting userSearchResult. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<UserSearchResult> GetTotalCountOfUsers()
        {
            var orgGuid = GetCurrentUser().OrgGuid;
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var count = await _userRepository.GetTotalCountOfUsers(orgGuid);
                    await _unitOfWork.CompleteAsync();
                    return new UserSearchResult { Total = count };
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting total count of users. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<User> GetUserAsync(Guid userGuid)
        {
            ValidationUtil.NotEmptyGuid(userGuid, nameof(User));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var user = await _userRepository.FetchAsync(userGuid);
                    if (user == null)
                        throw new EntityNotFoundException(nameof(User), userGuid.ToString());
                    await _unitOfWork.CompleteAsync();
                    return user;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while getting user by Guid {@UserGuid}. Rolling back...", userGuid);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            ValidationUtil.NotEmptyString(email, nameof(User));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var user = await _userRepository.FetchAsync(email);
                    await _unitOfWork.CompleteAsync();
                    if (user == null)
                        throw new EntityNotFoundException(nameof(User), email);
                    return user;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while getting user by Email {@Email}. Rolling back...", email);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task LoadCurrentUserAsync(string email, bool allowInactive = false)
        {
            var user = await GetUserAsync(email);
            if (user.IsActive || allowInactive)
            {
                _user = user;
            }
        }

        public async Task LoadCurrentUserAsync(Guid userGuid, bool allowInactive = false)
        {
            var user = await GetUserAsync(userGuid);
            if (user.IsActive || allowInactive)
            {
                _user = user;
            }
        }

        public void SetCurrentUser(User user, bool allowInactive = false)
        {
            if (user.IsActive || allowInactive)
            {
                _user = user;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    _logger.LogInformation("Creating user with following deatils {@User}", user);
                    var newUserId = await _userRepository.CreateUserAsync(user);
                    var newUser = await _userRepository.FetchAsync(newUserId);
                    await _unitOfWork.CompleteAsync();
                    //await _eventBus.Publish(new UserCreatedEvent
                    //{
                    //    User = newUser
                    //});
                    return newUser;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while creating user with command {@User}. Rolling back.", user);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<User> UpdateAsync(Guid userGuid, User user)
        {
            var currentUser = GetCurrentUser();
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    _logger.LogInformation("Updating user with following deatils {@User} by user {@UserId}", user, currentUser.UserGuid);
                    var updatedUserId = await _userRepository.UpdateAsync(userGuid, user);
                    var updatedUser = await _userRepository.FetchAsync(updatedUserId);
                    _logger.LogInformation("Updated user with following deatils {@User} by user {@UserId}", user, currentUser.UserGuid);
                    await _unitOfWork.CompleteAsync();
                    return updatedUser;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while updating user with userGuid:{@UserId} and user info :{@User}. Rolling back.", userGuid, user);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<User>> GetUsersByOrgGuidAsync(Guid orgGuid)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var users = await _userRepository.GetUsersByOrgGuidAsync(orgGuid);
                    await _unitOfWork.CompleteAsync();
                    return users;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting users for org {orgGuid}.Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public bool IsCurrentUserSet()
        {
            return _user != null;
        }

        public async Task<User> GetDefaultAdminUserAsync(Guid orgGuid)
        {
            ValidationUtil.NotEmptyGuid(orgGuid, nameof(OrganizationMetadata));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
                    var user = await _userRepository.GetDefaultAdminUserAsync(orgGuid);
                    await _unitOfWork.CompleteAsync();
                    return user;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, "Error occured while getting default admin user for org {@OrgId}. Rolling back...", orgGuid);
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteUserAsync(Guid userGuid)
        {
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    await _userRepository.DeleteUserAsync(userGuid);
                    await _unitOfWork.CompleteAsync();
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while deleting user with userId {userGuid}.Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Guid>> GetUserGuidsByOrgGuidAsync(Guid orgGuid)
        {
            ValidationUtil.NotEmptyGuid(orgGuid, nameof(orgGuid));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var userIds = await _userRepository.GetUserGuidsByOrgGuidAsync(orgGuid);
                    await _unitOfWork.CompleteAsync();
                    return userIds;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting userIds for org {orgGuid}.Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task CreateUnregisteredUserAsync(UnregisteredUserCreateCommand userCreateCommand)
        {
            ValidateUnregisteredUserCreateCommand(userCreateCommand);
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    await _userRepository.CreateUnregisteredUserAsync(userCreateCommand);
                    await _unitOfWork.CompleteAsync();
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while creating unregistered user with userGuid {userCreateCommand.UserGuid}. Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private void ValidateUnregisteredUserCreateCommand(UnregisteredUserCreateCommand userCreateCommand)
        {
            ValidationUtil.NotNull(userCreateCommand);
            ValidationUtil.NotEmptyString(userCreateCommand.FullName, nameof(userCreateCommand.FullName));
            ValidationUtil.NotEmptyString(userCreateCommand.Email, nameof(userCreateCommand.Email));
        }

        public async Task<UnregisteredUser> GetUnregisteredUserDetailsAsync(Guid userGuid)
        {
            ValidationUtil.NotEmptyGuid(userGuid, nameof(userGuid));
            using (_unitOfWork)
            {
                try
                {
                    await _unitOfWork.StartAsync();
                    var user = await _userRepository.GetUnregisteredUserDetailsAsync(userGuid);
                    if (user == null) throw new EntityNotFoundException(nameof(UnregisteredUser), userGuid.ToString());
                    await _unitOfWork.CompleteAsync();
                    return user;
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error occured while getting unregistered user details for userId: {userGuid}.Rolling back.");
                    _unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}