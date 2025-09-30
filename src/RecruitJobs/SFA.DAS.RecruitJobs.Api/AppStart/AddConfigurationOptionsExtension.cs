using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.RecruitJobs.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<RecruitApiConfiguration>(configuration.GetSection("RecruitAltApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiConfiguration>>().Value);
    }
}