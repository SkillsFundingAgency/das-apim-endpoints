using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.EmployerAan.Api.AppStart;
using SFA.DAS.EmployerAan.Api.Extensions;
using SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddConfigurationOptions(configuration)
    .AddServiceRegistration(configuration)
    .AddServiceHealthChecks()
    .AddAuthentication(configuration)
    .AddTelemetryRegistration((IConfigurationRoot)builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(x => x.FullName?.Replace("+", "_"));
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "EmployerAanOuterApi",
                Version = "v1"
            });
    })
    .AddControllers(o =>
    {
        if (!configuration.IsLocal()) o.Filters.Add(new AuthorizeFilter("default"));
    })
    .AddApplicationPart(typeof(AccountUsersController).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerAanOuterApi");
        c.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication();

app.MapControllers();

await app.RunAsync();
