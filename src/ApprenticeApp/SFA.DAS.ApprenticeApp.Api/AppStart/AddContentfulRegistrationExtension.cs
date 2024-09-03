using System.Diagnostics.CodeAnalysis;
using Contentful.AspNetCore;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApprenticeApp.Client;

namespace SFA.DAS.ApprenticeApp.Api.AppStart
{
    public static class AddContentfulRegistrationExtension
    {
        [ExcludeFromCodeCoverage]
        public static void AddContentfulServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContentful(configuration);

            var builder = services.BuildServiceProvider();

            services.AddTransient<IContentClient>(
                x => new ContentClient(
                    builder.GetRequiredService<IContentTypeResolver>(),
                    builder.GetRequiredService<IContentfulClient>(),
                    configuration)
            );
        }
    }
}
