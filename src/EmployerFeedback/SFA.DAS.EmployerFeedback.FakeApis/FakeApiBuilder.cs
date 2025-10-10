using System;
using System.Net;
using AutoFixture;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.EmployerFeedback.FakeApis
{
    public abstract class FakeApiBuilder
    {
        protected readonly string _name;
        protected readonly WireMockServer _server;
        protected readonly Fixture _fixture;

        protected FakeApiBuilder(string name, int port)
        {
            _name = name;
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public void Build()
        {
            Console.WriteLine($"{_name} Fake Api Running ({_server.Urls[0]})");
        }

        public void Stop()
        {
            _server.Stop();
        }

        public string Name => _name;
        public int Port => _server.Port;

        protected void AddPingPath()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }
    }

    public abstract class FakeApiBuilder<T> : FakeApiBuilder
    {
        protected FakeApiBuilder(string name, int port)
            : base(name, port)
        {
        }

        public abstract T WithPing();
    }
}
