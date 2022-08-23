using System.Net;
using AutoFixture;
using JustEat.HttpClientInterception;

namespace SFA.DAS.TrackProgress.Tests;

public class ApiFixture
{
    private protected TrackProgressApiFactory factory = null!;
    private protected Fixture fixture = null!;
    private protected HttpClient client = null!;

    protected HttpClientInterceptorOptions Interceptor { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Interceptor = new HttpClientInterceptorOptions
        {
            OnSend = request =>
            {
                Console.WriteLine($"HTTP {request.Method} {request.RequestUri}");
                return Task.CompletedTask;
            },
            // The Just Eat interceptor class is not fully functional, it doesn't seem to find POST methods
            // This is a work around to handle posting to the inner TP API, so we can successfully run our tests
            // This testing framework should be replaced with WireMock
            OnMissingRegistration = request =>
            {
                Console.WriteLine("HTTP muissing");
                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("{}")
                };
                return Task.FromResult(response);
            },
            ThrowOnMissingRegistration = true
        };

        factory = new TrackProgressApiFactory(Interceptor);
        fixture = new Fixture();
    }

    [SetUp]
    public void Setup()
    {
        client = factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        client?.Dispose();
    }
}