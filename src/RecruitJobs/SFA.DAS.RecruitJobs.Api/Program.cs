using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.RecruitJobs.Api;
using SFA.DAS.SharedOuterApi.AppStart;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new NServiceBusServiceProviderFactory());

var configuration = builder.Configuration.BuildSharedConfiguration();
builder.Host.ConfigureContainer<UpdateableServiceProvider>(containerBuilder =>
{
    Task.FromResult(containerBuilder.StartNServiceBus(configuration, "SFA.DAS.APIMRecruitJobs"));
});
Startup.ConfigureServices(builder.Services, builder.Environment, configuration);

var app = builder.Build();
Startup.ConfigureApp(app, configuration);
app.Run();


[ExcludeFromCodeCoverage]
public partial class Program { }