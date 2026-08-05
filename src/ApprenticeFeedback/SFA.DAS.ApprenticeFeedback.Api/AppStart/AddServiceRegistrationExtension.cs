using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.ApprenticeFeedback.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));

            services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
            services.AddTransient<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>, ApprenticeFeedbackApiClient>();
            services.AddTransient<IAssessorsApiClient<AssessorsApiConfiguration>, AssessorsApiClient>();

            services.AddTransient<IApprenticeshipDetailsService, ApprenticeshipDetailsService>();
        }
    }
}
