using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.EmployerFinanceJobs.Api.AppStart;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerFinanceJobs.Api;

[ExcludeFromCodeCoverage]
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

        services.AddSingleton(new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
        });
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
        }).AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        if (configuration["Environment"] != "DEV")
        {
            services.AddHealthChecks();
        }

        services.AddOpenTelemetryRegistration(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]!);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerFinanceJobsOuterApi", Version = "v1" });
        });

        services.AddLogging();
    }

    public static void ConfigureApp(IApplicationBuilder app, IConfigurationRoot configuration, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseAuthentication();

        if (!configuration["Environment"]!.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            app.UseHealthChecks();
        }

        app.UseRouting();
        // Add this BEFORE app.UseEndpoints(...)
        var assembly = typeof(Program).Assembly;
        try
        {
            assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            foreach (var e in ex.LoaderExceptions)
                Console.WriteLine(e?.Message);
            throw; // stop here so you can read the output
        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Providers}/{action=index}/{id?}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerFinanceJobsOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}