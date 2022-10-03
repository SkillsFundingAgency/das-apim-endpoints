using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgressInternal.Application.Configuration;
using SFA.DAS.TrackProgressInternal.Application.Services;

namespace SFA.DAS.TrackProgressInternal.Api.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddSingleton<IOwnerApiConfiguration>(s => s.GetRequiredService<TrackProgressOwnerApiConfiguration>());
        services.AddTransient<ResponseReturningApiClient>();
        services.AddTransient<ApimClient>();
    }
}