using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Campaign.Api.AppStart;
using SFA.DAS.Campaign.Application.Queries.Sectors;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration.BuildSharedConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_env);

            services.AddConfigurationOptions(_configuration);

            if (!_configuration.IsLocalOrDev())
            {
                var azureAdConfiguration = _configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();
                var policies = new Dictionary<string, string>
                {
                    {"default", "APIM"}
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetSectorsQuery).Assembly));
            services.AddServiceRegistration();

            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                });

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription);
            }

            if (_configuration.IsLocalOrDev())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                var configuration = _configuration
                    .GetSection("CampaignConfiguration")
                    .Get<CampaignConfiguration>();

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.ApimEndpointsRedisConnectionString;
                });
            }

            services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]!);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CampaignOuterApi", Version = "v1" });
            });

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CampaignOuterApi");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Remove("X-AspNet-Version");
                    context.Response.Headers.Remove("X-Powered-By");
                    return Task.CompletedTask;
                });
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Sectors}/{action=GetSectors}/{id?}");
            });

        }
    }
}