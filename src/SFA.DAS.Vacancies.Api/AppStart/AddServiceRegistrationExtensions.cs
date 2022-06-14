using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Vacancies.Services;
using SFA.DAS.Vacancies.Configuration;

namespace SFA.DAS.Vacancies.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<IAccountLegalEntityPermissionService, AccountLegalEntityPermissionService>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>, FindApprenticeshipApiClient>();
            services.AddTransient<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>, FindTraineeshipApiClient>();
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
            services.AddTransient<ICourseService, CourseService>();
        }
    }
}