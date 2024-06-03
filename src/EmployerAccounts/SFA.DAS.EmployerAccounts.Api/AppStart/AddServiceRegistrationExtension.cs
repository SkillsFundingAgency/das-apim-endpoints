using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using PublicSectorOrganisationApiConfiguration = SFA.DAS.SharedOuterApi.Configuration.PublicSectorOrganisationApiConfiguration;

namespace SFA.DAS.EmployerAccounts.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient<IOrganisationApiStrategyFactory, OrganisationApiStrategyFactory>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IFinanceApiClient<FinanceApiConfiguration>, FinanceApiClient>();
            services.AddTransient<IReservationApiClient<ReservationApiConfiguration>, ReservationApiClient>();
            services.AddTransient<IOrganisationApiStrategyFactory, OrganisationApiStrategyFactory>();
            services
                .AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>,
                    ProviderRelationshipsApiClient>();
            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
            services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
            services.AddTransient<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>, LevyTransferMatchingApiClient>();
            services.AddTransient<IReferenceDataApiClient<ReferenceDataApiConfiguration>, ReferenceDataApiClient>();
            services.AddTransient<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>, EducationalOrganisationApiClient>();
            services.AddTransient<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>, PublicSectorOrganisationApiClient>();
        }
    }
}