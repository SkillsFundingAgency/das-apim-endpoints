using Microsoft.AspNetCore.Builder;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.VacanciesManage.Api;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.BuildSharedConfiguration();

Startup.ConfigureServices(builder.Services, builder.Environment, configuration);
var app = builder.Build();
Startup.ConfigureApp(app, configuration);

app.Run();