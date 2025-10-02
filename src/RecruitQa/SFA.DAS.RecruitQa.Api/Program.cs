using SFA.DAS.RecruitQa.Api;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.BuildSharedConfiguration();
Startup.ConfigureServices(builder.Services, builder.Environment, configuration);
var app = builder.Build();
Startup.ConfigureApp(app, configuration);
app.Run();