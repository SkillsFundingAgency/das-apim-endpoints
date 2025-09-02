using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Earnings.Api.AppStart;
using SFA.DAS.Earnings.Api.Controllers;
using SFA.DAS.Earnings.Application.ApprovedApprenticeships;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Earnings.Api;

[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration, IWebHostEnvironment env)
{
    private readonly IConfiguration _configuration = configuration.BuildSharedConfiguration();

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddNLog();
        services.AddOptions();
        services.AddSingleton(env);

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

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Earning).Assembly));
        services.AddSingleton<IApprovedApprenticeshipsStore, ApprovedApprenticeshipsStore>();
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
                .AddCheck<LearningApiHealthCheck>(LearningApiHealthCheck.HealthCheckResultDescription)
                .AddCheck<EarningsApiHealthCheck>(EarningsApiHealthCheck.HealthCheckResultDescription)
                .AddCheck<CollectionCalendarApiHealthCheck>(CollectionCalendarApiHealthCheck.HealthCheckResultDescription);
        }

        services.AddApplicationInsightsTelemetry(x => x.ConnectionString = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EarningsOuterApi", Version = "v1" });
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
                pattern: "{controller}/{action}/{id}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EarningsOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}