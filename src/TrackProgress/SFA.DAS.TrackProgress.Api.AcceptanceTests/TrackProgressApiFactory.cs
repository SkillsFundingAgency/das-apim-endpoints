using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests;

public class TrackProgressApiFactory : WebApplicationFactory<Program>
{
    public MockApi InnerApis { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(webBuilder =>
        {
            var config = new Dictionary<string, string>
            {
                {"CommitmentsV2InnerApi:Url", InnerApis.BaseAddress.ToString() },
                {"CoursesApi:Url", InnerApis.BaseAddress.ToString() },
                {"TrackProgressApi:Url", InnerApis.BaseAddress.ToString() },
            };
            webBuilder.AddInMemoryCollection(config);
        });

        // Add the test configuration file to override the application configuration
        var directory = Path.GetDirectoryName(typeof(TrackProgressApiFactory).Assembly.Location)!;
        var fullPath = Path.Combine(directory, "testsettings.json");

        builder.ConfigureAppConfiguration((_, config) => config.AddJsonFile(fullPath, optional: true));
        builder.UseEnvironment("LOCAL_ACCEPTANCE_TESTS");
    }
}