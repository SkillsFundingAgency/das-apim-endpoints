using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ReferenceDataJobs.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.Configure<PublicSectorOrganisationApiConfiguration>(configuration.GetSection("PublicSectorOrganisationsInnerApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<PublicSectorOrganisationApiConfiguration>>().Value);
        services.Configure<EducationalOrganisationApiConfiguration>(configuration.GetSection("EducationOrganisationsInnerApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<EducationalOrganisationApiConfiguration>>().Value);
    }
}
