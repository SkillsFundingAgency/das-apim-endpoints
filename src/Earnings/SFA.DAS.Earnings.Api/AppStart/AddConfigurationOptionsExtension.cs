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

        services.AddConfigurationOptions<LearningApiConfiguration>(configuration);
        services.AddConfigurationOptions<EarningsApiConfiguration>(configuration);
        services.AddConfigurationOptions<CollectionCalendarApiConfiguration>(configuration);
        services.AddConfigurationOptions<CoursesApiConfiguration>(configuration);
    }

    private static void AddConfigurationOptions<T>(this IServiceCollection services, IConfiguration configuration, string? name = null) where T : class
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        services.Configure<T>(configuration.GetSection(name));
        services.AddSingleton(cfg => cfg.GetService<IOptions<T>>()!.Value);
    }
}