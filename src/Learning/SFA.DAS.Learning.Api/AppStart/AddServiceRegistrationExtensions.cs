using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Encoding;
using SFA.DAS.Learning.Application.Notification;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Learning.Api.AppStart;

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
        services.AddTransient<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>, CollectionCalendarApiClient>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IProviderAccountApiClient<ProviderAccountApiConfiguration>, ProviderAccountApiClient>();
        services.AddTransient<IExtendedNotificationService, ExtendedNotificationService>();
        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();


        services.AddSingleton(new UrlBuilder(configuration["ResourceEnvironmentName"]));

        var encodingList = configuration.GetSection(nameof(EncodingConfig.Encodings)).Get<List<SFA.DAS.Encoding.Encoding>>();
        var encodingConfig = new EncodingConfig { Encodings = encodingList };
        services.AddSingleton<IEncodingService, EncodingService>((sp) => new EncodingService(encodingConfig));
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddConfigurationOptions<CoursesApiConfiguration>(configuration);
        services.AddConfigurationOptions<LearningApiConfiguration>(configuration);
        services.AddConfigurationOptions<AzureActiveDirectoryConfiguration>(configuration, "AzureAd");
        services.AddConfigurationOptions<CommitmentsV2ApiConfiguration>(configuration);
        services.AddConfigurationOptions<AccountsConfiguration>(configuration, "AccountsInnerApi");
        services.AddConfigurationOptions<CollectionCalendarApiConfiguration>(configuration);
        services.AddConfigurationOptions<NServiceBusConfiguration>(configuration);

        services.AddConfigurationOptions<ProviderAccountApiConfiguration>(configuration);

    }

    private static void AddConfigurationOptions<T>(this IServiceCollection services, IConfiguration configuration, string? name = null) where T : class
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        services.Configure<T>(configuration.GetSection(name));
        services.AddSingleton(cfg => cfg.GetService<IOptions<T>>()!.Value);
    }

}
