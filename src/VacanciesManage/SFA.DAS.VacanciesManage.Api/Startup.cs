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
using SFA.DAS.VacanciesManage.Api.AppStart;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;
using SFA.DAS.VacanciesManage.Configuration;

namespace SFA.DAS.VacanciesManage.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration.BuildSharedConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_env);

            services.AddConfigurationOptions(_configuration);

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

            services.AddMediatR(typeof(GetQualificationsQuery).Assembly);
            services.AddServiceRegistration();
            
            services.Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                }).AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true)
                .AddJsonOptions(options=>options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks();
            }
            
            if (_configuration.IsLocalOrDev())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                var configuration = _configuration
                    .GetSection("VacanciesManageConfiguration")
                    .Get<VacanciesManageConfiguration>();

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.ApimEndpointsRedisConnectionString;
                });
            }
            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recruitment API", Version = "v1", Description = "Create an advert on Find an apprenticeship using your existing systems."});
                var filePath = Path.Combine(AppContext.BaseDirectory,  $"{typeof(Startup).Namespace}.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            if (!_configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
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