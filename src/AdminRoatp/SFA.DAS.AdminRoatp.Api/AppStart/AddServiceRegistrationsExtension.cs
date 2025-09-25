using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.AdminRoatp.Api.AppStart;

public static class AddServiceRegistrationsExtension
{
    public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetOrganisationsQuery).Assembly));
        services.AddHttpClient();
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));

        services.AddTransient<IRoatpServiceApiClient<RoatpConfiguration>, RoatpServiceApiClient>();
        services.AddTransient<IApplyApiClient<ApplyApiConfiguration>, ApplyApiClient>();

        return services;
    }
}
