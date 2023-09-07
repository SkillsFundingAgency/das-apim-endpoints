using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.VacanciesManage.Api.AppStart;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;
using SFA.DAS.VacanciesManage.Configuration;

namespace SFA.DAS.VacanciesManage.Api
{
    public static class Startup
    {
        public static void ConfigureServices(
            IServiceCollection services, 
            IWebHostEnvironment environment, 
            IConfigurationRoot configuration)
        {
            services.AddSingleton(environment);

            services.AddConfigurationOptions(configuration);

            if (!configuration.IsLocalOrDev())
            {
                var azureAdConfiguration = configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();
                var policies = new Dictionary<string, string>
                {
                    {"default", "APIM"}
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services.AddMediatR(typeof(GetQualificationsQuery).Assembly);
            services.AddServiceRegistration();
            
            services.Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                }).AddMvc(o =>
                {
                    if (!configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            if (configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddCheck<CoursesApiHealthCheck>("Courses API health check");
            }
            
            if (configuration.IsLocalOrDev())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                var vacanciesManageConfiguration = configuration
                    .GetSection("VacanciesManageConfiguration")
                    .Get<VacanciesManageConfiguration>();

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = vacanciesManageConfiguration.ApimEndpointsRedisConnectionString;
                });
            }
            
            //todo obsolete, should use connectionstring instead https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560
            //services.AddApplicationInsightsTelemetry(options => options.ConnectionString = configuration.GetConnectionString("ApplicationInsights"));
            services.AddApplicationInsightsTelemetry();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recruitment API", Version = "v1", Description = "Create an advert on Find an apprenticeship using your existing systems."});
                var filePath = Path.Combine(AppContext.BaseDirectory,  $"{typeof(Startup).Namespace}.xml");
                c.IncludeXmlComments(filePath);
            });
        }
        
        public static void ConfigureApp(IApplicationBuilder app, IConfigurationRoot configuration)
        {
            app.UseAuthentication();

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseHealthChecks();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=vacancy}/{action=index}/{id?}");
            });
        
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "recruitment-api-live-name");
                c.DocumentTitle = "";
                c.RoutePrefix = string.Empty;
            });
        }
    }
}