using System.Net.Http;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Campaign.Application.Services;
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
            services.AddTransient<ICacheStorageService, CacheStorageService>();

            services.AddSingleton<IContentfulClient>(provider => new ContentfulClient(new HttpClient(), provider.GetService<ContentfulOptions>()));
            services.AddSingleton<IContentfulService, ContentfulService>(sp => new ContentfulService(sp.GetService<IContentfulClient>()));
        }
    }
}