//using Arya.Vis.Core.Repositories;
//using Arya.Vis.Core.Services;
//using Microsoft.Extensions.Logging;

//namespace Arya.Vis.Core.ServicesImpl
//{
//    public class UserService : IUserService {
//        private readonly ILogger<UserService> _logger;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IUserRepository _userRepository;
//        private readonly IEventBus _eventBus;
//        private User _user;
//        public UserService(
//            ILogger<UserService> logger,
//            IUnitOfWork unitOfWork,
//            IUserRepository userRepository,
//            IEventBus eventBus) {
//            _logger = logger;
//            _unitOfWork = unitOfWork;
//            _userRepository = userRepository;
//            _eventBus = eventBus;
//        }

//        public User GetCurrentUser() {

//            if (_user == null) {

//                throw new UserNotFoundException();

//            }
//            return _user;
//        }

//        public async Task<UserSearchResult> GetAllUsersAsync(UserSearchQuery query, int? orgId) {
//            orgId = orgId ?? GetCurrentUser().OrgId;
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
//                    var userSearchResult = await _userRepository.GetAllUsersAsync(query, (int) orgId);
//                    await _unitOfWork.CompleteAsync();
//                    return userSearchResult;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while getting getting userSearchResult. Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<UserSearchResult> GetTotalCountOfUsers() {
//            var orgId = GetCurrentUser().OrgId;
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
//                    var count = await _userRepository.GetTotalCountOfUsers(orgId);
//                    await _unitOfWork.CompleteAsync();
//                    return new UserSearchResult { Total = count };
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while getting total count of users. Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<User> GetUserAsync(int userId) {
//            ValidationUtil.NotZero(userId, nameof(User));
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
//                    var user = await _userRepository.GetUserAsync(userId);
//                    await _unitOfWork.CompleteAsync();
//                    if (user == null)
//                        throw new EntityNotFoundException(nameof(User), userId.ToString());
//                    return user;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while getting {userId}. Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<User> GetUserAsync(Guid userGuid) {
//            ValidationUtil.NotEmptyGuid(userGuid, nameof(User));
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
//                    var user = await _userRepository.FetchAsync(userGuid);
//                    if (user == null)
//                        throw new EntityNotFoundException(nameof(User), userGuid.ToString());
//                    await _unitOfWork.CompleteAsync();
//                    return user;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, "Error occured while getting user by Guid {@UserGuid}. Rolling back...", userGuid);
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<User> GetUserAsync(string email) {
//            ValidationUtil.NotEmptyString(email, nameof(User));
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
//                    var user = await _userRepository.FetchAsync(email);
//                    await _unitOfWork.CompleteAsync();
//                    if (user == null)
//                        throw new EntityNotFoundException(nameof(User), email);
//                    return user;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, "Error occured while getting user by Email {@Email}. Rolling back...", email);
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task LoadCurrentUserAsync(string email, bool allowInactive = false) {
//            var user = await GetUserAsync(email);
//            if (user.IsActive || allowInactive) {
//                _user = user;
//            }
//        }

//        public async Task LoadCurrentUserAsync(Guid id, bool allowInactive = false) {
//            var user = await GetUserAsync(id);
//            if (user.IsActive || allowInactive) {
//                _user = user;
//            }
//        }

//        public async Task LoadCurrentUserAsync(int userId, bool allowInactive = false) {
//            var user = await GetUserAsync(userId);
//            if (user.IsActive || allowInactive) {
//                _user = user;

//            }
//        }

//        private void ValidateUserId(int userId) {
//            if (userId < 1) {
//                throw new InvalidArgumentException(nameof(userId), userId.ToString(), "non zero and non negative integer");
//            }
//        }

//        public void SetCurrentUser(User user, bool allowInactive = false) {
//            if (user.IsActive || allowInactive) {
//                _user = user;
//            }
//        }

//        public async Task<User> CreateUserAsync(User user) {
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    _logger.LogInformation("Creating user with following deatils {@User}", user);
//                    var newUserId = await _userRepository.CreateUserAsync(user);
//                    var newUser = await _userRepository.GetUserAsync(newUserId);
//                    await _unitOfWork.CompleteAsync();
//                    await _eventBus.Publish(new UserCreatedEvent {
//                        User = newUser
//                    });
//                    return newUser;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, "Error occured while creating user with command {@User}. Rolling back.", user);
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<User> UpdateAsync(int userId, User user) {
//            var currentUser = GetCurrentUser();
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    _logger.LogInformation("Updating user with following deatils {@User} by user {@UserId}", user, currentUser.Id);
//                    var updatedUserId = await _userRepository.UpdateAsync(userId, user);
//                    var updatedUser = await _userRepository.GetUserAsync(updatedUserId);
//                    _logger.LogInformation("Updated user with following deatils {@User} by user {@UserId}", user, currentUser.Id);
//                    await _unitOfWork.CompleteAsync();
//                    return updatedUser;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, "Error occured while updating user with userId:{@UserId} and user info :{@User}. Rolling back.", userId, user);
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<IEnumerable<User>> GetUsersByOrgIdAsync(int orgId) {
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    var users = await _userRepository.GetUsersByOrgIdAsync(orgId);
//                    await _unitOfWork.CompleteAsync();
//                    return users;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while getting users for org {orgId}.Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public bool IsCurrentUserSet() {
//            return _user != null;
//        }

//        public async Task<User> GetDefaultAdminUserAsync(int orgId) {
//            ValidationUtil.NotZero(orgId, nameof(OrganizationMetadata));
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync(IsolationLevel.ReadUncommitted);
//                    var user = await _userRepository.GetDefaultAdminUserAsync(orgId);
//                    await _unitOfWork.CompleteAsync();
//                    return user;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, "Error occured while getting default admin user for org {@OrgId}. Rolling back...", orgId);
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task DeleteUserAsync(int userId) {
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    await _userRepository.DeleteUserAsync(userId);
//                    await _unitOfWork.CompleteAsync();
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while deleting user with userId {userId}.Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task<IEnumerable<int>> GetUserIdsByOrgIdAsync(int orgId) {
//            ValidationUtil.NotZero(orgId, nameof(orgId));
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    var userIds = await _userRepository.GetUserIdsByOrgIdAsync(orgId);
//                    await _unitOfWork.CompleteAsync();
//                    return userIds;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while getting userIds for org {orgId}.Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        public async Task CreateUnregisteredUserAsync(UnregisteredUserCreateCommand userCreateCommand) {
//            ValidateUnregisteredUserCreateCommand(userCreateCommand);
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    await _userRepository.CreateUnregisteredUserAsync(userCreateCommand);
//                    await _unitOfWork.CompleteAsync();
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while creating unregistered user with userGuid {userCreateCommand.UserGuid}. Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }

//        private void ValidateUnregisteredUserCreateCommand(UnregisteredUserCreateCommand userCreateCommand) {
//            ValidationUtil.NotNull(userCreateCommand);
//            ValidationUtil.NotEmptyString(userCreateCommand.FullName, nameof(userCreateCommand.FullName));
//            ValidationUtil.NotEmptyString(userCreateCommand.Email, nameof(userCreateCommand.Email));
//        }

//        public async Task<UnregisteredUser> GetUnregisteredUserDetailsAsync(Guid id) {
//            ValidationUtil.NotEmptyGuid(id, nameof(id));
//            using(_unitOfWork) {
//                try {
//                    await _unitOfWork.StartAsync();
//                    var user = await _userRepository.GetUnregisteredUserDetailsAsync(id);
//                    if (user == null) throw new EntityNotFoundException(nameof(UnregisteredUser), id.ToString());
//                    await _unitOfWork.CompleteAsync();
//                    return user;
//                } catch (RepositoryException ex) {
//                    _logger.LogError(ex, $"Error occured while getting unregistered user details for userId: {id}.Rolling back.");
//                    _unitOfWork.Rollback();
//                    throw;
//                }
//            }
//        }
//    }
//}