using MediatR;
using Microsoft.OpenApi.Models;
using SFA.DAS.RoatpOversight.Api.Extensions;
using SFA.DAS.RoatpOversight.Application.Providers.Command.CreateProvider;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.BuildSharedConfiguration();

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddConfigurationOptions(configuration)
    .AddAuthentication(configuration)
    .AddApplicationInsightsTelemetry()
    .AddEndpointsApiExplorer()
    .AddControllers(configuration)
    .AddMediatR(typeof(CreateProviderCommand).Assembly)
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "RoatpOversightOuterApi", Version = "v1" });
    })
    .AddHealthChecks().AddDependenciesToHealthChecks(builder.Services)
    .AddServiceRegistrations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app
    .UseHsts()
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication()
    .UseAuthorization()
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RoatpOversightOuterApi");
        c.RoutePrefix = string.Empty;
    });

app.MapControllers();

app.Run();
