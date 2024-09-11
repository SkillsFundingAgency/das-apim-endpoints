using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.Earnings.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient(typeof(ITokenPassThroughInternalApiClient<>), typeof(TokenPassThroughInternalApiClient<>));
        services.AddTransient<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>, ApprenticeshipsApiClient>();
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

		services.Configure<ApprenticeshipsApiConfiguration>(configuration.GetSection(nameof(ApprenticeshipsApiConfiguration)));
		services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeshipsApiConfiguration>>()!.Value);
	}

}