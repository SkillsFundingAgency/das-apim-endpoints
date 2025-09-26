using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.FindAnApprenticeship.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
            services.AddTransient<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>, FindApprenticeshipApiClient>();
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<ICandidateApiClient<CandidateApiConfiguration>, CandidateApiClient>();
            services.AddTransient<IRecruitApiClient<RecruitApiConfiguration>, RecruitApiClient>();
            services.AddTransient<IRecruitApiClient<RecruitApiV2Configuration>, RecruitApiV2Client>();
            services.AddTransient<ILocationLookupService, LocationLookupService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<IVacancyService, VacancyService>();
			services.AddSingleton<IDateTimeService>(new DateTimeService());
            services.AddTransient<INotificationService, NotificationService>();
            services.AddSingleton(new EmailEnvironmentHelper(configuration["ResourceEnvironmentName"]));
            services.AddTransient<ITotalPositionsAvailableService, TotalPositionsAvailableService>();
        }
    }
}