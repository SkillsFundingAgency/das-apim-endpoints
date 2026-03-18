using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.ApprenticeApp.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        [ExcludeFromCodeCoverage]
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();            
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
            services.AddTransient<IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration>, ApprenticeProgressApiClient>();
            services.AddTransient<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>, ApprenticeCommitmentsApiClient>();
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>,CourseApiClient>();
            services.AddTransient<CourseApiClient>();
            services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<TrainingProviderService>();
            services.AddTransient<CoursesService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<SubscriptionService>();
            services.AddTransient<ContentService>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();            
        }
    }
}
