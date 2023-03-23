using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.RoatpOversight.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>()!.Value);
        return services;
    }
}
