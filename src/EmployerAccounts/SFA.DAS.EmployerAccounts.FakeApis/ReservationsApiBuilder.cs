using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using AutoFixture;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.EmployerAccounts.FakeApis
{
    [ExcludeFromCodeCoverage]
    public class ReservationsApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;

        public ReservationsApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static ReservationsApiBuilder Create(int port)
        {
            return new ReservationsApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Reservations API running on port ({_server.Urls[0]})");
            return _server;
        }

        public ReservationsApiBuilder WithPing()
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

            return this;
        }

        public ReservationsApiBuilder WithReservations(string accountId)
        {
            IEnumerable<GetReservationsResponseListItem> reservations =
                _fixture.Build<GetReservationsResponseListItem>().CreateMany(5);

            _server
                .Given(
                        Request.Create()
                            .WithPath($"/api/accounts/{accountId}/reservations")
                            .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(reservations)
                );

            return this;
        }
    }
}