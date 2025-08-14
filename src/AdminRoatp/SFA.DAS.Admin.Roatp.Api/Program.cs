using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.Admin.Roatp.Api.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = builder.Configuration.BuildSharedConfiguration();

// Add services to the container.

builder.Services
    .AddLogging()
    .AddOpenTelemetryRegistration(configuration)
    .AddConfigurationOptions(configuration)
    .AddServiceHealthChecks()
    .AddAuthentication(configuration)
    .AddEndpointsApiExplorer()
    .AddSwagger(configuration)
    .AddServiceRegistrations()
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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminRoatpOuterApi");
    c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseHealthChecks();
app.UseAuthentication();
app.MapControllers();

app.Run();
