using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using SFA.DAS.Telemetry.Telemetry;

namespace SFA.DAS.Apim.Shared.AppStart;

public static class AddOpenTelemetryExtensions
{
    /// <summary>
    /// Add the OpenTelemetry telemetry service to the application.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="appInsightsConnectionString">Azure app insights connection string.</param>
    public static void AddOpenTelemetryRegistration(this IServiceCollection services, string appInsightsConnectionString)
    {
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            // This service will collect and send telemetry data to Azure Monitor.
            services.AddOpenTelemetry().UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            });
        }
    }

    /// <summary>
    /// Add the OpenTelemetry telemetry service to the application.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="appInsightsConnectionString">Azure app insights connection string.</param>
    /// <param name="serviceNamespace">Namespace of the service</param>
    /// <param name="serviceMeterName">Meter name</param>
    /// <param name="serviceName">Name of the service</param>
    public static void AddOpenTelemetryRegistration(
        this IServiceCollection services,
        string appInsightsConnectionString, string serviceNamespace, string serviceMeterName, string serviceName)
    {
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            // This service will collect and send telemetry data to Azure Monitor.
            services.AddOpenTelemetry().UseAzureMonitor(options =>
                {
                    options.ConnectionString = appInsightsConnectionString;
                })
                // Configure metrics
                .WithMetrics(opts => opts
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName,
                        serviceNamespace))
                    .AddMeter(serviceMeterName));
        }
    }

    /// <summary>
    /// Add the OpenTelemetry telemetry service to the application.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="appInsightsConnectionString">Azure app insights connection string.</param>
    /// <param name="keysForRedaction">Keys which will be redacted when writing telemetry</param>
    public static IServiceCollection AddOpenTelemetryRegistration(
        this IServiceCollection services, 
        string appInsightsConnectionString, string keysForRedaction)
    {
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            // This service will collect and send telemetry data to Azure Monitor.
            var openTelemetry = services.AddOpenTelemetry().UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            });

            if (!string.IsNullOrEmpty(keysForRedaction))
            {
                openTelemetry.WithTracing(builder => builder.AddUriRedaction(keysForRedaction));
            }
        }

        return services;
    }

    /// <summary>
    /// Add the OpenTelemetry telemetry service to the application.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="configuration">Configuration</param>
    /// <remarks>The Configuration should contain an APPLICATIONINSIGHTS_CONNECTION_STRING key</remarks>
    public static IServiceCollection AddOpenTelemetryRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var appInsightsConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            services.AddOpenTelemetryRegistration(appInsightsConnectionString);
        }

        return services;
    }
}
