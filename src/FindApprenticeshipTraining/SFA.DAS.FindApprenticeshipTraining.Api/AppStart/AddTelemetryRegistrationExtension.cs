using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Apim.Shared.AppStart;

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