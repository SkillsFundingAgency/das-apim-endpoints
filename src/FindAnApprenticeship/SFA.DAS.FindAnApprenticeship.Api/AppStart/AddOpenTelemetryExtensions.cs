using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using SFA.DAS.FindAnApprenticeship.Api.Telemetry;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.AppStart
{
    public static class AddOpenTelemetryExtensions
    {
        public static void AddOpenTelemetryRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add the OpenTelemetry telemetry service to the application.
            // This service will collect and send telemetry data to Azure Monitor.
            services.AddOpenTelemetry().UseAzureMonitor(options =>
                {
                    options.ConnectionString = configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                })
                // Configure metrics
                .WithMetrics(opts => opts
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        Constants.OpenTelemetry.ServiceName,
                        nameof(FindAnApprenticeship),
                        "1.0"))
                    .AddMeter(Constants.OpenTelemetry.ServiceMeterName)
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation())
                // Configure meters
                .WithMetrics(opts => opts
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("Microsoft.AspNetCore.Http.Connections")
                    .AddMeter("Microsoft.AspNetCore.Routing")
                    .AddMeter("Microsoft.AspNetCore.Diagnostics")
                    .AddMeter("Microsoft.AspNetCore.RateLimiting"));

            services.AddSingleton<FindAnApprenticeshipMetrics>();
        }
    }
}