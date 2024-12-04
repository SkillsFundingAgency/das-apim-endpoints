using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using Microsoft.Extensions.Logging;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.FindAnApprenticeship.Api.AppStart;
using SFA.DAS.FindAnApprenticeship.Api.Telemetry;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetAddresses;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Domain.Configuration;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.FindAnApprenticeship.Telemetry;

namespace SFA.DAS.FindAnApprenticeship.Api
{
    public class Startup
    {
        private const string EndpointName = "SFA.DAS.FindAnApprenticeship";
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

            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetAddressesQuery).Assembly));
            services.AddServiceRegistration(_configuration);

            services.Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                }).AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                });

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddCheck<LocationsApiHealthCheck>(LocationsApiHealthCheck.HealthCheckResultDescription);
            }
            
            if (_configuration.IsLocalOrDev())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                var configuration = _configuration
                    .GetSection(nameof(FindAnApprenticeshipConfiguration))
                    .Get<FindAnApprenticeshipConfiguration>();

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.ApimEndpointsRedisConnectionString;
                });
            }

            services.AddOpenTelemetryRegistration(
                _configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"],
                nameof(FindAnApprenticeship), 
                Constants.OpenTelemetry.ServiceMeterName,
                Constants.OpenTelemetry.ServiceName);

            if (!string.IsNullOrEmpty(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
            {
                // This service will collect and send telemetry data to Azure Monitor.
                services.AddSingleton<IMetrics, FindAnApprenticeshipMetrics>();
            }
            else
            {
                services.AddSingleton<IMetrics, StubFindAnApprenticeshipMetrics>();
            }
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FindAnApprenticeshipOuterApi", Version = "v1" });
            });

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler(logger);

            app.UseAuthentication();

            if (!_configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseHealthChecks();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Locations}/{action=index}/{query?}");
            });
        
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindAnApprenticeshipOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
        
        public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
        {
            serviceProvider.StartNServiceBus(_configuration, EndpointName);
        }
    }
}
