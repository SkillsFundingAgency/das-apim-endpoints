using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Funding.Application.Services;
using SFA.DAS.Funding.Clients;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Funding.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        { 
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));

            services.AddTransient<IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration>, FundingApprenticeshipEarningsApiClient>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
            services.AddTransient<IFundingApprenticeshipEarningsService, FundingApprenticeshipEarningsService>();
            services.AddTransient<IFundingProviderEarningsService, FundingProviderEarningsService>();
            services.AddTransient<IApprenticeshipsApiClient<ApprenticeshipsConfiguration>, ApprenticeshipsApiClient>();
            services.AddTransient<IApprenticeshipsService, ApprenticeshipsService>();
            services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();
        }
    }
}