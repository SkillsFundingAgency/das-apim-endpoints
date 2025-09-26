using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Encoding;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Apprenticeships.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<ILearningApiClient<LearningApiConfiguration>, LearningApiClient>();
        services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
        services.AddTransient<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>, CollectionCalendarApiClient>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IProviderAccountApiClient<ProviderAccountApiConfiguration>, ProviderAccountApiClient>();
        services.AddTransient<IExtendedNotificationService, ExtendedNotificationService>();


        services.AddSingleton(new UrlBuilder(configuration["ResourceEnvironmentName"]));

        var encodingList = configuration.GetSection(nameof(EncodingConfig.Encodings)).Get<List<SFA.DAS.Encoding.Encoding>>();
        var encodingConfig = new EncodingConfig { Encodings = encodingList };
        services.AddSingleton<IEncodingService, EncodingService>((sp)=>new EncodingService(encodingConfig));
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>()!.Value);

        services.Configure<LearningApiConfiguration>(configuration.GetSection(nameof(LearningApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LearningApiConfiguration>>()!.Value);

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>()!.Value);

        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection(nameof(CommitmentsV2ApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>()!.Value);

        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>()!.Value);

        services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>()!.Value);

		services.Configure<CollectionCalendarApiConfiguration>(configuration.GetSection(nameof(CollectionCalendarApiConfiguration)));
		services.AddSingleton(cfg => cfg.GetService<IOptions<CollectionCalendarApiConfiguration>>()!.Value);

        services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>()!.Value);

        services.Configure<ProviderAccountApiConfiguration>(configuration.GetSection(nameof(ProviderAccountApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderAccountApiConfiguration>>()!.Value);
    }

}