using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EmployerFinance.Api.AppStart;
using SFA.DAS.EmployerFinance.Application.Queries.GetFrameworks;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.EmployerFinance.Api
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

            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetFrameworksQuery).Assembly));
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetAccountsQuery).Assembly));
            services.AddServiceRegistration();

            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                });

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription)
                    .AddCheck<RoatpCourseManagementApiHealthCheck>(RoatpCourseManagementApiHealthCheck.HealthCheckResultDescription);
            }

            services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerFinanceOuterApi", Version = "v1" });
            });

        }

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerFinanceOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}