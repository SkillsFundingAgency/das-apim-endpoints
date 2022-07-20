using System;
using System.Collections.Generic;
using System.Net;
using AutoFixture;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.Approvals.FakeApis
{
    public class ProviderCoursesApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;
        
        public ProviderCoursesApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static ProviderCoursesApiBuilder Create(int port)
        {
            return new ProviderCoursesApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"TRaining Provider Courses Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public ProviderCoursesApiBuilder WithPing()
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

        public ProviderCoursesApiBuilder WithHasPortableFlexiJobOption(int providerId, string courseCode)
        {
            var course = _fixture.Build<GetHasPortableFlexiJobOptionResponse>()
                .With(a => a.HasPortableFlexiJobOption, true)
                .Create();

            _server
                .Given(
                        Request.Create()
                            .WithPath($"/providers/{providerId}/courses/{courseCode}")
                            .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(course)
                );

            return this;
        }

        public ProviderCoursesApiBuilder WithHasPortableFlexiJobOptionFalse(int providerId, string courseCode)
        {
            var course = _fixture.Build<GetHasPortableFlexiJobOptionResponse>()
                .With(a => a.HasPortableFlexiJobOption, false)
                .Create();

            _server
                .Given(
                    Request.Create()
                        .WithPath($"/providers/{providerId}/courses/{courseCode}")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(course)
                );

            return this;
        }

        public ProviderCoursesApiBuilder WithNoCoursesDeliveryModels404(int providerId, string courseCode)
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath($"/providers/{providerId}/courses/{courseCode}")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NotFound)
                );

            return this;
        }


    }
}