using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SFA.DAS.Vacancies.Api.OpenApi
{
    public class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                // add swagger doc for each api version
                const string swaggerV1Endpoint = "/swagger/v1/swagger.json";

                var openApiInfo = new OpenApiInfo
                {
                    Title = "Display advert API",
                    Description = $"""
                                  <p>The new API version (version 2) lets you display apprenticeships that are available in more than one location.</p>
                                   
                                  <p>You must update to this new version by DD Month YYYY. Read more about changing the version you use in the Versioning section of this page.</p>
                                   
                                  <p>If you need to check your current implementation, you can view the old documentation for Display advert API (<a href='{swaggerV1Endpoint}'>version 1</a>).</p>
                                  """,
                    Version = description.ApiVersion.ToString(),

                };
                options.SwaggerDoc(description.GroupName, openApiInfo);
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }
    }
}
