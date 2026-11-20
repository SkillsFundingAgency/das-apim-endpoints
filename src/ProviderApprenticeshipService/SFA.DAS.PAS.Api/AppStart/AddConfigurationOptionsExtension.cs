using Microsoft.Extensions.Options;
using SFA.DAS.PAS.Infrastructure;

namespace SFA.DAS.PAS.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<PasApiConfiguration>(configuration.GetSection(nameof(PasApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<PasApiConfiguration>>()!.Value);
        return services;
    }
}
