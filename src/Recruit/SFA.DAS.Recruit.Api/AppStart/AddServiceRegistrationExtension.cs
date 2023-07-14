using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Recruit.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<ILocationLookupService, LocationLookupService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
            services.AddTransient<ITrainingProviderService, TrainingProviderService>();
            services.Configure<TrainingProviderConfiguration>(configuration.GetSection("TrainingProviderApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderConfiguration>>().Value);
        }
    }
}