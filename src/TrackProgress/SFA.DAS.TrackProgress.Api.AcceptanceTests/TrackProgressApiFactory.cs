using JustEat.HttpClientInterception;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests;

public class TrackProgressApiFactory : WebApplicationFactory<Program>
{
    public TrackProgressApiFactory(HttpClientInterceptorOptions interceptor)
        : base()
    {
        Interceptor = interceptor;
    }

    public HttpClientInterceptorOptions Interceptor { get; }

    public CommitmentsV2Api CommitmentsApi { get; } = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(webBuilder =>
        {
            var config = new Dictionary<string, string>
            {
                {"CommitmentsV2InnerApi:Url", CommitmentsApi.BaseAddress.ToString() },
                {"CoursesApi:Url", CommitmentsApi.BaseAddress.ToString() },
                {"TrackProgressApi:Url", CommitmentsApi.BaseAddress.ToString() },
            };
            webBuilder.AddInMemoryCollection(config);
        });

        //builder.ConfigureTestServices(services =>
        //{
        //    services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpClientInterceptionFilter>(
        //        (_) => new HttpClientInterceptionFilter(Interceptor));
        //});

        // Add the test configuration file to override the application configuration
        var directory = Path.GetDirectoryName(typeof(TrackProgressApiFactory).Assembly.Location)!;
        var fullPath = Path.Combine(directory, "testsettings.json");

        builder.ConfigureAppConfiguration((_, config) => config.AddJsonFile(fullPath, optional: true));
        builder.UseEnvironment("LOCAL_ACCEPTANCE_TESTS");
    }
}

public sealed class HttpClientInterceptionFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly HttpClientInterceptorOptions _options;

    public HttpClientInterceptionFilter(HttpClientInterceptorOptions options)
    {
        _options = options;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return (builder) =>
        {
            // Run any actions the application has configured for itself
            next(builder);

            // Add the interceptor as the last message handler
            builder.AdditionalHandlers.Add(_options.CreateHttpMessageHandler());
        };
    }
}

public sealed class CommitmentsV2Api : IDisposable
{
    public CommitmentsV2Api(int port = 0, bool ssl = true)
        => MockServer = WireMockServer.Start(new WireMockServerSettings
        {
            Port = port,
            UseSSL = ssl,
            StartAdminInterface = true,
            Logger = new WireMockConsoleLogger(),
        });

    public WireMockServer MockServer { get; }

    public Uri BaseAddress =>
        new(MockServer.Url
        ?? throw new InvalidOperationException("No mock server Url"));

    public void Dispose() => MockServer?.Dispose();
}