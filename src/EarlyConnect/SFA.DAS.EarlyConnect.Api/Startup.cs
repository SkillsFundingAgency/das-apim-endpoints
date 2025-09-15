using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EarlyConnect.Api.AppStart;
using SFA.DAS.EarlyConnect.Api.ErrorHandler;
using SFA.DAS.EarlyConnect.Application.Queries;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Authentication;
using System.Runtime.InteropServices;

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
        // Configure Kestrel to disable CBC ciphers
        services.Configure<KestrelServerOptions>(options =>
        {
            options.AddServerHeader = false; // Disable 'Server' header
            
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.OnAuthenticate = (context, sslOptions) =>
                {
                    // Allow only non-CBC cipher suites
                    sslOptions.CipherSuitesPolicy = new CipherSuitesPolicy(
                        new[] {
                            // TLS 1.3 ciphers
                            TlsCipherSuite.TLS_AES_128_GCM_SHA256,
                            TlsCipherSuite.TLS_AES_256_GCM_SHA384,
                            TlsCipherSuite.TLS_CHACHA20_POLY1305_SHA256,
                            
                            // TLS 1.2 non-CBC ciphers
                            TlsCipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256,
                            TlsCipherSuite.TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384,
                            TlsCipherSuite.TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256,
                            TlsCipherSuite.TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384
                        }
                    );
                };
            });
        });

        services.AddOptions();
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

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(DummyQuery).Assembly));
        services.AddServiceRegistration(_configuration);

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
                .AddCheck<EarlyConnectApiHealthCheck>(EarlyConnectApiHealthCheck.HealthCheckResultDescription);
        }

        services.AddOpenTelemetryRegistration(_configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]!);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EarlyConnectOuterApi", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        // Security headers middleware - placed early in pipeline
        app.Use(async (context, next) =>
        {
            // Remove identifying headers
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-AspNet-Version");
            context.Response.Headers.Remove("X-AspNetMvc-Version");
            
            // Add security headers
            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            
            await next();
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthentication();
        app.UseHealthChecks();

        app.UseRouting();
        app.UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"));
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Users}/{action=Index}/{id?}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EarlyConnectOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}