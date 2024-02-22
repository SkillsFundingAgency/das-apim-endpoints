using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EmployerIncentives.Api.AppStart;
using SFA.DAS.EmployerIncentives.Api.ErrorHandler;
using SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.EmployerIncentives.Api
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
            services.AddNLog();
            services.AddOptions();
            services.AddSingleton(_env);
            services.Configure<EmployerIncentivesConfiguration>(_configuration.GetSection("EmployerIncentivesInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerIncentivesConfiguration>>().Value);
            services.Configure<CommitmentsConfiguration>(_configuration.GetSection("CommitmentsV2InnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsConfiguration>>().Value);
            services.Configure<AccountsConfiguration>(_configuration.GetSection("AccountsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<CustomerEngagementFinanceConfiguration>(_configuration.GetSection("CustomerEngagementFinanceApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CustomerEngagementFinanceConfiguration>>().Value);
            services.Configure<EmploymentCheckConfiguration>(_configuration.GetSection("EmploymentCheckInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmploymentCheckConfiguration>>().Value);
            services.Configure<EmployerProfilesApiConfiguration>(_configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);

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
                .AddCheck<EmployerIncentivesHealthCheck>(nameof(EmployerIncentivesHealthCheck))
                .AddCheck<CommitmentsHealthCheck>(nameof(CommitmentsHealthCheck))
                .AddCheck<CustomerEngagementFinanceApiHealthCheck>(nameof(CustomerEngagementFinanceApiHealthCheck))
                .AddCheck<AccountsApiHealthCheck>(nameof(AccountsApiHealthCheck))
                ;

            services.AddMediatR(typeof(GetEligibleApprenticeshipsSearchQuery).Assembly);
            services.AddServiceRegistration();

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