using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerFinanceJobs.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<FundingProjectionApiConfiguration>(configuration.GetSection("FundingProjectionApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FundingProjectionApiConfiguration>>()!.Value);
        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection("CommitmentsV2ApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>()!.Value);
    }
}