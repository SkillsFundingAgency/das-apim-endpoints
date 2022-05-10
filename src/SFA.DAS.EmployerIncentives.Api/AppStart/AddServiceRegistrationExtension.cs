using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Clients;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerIncentives.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        { 
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient(typeof(ICustomerEngagementApiClient<>), typeof(CustomerEngagementApiClient<>));

            services.AddTransient<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>, EmployerIncentivesApiClient>();
            services.AddTransient<ICommitmentsApiClient<CommitmentsConfiguration>, CommitmentsApiClient>();
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>, CustomerEngagementFinanceApiClient>();
            services.AddTransient<IEmployerIncentivesService, EmployerIncentivesService>();
            services.AddTransient<ILegalEntitiesService, LegalEntitiesService>();
            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IVendorRegistrationService, VendorRegistrationService>();
            services.AddTransient<IApprenticeshipService, ApprenticeshipService>();
            services.AddTransient<IEarningsResilienceCheckService, EarningsResilienceCheckService>();
            services.AddTransient<ICollectionCalendarService, CollectionCalendarService>();
            services.AddTransient<ICommitmentsService, CommitmentsService>();
            services.AddTransient<IAccountsService, AccountsService>();
            services.AddTransient<ICustomerEngagementFinanceService, CustomerEngagementFinanceService>();
            services.AddTransient<IIncentivesEmploymentCheckService, IncentivesEmploymentCheckService>();
            services.AddTransient<IEmploymentCheckService, EmploymentCheckService>();
        }
    }
}