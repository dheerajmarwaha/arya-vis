using Arya.Exceptions;
using Arya.Vis.Container.Web.Services;
using Arya.Vis.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Middleware
{
    public class UserServiceMiddleware
    {
        private const string ImpersonateHeader = "x-impersonate";
        private const string EmulateHeader = "x-emulate";
        private const string IgnoreUserHeader = "x-ignore-user";

        private readonly ILogger<UserServiceMiddleware> _logger;
        private readonly RequestDelegate _next;

        public UserServiceMiddleware(RequestDelegate next, ILogger<UserServiceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService, IConfigurationService configurationService, IOrgApiClientService orgApiClientService)
        {
            await SetCurrentUserAsync(context, userService, configurationService, orgApiClientService);
            await _next(context);
        }

        private string GetHeaderValue(string headerKey, HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(headerKey))
            {
                throw new InvalidArgumentException(nameof(headerKey), headerKey, "non empty header key");
            }
            if (context == null)
            {
                throw new InvalidArgumentException(nameof(context), null, "non null HttpContext");
            }
            var headerValues = context.Request.Headers[headerKey];
            if (StringValues.IsNullOrEmpty(headerValues))
            {
                return null;
            }
            if (headerValues.Count > 1)
            {
                throw new NotSupportedException($"More than one value for header {headerKey} is not supported");
            }
            return headerValues.Single();
        }

        private string GetUsername(HttpContext context)
        {
            var pathValue = context.Request.Path.Value;
            bool isAryaVisRequest = pathValue.Contains("aryavis");
            var ignoreUserHeader = GetHeaderValue(IgnoreUserHeader, context);
            var ignoreUser = !StringValues.IsNullOrEmpty(ignoreUserHeader) && Convert.ToBoolean(ignoreUserHeader);
            if (isAryaVisRequest || ignoreUser)
            {
                return null;
            }
            var userClaims = context?.User?.Claims;
            if (userClaims?.Any() != true)
            {
                return null;
            }
            var username = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            return username;
        }

        private async Task SetCurrentUserAsync(HttpContext context, IUserService userService, IConfigurationService configurationService, IOrgApiClientService orgApiClientService)
        {
            var username = GetUsername(context);
            if (string.IsNullOrWhiteSpace(username))
            {
                return;
            }
            if (Guid.TryParse(username, out Guid userId))
            {
                _logger.LogInformation("Loading the user from database for the current request with id: {@UserGuid}.", userId);
                await userService.LoadCurrentUserAsync(userId);
            }
            else
            {
                await EmulateUserAsync(context, username, userService, orgApiClientService);
            }
            await ImpersonateUserAsync(context, userService, configurationService);
            if (!userService.IsCurrentUserSet())
            {
                return;
            }
            var user = userService.GetCurrentUser();
            if (user.ImpersonatedBy == null)
            {
                _logger.LogInformation("{@LogEvent}. User {@User} is logged in.", "USER_LOGIN", user);
                return;
            }
        }

        private async Task ImpersonateUserAsync(HttpContext context, IUserService userService, IConfigurationService configurationService)
        {
            var impersonateAsUsername = GetHeaderValue(ImpersonateHeader, context);
            if (string.IsNullOrWhiteSpace(impersonateAsUsername) || !userService.IsCurrentUserSet())
            {
                return;
            }
            var user = userService.GetCurrentUser();
            var configuration = await configurationService.GetUserConfigurationAsync();
            if (configuration?.IsManagementUser != true)
            {
                throw new UnauthorizedAccessException($"User {user.UserGuid} do not have rights to impersonate.");
            }
            if (!Guid.TryParse(impersonateAsUsername, out var userIdToImpersonate))
            {
                throw new InvalidArgumentException(nameof(impersonateAsUsername), impersonateAsUsername, "Guid of the user to impersonate.");
            }
            var impersonatedAs = await userService.GetUserAsync(userIdToImpersonate);
            impersonatedAs.ImpersonatedBy = user;
            userService.SetCurrentUser(impersonatedAs, true);
            _logger.LogInformation("{@LogEvent}. User {@User} is impersonated by {@ImpersonatedBy}", "USER_LOGIN", impersonatedAs, user);
        }

        private async Task EmulateUserAsync(HttpContext context, string clientId, IUserService userService, IOrgApiClientService orgApiClientService)
        {
            var emulateAsEmail = GetHeaderValue(EmulateHeader, context);
            if (string.IsNullOrWhiteSpace(emulateAsEmail) || string.IsNullOrWhiteSpace(clientId) || userService.IsCurrentUserSet())
            {
                return;
            }
            _logger.LogInformation("Loading the user from database for the current request for apiClientId: {@ApiClientId} and userEmail: {@UserEmail}",
                clientId, emulateAsEmail);
            var organization = await orgApiClientService.GetAsync(clientId);
            var user = await userService.GetUserAsync(emulateAsEmail);
            // TODO: validation of scopes
            if (user == null || user?.OrgGuid != organization?.OrgGuid)
            {
                throw new UnauthorizedAccessException($"Cannot emulate {emulateAsEmail} with client id {clientId}");
            }
            user.IsImpersonated = true;
            // ? do we allow inactive user?
            userService.SetCurrentUser(user);
        }
    }
}
