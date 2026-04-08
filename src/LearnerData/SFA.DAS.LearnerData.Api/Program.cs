using FluentValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using SFA.DAS.LearnerData.Api.AppStart;
using SFA.DAS.LearnerData.Api.Middleware;
using SFA.DAS.LearnerData.Application.CreateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Validators;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using System.Text.Json.Serialization;


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

builder.AddDistributedCache(configuration);

builder.Services.AddSingleton<ITelemetryInitializer, CorrelationTelemetryInitializer>();
builder.Host.AddNServiceBus(configuration);

builder.Logging.AddApplicationInsights();
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("SFA.DAS", LogLevel.Information);
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning);

builder.Services.AddAuthentication(configuration);
builder.Services.AddConfigurationOptions(configuration);

builder.Services.AddHealthChecks()
    .AddCheck<LearningApiHealthCheck>(LearningApiHealthCheck.HealthCheckResultDescription)
    .AddCheck<EarningsApiHealthCheck>(EarningsApiHealthCheck.HealthCheckResultDescription)
    .AddCheck<CollectionCalendarApiHealthCheck>(CollectionCalendarApiHealthCheck.HealthCheckResultDescription)
    .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription);

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateLearnerCommand).Assembly));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IValidator<CreateLearnerRequest>, CreateLearnerRequestValidator>();
builder.Services.AddScoped<IValidator<IEnumerable<LearnerDataRequest>>, BulkLearnerDataRequestsValidator>();

builder.Services.AddServices();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

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

app.UseMiddleware<StrictJsonValidationMiddleware<StubUpdateLearnerRequest>>();
app.UseMiddleware<StrictJsonValidationMiddleware<StubUpdateShortCourseRequest>>();
app.UseMiddleware<ConcurrencyTrackingMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }