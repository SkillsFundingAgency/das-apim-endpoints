using Microsoft.Extensions.Options;

namespace SFA.DAS.TrackProgress.Api.AppStart;

public static class AddConfigurationOptionsExtensions
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TrackProgressConfiguration>().BindConfiguration("");
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<TrackProgressConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().AzureAd);
        services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().CommitmentsV2InnerApi);
        services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().CoursesApi);
        services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().TrackProgressApi);
    }
}
