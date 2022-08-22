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
using SFA.DAS.Reservations.Api.AppStart;
using SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Reservations.Api
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

            services.Configure<AccountsConfiguration>(_configuration.GetSection("AccountsInnerApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<FinanceApiConfiguration>(_configuration.GetSection("EmployerFinanceApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FinanceApiConfiguration>>().Value);
            services.Configure<LevyTransferMatchingApiConfiguration>(_configuration.GetSection("LevyTransferMatchingApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LevyTransferMatchingApiConfiguration>>().Value);
            services.Configure<CommitmentsV2ApiConfiguration>(_configuration.GetSection("CommitmentsV2ApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);

            services.AddMediatR(typeof(GetTrainingCoursesQuery).Assembly);
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
                    .AddCheck<FinanceApiHealthCheck>("EmployerFinance API health check")
                    .AddCheck<CoursesApiHealthCheck>("Courses API health check")
                    .AddCheck<CourseDeliveryApiHealthCheck>("CourseDelivery API health check")
                    .AddCheck<CommitmentsV2HealthCheck>("CommitmentsV2 API health check");      
            }
            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReservationsOuterApi", Version = "v1" });
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
                    pattern: "api/{controller=TrainingCourses}/{action=GetList}/{id?}");
            });
        
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReservationsOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
