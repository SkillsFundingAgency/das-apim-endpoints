using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Earnings.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<ApprenticeshipsApiConfiguration>(configuration.GetSection(nameof(ApprenticeshipsApiConfiguration)));
        services.Configure<EarningsApiConfiguration>(configuration.GetSection(nameof(EarningsApiConfiguration)));

        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeshipsApiConfiguration>>()!.Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarningsApiConfiguration>>()!.Value);
    }
}