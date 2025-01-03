﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.Vacancies.Api.AppStart;
using SFA.DAS.Vacancies.Application.TrainingCourses.Queries;
using SFA.DAS.Vacancies.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using SFA.DAS.Vacancies.Services;
using SFA.DAS.Vacancies.Telemetry;

namespace SFA.DAS.Vacancies.Api;

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
                { "default", "APIM" }
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }

        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetTrainingCoursesQuery).Assembly));
        services.AddServiceRegistration();

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
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        if (configuration["Environment"] != "DEV")
        {
            services.AddHealthChecks()
                .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription);
        }

        if (configuration.IsLocalOrDev())
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var vacanciesConfiguration = configuration
                .GetSection(nameof(VacanciesConfiguration))
                .Get<VacanciesConfiguration>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = vacanciesConfiguration.ApimEndpointsRedisConnectionString;
            });
        }

        services.AddOpenTelemetryRegistration(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"], nameof(Vacancies),
            Constants.OpenTelemetry.ServiceMeterName, Constants.OpenTelemetry.ServiceName);

        if (!string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        {
            services.AddSingleton<IMetrics, VacancyMetrics>();    
        }
        else
        {
            services.AddSingleton<IMetrics, MockVacancyMetrics>();
        }
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Display advert API", Version = "v1", Description = @"
### Get and display adverts from Find an apprenticeship. ### 
**Note.** It is not recommended to use The Display Advert API directly from a browser and as such we have not enabled CORS for this API.  Instead, we recommend you call the API intermittently to retrieve the latest vacancies, store those vacancies in your own data store, and then change your website to read those vacancies from your own data store."
            });
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Namespace}.xml");
            c.IncludeXmlComments(filePath);
        });


    }

    public static void ConfigureApp(
        IApplicationBuilder app, 
        IConfigurationRoot configuration)
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
                pattern: "api/{controller=vacancy}/{action=index}/{id?}");
        });
            
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "VacanciesOuterApi");
            c.RoutePrefix = string.Empty;
        });


    }
}