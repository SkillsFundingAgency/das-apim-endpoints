using MediatR;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.TrackProgress.Api;
using SFA.DAS.TrackProgress.Api.AppStart;
using SFA.DAS.TrackProgress.Application.Commands.TrackProgress;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration.BuildSharedConfiguration();
var config = configuration.Get<TrackProgressConfiguration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrackProgressOuterApi", Version = "v1" });
});
builder.Services.AddMediatR(typeof(TrackProgressCommand));
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddServiceRegistration();
builder.Services.AddConfigurationOptions(builder.Configuration);
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrackProgressOuterApi");
    c.RoutePrefix = string.Empty;
});
app.UseHealthChecks();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ }