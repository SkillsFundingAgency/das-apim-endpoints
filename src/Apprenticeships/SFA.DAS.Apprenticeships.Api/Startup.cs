﻿using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Apprenticeships.Api.AppStart;
using SFA.DAS.Apprenticeships.Application.TrainingCourses;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Apprenticeships.Api;

[ExcludeFromCodeCoverage]
public class Startup
{
    private const string EndpointName = "SFA.DAS.Apprenticeships";
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _env = env;
        _configuration = configuration.BuildSharedConfiguration();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddNLog();
        services.AddOptions();
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
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetStandardQuery).Assembly));
        services.AddServiceRegistration(_configuration);

        services
            .AddMvc(o =>
            {
                if (!_configuration.IsLocalOrDev())
                {
                    o.Filters.Add(new AuthorizeFilter("default"));
                }
            });

        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        if (_configuration["Environment"] != "DEV")
        {
            services.AddHealthChecks()
                .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription);
        }

        services.AddApplicationInsightsTelemetry(x => x.ConnectionString = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApprenticeshipsOuterApi", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseAuthentication();
        app.UseHealthChecks();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=TrainingCourses}/{action=GetList}/{id?}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApprenticeshipsOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }

    public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
    {
        Task.FromResult(serviceProvider.StartNServiceBus(_configuration, EndpointName)).GetAwaiter().GetResult();
    }
}