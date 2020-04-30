using Arya.Vis.Container.Web.Security;
using Arya.Vis.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public static class ApiKeyAuthenticationExtension
    {
        public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services,
                                                                IConfiguration configuration,
                                                                IWebHostEnvironment environment)
        {
            services.Configure<ApiKeyAuthConfig>(configuration.GetSection("Authentication:ApiKey"));
            services
                .AddAuthorization(options =>
                {
                    options.AddApiKeyPolicy();
                })
                .AddAuthentication(ApiKeyAuthConstants.Scheme)
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthConstants.Scheme, options =>
                {
                    options.Events = new ApiKeyAuthenticationEvents
                    {
                        OnValidateCredentials = async (context, serviceProvider) =>
                        {
                            var userService = serviceProvider.GetRequiredService<IUserService>();
                            var authConfig = serviceProvider.GetRequiredService<IOptions<ApiKeyAuthConfig>>().Value;
                            var isAuthenticated = authConfig.ApplicationId == context.ApplicationId &&
                                authConfig.ApplicationKey == context.ApplicationKey;
                            await userService.LoadCurrentUserAsync(context.Email);
                            var user = userService.GetCurrentUser();
                            if (isAuthenticated)
                            {
                                var claims = new[] {
                                    new Claim(
                                    ClaimTypes.NameIdentifier,
                                    user.UserGuid.ToString(),
                                    ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer),
                                    new Claim(
                                    ClaimTypes.Name,
                                    user.UserGuid.ToString(),
                                    ClaimValueTypes.String,
                                    context.Options.ClaimsIssuer)
                                };

                                context.Principal = new ClaimsPrincipal(
                                    new ClaimsIdentity(claims, context.Scheme.Name));

                                context.Success();
                            }
                        },
                    };
                });
            return services;
        }

        public static AuthorizationOptions AddApiKeyPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(ApiKeyAuthConstants.Scheme)
                .RequireAuthenticatedUser()
                .Build();
            options.AddPolicy(ApiKeyAuthConstants.Policy, policy);
            return options;
        }
    }
}
