using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.ReferenceDataJobs.Configuration;

namespace SFA.DAS.ReferenceDataJobs.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.Configure<PublicSectorOrganisationsApiConfiguration>(configuration.GetSection("PublicSectorOrganisationsInnerApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<PublicSectorOrganisationsApiConfiguration>>().Value);
    }
}
