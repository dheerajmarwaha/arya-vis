using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Arya.Exceptions;
using Arya.Storage;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Utils;
using Arya.Vis.Core.ViewModels;

namespace Arya.Vis.Repository
{
    public class UserRepository : BaseRepository, IUserRepository {
        public UserRepository(ISqlProvider sqlProvider) : base(sqlProvider) { }

        public void ValidateUsersMetadataQuery(SimpleSearchQuery query) {

        }

        public async Task<User> FetchAsync(string email)
        {
            var command = SqlProvider.CreateCommand(Routines.GetUserDetailsByEmail);
            SqlProvider.AddParameterWithValue(command, "vLoginName", email);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                return await ReadAsync(reader);
            }
        }

        public async Task<User> FetchAsync(Guid userGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetUserDetailsByGuid);
            SqlProvider.AddParameterWithValue(command, "vUserGuid", userGuid);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                return await ReadAsync(reader);
            }
        }

        public async Task<UserSearchResult> GetAllUsersAsync(UserSearchQuery query, Guid orgGuid)
        {
            ValidateUsersMetadataQuery(query);
            var command = SqlProvider.CreateCommand(Routines.GetAllUsers);
            using (command)
            {
                AddGetUsersQueryParameters(command, query, orgGuid);
                using (var reader = await SqlProvider.ExecuteReaderAsync(command))
                {
                    var users = new UserSearchResult()
                    {
                        Users = await ReadUsersAsync(reader)
                    };

                    await SqlProvider.ReadAsync(reader);
                    users.Total = (int)await SqlProvider.GetFieldValueAsync<Int64>(reader, "count");
                    return users;
                }
            }
        }

        public async Task<int> GetTotalCountOfUsers(Guid orgGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetUserCount);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", orgGuid);
            using (command)
            {
                return Convert.ToInt32(await SqlProvider.ExecuteScalarAsync(command));
            }
        }

        public async Task<Guid> CreateUserAsync(User user)
        {
            var command = SqlProvider.CreateCommand(Routines.CreateUser);
            using (command)
            {
                AddUserParameters_Insert(command, user);
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                using (reader)
                {
                    if (await SqlProvider.ReadAsync(reader))
                    {
                        var UserGuId = await SqlProvider.GetFieldValueAsync<Guid>(reader, "UserGuId");
                        return UserGuId;
                    }
                    throw new RepositoryException("UserRepository", "No resultset returned on user creation");
                }
            }
        }

        public async Task<Guid> UpdateAsync(Guid userGuid, User user)
        {
            var command = SqlProvider.CreateCommand(Routines.UpdateUser);
            using (command)
            {
                AddUserParameters_Update(command, user);
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                using (reader)
                {
                    if (await SqlProvider.ReadAsync(reader))
                    {
                        var UserGuId = await SqlProvider.GetFieldValueAsync<Guid>(reader, "UserGuId");
                        return UserGuId;
                    }
                    throw new RepositoryException("UserRepository", "No resultset returned on user update");
                }
            }
        }

        public async Task<IEnumerable<User>> GetUsersByOrgGuidAsync(Guid orgGuid) {
            var command = SqlProvider.CreateCommand(Routines.GetUsersByOrgGuid);
            using(command) {
                SqlProvider.AddParameterWithValue(command, "vOrgGuid", orgGuid);
                using(var reader = await SqlProvider.ExecuteReaderAsync(command)) {
                    var users = await ReadUsersAsync(reader);
                    return users;
                }
            }
        }

        public async Task<IEnumerable<Guid>> GetUserGuidsByOrgGuidAsync(Guid orgGuid) {
            var command = SqlProvider.CreateCommand(Routines.GetUserGuidsByOrgGuid);
            using(command) {
                SqlProvider.AddParameterWithValue(command, "vOrgGuid", orgGuid);
                using (var reader = await SqlProvider.ExecuteReaderAsync(command))
                {
                    var userguids = await ReadUserGuidsAsync(reader);
                    return userguids;
                }
            }
        }

        public async Task<User> GetDefaultAdminUserAsync(Guid orgGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetDefaultAdminUser);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", orgGuid);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                return await ReadAsync(reader);
            }
        }

        public async Task DeleteUserAsync(Guid userGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.DeleteUser);
            SqlProvider.AddParameterWithValue(command, "vUserGuid", userGuid);
            using (command)
            {
                await SqlProvider.ExecuteScalarAsync(command);
            }
        }

        public async Task CreateUnregisteredUserAsync(UnregisteredUserCreateCommand userCreateCommand)
        {
            var command = SqlProvider.CreateCommand(Routines.CreateUnregisteredUser);
            AddCreateUserQueryParams(command, userCreateCommand);
            using (command)
            {
                await SqlProvider.ExecuteNonQueryAsync(command);
            }
        }

        public async Task<UnregisteredUser> GetUnregisteredUserDetailsAsync(Guid userGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetUnregisteredUserDetails);
            SqlProvider.AddParameterWithValue(command, "vUserGuid", userGuid);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                return await ReadUnregisteredUserAsync(reader);
            }
        }

       

        private async Task<IList<User>> ReadUsersAsync(DbDataReader reader) {
            var users = ModelUtils<User>.CreateObjects(await SqlProvider.ExecuteDataReaderAsync(reader));           
            return users;
        }

        private async Task<IList<Guid>> ReadUserGuidsAsync(DbDataReader reader) {
            var userguids = new List<Guid>();
            while (await SqlProvider.ReadAsync(reader)) {
                var userguid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "UserId");
                userguids.Add(userguid);
            }
            return userguids;
        }

        private async Task<User> ReadAsync(DbDataReader reader) {
            using(reader) {
                return ModelUtils<User>.CreateObject(await SqlProvider.ExecuteDataReaderAsync(reader));
            }
        }

        private void AddCreateUserQueryParams(DbCommand command, UnregisteredUserCreateCommand userCreateCommand) {
            SqlProvider.AddParameterWithValue(command, "vUserGuid", userCreateCommand.UserGuid);
            SqlProvider.AddParameterWithValue(command, "vName", userCreateCommand.FullName);
            SqlProvider.AddParameterWithValue(command, "vEmail", userCreateCommand.Email);
            SqlProvider.AddParameterWithValue(command, "vPhone", userCreateCommand.Phone);
            SqlProvider.AddParameterWithValue(command, "vCity", userCreateCommand.City);
            SqlProvider.AddParameterWithValue(command, "vStateCode", userCreateCommand.StateCode);
            SqlProvider.AddParameterWithValue(command, "vPostalCode", userCreateCommand.PostalCode);
            SqlProvider.AddParameterWithValue(command, "vCountryCode", userCreateCommand.CountryCode.ToString());
            SqlProvider.AddParameterWithValue(command, "vCompany", userCreateCommand.Company);
            SqlProvider.AddParameterWithValue(command, "vProfileType", userCreateCommand.ProfileType);
            SqlProvider.AddParameterWithValue(command, "vIndustries", userCreateCommand.Industries?.Any() == true ? string.Join(",", userCreateCommand.Industries) : null);
        }



        private async Task<UnregisteredUser> ReadUnregisteredUserAsync(DbDataReader reader) {
            UnregisteredUser user = null;
            using(reader) {
                if (await SqlProvider.ReadAsync(reader)) {
                    user = new UnregisteredUser {
                        UserGuid = await SqlProvider.GetFieldValueAsync<Guid>(reader, "UserGuid"),
                        FullName = await SqlProvider.GetFieldValueAsync<string>(reader, "FullName"),
                        Email = await SqlProvider.GetFieldValueAsync<string>(reader, "Email"),
                        Phone = await SqlProvider.GetFieldValueAsync<string>(reader, "Phone"),
                        City = await SqlProvider.GetFieldValueAsync<string>(reader, "City"),
                        StateCode = await SqlProvider.GetFieldValueAsync<string>(reader, "StateCode"),
                        PostalCode = await SqlProvider.GetFieldValueAsync<string>(reader, "PostalCode"),
                        Company = await SqlProvider.GetFieldValueAsync<string>(reader, "Company")
                    };
                    var countryCode = await SqlProvider.GetFieldValueAsync<string>(reader, "CountryCode");
                    if (!string.IsNullOrWhiteSpace(countryCode)) {
                        user.CountryCode = countryCode;
                    }
                    var industries = await SqlProvider.GetFieldValueAsync<string>(reader, "Industries");
                    if (!string.IsNullOrWhiteSpace(industries)) {
                        user.Industries = industries.Split(',');
                    }
                    var profileType = await SqlProvider.GetFieldValueAsync<string>(reader, "ProfileType");
                    if (!string.IsNullOrWhiteSpace(profileType)) {
                        user.ProfileType = EnumUtils.GetEnum<ProfileType>(profileType, "ProfileType");
                    }
                }
            }
            return user;
        }
        private void AddUserParameters_Insert(DbCommand command, User user)
        {
            SqlProvider.AddParameterWithValue(command, "vUserGuid", user.UserGuid); //This is populated from cognito response - Sub attribute
            SqlProvider.AddParameterWithValue(command, "vFirstName", user.FirstName);
            SqlProvider.AddParameterWithValue(command, "vLastName", user.LastName);
            SqlProvider.AddParameterWithValue(command, "vEmail", user.Email);
            SqlProvider.AddParameterWithValue(command, "vHomePhone", user.HomePhone);
            SqlProvider.AddParameterWithValue(command, "vWorkPhone", user.WorkPhone);
            SqlProvider.AddParameterWithValue(command, "vRoleGroupID", user.RoleGroupID);
            SqlProvider.AddParameterWithValue(command, "vIsActive", user.IsActive);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", user.OrgGuid);
            SqlProvider.AddParameterWithValue(command, "vVendorId", user.VendorId);
            SqlProvider.AddParameterWithValue(command, "vCity", user.City);
            SqlProvider.AddParameterWithValue(command, "vState", user.State);
            SqlProvider.AddParameterWithValue(command, "vCountry", user.Country);
            SqlProvider.AddParameterWithValue(command, "vZipCode", user.ZipCode);
            SqlProvider.AddParameterWithValue(command, "vCreatedByGuId", user.CreatedByGuId);
        }
        private void AddUserParameters_Update(DbCommand command, User user)
        {
            SqlProvider.AddParameterWithValue(command, "vUserGuid", user.UserGuid);
            SqlProvider.AddParameterWithValue(command, "vFirstName", user.FirstName);
            SqlProvider.AddParameterWithValue(command, "vLastName", user.LastName);
            SqlProvider.AddParameterWithValue(command, "vEmail", user.Email);
            SqlProvider.AddParameterWithValue(command, "vHomePhone", user.HomePhone);
            SqlProvider.AddParameterWithValue(command, "vWorkPhone", user.WorkPhone);
            SqlProvider.AddParameterWithValue(command, "vRoleGroupID", user.RoleGroupID);
            SqlProvider.AddParameterWithValue(command, "vIsActive", user.IsActive);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", user.OrgGuid);
            SqlProvider.AddParameterWithValue(command, "vModifiedByGuId", user.ModifiedByGuId);

        }

        private void AddGetUsersQueryParameters(DbCommand command, UserSearchQuery query, Guid orgGuid)
        {
            if (query.UserGuids != null)
            {
                var userguids = new List<Guid>(query.UserGuids);
                if (userguids.Count > 0) { query.Size = userguids.Count; }
            }
            SqlProvider.AddParameterWithValue(command, "vOrgGuId", orgGuid);
            SqlProvider.AddParameterWithValue(command, "vFrom", query.From);
            SqlProvider.AddParameterWithValue(command, "vSize", query.Size);
            SqlProvider.AddParameterWithValue(command, "vSearchTerm", query.SearchTerm);
            SqlProvider.AddParameterWithValue(command, "vVendorIds", query.VendorIds == null ? null : string.Join(",", query.VendorIds));
            SqlProvider.AddParameterWithValue(command, "vNames", query.Names == null ? null : string.Join(",", query.Names));
            SqlProvider.AddParameterWithValue(command, "vEmails", query.Emails == null ? null : string.Join(",", query.Emails));
            SqlProvider.AddParameterWithValue(command, "vUserGuids", query.UserGuids == null ? null : string.Join(",", query.UserGuids));
        }

    }
}