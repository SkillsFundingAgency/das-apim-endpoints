using System.Collections.Generic;
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
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Funding.Api.AppStart;
using SFA.DAS.Funding.Api.ErrorHandler;
using SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.Infrastructure;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.Funding.Api
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
            services.Configure<FundingApprenticeshipEarningsConfiguration>(_configuration.GetSection("FundingApprenticeshipEarningsInnerApi"));
            services.Configure<ApprenticeshipsConfiguration>(_configuration.GetSection("ApprenticeshipsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FundingApprenticeshipEarningsConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeshipsConfiguration>>().Value);

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
                .AddCheck<FundingApprenticeshipEarningsHealthCheck>(nameof(FundingApprenticeshipEarningsHealthCheck));

            services.AddMediatR(typeof(GetProviderEarningsSummaryQuery).Assembly);
            services.AddServiceRegistration();

            services
                .AddMvc(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });


            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FundingOuterApi", Version = "v1" });
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
                    pattern: "api/{controller=Funding}/{action=index}/{id?}");
            });

            app.UseHealthChecks();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FundingOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}