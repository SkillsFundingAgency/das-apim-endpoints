using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EmployerFeedback.Api.AppStart;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes;
using SFA.DAS.EmployerFeedback.Application.Queries.GetProvider;
using SFA.DAS.EmployerFeedback.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerFeedback.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

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

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetAccountsQuery).Assembly));
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetProviderQuery).Assembly));
            services.AddServiceRegistration();

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
                })
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

        if (_configuration["Environment"] != "DEV")
        {
            services.AddHealthChecks()
                 .AddCheck<AccountsApiHealthCheck>(AccountsApiHealthCheck.HealthCheckResultDescription);
        }
        
        if (_configuration.IsLocalOrDev())
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var configuration = _configuration
                .GetSection(nameof(EmployerFeedbackConfiguration))
                .Get<EmployerFeedbackConfiguration>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.ApimEndpointsRedisConnectionString;
            });
        }

        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]!);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerFeedbackOuterApi", Version = "v1" });
        });
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
                    pattern: "api/{controller=Account}/{action=index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerFeedbackOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}