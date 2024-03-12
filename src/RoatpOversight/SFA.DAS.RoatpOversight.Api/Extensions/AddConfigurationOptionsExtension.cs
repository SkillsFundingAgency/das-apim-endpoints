using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RoatpOversight.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        return services;
    }
}
