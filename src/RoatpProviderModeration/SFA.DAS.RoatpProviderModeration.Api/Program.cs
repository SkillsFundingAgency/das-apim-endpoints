using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using NLog.Web;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Queries.GetProvider;
using SFA.DAS.RoatpProviderModeration.OuterApi.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseNLog();

// Add services to the container.

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddConfigurationOptions(configuration)
    .AddApplicationInsightsTelemetry()
    .AddServiceRegistration()
    .AddAuthentication(configuration)
    .AddEndpointsApiExplorer()
    .AddMediatR(typeof(GetProviderQuery).Assembly)
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "RoatpProviderModerationOuterApi", Version = "v1" });
    })
    .AddControllers(o =>
    {
        if (!configuration.IsLocalOrDev())
        {
            o.Filters.Add(new AuthorizeFilter("default"));
        }
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddHealthChecks();

builder.Services.AddServiceRegistration();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

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
