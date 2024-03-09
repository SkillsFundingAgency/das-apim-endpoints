using System;
using System.Net;
using SFA.DAS.ApprenticeApp.InnerApi.CommitmentsV2.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeApp.MockApis
{
    public class CommitmentsV2InnerApiMock : ApiMock
    {
        public CommitmentsV2InnerApiMock() : this(0) {}

        public CommitmentsV2InnerApiMock(int port, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Commitments V2 Fake Api Running ({BaseAddress})");
        }

        public CommitmentsV2InnerApiMock WithPing()
        {
            MockServer
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

        public CommitmentsV2InnerApiMock WithApprenticeshipsResponseForApprentice(long apprenticeshipId, ApprenticeshipDetailsResponse apprenticeshipsResult)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/apprenticeships/{apprenticeshipId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(apprenticeshipsResult)
                );

            return this;
        }
    }
}