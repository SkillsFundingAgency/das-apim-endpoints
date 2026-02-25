using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.LearnerDataJobs.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        services.Configure<LearnerDataInnerApiConfiguration>(configuration.GetSection("LearnerDataInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LearnerDataInnerApiConfiguration>>().Value);

        services.Configure<CoursesApiConfiguration>(configuration.GetSection("CoursesApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
    }
}
