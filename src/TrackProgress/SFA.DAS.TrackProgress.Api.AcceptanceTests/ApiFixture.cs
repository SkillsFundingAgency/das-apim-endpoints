using AutoFixture;
using JustEat.HttpClientInterception;
using SFA.DAS.TrackProgress.Api.AcceptanceTests;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;
using System.Net;

namespace SFA.DAS.TrackProgress.Tests;

public class ApiFixture
{
    protected TrackProgressApiFactory factory = null!;
    protected Fixture fixture = null!;
    protected HttpClient client = null!;

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
        client = factory.CreateClient().ForProvider(An.Apprenticeship.ProviderId);
        factory.Reset();
    }

    [TearDown]
    public void TearDown()
    {
        client?.Dispose();
    }
}

public static class HttpClientExtension
{
    public static HttpClient ForProvider(this HttpClient client, long providerId)
    {
        client.DefaultRequestHeaders.Remove(SubscriptionHeaderConstants.ForProviderId);
        client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, $"Provider-{providerId}-TrackProgressOuterApi");
        return client;
    }
}