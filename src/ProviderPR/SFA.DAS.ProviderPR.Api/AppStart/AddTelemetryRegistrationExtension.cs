using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.AppStart;

namespace SFA.DAS.ProviderPR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddTelemetryRegistrationExtension
{
    public static IServiceCollection AddTelemetryRegistration(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddOpenTelemetryRegistration(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        return services;
    }
}
