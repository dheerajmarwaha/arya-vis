using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.ViewModels;

namespace Arya.Vis.Core.Services
{
    public interface IUserService {
        Task LoadCurrentUserAsync(string email, bool allowInactive = false);
        Task LoadCurrentUserAsync(System.Guid id, bool allowInactive = false);
        User GetCurrentUser();
        void SetCurrentUser(User user, bool allowInactive = false);
        Task<UserSearchResult> GetAllUsersAsync(UserSearchQuery query, int? orgId = null);
        Task<User> GetUserAsync(int userId);
        Task<User> GetUserAsync(Guid userGuid);
        Task<User> GetUserAsync(string email);
        Task<UserSearchResult> GetTotalCountOfUsers();
        Task LoadCurrentUserAsync(int userId, bool allowInactive = false);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateAsync(int userId, User user);
        Task<User> GetDefaultAdminUserAsync(int OrgId);
        Task<IEnumerable<User>> GetUsersByOrgIdAsync(int orgId);
        Task<IEnumerable<int>> GetUserIdsByOrgIdAsync(int orgId);
        bool IsCurrentUserSet();
        Task DeleteUserAsync(int userId);
        Task CreateUnregisteredUserAsync(UnregisteredUserCreateCommand userCreateCommand);
        Task<UnregisteredUser> GetUnregisteredUserDetailsAsync(Guid id);
    }
}