using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.ReferenceDataJobs.Api.AppStart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>, PublicSectorOrganisationApiClient>();
        services.AddTransient<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>, EducationalOrganisationApiClient>();
        return services;
    }
}