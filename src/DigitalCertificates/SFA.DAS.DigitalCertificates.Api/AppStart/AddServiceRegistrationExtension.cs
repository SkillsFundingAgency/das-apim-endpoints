using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Services;
using SFA.DAS.SharedOuterApi.Types.Services;
using DigitalCertificatesApiClient = SFA.DAS.DigitalCertificates.Contracts.Client.DigitalCertificatesApiClient;
using DigitalCertificatesApiConfiguration = SFA.DAS.DigitalCertificates.Contracts.Client.DigitalCertificatesApiConfiguration;
using IDigitalCertificatesApiClient = SFA.DAS.DigitalCertificates.Contracts.Client.IDigitalCertificatesApiClient<SFA.DAS.DigitalCertificates.Contracts.Client.DigitalCertificatesApiConfiguration>;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
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

            services.AddTransient<IAssessorsApiClient<AssessorsApiConfiguration>, AssessorsApiClient>();
            services.AddTransient<IDigitalCertificatesApiClient, DigitalCertificatesApiClient>();
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
        }
    }
}
