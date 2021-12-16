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
using SFA.DAS.ApprenticeCommitments.Api.AppStart;
using SFA.DAS.ApprenticeCommitments.Api.ErrorHandler;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.ApprenticeCommitments.Infrastructure;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Collections.Generic;
using MediatR.Extensions.FluentValidation.AspNetCore;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApproval;

namespace SFA.DAS.ApprenticeCommitments.Api
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
            services.Configure<ApprenticeCommitmentsConfiguration>(_configuration.GetSection("ApprenticeCommitmentsInnerApi"));
            services.Configure<ApprenticeAccountsConfiguration>(_configuration.GetSection("ApprenticeAccountsInnerApi"));
            services.Configure<ApprenticeLoginConfiguration>(_configuration.GetSection("ApprenticeLoginApi"));
            services.Configure<CommitmentsV2Configuration>(_configuration.GetSection("CommitmentsV2InnerApi"));
            services.Configure<TrainingProviderConfiguration>(_configuration.GetSection("TrainingProviderApi"));
            services.Configure<CoursesApiConfiguration>(_configuration.GetSection("CoursesApi"));
            services.AddSingleton<IOwnerApiConfiguration>(s => s.GetRequiredService<ApprenticeCommitmentsConfiguration>());
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeCommitmentsConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsConfiguration>>().Value); 
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeLoginConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2Configuration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);

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
                .AddCheck<ApprenticeCommitmentsHealthCheck>(nameof(ApprenticeCommitmentsHealthCheck))
                .AddCheck<CommitmentsV2HealthCheck>(nameof(CommitmentsV2HealthCheck))
                .AddCheck<TrainingProviderApiHealthCheck>(nameof(TrainingProviderApiHealthCheck))
                .AddCheck<ApprenticeLoginApiHealthCheck>(nameof(ApprenticeLoginApiHealthCheck))
                .AddCheck<CoursesApiHealthCheck>(nameof(CoursesApiHealthCheck));

            services.AddMediatR(GetType().Assembly, typeof(CreateApprovalCommandHandler).Assembly);
            services.AddFluentValidation(new[] { typeof(CreateApprovalCommandHandler).Assembly });
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

            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApprenticeCommitmentsOuterApi", Version = "v1" });
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
                    pattern: "api/{controller=ApprenticeCommitments}/{action=index}/{id?}");
            });

            app.UseHealthChecks();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApprenticeCommitmentsOuterApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}