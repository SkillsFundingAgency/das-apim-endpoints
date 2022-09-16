using NLog.Web;
using Microsoft.AspNetCore.Builder;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.Vacancies.Api;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNLog();
var configuration = builder.Configuration.BuildSharedConfiguration();

Startup.ConfigureServices(builder.Services, builder.Environment, configuration);
var app = builder.Build();
Startup.ConfigureApp(app, configuration);

app.Run();
