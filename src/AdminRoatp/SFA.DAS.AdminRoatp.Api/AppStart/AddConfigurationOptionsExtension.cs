using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.AdminRoatp.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<RoatpConfiguration>(configuration.GetSection("RoatpApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpConfiguration>>()!.Value);
        services.Configure<ApplyApiConfiguration>(configuration.GetSection("ApplyApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApplyApiConfiguration>>()!.Value);
        return services;
    }
}
