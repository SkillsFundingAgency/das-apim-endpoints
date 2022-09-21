using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RoatpCourseManagement.Api.Configuration;
using SFA.DAS.RoatpCourseManagement.Services;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System;

namespace SFA.DAS.RoatpCourseManagement.Api.AppStart
{
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
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
            services.AddTransient<ILocationLookupService, LocationLookupService>();
            services.AddTransient<IDataDownloadService, DataDownloadService>();
            services.AddTransient<INationalAchievementRatesPageParser, NationalAchievementRatesPageParser>();
            services.AddTransient<IZipArchiveHelper, ZipArchiveHelper>();
            ConfigureCourseDirectoryHttpClient(services, configuration);
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
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
        }

        private static void ConfigureCourseDirectoryHttpClient(IServiceCollection services, IConfiguration configuration)
        {
            var handlerLifeTime = TimeSpan.FromMinutes(5);
            services.AddHttpClient<ICourseDirectoryService, CourseDirectoryService>(config =>
            {
                var apiconfiguration = configuration
                    .GetSection(nameof(CourseDirectoryConfiguration))
                    .Get<CourseDirectoryConfiguration>();

                config.BaseAddress = new Uri(apiconfiguration.Url, UriKind.Absolute);
                config.DefaultRequestHeaders.Add("Accept", "application/json");
                config.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiconfiguration.Key);
            })
           .SetHandlerLifetime(handlerLifeTime);
        }
    }
}