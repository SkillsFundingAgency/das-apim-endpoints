using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using AssessorsApiClient = SFA.DAS.SharedOuterApi.Types.Services.AssessorsApiClient;
using IAssessorsApiClient = SFA.DAS.SharedOuterApi.Types.Interfaces.IAssessorsApiClient<SFA.DAS.SharedOuterApi.Types.Configuration.AssessorsApiConfiguration>;

namespace SFA.DAS.Admin.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>, DigitalCertificatesApiClient>();
            services.AddTransient<IAssessorsApiClient, AssessorsApiClient>();
        }
    }
}
