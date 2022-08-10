using JustEat.HttpClientInterception;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace SFA.DAS.TrackProgress.Tests;

public class TrackProgressApiFactory : WebApplicationFactory<Program>
{
    public TrackProgressApiFactory(HttpClientInterceptorOptions interceptor)
        : base()
    {
        Interceptor = interceptor;

        // HACK Force HTTP server startup
        using (CreateDefaultClient())
        {
        }
    }

    public HttpClientInterceptorOptions Interceptor { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpClientInterceptionFilter>(
                (_) => new HttpClientInterceptionFilter(Interceptor));
        });

        // Add the test configuration file to override the application configuration
        string directory = Path.GetDirectoryName(typeof(TrackProgressApiFactory).Assembly.Location);
        string fullPath = Path.Combine(directory, "testsettings.json");

        builder.ConfigureAppConfiguration((_, config) => config.AddJsonFile(fullPath, optional: true));
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