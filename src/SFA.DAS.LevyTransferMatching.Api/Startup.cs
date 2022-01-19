using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.LevyTransferMatching.Api.AppStart;
using SFA.DAS.LevyTransferMatching.Api.Authentication;
using SFA.DAS.LevyTransferMatching.Configuration;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration.BuildSharedConfiguration();
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var isLocalOrDev = _configuration.IsLocalOrDev();

            services.AddNLog();
            services.AddSingleton(_env);

            services.AddConfigurationOptions(_configuration);

            services.Configure<AccountsConfiguration>(_configuration.GetSection("AccountsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);

            services.Configure<EmployerAccountsConfiguration>(_configuration.GetSection("EmployerAccountsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerAccountsConfiguration>>().Value);

            services.Configure<CoursesApiConfiguration>(_configuration.GetSection("CoursesInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);

            if (!isLocalOrDev)
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

            services.AddSingleton<IAuthorizationHandler, PledgeAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.PledgeAccess, policy =>
                {
                    policy.Requirements.Add(new PledgeRequirement());
                });
            });

            services.AddServiceRegistration();
            services
                .AddMvc(o =>
                {
                    if (!isLocalOrDev)
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                        .AddCheck<LevyTransferMatchingApiHealthCheck>("Levy Transfer Matching Api Health Check");
            }

            services.AddMediatR(typeof(IAccountsService));

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            
            //services.AddNServiceBus();

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LevyTransferMatchingOuterApi", Version = "v1" });
                })
                .AddSwaggerGenNewtonsoftSupport();
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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=demand}/{action=index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LevyTransferMatchingOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}