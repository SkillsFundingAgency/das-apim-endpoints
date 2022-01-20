using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmploymentCheck.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        { 
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient(typeof(ICustomerEngagementApiClient<>), typeof(CustomerEngagementApiClient<>));
            services.AddTransient<IInternalApiClient<EmploymentCheckConfiguration>, InternalApiClient<EmploymentCheckConfiguration>>();
        }
    }
}