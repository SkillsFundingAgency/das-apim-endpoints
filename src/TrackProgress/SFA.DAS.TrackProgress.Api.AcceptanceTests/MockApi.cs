using WireMock.Logging;
using WireMock.Matchers.Request;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests;

public sealed class MockApi : IDisposable
{
    public readonly WireMockServer _mockServer;

    public MockApi(int port = 0, bool ssl = true)
        => _mockServer = WireMockServer.Start(new WireMockServerSettings
        {
            Port = port,
            UseSSL = ssl,
            StartAdminInterface = true,
            Logger = new WireMockConsoleLogger(),
        });

    public Uri BaseAddress =>
        new(_mockServer.Urls[0]
        ?? throw new InvalidOperationException("No mock server Url"));

    public IRespondWithAProvider Given(IRequestMatcher requestMatcher)
        => _mockServer.Given(requestMatcher);

    public void Reset() => _mockServer.Reset();

    public void Dispose() => _mockServer?.Dispose();
}