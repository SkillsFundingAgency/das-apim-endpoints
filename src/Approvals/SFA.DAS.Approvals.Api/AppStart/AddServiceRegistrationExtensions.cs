using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Approvals.Api.Clients;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Approvals.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        private static void AddCommitmentApiInternalClient(IServiceCollection services, IConfiguration configuration)
        {
            bool useLocalDevClient =
                    configuration.IsLocalOrDev() && configuration["UseLocalDevCommitmentApiClient"] == "True";

            if (useLocalDevClient)
            {
                services.AddTransient<IInternalApiClient<CommitmentsV2ApiConfiguration>, LocalCommitmentsApiInternalApiClient>();
            }
            else
            {
                services.AddTransient<IInternalApiClient<CommitmentsV2ApiConfiguration>, CommitmentsApiInternalApiClient>();
            }
        }
      

        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<IFjaaApiClient<FjaaApiConfiguration>, FjaaApiClient>();
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IAssessorsApiClient<AssessorsApiConfiguration>, AssessorsApiClient>();
            services.AddTransient<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>, CourseDeliveryApiClient>();
            services.AddTransient<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>, ApprenticeCommitmentsApiClient>();
            services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
            services.AddTransient<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>, LevyTransferMatchingApiClient>();
            services.AddTransient<IProviderAccountApiClient<ProviderAccountApiConfiguration>, ProviderAccountApiClient>();
            services.AddTransient<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>, ProviderCoursesApiClient>();
            services.AddTransient<IProviderPaymentEventsApiClient<ProviderEventsConfiguration>, ProviderPaymentEventsApiClient>();
            AddCommitmentApiInternalClient(services, configuration);
            services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
            services.AddTransient<IReservationApiClient<ReservationApiConfiguration>, ReservationApiClient>();
            services.AddTransient<IDeliveryModelService, DeliveryModelService>();
            services.AddServiceParameters();
        }
    }
}