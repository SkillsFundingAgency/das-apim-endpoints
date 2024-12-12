using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Payments.Api.AppStart;
using SFA.DAS.Payments.Application.Learners;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SFA.DAS.Payments.Api;

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

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetLearnersQueryHandler).Assembly));
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
                .AddCheck<LocationsApiHealthCheck>(LocationsApiHealthCheck.HealthCheckResultDescription);
        }

        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentsOuterApi", Version = "v1" });
        });

        services.AddLogging();
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

        if (!_configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            app.UseHealthChecks();
        }

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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentsOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}
