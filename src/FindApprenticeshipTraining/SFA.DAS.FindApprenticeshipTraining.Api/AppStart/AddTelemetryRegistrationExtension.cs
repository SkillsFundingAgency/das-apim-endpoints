using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindApprenticeshipTraining.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddTelemetryRegistrationExtension
{
    public static IServiceCollection AddTelemetryRegistration(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddOpenTelemetryRegistration(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        return services;
    }
}