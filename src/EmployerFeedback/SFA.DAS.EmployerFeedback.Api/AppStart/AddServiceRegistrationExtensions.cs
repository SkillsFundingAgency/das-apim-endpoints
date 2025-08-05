using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerFeedback.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
            services.AddTransient<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>, EmployerFeedbackApiClient>();
        }
    }
}