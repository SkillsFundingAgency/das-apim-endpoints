using SFA.DAS.Aodp.Wrapper;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Aodp.Api.AppStart
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAodpApiClient<AodpApiConfiguration>, AodpApiClient>();
            services.AddTransient<IMultipartFormDataSenderWrapper, MultipartFormDataSenderWrapper>();
            return services;
        }
    }
}
