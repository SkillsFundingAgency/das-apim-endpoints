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

        services.Configure<LearningApiConfiguration>(configuration.GetSection(nameof(LearningApiConfiguration)));
        services.Configure<EarningsApiConfiguration>(configuration.GetSection(nameof(EarningsApiConfiguration)));
        services.Configure<CollectionCalendarApiConfiguration>(configuration.GetSection(nameof(CollectionCalendarApiConfiguration)));

        services.AddSingleton(cfg => cfg.GetService<IOptions<LearningApiConfiguration>>()!.Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarningsApiConfiguration>>()!.Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<CollectionCalendarApiConfiguration>>()!.Value);
    }
}