using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.RecruitQa.Api;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.BuildSharedConfiguration();
builder.Host.ConfigureContainer<UpdateableServiceProvider>(containerBuilder =>
{
    Task.FromResult(containerBuilder.StartNServiceBus(configuration, "SFA.DAS.APIMRecruitQA"));
});
Startup.ConfigureServices(builder.Services, builder.Environment, configuration);
var app = builder.Build();
Startup.ConfigureApp(app, configuration);
app.Run();