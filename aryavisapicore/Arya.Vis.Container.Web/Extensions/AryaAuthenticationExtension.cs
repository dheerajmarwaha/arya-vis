using Arya.Vis.Container.Web.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class AryaAuthenticationExtension
    {
        public static IServiceCollection AddAryaAuthentication(this IServiceCollection services,
                                                            IConfiguration configuration,
                                                            IWebHostEnvironment environment)
        {
            services.Configure<JwtBearerOptions>(configuration.GetSection("Authentication:Cognito"));
            var serviceProvider = services.BuildServiceProvider();
            var authOptions = serviceProvider.GetService<IOptions<JwtBearerOptions>>();

            services
                .AddAuthorization(options =>
                {
                    options.AddCognitoPolicy();
                    options.AddAdminAccessPolicy();
                    options.AddSuperAdminAccessPolicy();
                })
                // registration of the security handler
                .AddScoped<IAuthorizationHandler, SecurityRoleHandler>()

                .AddAuthentication("Bearer")

                .AddJwtBearer(options =>
                {
                    options.Audience = authOptions.Value.Audience;
                    options.Authority = authOptions.Value.Authority;
                    options.SaveToken = authOptions.Value.SaveToken;
                    options.IncludeErrorDetails = authOptions.Value.IncludeErrorDetails;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = authOptions.Value.TokenValidationParameters.ValidateAudience,
                        IssuerSigningKey = authOptions.Value.TokenValidationParameters.IssuerSigningKey
                    };
                    // We have to hook the OnMessageReceived event in order to
                    // allow the JWT authentication handler to read the access
                    // token from the query string when a WebSocket or
                    // Server-Sent Events request comes in.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs/notification")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static AuthorizationOptions AddCognitoPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            options.AddPolicy("Bearer", policy);
            return options;
        }

        public static AuthorizationOptions AddAdminAccessPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new SecurityRoleRequirement(SecurityRoles.AdminRole))
                .Build();
            options.AddPolicy(SecurityPolicy.AdminAccessPolicy, policy);
            return options;
        }

        public static AuthorizationOptions AddSuperAdminAccessPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new SecurityRoleRequirement(SecurityRoles.SuperAdminRole))
                .Build();
            options.AddPolicy(SecurityPolicy.SuperAdminAccessPolicy, policy);
            return options;
        }
    }
}
