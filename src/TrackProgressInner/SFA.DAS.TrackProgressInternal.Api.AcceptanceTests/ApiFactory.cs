using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests;

public class ApiFactory : WebApplicationFactory<Program>
{
    public MockApi InnerApis { get; } = new();
    public MockApi TrackProgressInnerApi { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            var config = new Dictionary<string, string>
            {
                {"CommitmentsV2InnerApi:Url", InnerApis.BaseAddress.ToString() },
                {"CoursesApi:Url", InnerApis.BaseAddress.ToString() },
                {"TrackProgressApi:Url", TrackProgressInnerApi.BaseAddress.ToString() },
            };
            configBuilder.Sources.Clear();
            configBuilder.AddInMemoryCollection(config);
        });

        builder.UseEnvironment("DEV");
    }
}