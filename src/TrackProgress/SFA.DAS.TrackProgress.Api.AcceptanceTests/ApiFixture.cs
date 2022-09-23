using AutoFixture;
using SFA.DAS.TrackProgress.Api.AcceptanceTests;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;

namespace SFA.DAS.TrackProgress.Tests;

public class ApiFixture
{
    protected TrackProgressApiFactory factory = null!;
    protected Fixture fixture = null!;
    protected HttpClient client = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        factory = new TrackProgressApiFactory();
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