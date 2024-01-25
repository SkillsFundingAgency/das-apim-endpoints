using System.Diagnostics.CodeAnalysis;
using AngleSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RoatpCourseManagement.Api.Infrastructure;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;
using SFA.DAS.RoatpCourseManagement.Configuration;
using SFA.DAS.RoatpCourseManagement.Infrastructure;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SFA.DAS.RoatpCourseManagement.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
            services.AddTransient<IRoatpServiceApiClient<RoatpConfiguration>, RoatpServiceApiClient>();
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<IDataDownloadService, DataDownloadService>();
            services.AddTransient<INationalAchievementRatesPageParser, NationalAchievementRatesPageParser>();
            services.AddTransient<IZipArchiveHelper, ZipArchiveHelper>();
            services.AddTransient<IUkrlpSoapSerializer, UkrlpSoapSerializer>();
            services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();

            AddLocationApiClient(services, configuration);
        }

        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<RoatpConfiguration>(configuration.GetSection(nameof(RoatpConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpConfiguration>>().Value);
            services.AddSingleton(BrowsingContext.New(AngleSharp.Configuration.Default.WithDefaultLoader()));
            services.Configure<UkrlpApiConfiguration>(configuration.GetSection(nameof(UkrlpApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<UkrlpApiConfiguration>>().Value);
        }

        private static void AddLocationApiClient(IServiceCollection services, IConfiguration configuration)
        {
            var apiConfig = GetApiConfiguration(configuration, "LocationApiConfiguration");

            services.AddRestEaseClient<ILocationApiClient>(apiConfig.Url)
                .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
        }

        private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
            => configuration.GetSection(configurationName).Get<InnerApiConfiguration>();
    }
}