using System.Diagnostics.CodeAnalysis;
using MediatR;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Services;


namespace SFA.DAS.EmployerAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{

    private static readonly string Ready = "ready";
    //MFCMFC to do add healthchecks

    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));

        return services;
    }

    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddHttpClient();
        services.AddMediatR(typeof(GetMyApprenticeshipQuery).Assembly);
        services.AddMediatR(typeof(GetEmployerMemberQuery).Assembly);
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();

        AddAanHubApiClient(services, configuration);
        AddCommitmentsV2ApiClient(services, configuration);
        AddApprenticeAccountsApiClient(services, configuration);
        AddAccountsApiClient(services, configuration);
        AddCoursesApiClient(services, configuration);
        AddEmployerProfilesApiClient(services, configuration);
        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        if (configuration.IsLocalAcceptanceTestsOrDev() || configuration.IsLocal()) return services;

        var azureAdConfiguration = configuration
            .GetSection("AzureAd")
            .Get<AzureActiveDirectoryConfiguration>();
        var policies = new Dictionary<string, string>
        {
            {"default", "APIM"}
        };

        services.AddAuthentication(azureAdConfiguration, policies);

        return services;
    }

    // private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    // {
    //     var apiConfig = configuration
    //             .GetSection(nameof(AanHubApiConfiguration))
    //             .Get<AanHubApiConfiguration>();
    //
    //     services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection(nameof(ApprenticeAccountsApiConfiguration)));
    //     services.AddSingleton(c => c.GetService<IOptions<ApprenticeAccountsApiConfiguration>>()!.Value);
    //
    //     services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
    //     services.AddSingleton(c => c.GetService<IOptions<CoursesApiConfiguration>>()!.Value);
    //
    //     services.AddSingleton(apiConfig);
    //
    //     services.AddScoped<AanHubApiHttpMessageHandler>();
    //
    //     services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
    //         .AddHttpMessageHandler<AanHubApiHttpMessageHandler>();
    // }

    private static void AddApprenticeAccountsApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "ApprenticeAccountsApiConfiguration");

        services.AddRestEaseClient<IApprenticeAccountsApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));

    }

    private static void AddAccountsApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "AccountsInnerApi");

        services.AddRestEaseClient<IAccountsApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));

    }

    private static void AddCoursesApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "CoursesApiConfiguration");

        services.AddRestEaseClient<ICoursesApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));

    }

    private static void AddEmployerProfilesApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "EmployerProfilesApiConfiguration");

        services.AddRestEaseClient<IEmployerProfilesApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));

    }

    private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "AanHubApiConfiguration");

        services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));

    }

    private static void AddCommitmentsV2ApiClient(IServiceCollection services, IConfiguration configuration)
    {

        var apiConfig = GetApiConfiguration(configuration, "CommitmentsV2ApiConfiguration");

        services.AddSingleton(apiConfig);
        services.AddRestEaseClient<ICommitmentsV2ApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>()!;
}
