using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.ProviderPR.Api.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddLogging()
    .AddTelemetryRegistration(configuration)
    .AddConfigurationOptions(configuration)
    .AddServiceRegistration(configuration)
    .AddAuthentication(configuration)
    .AddServiceHealthChecks()
    .AddEndpointsApiExplorer()
    .AddSwagger(configuration)
    .AddControllers(o =>
    {
        if (!configuration.IsLocalOrDev()) o.Filters.Add(new AuthorizeFilter("default"));
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProviderPROuterApi");
        c.RoutePrefix = string.Empty;
    })
    .UseHttpsRedirection()
    .UseHealthChecks()
    .UseAuthentication();

app.MapControllers();

app.Run();
