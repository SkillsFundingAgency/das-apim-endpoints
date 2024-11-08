using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Identity.Client;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Diagnostics.Metrics;


namespace SFA.DAS.ApprenticeApp.Telemetry
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
                });
                // Configure metrics

                services.AddSingleton<IApprenticeAppMetrics, ApprenticeAppMetrics>();
            }

        }


    }
}