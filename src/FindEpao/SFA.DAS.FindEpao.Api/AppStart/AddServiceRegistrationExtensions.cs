using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindEpao.Application.Courses.Services;
using SFA.DAS.FindEpao.Application.Epaos.Services;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.FindEpao.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<IAssessorsApiClient<AssessorsApiConfiguration>, AssessorsApiClient>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<ICachedDeliveryAreasService, CachedDeliveryAreasService>();
            services.AddTransient<ICachedCoursesService, CachedCoursesService>();
            services.AddTransient<ICourseEpaoIsValidFilterService, CourseEpaoIsValidFilterService>();
        }
    }
}