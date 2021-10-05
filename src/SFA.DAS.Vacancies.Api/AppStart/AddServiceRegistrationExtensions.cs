using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;
using SFA.DAS.Vacancies.Services;

namespace SFA.DAS.Vacancies.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        }
    }
}