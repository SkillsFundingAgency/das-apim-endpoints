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
            }
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