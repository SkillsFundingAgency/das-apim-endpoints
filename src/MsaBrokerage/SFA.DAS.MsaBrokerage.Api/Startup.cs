using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.MsaBrokerage.Api.AppStart;
using SFA.DAS.MsaBrokerage.Configuration;
using SFA.DAS.MsaBrokerage.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

namespace SFA.DAS.MsaBrokerage.Api;

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
        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromDays(90);
        });
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
                .AddCheck<CandidateApiHealthCheck>(CandidateApiHealthCheck.HealthCheckResultDescription)
                .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription);
        }

        // Course lookups are cached (see CourseService). In-memory locally; Redis elsewhere, as FAA does.
        if (_configuration.IsLocalOrDev())
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var msaBrokerageConfiguration = _configuration
                .GetSection(nameof(MsaBrokerageConfiguration))
                .Get<MsaBrokerageConfiguration>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = msaBrokerageConfiguration?.ApimEndpointsRedisConnectionString;
            });
        }

        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(MsaBrokerageConfiguration).Assembly));

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

        services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MsaBrokerageOuterApi", Version = "v1" });

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
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
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
            // HSTS configured to 90 days in ConfigureServices.
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MsaBrokerageOuterApi");
            c.RoutePrefix = string.Empty;
        });

        app.UseRouting();
        app.UseMiddleware<SecurityHeadersMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Applicants}/{action=index}/{id?}");
        });
    }
}
