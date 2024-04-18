using System.Diagnostics.CodeAnalysis;
using NLog.Web;

namespace SFA.DAS.EarlyConnect.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("WEBSITE_LOAD_USER_PROFILE", "1");

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .UseNLog();
}