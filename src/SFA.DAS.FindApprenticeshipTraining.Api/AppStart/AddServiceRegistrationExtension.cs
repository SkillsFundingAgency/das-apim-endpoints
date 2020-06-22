using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            
            services.AddHttpClient<IApiClient, ApiClient>();
        }
    }
}