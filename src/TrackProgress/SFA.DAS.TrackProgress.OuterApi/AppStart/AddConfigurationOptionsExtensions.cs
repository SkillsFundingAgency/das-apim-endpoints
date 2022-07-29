using Microsoft.Extensions.Options;

namespace SFA.DAS.TrackProgress.Api.AppStart;

public static class AddConfigurationOptionsExtensions
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TrackProgressConfiguration>().BindConfiguration("");
        //services.AddOptions();
        //services.Configure<TrackProgressConfiguration>(configuration);
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<TrackProgressConfiguration>>().Value);
        services.AddSingleton(cfg => cfg.GetRequiredService<TrackProgressConfiguration>().AzureAd);
    }
}
