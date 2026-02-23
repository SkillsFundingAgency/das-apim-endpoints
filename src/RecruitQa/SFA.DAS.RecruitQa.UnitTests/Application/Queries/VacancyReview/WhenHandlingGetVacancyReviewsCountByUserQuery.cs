using System.Net;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByUser;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewsCountByUserQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Count_Returned(
        GetVacancyReviewsCountByUserQuery query,
        int apiCount,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacancyReviewsCountByUserQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacancyReviewsCountByUserRequest(query.UserEmail, query.ApprovedFirstTime, query.AssignationExpiry);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<int>(
                It.Is<GetVacancyReviewsCountByUserRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<int>(apiCount, HttpStatusCode.OK, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Count.Should().Be(apiCount);
        recruitApiClient.Verify(x => x.GetWithResponseCode<int>(It.IsAny<GetVacancyReviewsCountByUserRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Zero_Returned_When_NotFound(
        GetVacancyReviewsCountByUserQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacancyReviewsCountByUserQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacancyReviewsCountByUserRequest(query.UserEmail, query.ApprovedFirstTime, query.AssignationExpiry);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<int>(
                It.Is<GetVacancyReviewsCountByUserRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<int>(0, HttpStatusCode.NotFound, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Count.Should().Be(0);
    }
}
