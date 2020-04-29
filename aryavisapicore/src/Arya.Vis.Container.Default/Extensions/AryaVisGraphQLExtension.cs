using Arya.Vis.Core.GraphQL.GraphQLQueries;
using Arya.Vis.Core.GraphQL.GraphQLType;
using Arya.Vis.Core.GraphQLSchema;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Arya.Vis.Container.Default.Extensions
{
    public static class AryaVisGraphQLExtension
    {
        public static IServiceCollection AddAryaVisGraphQL(this IServiceCollection services
                                                        , IConfiguration configuration
                                                        , IWebHostEnvironment environment)
        {
            
            services.AddSingleton <InterviewType>();
            services.AddSingleton<InterviewQuery>();            

            services.AddSingleton<InterviewInputType>();
            services.AddSingleton<InterviewMutation>();

            services.AddSingleton<InterviewSchema>();

            //Register dependency resolver for GraphQL
            services.AddSingleton<IDependencyResolver>(
                c=> new FuncDependencyResolver(type => c.GetRequiredService(type)));

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = environment.IsDevelopment();
            })
                .AddWebSockets()
                .AddUserContextBuilder(httpContext=> httpContext.User)
                .AddDataLoader()
                .AddGraphTypes(ServiceLifetime.Scoped);
            return services;
        }
    }
}
