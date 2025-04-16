﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Learners.Queries
{
    public class WhenGettingLearnersForProvider
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Both_Clients_Return_Responses_The_result_Is_Returned(
            GetLearnersForProviderQuery query,
            GetLearnersForProviderRequest learnersRequest,
            GetLearnersForProviderResponse learnersResponse,
            GetAccountLegalEntityRequest aleRequest,
            GetAccountLegalEntityResponse aleResponse,
            List<LearnerSummary> learners,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
            [Frozen] Mock<IMapLearnerRecords> mapper,
            [Greedy] GetLearnersForProviderQueryHandler handler
            )
        {
            GetLearnersForProviderRequest input;
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

            commitmentsClient.Setup(x => x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

            mapper.Setup(x => x.Map(learnersResponse.Data)).ReturnsAsync(learners);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.AccountLegalEntityId.Should().Be(query.AccountLegalEntityId);
            actual.EmployerName.Should().Be(aleResponse.LegalEntityName);
            actual.LastSubmissionDate.Should().Be(learnersResponse.LastSubmissionDate);
            actual.Page.Should().Be(learnersResponse.Page);
            actual.PageSize.Should().Be(learnersResponse.PageSize);
            actual.Total.Should().Be(learnersResponse.TotalItems);
            actual.TotalPages.Should().Be(learnersResponse.TotalPages);
            actual.Learners.Should().BeEquivalentTo(learners);
        }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_learnerData_Clients_fails(
        GetLearnersForProviderQuery query,
        GetLearnersForProviderRequest learnersRequest,
        GetLearnersForProviderResponse learnersResponse,
        GetAccountLegalEntityRequest aleRequest,
        GetAccountLegalEntityResponse aleResponse,
        List<LearnerSummary> learners,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IMapLearnerRecords> mapper,
        [Greedy] GetLearnersForProviderQueryHandler handler
        )
        {
            GetLearnersForProviderRequest input;
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.InternalServerError, "Call to learner data failed"));

            commitmentsClient.Setup(x => x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.OK, null));

            mapper.Setup(x => x.Map(learnersResponse.Data)).ReturnsAsync(learners);

            var result = async () => await handler.Handle(query, CancellationToken.None);

            await result.Should().ThrowAsync<ApplicationException>().WithMessage("*Call to learner data failed");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_aleClients_fails(
            GetLearnersForProviderQuery query,
            GetLearnersForProviderRequest learnersRequest,
            GetLearnersForProviderResponse learnersResponse,
            GetAccountLegalEntityRequest aleRequest,
            GetAccountLegalEntityResponse aleResponse,
            List<LearnerSummary> learners,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
            [Frozen] Mock<IMapLearnerRecords> mapper,
            [Greedy] GetLearnersForProviderQueryHandler handler
        )
        {
            GetLearnersForProviderRequest input;
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnersForProviderResponse>(It.IsAny<GetLearnersForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnersForProviderResponse>(learnersResponse, HttpStatusCode.OK, null));

            commitmentsClient.Setup(x => x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(aleResponse, HttpStatusCode.InternalServerError, "ALE failed"));

            mapper.Setup(x => x.Map(learnersResponse.Data)).ReturnsAsync(learners);

            var result = async () => await handler.Handle(query, CancellationToken.None);

            await result.Should().ThrowAsync<ApplicationException>().WithMessage("*ALE failed");
        }

    }
}