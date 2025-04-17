using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.ErrorHandling;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Approvals.Api
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
                    { "default", "APIM" }
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetStandardsQuery).Assembly));
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetAccountsQuery).Assembly));
            services.AddServiceRegistration(_configuration);
            services.AddAutoMapper(typeof(Startup));

            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                });

            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription)
                    .AddCheck<RoatpCourseManagementApiHealthCheck>(RoatpCourseManagementApiHealthCheck.HealthCheckResultDescription)
                    .AddCheck<ApprenticeCommitmentsApiHealthCheck>(ApprenticeCommitmentsApiHealthCheck.HealthCheckResultDescription)
                    .AddCheck<ApprenticeAccountsApiHealthCheck>(ApprenticeAccountsApiHealthCheck.HealthCheckResultDescription)
                    .AddCheck<ProviderCoursesApiHealthCheck>(ProviderCoursesApiHealthCheck.HealthCheckResultDescription);
            }

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApprovalsOuterApi", Version = "v1" });
            });

            services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.Headers.ContainsKey("X-Powered-By"))
                    {
                        context.Response.Headers.Remove("X-Powered-By");
                    }
                    return Task.CompletedTask;
                });
                await next();
            });

            app.UseAuthentication();

            app.UseRouting();
            app.UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=TrainingCourses}/{action=GetList}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApprovalsOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}