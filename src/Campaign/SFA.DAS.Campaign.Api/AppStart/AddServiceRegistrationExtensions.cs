using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Campaign.Application.Services;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Campaign.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
            services.AddTransient<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>, FindApprenticeshipApiClient>();
            services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ILocationLookupService, LocationLookupService>();

            services.AddTransient(typeof(IContentfulApiClient<>), typeof(ContentfulApiClient<>));

            services.AddTransient<IContentfulMasterApiClient<ContentfulApiConfiguration>, ContentfulMasterApiClient>();
            services.AddTransient<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>, ContentfulPreviewApiClient>();

            services.AddTransient(typeof(IGetApiClient<>), typeof(ContentfulApiClient<>));
            
            services.AddTransient<IReliableCacheStorageService, ReliableCacheStorageService<ContentfulApiConfiguration>>(
                cfg =>
                {
                    var client = cfg.GetService<IContentfulMasterApiClient<ContentfulApiConfiguration>>();
                    var cache = cfg.GetService<ICacheStorageService>();
                    return new ReliableCacheStorageService<ContentfulApiConfiguration>(client, cache);
                });
            
        }
    }
}