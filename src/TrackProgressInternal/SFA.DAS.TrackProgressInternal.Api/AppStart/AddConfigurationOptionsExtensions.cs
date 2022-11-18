using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.TrackProgressInternal.Application.Configuration;

namespace SFA.DAS.TrackProgressInternal.Api.AppStart;

public static class AddConfigurationOptionsExtensions
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection("CommitmentsV2InnerApi"));
        services.Configure<CoursesApiConfiguration>(configuration.GetSection("CoursesApi"));
        services.Configure<TrackProgressApiConfiguration>(configuration.GetSection("TrackProgressApi"));
        services.Configure<TrackProgressOwnerApiConfiguration>(configuration.GetSection("TrackProgressApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<CoursesApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<TrackProgressApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<TrackProgressOwnerApiConfiguration>>().Value);
    }
}