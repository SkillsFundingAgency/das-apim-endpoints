using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.LearnerDataJobs.Api.AppStart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        //services.AddTransient<IAccountsService, AccountsService>();
        //services.AddTransient<IPayeSchemeObfuscator, PayeSchemeObfuscator>();
        //services.AddTransient<IDatetimeService, DatetimeService>();
        //services.AddTransient<IEmployerFinanceService, EmployerFinanceService>();
        //services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        //services.AddTransient<IFinanceApiClient<FinanceApiConfiguration>, FinanceApiClient>();
        //services.AddTransient<IHmrcApiClient<HmrcApiConfiguration>, HmrcApiClient<HmrcApiConfiguration>>();
        //services.AddTransient<ITokenApiClient<TokenServiceApiConfiguration>, TokenApiClient>();
        //services.AddTransient<ITokenService, TokenService>();
        //services.AddTransient<IChallengeService, ChallengeService>();
        //services.AddTransient<IFinanceDataService, FinanceDataService>();

        //services.AddTransient<IPendingChangesMapper, PendingChangesMapper>();
        return services;
    }
}