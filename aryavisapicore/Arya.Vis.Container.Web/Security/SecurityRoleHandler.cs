using Arya.Vis.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Security
{
    public class SecurityRoleHandler : AuthorizationHandler<SecurityRoleRequirement>
    {
        private readonly ILogger<SecurityRoleHandler> _logger;
        private readonly IUserService _userService;

        public SecurityRoleHandler(ILogger<SecurityRoleHandler> logger, 
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(
                                        AuthorizationHandlerContext context,
                                        SecurityRoleRequirement requirement)
        {
            if (context?.User != null)
            {
                var userClaims = context?.User?.Claims;
                var username = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrWhiteSpace(username))
                {
                    var userGuid = Guid.Parse(username);
                    _logger.LogInformation("Getting the user for the current request with id: {@UserGuid}.", userGuid);

                    var user =  _userService.GetCurrentUser();
                    var roleName = user.RoleName;
                    if (roleName == SecurityRoles.GodViewRole)
                    {
                        roleName = SecurityRoles.AdminRole;
                    }
                    if (requirement.SecurityRole != SecurityRoles.AdminRole)
                    {
                        roleName = SecurityRoles.SuperAdminRole;
                    }
                    if (roleName != SecurityRoles.SuperAdminRole && roleName != requirement.SecurityRole)
                    {
                        throw new UnauthorizedAccessException($"User with guid: {user.UserGuid} do not have the {requirement.SecurityRole} rights.");
                    }
                    context.Succeed(requirement);
                }
            }
        }
    }
}
