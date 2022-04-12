using AutoFixture;
using System;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
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

        public CoursesApiBuilder WithCourses(int courseId)
        {
            var course = _fixture.Build<Apis.Courses.StandardResponse>()
                .With(a => a.Id, courseId)
                .Create();

            _server
                .Given(
                        Request.Create()
                            .WithPath($"/api/courses/standards/{courseId}")
                            .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(course)
                );

            return this;
        }

        public CoursesApiBuilder WithCoursesForStandardUIds()
        {
            var course = _fixture.Build<Apis.Courses.StandardResponse>()
                .Create();

            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/courses/standards/ST*")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(course)
                );

            return this;
        }
    }
}