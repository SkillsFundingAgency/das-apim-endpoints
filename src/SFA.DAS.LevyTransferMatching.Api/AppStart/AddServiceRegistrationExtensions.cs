using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.LevyTransferMatching.Configuration;
using SFA.DAS.LevyTransferMatching.Clients;

namespace SFA.DAS.LevyTransferMatching.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>, LevyTransferMatchingApiClient>();
            services.AddTransient<ILevyTransferMatchingService, LevyTransferMatchingService>();
            services.AddTransient<IReferenceDataService, ReferenceDataService>();
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<IEmployerAccountsApiClient<EmployerAccountsConfiguration>, EmployerAccountsApiClient>();

            services.AddTransient<IAccountsService, AccountsService>();

            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
