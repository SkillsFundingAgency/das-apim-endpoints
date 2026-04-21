using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AparRegister.Application.Queries.ProviderRegister;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.AparRegister.Api.AppStart;

internal static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetProvidersQuery).Assembly));

        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IRoatpServiceApiClient<RoatpConfiguration>, RoatpServiceApiClient>();
        services.AddTransient<ICacheStorageService, CacheStorageService>();
    }
}
