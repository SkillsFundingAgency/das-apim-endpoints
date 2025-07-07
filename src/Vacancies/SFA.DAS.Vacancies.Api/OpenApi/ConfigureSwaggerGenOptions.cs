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

                var openApiInfo = new OpenApiInfo
                {
                    Title = $"Display Advert API",
                    Description = """
                                  Get and display adverts from Find an apprenticeship. 
                                  **Note.** It is not recommended to use The Display Advert API directly from a browser and as such we have not enabled CORS for this API.  Instead, we recommend you call the API intermittently to retrieve the latest vacancies, store those vacancies in your own data store, and then change your website to read those vacancies from your own data store.
                                  """,
                    Version = description.ApiVersion.ToString()
                };
                if (description.IsDeprecated)
                {
                    openApiInfo.Description += "This API version has been deprecated.";
                }
                options.SwaggerDoc(description.GroupName, openApiInfo);
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }
    }
}