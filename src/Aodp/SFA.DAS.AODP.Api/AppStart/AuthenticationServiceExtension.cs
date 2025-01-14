using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AODP.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AuthenticationServiceExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfigurationRoot configuration)
        {
            if (!configuration.IsLocalOrDev())
            {
                var azureAdConfiguration = configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();
                var policies = new Dictionary<string, string>
            {
                {"default", "APIM"}
            };

                services.AddAuthentication(azureAdConfiguration, policies);
            }
            return services;
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAodpApiClient<AodpApiConfiguration>, AodpApiClient>();
            return services;
        }
    }
}
