using System.Diagnostics.CodeAnalysis;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ApprenticeCommitments.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddTelemetryRegistrationExtension
{
    public static IServiceCollection AddTelemetryRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var appInsightsConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            services.AddOpenTelemetry().UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            });
        }

        return services;
    }
}