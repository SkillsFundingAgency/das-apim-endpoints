using System;
using System.Net;
using AutoFixture;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class CommitmentsV2ApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;

        public CommitmentsV2ApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static CommitmentsV2ApiBuilder Create(int port)
        {
            return new CommitmentsV2ApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Commitments V2 Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public CommitmentsV2ApiBuilder WithPing()
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

        public CommitmentsV2ApiBuilder WithAValidApprentice(long accountId, long apprenticeshipId, int courseId)
        {
            var response = _fixture.Build<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>()
                .With(x => x.EmployerAccountId, accountId)
                .With(x => x.CourseCode, courseId.ToString())
                .Without(x=>x.StandardUId)
                .With(x=>x.Email, "valid@email.com")
                .Create();

            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/apprenticeships/{apprenticeshipId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }

        public CommitmentsV2ApiBuilder WithAnyApprenticeship()
        {
            var response = _fixture.Build<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>()
                .Create();

            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/apprenticeships/*")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }


    }
}