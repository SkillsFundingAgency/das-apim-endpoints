using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.Vacancies.Api.AppStart;
using SFA.DAS.Vacancies.Api.OpenApi;
using SFA.DAS.Vacancies.Application.TrainingCourses.Queries;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Services;
using SFA.DAS.Vacancies.Telemetry;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

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

        services.AddSwaggerGen(options =>
        {
            var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            // integrate xml comments
            options.IncludeXmlComments(filePath);
        });
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;

            //Use URL segment versioning, header versioning, or query string versioning
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Version"),
                new MediaTypeApiVersionReader("ver"));
        })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // e.g., v1
                options.SubstituteApiVersionInUrl = true;
            });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
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
            endpoints.MapControllers();
        });

        app.UseSwagger();

        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    options.SwaggerEndpoint(url, "VacanciesOuterApi");
                }
                options.RoutePrefix = string.Empty;
            });
    }
}