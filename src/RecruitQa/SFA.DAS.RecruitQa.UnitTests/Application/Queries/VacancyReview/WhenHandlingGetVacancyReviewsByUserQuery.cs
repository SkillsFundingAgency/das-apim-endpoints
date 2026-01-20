using System.Net;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Dashboard;

public class WhenHandlingGetVacancyReviewsByUserQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsByUserQuery query,
        List<GetVacancyReviewResponse> apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetVacancyReviewsByUserQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacancyReviewsByUserRequest(query.UserId, query.AssignationExpiry, query.Status);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<GetVacancyReviewResponse>>(
                It.Is<GetVacancyReviewsByUserRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<GetVacancyReviewResponse>>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().BeEquivalentTo(apiResponse);
        recruitApiClient.Verify(x => x.GetWithResponseCode<List<GetVacancyReviewResponse>>(It.IsAny<GetVacancyReviewsByUserRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_When_NotFound(
        GetVacancyReviewsByUserQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetVacancyReviewsByUserQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacancyReviewsByUserRequest(query.UserId, query.AssignationExpiry, query.Status);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<GetVacancyReviewResponse>>(
                It.Is<GetVacancyReviewsByUserRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<GetVacancyReviewResponse>>(null!, HttpStatusCode.NotFound, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().NotBeNull();
        actual.VacancyReviews.Should().BeEmpty();
    }
}
