using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using SFA.DAS.ApprenticeApp.Services;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeApp.MockApis
{
    [ExcludeFromCodeCoverage]
    public class CoursesInnerApiMock : ApiMock
    {
        public CoursesInnerApiMock() : this(0) {}

        public CoursesInnerApiMock(int port, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Courses Fake Api Running ({BaseAddress})");
        }

        public CoursesInnerApiMock WithPing()
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }

        public CoursesInnerApiMock WithFrameworkCourse(string courseCode, FrameworkApiResponse response)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/courses/frameworks/{courseCode}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }

        public CoursesInnerApiMock WithStandardCourse(string standardUId, StandardApiResponse response)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/courses/standards/{standardUId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }

        public CoursesInnerApiMock WithAnyStandardCourse(StandardApiResponse response)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/courses/standards/*")
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