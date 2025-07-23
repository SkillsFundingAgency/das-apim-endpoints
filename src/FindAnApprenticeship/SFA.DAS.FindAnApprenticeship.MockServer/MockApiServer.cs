using SFA.DAS.FindAnApprenticeship.MockServer.MockServerBuilder;
using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Net.StandAlone;

namespace SFA.DAS.FindAnApprenticeship.MockServer;

public static class MockApiServer
{
    public static IWireMockServer Start()
    {
        var settings = new WireMockServerSettings
        {
            Port = 5051,
            Logger = new WireMockConsoleLogger()
        };

        var server = StandAloneApp.Start(settings)
                .WithFindAnApprenticeshipFiles();
        
        return server;
    }
}