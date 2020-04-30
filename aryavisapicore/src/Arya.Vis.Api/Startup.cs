using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using Arya.Vis.Api.Config;
using Arya.Vis.Api.IdentityProviders;
using Arya.Vis.Api.Security;
using Arya.Vis.Api.ServiceImpl;
using Arya.Vis.Api.Services;
using Arya.Vis.Container.Default.Extensions;
using Arya.Vis.Container.Web.Extensions;
using Arya.Vis.Core.GraphQLSchema;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.WebSockets;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Arya.Vis.Api {
    public class Startup {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration, IWebHostEnvironment environment) {
            Environment = environment;
            Configuration = configuration;            
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddApiKeyAuthentication(Configuration, Environment)
                .AddAryaAuthentication(Configuration, Environment)
                .AddAryaVisDefault (Configuration, Environment);

            //register cognito service
            services.Configure<CognitoIAMCredentials>(Configuration.GetSection("AWSCognito"));
            services.Configure<CognitoClientConfiguration>(Configuration.GetSection("Authentication:Client"));

            services.AddScoped<IIdentityService, CognitoService>();
            services.AddScoped<AmazonCognitoIdentityProviderClient>(provider => {
                var config = provider.GetRequiredService<IOptions<CognitoIAMCredentials>>();
                return new AmazonCognitoIdentityProviderClient(config.Value.AccessKeyId, config.Value.SecretAccessKey, config.Value.RegionEndpoint);
            });
            services.AddScoped<IAryaIdentityProviderClient, AryaCognitoIdentityProviderClient>(provider => {
                var cognitoClient = provider.GetRequiredService<AmazonCognitoIdentityProviderClient>();
                return new AryaCognitoIdentityProviderClient(cognitoClient);
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddControllers ();

            // Configure CORS
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build()
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();

                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseWebSockets();
            app.UseGraphQLWebSockets<InterviewSchema>();
            app.UseGraphQL<InterviewSchema>();
            
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}