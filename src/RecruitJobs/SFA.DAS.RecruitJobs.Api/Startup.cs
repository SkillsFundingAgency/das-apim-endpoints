using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
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
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RecruitJobs.Api.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.RecruitJobs.Api;

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
            services.AddHealthChecks();
        }
        
        services.AddOpenTelemetryRegistration(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecruitJobsOuterApi", Version = "v1" });
        });

        services
            .AddRecruitGqlClient()
            .ConfigureHttpClient((sp, x) =>
            {
                var clientConfig = sp.GetService<RecruitApiConfiguration>();
                x.BaseAddress = new Uri($"{clientConfig.Url.TrimEnd('/')}/graphql");
                
                if (configuration.IsLocalOrDev())
                {
                    return;
                }
                
                var correlationId = CorrelationContext.CorrelationId;
                if (!string.IsNullOrWhiteSpace(correlationId))
                {
                    x.DefaultRequestHeaders.Remove("x-correlation-id");
                    x.DefaultRequestHeaders.Add("x-correlation-id", correlationId);
                }

                var credentialHelper = sp.GetService<IAzureClientCredentialHelper>();
                var token = credentialHelper.GetAccessTokenAsync(clientConfig.Identifier).Result;
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            });
    }
    
    public static void ConfigureApp(IApplicationBuilder app, IConfigurationRoot configuration)
    {
        app.UseAuthentication();

        if (!configuration["Environment"]!.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecruitJobsOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}