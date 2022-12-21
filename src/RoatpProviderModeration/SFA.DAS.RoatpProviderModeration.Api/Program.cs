using MediatR;
using Microsoft.OpenApi.Models;
using NLog.Web;
using SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.RoatpProviderModeration.OuterApi.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNLog();

// Add services to the container.

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services.AddAuthentication(configuration);
builder.Services.AddConfigurationOptions(configuration);
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services
       .AddMediatR(typeof(GetProviderQuery).Assembly)
       .AddSwaggerGen(c =>
       {
           c.SwaggerDoc("v1", new OpenApiInfo { Title = "RoatpProviderModerationOuterApi", Version = "v1" });
       })
       .AddHealthChecks();
builder.Services.AddServiceRegistration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app
    .UseHealthChecks()
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RoatpProviderModerationOuterApi");
        c.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();


app.Run();
