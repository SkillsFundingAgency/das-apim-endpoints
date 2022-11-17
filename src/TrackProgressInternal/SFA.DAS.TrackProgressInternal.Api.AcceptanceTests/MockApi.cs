using WireMock.Logging;
using WireMock.Matchers.Request;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests;

public sealed class MockApi : IDisposable
{
    public MockApi(int port = 0, bool ssl = false)
        => Server = WireMockServer.Start(new WireMockServerSettings
        {
            Port = port,
            UseSSL = ssl,
            StartAdminInterface = true,
            Logger = new WireMockConsoleLogger(),
        });

    public WireMockServer Server { get; }

    public Uri BaseAddress =>
        new(Server.Urls[0]
        ?? throw new InvalidOperationException("No mock server Url"));

    public IRespondWithAProvider Given(IRequestMatcher requestMatcher)
        => Server.Given(requestMatcher);

    public void Reset() => Server.Reset();

    public void Dispose() => Server?.Dispose();
}