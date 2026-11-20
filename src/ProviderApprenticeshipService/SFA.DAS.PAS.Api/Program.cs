using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.PAS.Api.AppStart;

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
    .AddApplicationServices(configuration)
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

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PASOuterApi");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
