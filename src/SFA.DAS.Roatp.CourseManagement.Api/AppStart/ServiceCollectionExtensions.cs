﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Roatp.CourseManagement.Api.Configuration;
using SFA.DAS.Roatp.CourseManagement.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System;

namespace SFA.DAS.Roatp.CourseManagement.Api.AppStart
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