using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EarlyConnect.Api.AppStart;
using SFA.DAS.EarlyConnect.Api.ErrorHandler;
using SFA.DAS.EarlyConnect.Application.Queries;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

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

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(DummyQuery).Assembly));
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
                .AddCheck<EarlyConnectApiHealthCheck>("EarlyConnect API health check");
        }

        services.AddApplicationInsightsTelemetry(x => x.ConnectionString = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EarlyConnectOuterApi", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthentication();
        app.UseHealthChecks();

        app.UseRouting();
        app.UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"));
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Users}/{action=Index}/{id?}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EarlyConnectOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}