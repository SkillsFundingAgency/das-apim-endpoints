using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SFA.DAS.FindApprenticeshipJobs.Api.AppStart;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging.ApplicationInsights;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory<UpdateableServiceProvider>(
    (IServiceProviderFactory<UpdateableServiceProvider>)new NServiceBusServiceProviderFactory());

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
                Title = "FindApprenticeshipJobsOuterApi",
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
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetLiveVacanciesQuery).Assembly));
builder.Services.AddCache(configuration);

builder.Host.ConfigureContainer<UpdateableServiceProvider>(containerBuilder =>
{
    Task.FromResult(containerBuilder.StartNServiceBus(configuration, "SFA.DAS.FAAJobs"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app
    .UseSwagger()
    .UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "FindApprenticeshipJobsOuterApi");
        s.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication();

app.MapControllers();

app.Run();

