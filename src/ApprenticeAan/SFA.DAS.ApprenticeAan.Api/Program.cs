using NLog.Web;
using SFA.DAS.ApprenticeAan.Api.AppStart;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNLog();

// Add services to the container.

var configuration = builder.Configuration.BuildSharedConfiguration();

builder.Services
    .AddConfigurationOptions(configuration)
    .AddServiceRegistration()
    .AddAuthentication(configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app
    .UseHealthChecks()
    .UseSwagger()
    .UseSwaggerUI()
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
