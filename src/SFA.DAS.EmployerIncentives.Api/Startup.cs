using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerIncentives.Api.AppStart;
using SFA.DAS.EmployerIncentives.Api.Configuration;
using SFA.DAS.EmployerIncentives.Api.ErrorHandler;
using SFA.DAS.EmployerIncentives.Api.HealthChecks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Services;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.EmployerIncentives.Api.Extensions;

namespace SFA.DAS.EmployerIncentives.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        { 
            _env = env;
            _configuration = configuration;

            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration);

            if (!_env.IsLocal())
            {
                config.AddAzureTableStorage(
                    EmployerIncentivesConfigurationKeys.EmployerIncentivesOuterApi,
                    EmployerIncentivesConfigurationKeys.AzureActiveDirectoryApiConfiguration,
                    EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration);

            }
            config.AddEnvironmentVariables();

            if (_env.IsDevelopment())
            {
                config.AddJsonFile($"appsettings.development.json", optional: true);
            }

            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfigurationSections(_configuration);

            if (!_env.IsLocalOrDev())
            {
                var azureAdConfiguration = _configuration.GetSection(EmployerIncentivesConfigurationKeys.AzureActiveDirectoryApiConfiguration).Get<AzureActiveDirectoryConfiguration>();
                services.AddAuthentication(azureAdConfiguration);
            }

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddDasHealthChecks();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerIncentivesOuterApi", Version = "v1" });
            });

            services.AddControllers(c=>
                {
                    if (!_env.IsLocalOrDev())
                    {
                        c.Filters.Add(new AuthorizeFilter("default"));
                    }
                })
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddMediatR(typeof(EmployerIncentivesService).Assembly);
            services.AddDasHttpClientsAndAssociatedServices(_configuration, _env);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
                .UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"))
                .UseHealthChecks()
                .UseAuthentication();
            
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(ep =>
            {
                ep.MapControllers();
            });

            app.UseSwagger()
                .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerIncentivesOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
