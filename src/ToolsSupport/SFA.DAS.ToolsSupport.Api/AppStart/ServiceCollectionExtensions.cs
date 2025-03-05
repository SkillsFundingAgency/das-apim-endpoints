using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.Mappers;

namespace SFA.DAS.ToolsSupport.Api.AppStart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IPendingChangesMapper, PendingChangesMapper>();
        return services;
    }
}