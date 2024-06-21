using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.FindAnApprenticeship.Telemetry;

namespace SFA.DAS.FindAnApprenticeship.Api.AppStart
{
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
                    })
                    // Configure metrics
                    .WithMetrics(opts => opts
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                            Constants.OpenTelemetry.ServiceName,
                            nameof(FindAnApprenticeship)))
                        .AddMeter(Constants.OpenTelemetry.ServiceMeterName));
                services.AddSingleton<IMetrics, FindAnApprenticeshipMetrics>();
            }
        }
    }
}