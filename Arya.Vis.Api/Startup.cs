using Arya.Vis.Api.GraphQL.Extensions;
using Arya.Vis.Container.Default;
using Arya.Vis.Container.Web.Extensions;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Arya.Vis.Api {
    public class Startup {

        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices (IServiceCollection services) {
            services
                .AddAryaVisDefault(Configuration)
                .AddDataLoaders();

            // Create schema
            services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddTypes()
                .Create()
            );

            // Add CORS
            services.AddCors (options => {
                options.AddPolicy ("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed (host => true)
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .AllowCredentials ()
                    .Build ()
                );
            });
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime) {
            if (!env.IsDevelopment()) {
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseForwardedHeaders (new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.LoadCurrentUser();
            app.Use((context, next) => {
                context.Response.Headers["Cache-Control"] = "no-cache";
                return next.Invoke();
            });
            app.UseGraphQL ("/graphql");
        }
    }
}
