using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApimDeveloper.Application.Services;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.ApimDeveloper.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Services;
using SFA.DAS.SharedOuterApi.Types.Services;

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