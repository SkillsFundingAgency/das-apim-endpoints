using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using ApplicationReview = SFA.DAS.Recruit.Domain.ApplicationReview;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries;
[TestFixture]
internal class WhenHandlingGetApplicationReviewsByIdsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        Guid applicationId,
        GetApplicationReviewsByIdsQuery query,
            List<ApplicationReview> apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewsByIdsQueryHandler handler)
    {
        //Arrange
        foreach (var applicationReview in apiResponse)
        {
            applicationReview.ApplicationId = applicationId;
        }
        
        var expectedPostUrl = new GetApplicationReviewsByIdsApiRequest(query.ApplicationIds);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<List<ApplicationReview>>(
                It.Is<GetApplicationReviewsByIdsApiRequest>(r => r.PostUrl == expectedPostUrl.PostUrl), true))
            .ReturnsAsync(new ApiResponse<List<ApplicationReview>>(apiResponse, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.ApplicationReviews.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.PostWithResponseCode<List<ApplicationReview>>(It.IsAny<GetApplicationReviewsByIdsApiRequest>(), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned(
        Guid applicationId,
        GetApplicationReviewsByIdsQuery query,
        List<ApplicationReview> apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetApplicationReviewsByIdsQueryHandler handler)
    {
        //Arrange
        foreach (var applicationReview in apiResponse)
        {
            applicationReview.ApplicationId = applicationId;
        }

        var expectedPostUrl = new GetApplicationReviewsByIdsApiRequest(query.ApplicationIds);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<List<ApplicationReview>>(
                It.Is<GetApplicationReviewsByIdsApiRequest>(r => r.PostUrl == expectedPostUrl.PostUrl), true))
            .ReturnsAsync(new ApiResponse<List<ApplicationReview>>(null!, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(new GetApplicationReviewsByIdsQueryResult());
        recruitApiClient.Verify(x => x.PostWithResponseCode<List<ApplicationReview>>(It.IsAny<GetApplicationReviewsByIdsApiRequest>(), true), Times.Once);
    }
}
