using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ReferenceDataJobs.Configuration;
using SFA.DAS.ReferenceDataJobs.Interfaces;
using SFA.DAS.ReferenceDataJobs.Services;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ReferenceDataJobs.Api.AppStart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IPublicSectorOrganisationsApiClient<PublicSectorOrganisationsApiConfiguration>, PublicSectorOrganisationsApiClient>();
        return services;
    }
}