using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerAccounts.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerAccounts.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IFinanceApiClient<FinanceApiConfiguration>, FinanceApiClient>();
            services.AddTransient<IReservationApiClient<ReservationApiConfiguration>, ReservationApiClient>();
            services
                .AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>,
                    ProviderRelationshipsApiClient>();
            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
            services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
            services.AddTransient<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>, LevyTransferMatchingApiClient>();
            services.AddTransient<IReferenceDataApiClient<ReferenceDataApiConfiguration>, ReferenceDataApiClient>();
            AddReferenceDataApiClient(services, configuration);


            
        }
        private static void AddReferenceDataApiClient(IServiceCollection services, IConfiguration configuration)
        {
            var apiConfig = GetApiConfiguration(configuration, "ReferenceDataApiConfiguration");

            services
                .AddRestEaseClient<IReferenceDataApiClient>(apiConfig.Url)
                .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
        }

        private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>();
    }
}
public class InnerApiConfiguration
{
    public string Url { get; set; } = null!;
    public string Identifier { get; set; } = null!;
}