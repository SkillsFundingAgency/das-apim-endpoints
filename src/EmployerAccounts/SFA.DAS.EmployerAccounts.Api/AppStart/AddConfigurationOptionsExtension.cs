using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerAccounts.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<ReservationApiConfiguration>(configuration.GetSection(nameof(ReservationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ReservationApiConfiguration>>().Value);
            services.Configure<FinanceApiConfiguration>(configuration.GetSection(nameof(FinanceApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FinanceApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
            services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection(nameof(ProviderRelationshipsApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);
            services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection(nameof(CommitmentsV2ApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
            services.Configure<LevyTransferMatchingApiConfiguration>(configuration.GetSection(nameof(LevyTransferMatchingApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LevyTransferMatchingApiConfiguration>>().Value);
            services.Configure<ReferenceDataApiConfiguration>(configuration.GetSection(nameof(ReferenceDataApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ReferenceDataApiConfiguration>>().Value);
            services.Configure<EducationalOrganisationApiConfiguration>(configuration.GetSection(nameof(EducationalOrganisationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EducationalOrganisationApiConfiguration>>().Value);
        }
    }
}