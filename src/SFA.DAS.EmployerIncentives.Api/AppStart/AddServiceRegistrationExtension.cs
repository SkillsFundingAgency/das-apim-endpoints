using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Clients;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        { 
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IApiClient<>), typeof(ApiClient<>));
            
            services.AddTransient<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>, EmployerIncentivesApiClient>();
            services.AddTransient<ICommitmentsApiClient<CommitmentsConfiguration>, CommitmentsApiClient>();
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IEmployerIncentivesService, EmployerIncentivesService>();
            services.AddTransient<ICommitmentsService, CommitmentsService>();
            services.AddTransient<IAccountsService, AccountsService>();
        }
    }
}