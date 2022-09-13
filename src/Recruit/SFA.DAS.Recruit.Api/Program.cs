using Microsoft.Extensions.Hosting;
using NLog.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Recruit.Api.AppStart;
using SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNLog();

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services.AddSingleton(builder.Environment);
builder.Services.AddConfigurationOptions(configuration);

if (!configuration.IsLocalOrDev())
{
    var azureAdConfiguration = configuration
        .GetSection("AzureAd")
        .Get<AzureActiveDirectoryConfiguration>();
    var policies = new Dictionary<string, string>
    {
        {"default", "APIM"}
    };

    builder.Services.AddAuthentication(azureAdConfiguration, policies);
}

builder.Services.AddMediatR(typeof(GetTrainingProgrammesQuery).Assembly);
builder.Services.AddServiceRegistration();

builder.Services.Configure<RouteOptions>(options =>
    {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    }).AddMvc(o =>
    {
        if (!configuration.IsLocalOrDev())
        {
            o.Filters.Add(new AuthorizeFilter("default"));
        }
    }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

if (configuration["Environment"] != "DEV")
{
    builder.Services.AddHealthChecks()
        .AddCheck<CoursesApiHealthCheck>("Courses API health check")
        .AddCheck<CourseDeliveryApiHealthCheck>("Course Delivery API health check");
}
            
builder.Services.AddApplicationInsightsTelemetry(configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecruitOuterApi", Version = "v1" });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();

if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
{
    app.UseHealthChecks();
}
            
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "api/{controller=Providers}/{action=index}/{id?}");
});
        
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecruitOuterApi");
    c.RoutePrefix = string.Empty;
});



app.Run();
