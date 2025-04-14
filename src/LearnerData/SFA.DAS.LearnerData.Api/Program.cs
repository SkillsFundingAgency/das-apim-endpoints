using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;
using SFA.DAS.LearnerData.Application;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.LearnerData.Api.AppStart;
using System.Net;
using Azure.Identity;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "LearnerDataOuterApi",
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

builder.Services.AddSingleton<IMessageSession>(provider =>
{
    var endpointConfiguration = new EndpointConfiguration("SFA.DAS.LearnerData.OuterApi");
    endpointConfiguration.EnableInstallers();
    endpointConfiguration.UseMessageConventions();
    endpointConfiguration.UseNewtonsoftJsonSerializer();

    endpointConfiguration.SendOnly();

    var nsbConnection = configuration["NServiceBusConfiguration:NServiceBusConnectionString"];
    var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
    transport.CustomTokenCredential(new DefaultAzureCredential());
    transport.ConnectionString(nsbConnection);

    var decodedLicence = WebUtility.HtmlDecode(configuration["NServiceBusConfiguration:NServiceBusLicense"]);
    if (!string.IsNullOrWhiteSpace(decodedLicence)) endpointConfiguration.License(decodedLicence);

    return NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
});

builder.Logging.AddApplicationInsights();
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("SFA.DAS", LogLevel.Information);
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning);

builder.Services.AddAuthentication(configuration);
builder.Services.AddConfigurationOptions(configuration);
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ProcessLearnersCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger()
    .UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnerDataOuterApi");
        s.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication();

app.MapControllers();

app.Run();