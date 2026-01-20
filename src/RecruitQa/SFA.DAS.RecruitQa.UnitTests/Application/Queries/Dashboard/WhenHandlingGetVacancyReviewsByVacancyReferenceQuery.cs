using System.Net;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByVacancyReference;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Dashboard;

public class WhenHandlingGetVacancyReviewsByVacancyReferenceQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsByVacancyReferenceQuery query,
        List<GetVacancyReviewResponse> apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetVacancyReviewsByVacancyReferenceQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacancyReviewsByVacancyReferenceRequest(
            query.VacancyReference,
            query.Status,
            query.IncludeNoStatus,
            query.ManualOutcome);

        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<GetVacancyReviewResponse>>(
                It.Is<GetVacancyReviewsByVacancyReferenceRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<GetVacancyReviewResponse>>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().BeEquivalentTo(apiResponse);
        recruitApiClient.Verify(x => x.GetWithResponseCode<List<GetVacancyReviewResponse>>(It.IsAny<GetVacancyReviewsByVacancyReferenceRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_When_NotFound(
        GetVacancyReviewsByVacancyReferenceQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetVacancyReviewsByVacancyReferenceQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacancyReviewsByVacancyReferenceRequest(
            query.VacancyReference,
            query.Status,
            query.IncludeNoStatus,
            query.ManualOutcome);

        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<GetVacancyReviewResponse>>(
                It.Is<GetVacancyReviewsByVacancyReferenceRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<GetVacancyReviewResponse>>(null!, HttpStatusCode.NotFound, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().NotBeNull();
        actual.VacancyReviews.Should().BeEmpty();
    }
}
