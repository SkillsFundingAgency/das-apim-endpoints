using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.MsaBrokerage.Configuration;
using SFA.DAS.SharedOuterApi.Types.Configuration;

namespace SFA.DAS.MsaBrokerage.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<CandidateApiConfiguration>(configuration.GetSection(nameof(CandidateApiConfiguration)));
        services.Configure<MsaBrokerageConfiguration>(configuration.GetSection(nameof(MsaBrokerageConfiguration)));
        services.Configure<FindApprenticeshipApiConfiguration>(configuration.GetSection(nameof(FindApprenticeshipApiConfiguration)));
        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
        // Same section name FAA uses for the Recruit (v2) API.
        services.Configure<RecruitApiV2Configuration>(configuration.GetSection("RecruitAltApiConfiguration"));

        services.AddSingleton(cfg => cfg.GetService<IOptions<CandidateApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<MsaBrokerageConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiV2Configuration>>().Value);
    }
}
