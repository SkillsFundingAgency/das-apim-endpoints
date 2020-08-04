using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.EmployerIncentives.Api.AppStart;
using SFA.DAS.EmployerIncentives.Api.ErrorHandler;
using SFA.DAS.EmployerIncentives.Api.HealthChecks;
using SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerIncentives.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration.BuildSharedConfiguration(env);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton(_env);
            services.Configure<EmployerIncentivesConfiguration>(_configuration.GetSection("EmployerIncentivesInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerIncentivesConfiguration>>().Value);
            services.Configure<CommitmentsConfiguration>(_configuration.GetSection("CommitmentsV2InnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsConfiguration>>().Value);
            
            if (!_configuration.IsLocalOrDev())
            {
                var azureAdConfiguration = _configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();
                services.AddAuthentication(azureAdConfiguration);
            }

            //services.AddHealthChecks()
            //    .AddCheck<EmployerIncentivesHealthCheck>(nameof(EmployerIncentivesHealthCheck))
            //    .AddCheck<CommitmentsHealthCheck>(nameof(CommitmentsHealthCheck));

            services.AddDasHealthChecks();

            services.AddMediatR(typeof(GetEligibleApprenticeshipsSearchQuery).Assembly);
            services.AddServiceRegistration();
            
            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerIncentivesOuterApi", Version = "v1" });
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
                    pattern: "api/{controller=EmployerIncentives}/{action=index}/{id?}");
            });

            app.UseHealthChecks();
        
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerIncentivesOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}