using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.EmployerFinanceJobs.Api;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.BuildSharedConfiguration();

Startup.ConfigureServices(builder.Services, builder.Environment, configuration);

var app = builder.Build();
Startup.ConfigureApp(app, configuration, builder.Environment);
app.Run();


[ExcludeFromCodeCoverage]
public partial class Program;