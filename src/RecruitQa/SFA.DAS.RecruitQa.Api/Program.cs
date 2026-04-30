using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.RecruitQa.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new NServiceBusServiceProviderFactory());

var configuration = builder.Configuration.BuildSharedConfiguration();
builder.Host.ConfigureContainer<UpdateableServiceProvider>(containerBuilder =>
{
    Task.FromResult(containerBuilder.StartNServiceBus(configuration, "SFA.DAS.APIMRecruitQA"));
});
Startup.ConfigureServices(builder.Services, builder.Environment, configuration);
var app = builder.Build();
Startup.ConfigureApp(app, configuration);
app.Run();