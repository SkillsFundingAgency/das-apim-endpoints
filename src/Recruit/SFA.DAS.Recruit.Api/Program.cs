using System.Threading.Tasks;
using NLog.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.Recruit.Api;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNLog();
builder.Host.UseServiceProviderFactory(new NServiceBusServiceProviderFactory());

var configuration = builder.Configuration.BuildSharedConfiguration();
builder.Host.ConfigureContainer<UpdateableServiceProvider>(containerBuilder =>
{
    Task.FromResult(containerBuilder.StartNServiceBus(configuration, "SFA.DAS.APIMRecruit"));
});
Startup.ConfigureServices(builder.Services, builder.Environment, configuration);

var app = builder.Build();
Startup.ConfigureApp(app, configuration);
app.Run();
