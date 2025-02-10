using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;
using SFA.DAS.ToolsSupport.Api.AppStart;
using SFA.DAS.ToolsSupport.Application.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddServiceRegistration(configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "ToolsSupportOuterApi",
                Version = "v1"
            });
    })
    .AddControllers(o =>
    {
        if (!configuration.IsLocalOrDev()) o.Filters.Add(new AuthorizeFilter("default"));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Logging.AddApplicationInsights();
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("SFA.DAS", LogLevel.Information);
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning);

builder.Services.AddAuthentication(configuration);
builder.Services.AddConfigurationOptions(configuration);
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetUsersByEmailQuery).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
    app.UseDeveloperExceptionPage();

app
    .UseSwagger()
    .UseSwaggerUI(s =>
    {
        s.RoutePrefix = "swagger";
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "ToolsSupportOuterApi");
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication();

app.MapControllers();

app.Run();