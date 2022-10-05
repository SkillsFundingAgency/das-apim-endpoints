using AutoFixture;
using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests;

namespace SFA.DAS.TrackProgressInternal.Tests;

public class ApiFixture
{
    protected ApiFactory factory = null!;
    protected Fixture fixture = null!;
    protected HttpClient client = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        factory = new ApiFactory();
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