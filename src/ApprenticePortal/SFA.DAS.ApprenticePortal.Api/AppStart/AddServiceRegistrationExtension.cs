using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticePortal.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ApprenticePortal.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();            
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));

            services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
            services.AddTransient<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>, ApprenticeCommitmentsApiClient>();
            services.AddTransient<ProviderAccountApiClient>();
            services.AddTransient<CourseApiClient>();
            services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
            services.AddTransient<TrainingProviderService>();
            services.AddTransient<CoursesService>();
        }
    }
}
