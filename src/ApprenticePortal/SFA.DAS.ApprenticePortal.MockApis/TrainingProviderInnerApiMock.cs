﻿using System;
using System.Net;
using SFA.DAS.ApprenticePortal.InnerApi.CommitmentsV2.Responses;
using SFA.DAS.ApprenticePortal.InnerApi.ProviderAccounts.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class TrainingProviderInnerApiMock : ApiMock
    {
        private readonly ApprenticeshipDetailsResponse _existingApprenticeship;


        public TrainingProviderInnerApiMock() : this(0) {}

        public TrainingProviderInnerApiMock(int port, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Training Provider Fake Api Running ({BaseAddress})");
            _existingApprenticeship = new ApprenticeshipDetailsResponse();
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
            var response = new SearchResponse { SearchResults = new TrainingProviderResponse[] { trainingProviderResponse } };

            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/v1/search")
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