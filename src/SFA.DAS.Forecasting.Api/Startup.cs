using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.Forecasting.Api.AppStart;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetFrameworkCoursesList;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Forecasting.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration.BuildSharedConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton(_env);
            services.Configure<CoursesApiConfiguration>(_configuration.GetSection("CoursesApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(_configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<LevyTransferMatchingApiConfiguration>(_configuration.GetSection("LevyTransferMatchingApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LevyTransferMatchingApiConfiguration>>().Value);


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

            services.AddMediatR(typeof(GetFrameworkCoursesQuery).Assembly);
                
            services.AddServiceRegistration();

            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddCheck<CoursesApiHealthCheck>("Courses API health check");
            }

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForecastingOuterApi", Version = "v1" });
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
                    pattern: "api/{controller=Standards}/{action=index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ForecastingOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
