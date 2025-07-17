﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Recruit.Api.AppStart;
using SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Recruit.Api;

public static class Startup
{
    public static void ConfigureServices(
        IServiceCollection services, 
        IWebHostEnvironment environment,
        IConfigurationRoot configuration)
    {
        services.AddSingleton(environment);
        services.AddConfigurationOptions(configuration);

        if (!configuration.IsLocalOrDev())
        {
            var azureAdConfiguration = configuration
                .GetSection("AzureAd")
                .Get<AzureActiveDirectoryConfiguration>();
            var policies = new Dictionary<string, string>
            {
                {"default", "APIM"}
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }

        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetTrainingProgrammesQuery).Assembly));
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetAccountsQuery).Assembly));
        services.AddServiceRegistration(configuration);

        services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            }).AddMvc(o =>
            {
                if (!configuration.IsLocalOrDev())
                {
                    o.Filters.Add(new AuthorizeFilter("default"));
                }
            }).AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        if (configuration["Environment"] != "DEV")
        {
            services.AddHealthChecks()
                .AddCheck<CoursesApiHealthCheck>("Courses API health check")
                .AddCheck<RoatpCourseManagementApiHealthCheck>("Roatp Course Management API health check");
        }
        
        services.AddOpenTelemetryRegistration(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecruitOuterApi", Version = "v1" });
        });
    }
    
    public static void ConfigureApp(IApplicationBuilder app, IConfigurationRoot configuration)
    {
        app.UseAuthentication();

        if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            app.UseHealthChecks();
        }
                
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Providers}/{action=index}/{id?}");
        });
            
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecruitOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}