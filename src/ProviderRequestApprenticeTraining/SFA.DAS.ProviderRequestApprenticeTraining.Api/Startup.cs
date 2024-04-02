using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.AppStart;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Provider.DfeSignIn.Auth.Application.Queries.ProviderAccounts;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api
{
    [ExcludeFromCodeCoverage]
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
            
                services.AddHealthChecks()
                    .AddCheck<RoatpCourseManagementApiHealthCheck>(RoatpCourseManagementApiHealthCheck.HealthCheckResultDescription)
                    .AddCheck<RequestApprenticeTrainingApiHealthCheck>(RequestApprenticeTrainingApiHealthCheck.HealthCheckResultDescription);
            }

            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetEmployerRequestQuery).Assembly));
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetRoatpV2ProviderQuery).Assembly));

            services.AddServiceRegistration();

            services
                .AddControllers(o =>
                {
                    if (!_configuration.IsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
            });

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ProviderRequestApprenticeTrainingOuterApi", Version = "v1" });

                if (!_configuration.IsLocalOrDev())
                {
                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            if (!_configuration.IsLocalOrDev())
            {
                app.UseHealthChecks();
            }
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProviderRequestApprenticeTrainingOuterApi");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Account}/{action=index}/{id?}");
            });

        }
    }
}
