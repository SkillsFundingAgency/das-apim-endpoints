using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.ExternalApi;
using SFA.DAS.ToolsSupport.Helpers;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Services;

namespace SFA.DAS.ToolsSupport.Api.AppStart;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IAccountsService, AccountsService>();
        services.AddTransient<IPayeSchemeObfuscator, PayeSchemeObfuscator>();
        services.AddTransient<IDatetimeService, DatetimeService>();
        services.AddTransient<IEmployerFinanceService, EmployerFinanceService>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IFinanceApiClient<FinanceApiConfiguration>, FinanceApiClient>();
        services.AddTransient<IHmrcApiClient<HmrcApiConfiguration>, HmrcApiClient<HmrcApiConfiguration>>();
        services.AddTransient<ISecureTokenHttpClient<TokenServiceApiConfiguration>, SecureTokenHttpClient>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IChallengeService, ChallengeService>();
        services.AddTransient<IFinanceDataService, FinanceDataService>();

        return services;
    }
}