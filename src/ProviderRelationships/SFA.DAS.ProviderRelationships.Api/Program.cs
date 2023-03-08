using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using NLog.Web;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.ProviderRelationships.Api.AppStart;
using SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNLog();

var configuration = builder.Configuration.BuildSharedConfiguration();

Startup.ConfigureServices(builder.Services, builder.Environment, configuration);

var app = builder.Build();
Startup.ConfigureApp(app, configuration);
app.Run();

public static class Startup
{
    public static void ConfigureServices(
        IServiceCollection services,
        IWebHostEnvironment environment,
        IConfigurationRoot configuration)
    {
        services.AddSingleton(environment);
        services.AddConfigurationOptions(configuration);

        if (!configuration.IsLocalOrDev())
        {
            var azureAdConfiguration = configuration
                .GetSection("AzureAd")
                .Get<AzureActiveDirectoryConfiguration>();
            var policies = new Dictionary<string, string>
            {
                {"default", "APIM"}
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }

        services.AddMediatR(typeof(GetAccountsQuery).Assembly);
        services.AddServiceRegistration();

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        }).AddMvc(o =>
        {
            if (!configuration.IsLocalOrDev())
            {
                o.Filters.Add(new AuthorizeFilter("default"));
            }
        }).AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        if (configuration["Environment"] != "DEV")
        {
            services.AddHealthChecks();
        }
        
        //todo obsolete, should use connectionstring instead https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560
        //services.AddApplicationInsightsTelemetry(options => options.ConnectionString = configuration.GetConnectionString("ApplicationInsights"));
        services.AddApplicationInsightsTelemetry(configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProviderRelationshipsOuterApi", Version = "v1" });
        });
    }
    
    public static void ConfigureApp(IApplicationBuilder app, IConfigurationRoot configuration)
    {
        app.UseAuthentication();

        if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            app.UseHealthChecks();
        }
                
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=EmployerAccounts}/{action=index}/{id?}");
        });
            
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProviderRelationshipsOuterApi");
            c.RoutePrefix = string.Empty;
        });
    }
}