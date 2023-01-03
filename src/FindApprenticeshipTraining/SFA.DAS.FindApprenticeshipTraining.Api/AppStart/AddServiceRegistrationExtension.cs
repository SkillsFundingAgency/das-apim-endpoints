using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
namespace SFA.DAS.FindApprenticeshipTraining.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>, CourseDeliveryApiClient>();
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
            services.AddTransient<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>, ApprenticeFeedbackApiClient>();
            services.AddTransient<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>, EmployerFeedbackApiClient>();
            services.AddTransient<IShortlistApiClient<ShortlistApiConfiguration>, ShortlistApiClient>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<ICachedCoursesService, CachedCoursesService>();
            services.AddTransient<IShortlistService, ShortlistService>();
            services.AddTransient<ILocationLookupService, LocationLookupService>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
        }
    }
}