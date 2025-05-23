using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApimDeveloper.Application.Services;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.ApimDeveloper.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ApimDeveloper.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>, ApimDeveloperApiClient>();
            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IApimApiService, ApimApiService>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
            services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();
        }
    }
}