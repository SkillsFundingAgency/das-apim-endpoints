using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Services;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Apim.Shared.Services;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));

            services.AddTransient<INotificationService, NotificationService>();

            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>, RequestApprenticeTrainingApiClient>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();

            services.AddTransient<IEmployerAccountsService,  EmployerAccountsService>();
        }
    }
}
