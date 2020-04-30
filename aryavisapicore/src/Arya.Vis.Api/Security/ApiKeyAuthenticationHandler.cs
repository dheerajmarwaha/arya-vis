using Arya.Vis.Container.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {

        private readonly IServiceProvider _serviceProvider;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider
        ) : base(options, logger, encoder, clock)
        {
            _serviceProvider = serviceProvider;
        }

        protected new ApiKeyAuthenticationEvents Events
        {
            get { return (ApiKeyAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new ApiKeyAuthenticationEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (!authorizationHeader.StartsWith(ApiKeyAuthConstants.Scheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            string credentials = authorizationHeader.Substring(ApiKeyAuthConstants.Scheme.Length).Trim();

            if (string.IsNullOrEmpty(credentials))
            {
                return AuthenticateResult.Fail("Credentials not provided");
            }

            var credentialParts = credentials.Split(";");
            if (credentialParts.Length != 3)
            {
                return AuthenticateResult.Fail("Expecred credentials to be in format 'ApplicationId;ApplicationKey;UserId'");
            }

            if (!Guid.TryParse(credentialParts[0], out var applicationId))
            {
                return AuthenticateResult.Fail("ApplicationId must be a Guid");
            }
            var applicationKey = credentialParts[1];
            var email = credentialParts[2];
            if (string.IsNullOrWhiteSpace(email))
            {
                return AuthenticateResult.Fail("User email must exist");
            }

            try
            {

                email = Encoding.UTF8.GetString(Convert.FromBase64String(email));

                var validateCredentialsContext = new ValidateCredentialsContext(Context, Scheme, Options)
                {
                    ApplicationId = applicationId,
                    ApplicationKey = applicationKey,
                    Email = email,
                };

                await Events.ValidateCredentials(validateCredentialsContext, _serviceProvider);

                if (validateCredentialsContext.Result != null &&
                    validateCredentialsContext.Result.Succeeded)
                {
                    var ticket = new AuthenticationTicket(validateCredentialsContext.Principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }

                if (validateCredentialsContext.Result != null &&
                    validateCredentialsContext.Result.Failure != null)
                {
                    return AuthenticateResult.Fail(validateCredentialsContext.Result.Failure);
                }

                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                var authenticationFailedContext = new ApiKeyAuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                await Events.AuthenticationFailed(authenticationFailedContext);

                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader) ||
                !authorizationHeader.StartsWith(ApiKeyAuthConstants.Scheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            if (!Request.IsHttps)
            {
                const string insecureProtocolMessage = "Request is HTTP, ApiKey Authentication will not respond.";
                Logger.LogInformation(insecureProtocolMessage);
                Response.StatusCode = 500;
                var encodedResponseText = Encoding.UTF8.GetBytes(insecureProtocolMessage);
                Response.Body.Write(encodedResponseText, 0, encodedResponseText.Length);
            }
            else
            {
                Response.StatusCode = 401;

                var headerValue = $"{ApiKeyAuthConstants.Scheme} ApplicationId;ApplicationKey;UserId";
                Response.Headers.Append(HeaderNames.WWWAuthenticate, headerValue);
            }

            return Task.CompletedTask;
        }
    }
}
