using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Aodp.Api;
using SFA.DAS.Aodp.Api.AppStart;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using System.Text.Json.Serialization;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory<UpdateableServiceProvider>(
    new NServiceBusServiceProviderFactory());

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Host.ConfigureContainer<UpdateableServiceProvider>(containerBuilder =>
{
    containerBuilder.StartNServiceBus(configuration, "SFA.DAS.APIMAODP");
});

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

builder.Services.AddApplicationInsightsTelemetry(configuration);
builder.Services.Configure<ApplicationInsightsServiceOptions>(o =>
    o.RequestCollectionOptions.InjectResponseHeaders = false);

builder.Services.AddAuthentication(configuration);
builder.Services.AddConfigurationOptions(configuration);
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateFormVersionCommandHandler).Assembly));

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    if (!app.Environment.IsDevelopment())
        context.Response.Headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    await next();
});

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
    .UseAuthentication();

app.MapControllers();

app.Run();
