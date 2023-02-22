using System;
using System.Net;
using AutoFixture;
using SFA.DAS.Approvals.InnerApi.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.Approvals.FakeApis
{
    public class CoursesApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;
        
        public CoursesApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static CoursesApiBuilder Create(int port)
        {
            return new CoursesApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Courses Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public CoursesApiBuilder WithPing()
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

        public CoursesApiBuilder WithAnyCourseStandard()
        {
            var standardCourseResponse = _fixture.Build<GetStandardsListItem>()
                .Create();

            _server
                .Given(
                        Request.Create()
                            .WithPath("/api/courses/standards/*")
                            .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(standardCourseResponse)
                );

            return this;
        }
    }
}