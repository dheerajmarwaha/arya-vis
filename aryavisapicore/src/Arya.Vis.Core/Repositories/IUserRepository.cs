using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.ViewModels;

namespace Arya.Vis.Core.Repositories
{
    public interface IUserRepository {
        Task<User> FetchAsync(string email);
        Task<User> FetchAsync(Guid userGuid);
        Task<UserSearchResult> GetAllUsersAsync(UserSearchQuery query, Guid orgGuid);
        Task<int> GetTotalCountOfUsers(Guid orgGuid);
        Task<Guid> CreateUserAsync(User user);
        Task<Guid> UpdateAsync(Guid userGuid, User user);
        Task<IEnumerable<User>> GetUsersByOrgIdAsync(Guid orgGuid);
        Task<IEnumerable<Guid>> GetUserGuidsByOrgIdAsync(Guid orgGuid);
        Task<User> GetDefaultAdminUserAsync(Guid orgGuid);
        Task DeleteUserAsync(Guid userGuid);
        Task CreateUnregisteredUserAsync(UnregisteredUserCreateCommand userCreateCommand);
        Task<UnregisteredUser> GetUnregisteredUserDetailsAsync(Guid userGuid);
    }
}