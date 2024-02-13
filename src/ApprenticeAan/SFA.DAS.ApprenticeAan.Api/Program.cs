using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.ApprenticeAan.Api.AppStart;
using SFA.DAS.Telemetry.Startup;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration.BuildConfiguration();

builder.Services
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddTelemetryUriRedaction("firstName,lastName,dateOfBirth,email")
    .AddServiceRegistration(configuration)
    .AddServiceHealthChecks()
    .AddAuthentication(configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "ApprenticeAanOuterApi",
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


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApprenticeAanOuterApi");
        c.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication();

app.MapControllers();

app.Run();
