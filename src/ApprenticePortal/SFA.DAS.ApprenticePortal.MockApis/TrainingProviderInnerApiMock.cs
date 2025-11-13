using System;
using System.Net;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class TrainingProviderInnerApiMock : ApiMock
    {
        public TrainingProviderInnerApiMock() : this(0) { }

        public TrainingProviderInnerApiMock(int port, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Training Provider Fake Api Running ({BaseAddress})");
        }

        public TrainingProviderInnerApiMock WithPing()
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

        public TrainingProviderInnerApiMock WithValidSearch(long trainingProviderId, OrganisationResponse response)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/organisations/{trainingProviderId}")
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