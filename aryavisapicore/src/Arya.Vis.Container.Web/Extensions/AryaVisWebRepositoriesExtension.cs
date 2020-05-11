using Arya.Vis.Core.Repositories;
using Arya.Vis.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class AryaVisWebRepositoriesExtension
    {
        public static IServiceCollection AddAryaVisWebRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrgApiClientRepository, OrgApiClientRepository>();
            return services;
        }
    }
}
