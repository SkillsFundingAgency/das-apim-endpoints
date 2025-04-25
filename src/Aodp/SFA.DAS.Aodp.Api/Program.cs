using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using SFA.DAS.Aodp.Api;
using SFA.DAS.Aodp.Api.AppStart;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddServiceRegistration()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        //Helps with schema's of the same name by adding a number at the end
        var schemaHelper = new SwashbuckleSchemaHelper();
        c.CustomSchemaIds(type => schemaHelper.GetSchemaId(type));
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "AodpOuterApi",
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
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateFormVersionCommandHandler).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app
    .UseSwagger()
    .UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "AodpOuterApi");
        s.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication()
    .Use(async (context, next) =>
     {
         context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
         context.Response.Headers.Add("X-Frame-Options", "DENY");
         context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
         context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
         context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; object-src 'none';");
         await next();
     }); 

app.MapControllers();

app.Run();
