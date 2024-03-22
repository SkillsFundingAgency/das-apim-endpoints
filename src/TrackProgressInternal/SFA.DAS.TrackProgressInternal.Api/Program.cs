using MediatR;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.TrackProgressInternal.Api;
using SFA.DAS.TrackProgressInternal.Api.AppStart;
using SFA.DAS.TrackProgressInternal.Application.Commands.TrackProgress;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.BuildSharedConfiguration();
var config = configuration.Get<TrackProgressConfiguration>();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrackProgressInternalOuterApi", Version = "v1" });
});

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(PopulateKsbsCommand).Assembly));
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddServiceRegistration();
builder.Services.AddConfigurationOptions(configuration);
builder.Services.AddHealthChecks();

if (!builder.Configuration.IsLocalOrDev())
{
    builder.Services.AddAuthentication(
        config.AzureAd,
        new Dictionary<string, string> { { "default", "APIM" } });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Track Apprenticeship Progress Internal Api");
    c.RoutePrefix = string.Empty;
});
app.UseHealthChecks();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ }