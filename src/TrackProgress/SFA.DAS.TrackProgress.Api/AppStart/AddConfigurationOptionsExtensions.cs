using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.TrackProgress.Api.AppStart;

public static class AddConfigurationOptionsExtensions
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddOptions<TrackProgressConfiguration>().BindConfiguration("");
        //services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<TrackProgressConfiguration>>().Value);
        //services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().AzureAd);
        //services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().CommitmentsV2InnerApi);
        //services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().CoursesApi);
        //services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().TrackProgressApi);
        services.AddOptions();
        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection("CommitmentsV2InnerApi"));
        services.Configure<CoursesApiConfiguration>(configuration.GetSection("CoursesApi"));
        services.Configure<TrackProgressApiConfiguration>(configuration.GetSection("TrackProgressApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<CoursesApiConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<TrackProgressApiConfiguration>>().Value);
    }
}
