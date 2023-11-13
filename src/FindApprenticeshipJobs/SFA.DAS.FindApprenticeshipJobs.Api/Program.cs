using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SFA.DAS.FindApprenticeshipJobs.Api.AppStart;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;
using SFA.DAS.FindApprenticeshipJobs.Configuration;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

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
    .AddControllers()
    .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
         options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
     });

builder.Services.AddAuthentication();
builder.Services.AddConfigurationOptions(configuration);
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(typeof(GetLiveVacanciesQuery).Assembly);

if (configuration.IsLocalOrDev())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    var sp = builder.Services.BuildServiceProvider();
    var config = sp.GetService<FindApprenticeshipJobsConfiguration>();

    builder.Services.AddStackExchangeRedisCache((options) =>
    {
        options.Configuration = config.ApimEndpointsRedisConnectionString;
    });
}

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
