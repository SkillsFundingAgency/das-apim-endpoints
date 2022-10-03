using AutoFixture;
using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests;
using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.ApiTests;

namespace SFA.DAS.TrackProgressInternal.Tests;

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
        client = factory.CreateClient();
        factory.Reset();
    }

    [TearDown]
    public void TearDown()
    {
        client?.Dispose();
    }

    protected string InnerApiUrl(string path)
        => $"{factory.TrackProgressInnerApi.BaseAddress}{path}";
}