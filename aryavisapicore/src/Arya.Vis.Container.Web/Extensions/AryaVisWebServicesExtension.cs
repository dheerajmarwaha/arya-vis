using Arya.Vis.Container.Web.ServiceImpl;
using Arya.Vis.Container.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class AryaVisWebServicesExtension
    {

        public static IServiceCollection AddAryaVisWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrgApiClientService, OrgApiClientService>();
            return services;
        }

    }
}
