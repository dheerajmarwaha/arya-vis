using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arya.Vis.Api.RequestModels;
using Arya.Vis.Api.Services;
using Arya.Vis.Container.Web.Models;
using Arya.Vis.Container.Web.Security;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.QueryModels;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Arya.Vis.Api.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.AuthSchemes)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IIdentityService _identityService;

        public UsersController(ILogger<UsersController> logger, IUserService service, IIdentityService identityService)
        {
            _logger = logger;
            _userService = service;
            _identityService = identityService;
        }

        [HttpGet]
        /// <summary>
        /// Get user's information for all the users in the origanistaion
        /// </summary>
        /// <param name="from">int - index</param>
        /// <param name="size">int - no. of users</param>
        /// <returns>UserSearchResult object</returns>
        public async Task<ActionResult<UserSearchResult>> GetAllAsync(int from = 0, int size = 10)
        {
            var query = new UserSearchQuery { Size = size, From = from, };
            return Ok(await _userService.GetAllUsersAsync(query));
        }

        [HttpPost("_search")]
        /// <summary>
        /// Search for users based on their ids or a string sample
        /// </summary>
        /// <param name="query">UserSearchQuery object</param>
        /// <returns>UserSearchResult object</returns>
        public async Task<ActionResult<UserSearchResult>> SearchAsync(UserSearchQuery query)
        {
            return Ok(await _userService.GetAllUsersAsync(query));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("_adminsearch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 503)]
        /// <summary>
        /// Management Users: Search for users based on their ids or a string sample
        /// </summary>
        /// <param name="query">UserSearchQuery object</param>
        /// <returns>UserSearchResult object</returns>
        public async Task<ActionResult<UserSearchResult>> SearchAsync(UserSearchQuery query, Guid orgGuid)
        {
            return Ok(await _userService.GetAllUsersAsync(query, orgGuid));
        }

        [HttpGet("{userGuid}", Name = "GetUser")]
        /// <summary>
        /// Get user information of the user with the specific user Id
        /// </summary>
        /// <param name="userGuid">Guid - user guid of the user required</param>
        /// <returns>User object</returns>
        public async Task<ActionResult<User>> GetAsync(Guid userGuid)
        {
            return Ok(await _userService.GetUserAsync(userGuid));
        }

        [HttpGet("_count")]
        /// <summary>
        /// Get total no. of users in the same organisation as the current user
        /// </summary>
        /// <returns>UserSearchResult object - int count</returns>
        public async Task<ActionResult<UserSearchResult>> GetTotalCountOfUsers()
        {
            return Ok(await _userService.GetTotalCountOfUsers());
        }

        [Route("/api/v3/organizations/{orgGuid}/users/pulse")]
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        /// <summary>
        /// creates an user
        /// </summary>
        /// <param name="user">User object as the request body</param>
        /// <returns>User object as the response</returns>
        public async Task<ActionResult> CreateUserPulseAsync(Guid orgGuid, [FromBody] UserCreateRequest userCreateRequest)
        {
            return CreatedAtRoute("GetUser", new { UserId = userCreateRequest.User.UserGuid }, await _userService.CreateUserAsync(userCreateRequest.User));
        }

        [Route("/api/v3/organizations/{orgGuid}/users")]
        [HttpPost]
        [Authorize(Policy = SecurityPolicy.SuperAdminAccessPolicy)]
        [ApiExplorerSettings(IgnoreApi = true)]
        /// <summary>
        /// creates an user
        /// </summary>
        /// <param name="user">User object as the request body</param>
        /// <returns>User object as the response</returns>
        public async Task<ActionResult> CreateUserAsync(Guid orgGuid, [FromBody] UserCreateRequest userCreateRequest)
        {
            return CreatedAtRoute("GetUser", new { UserId = userCreateRequest.User.UserGuid }, await _identityService.CreateUserAsync(orgGuid, userCreateRequest.User, userCreateRequest.Password));
        }

        [HttpPost]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> CreateUnregisteredUserAsync([FromBody] UnregisteredUserCreateCommand userCreateCommand)
        {
            await _userService.CreateUnregisteredUserAsync(userCreateCommand);
            return NoContent();
        }

        [Route("/api/v3/organizations/{orgGuid}/users")]
        [HttpGet]
        [Authorize(Policy = SecurityPolicy.SuperAdminAccessPolicy)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetUsersByOrgIdAsync(Guid orgGuid)
        {
            return Ok(await _userService.GetUsersByOrgGuidAsync(orgGuid));
        }

        [Route("/api/v3/organizations/{orgGuid}/user_ids")]
        [HttpGet]
        [Authorize(Policy = SecurityPolicy.SuperAdminAccessPolicy)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetTotalUserIdsByOrgIdAsync(Guid orgGuid)
        {
            return Ok(await _userService.GetUserGuidsByOrgGuidAsync(orgGuid));
        }

        [HttpPut("{userGuid}")]
        [Authorize(Policy = SecurityPolicy.SuperAdminAccessPolicy)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> UpdateAsync(Guid userGuid, [FromBody] User user)
        {
            return Ok(await _identityService.UpdateUserAsync(userGuid, user));
        }

        [HttpDelete("{userGuid}")]
        [Authorize(Policy = SecurityPolicy.SuperAdminAccessPolicy)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> DeleteUserAsync(Guid userGuid)
        {
            await _userService.DeleteUserAsync(userGuid);
            return NoContent();
        }

        [HttpPut("{userGuid}/password")]
        [Authorize(Policy = SecurityPolicy.SuperAdminAccessPolicy)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> ResetUserPasswordAsync(Guid userGuid, [FromBody] ResetUserPasswordRequest passwordResetRequest)
        {
            await _identityService.ResetUserPasswordAsync(userGuid, passwordResetRequest);
            return Ok();
        }

        [HttpPost("_verify")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> VerifyUserAsync([FromBody] UserVerificationRequest request)
        {
            await _identityService.VerifyUserAsync(request);
            return NoContent();
        }
    }
}