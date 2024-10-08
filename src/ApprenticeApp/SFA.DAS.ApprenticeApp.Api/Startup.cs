using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Contentful.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.ApprenticeApp.Api.AppStart;
using SFA.DAS.ApprenticeApp.Api.ErrorHandler;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.ApprenticeApp.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private const string EndpointName = "SFA.DAS.PushNotifications";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration.BuildSharedConfiguration();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_env);

            services.AddConfigurationOptions(_configuration);

            ContentfulConfigMapping();

            if (!_configuration.IsLocalOrDev())
            {
                var azureAdConfiguration = _configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();
                var policies = new Dictionary<string, string>
                {
                    {"default", "APIM"}
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services.AddHealthChecks()
                .AddCheck<ApprenticeAccountsApiHealthCheck>(nameof(ApprenticeAccountsApiHealthCheck))
                .AddCheck<CoursesApiHealthCheck>(nameof(CoursesApiHealthCheck))
                .AddCheck<ApprenticeCommitmentsApiHealthCheck>(nameof(ApprenticeCommitmentsApiHealthCheck))
                .AddCheck<ApprenticeProgressApiHealthCheck>(nameof(ApprenticeProgressApiHealthCheck));

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetApprenticeDetailsQuery).Assembly));
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(UpsertApprenticeCommand).Assembly));

            services.AddServiceRegistration();

            services.AddSingleton<IContentTypeResolver, ContentfulEntityResolver>();
            services.AddContentfulServices(_configuration);

            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });           

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApprenticeAppOuterApi", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
                .UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"))
                .UseHealthChecks()
                .UseAuthentication();

            app.UseAuthentication();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=ApprenticeApp}/{action=index}/{id?}");
            });

            app.UseHealthChecks();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApprenticeAppOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }

        public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
        {
            serviceProvider.StartServiceBus(_configuration, EndpointName).GetAwaiter().GetResult();
        }

        public void ContentfulConfigMapping()
        {
            _configuration["ContentfulOptions:DeliveryApiKey"] = _configuration["ApprenticeAppContentfulOptions:ApprenticeAppDeliveryApiKey"];
            _configuration["ContentfulOptions:PreviewApiKey"] = _configuration["ApprenticeAppContentfulOptions:ApprenticeAppPreviewApiKey"];
            _configuration["ContentfulOptions:SpaceId"] = _configuration["ApprenticeAppContentfulOptions:ApprenticeAppSpaceId"];
            _configuration["ContentfulOptions:UsePreviewApi"] = _configuration["ApprenticeAppContentfulOptions:ApprenticeAppUsePreviewApi"];
            _configuration["ContentfulOptions:MaxNumberOfRateLimitRetries"] = _configuration["ApprenticeAppContentfulOptions:ApprenticeAppMaxNumberOfRateLimitRetries"];
        }
    }
}
