using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.EmployerAccounts.Api.AppStart;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.EmployerAccounts.Api
{
    [ExcludeFromCodeCoverage]
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
            services.AddLogging(builder =>
            {
                builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
                builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
            });

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

            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetEnglishFractionCurrentQuery).Assembly));
            services.AddServiceRegistration(_configuration);

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
                    .AddCheck<AccountsApiHealthCheck>($"{AccountsApiHealthCheck.AccountsApiHealthCheckDescription} health check")
                    .AddCheck<FinanceApiHealthCheck>($"{FinanceApiHealthCheck.FinanceApiHealthCheckDescription} health check");
            }

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerAccountsOuterApi", Version = "v1" });
            });

            services.AddDateTimeServices(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
                    pattern: "{controller?}/{action?}/{id?}");

                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse,
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerAccountsOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}