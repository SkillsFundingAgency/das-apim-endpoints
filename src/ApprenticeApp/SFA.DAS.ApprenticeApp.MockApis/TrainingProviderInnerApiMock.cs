using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeApp.MockApis
{
    [ExcludeFromCodeCoverage]
    public class TrainingProviderInnerApiMock : ApiMock
    {
        public TrainingProviderInnerApiMock() : this(0) {}

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

        public TrainingProviderInnerApiMock WithValidSearch(long trainingProviderId, TrainingProviderResponse trainingProviderResponse)
        {
            var response = new SearchResponse { SearchResults = new [] { trainingProviderResponse } };

            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/v1/search")
                        .WithParam("searchterm", trainingProviderId.ToString())
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