using System;
using System.Net;
using AutoFixture;
using SFA.DAS.Approvals.InnerApi.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.Approvals.FakeApis
{
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
            Console.WriteLine($"Reservations Fake Api Running ({_server.Urls[0]})");
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

        public ReservationsApiBuilder WithBulkUploadValidate(bool withErrors = false)
        {
            BulkReservationValidationResults response;
            if (withErrors)
            {
                response = _fixture.Build<BulkReservationValidationResults>().Create();
            }
            else
            {
                response = new BulkReservationValidationResults();
            }

            _server
                .Given(
                        Request.Create()
                            .WithPath("/api/Reservations/accounts/bulk-validate")
                            .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }
    }
}