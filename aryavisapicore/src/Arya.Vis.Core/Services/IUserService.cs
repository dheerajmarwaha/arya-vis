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
        Task LoadCurrentUserAsync(System.Guid userGuid, bool allowInactive = false);
        User GetCurrentUser();
        void SetCurrentUser(User user, bool allowInactive = false);
        Task<UserSearchResult> GetAllUsersAsync(UserSearchQuery query, Guid? orgGuid = null);
        Task<User> GetUserAsync(Guid userGuid);
        Task<User> GetUserAsync(string email);
        Task<UserSearchResult> GetTotalCountOfUsers();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateAsync(Guid userGuid, User user);
        Task<User> GetDefaultAdminUserAsync(Guid orgGuid);
        Task<IEnumerable<User>> GetUsersByOrgGuidAsync(Guid orgGuid);
        Task<IEnumerable<Guid>> GetUserGuidsByOrgGuidAsync(Guid orgGuid);
        bool IsCurrentUserSet();
        Task DeleteUserAsync(Guid userGuid);
        Task CreateUnregisteredUserAsync(UnregisteredUserCreateCommand userCreateCommand);
        Task<UnregisteredUser> GetUnregisteredUserDetailsAsync(Guid userGuid);
    }
}