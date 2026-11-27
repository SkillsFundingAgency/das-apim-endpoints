using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByVacancyReference;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewsByVacancyReferenceQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsByVacancyReferenceQuery query,
        List<Recruit.Contracts.ApiResponses.VacancyReview> apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetVacancyReviewsByVacancyReferenceQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacanciesByVacancyReferenceReviewsApiRequest(
            query.VacancyReference.ToString(),
            query.Status,
            query.ManualOutcome,
            query.IncludeNoStatus);

        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(
                It.Is<GetVacanciesByVacancyReferenceReviewsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<Recruit.Contracts.ApiResponses.VacancyReview>>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().BeEquivalentTo(apiResponse);
        recruitApiClient.Verify(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(It.IsAny<GetVacanciesByVacancyReferenceReviewsApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_When_NotFound(
        GetVacancyReviewsByVacancyReferenceQuery query,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetVacancyReviewsByVacancyReferenceQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetVacanciesByVacancyReferenceReviewsApiRequest(
            query.VacancyReference.ToString(),
            query.Status,
            query.ManualOutcome,
            query.IncludeNoStatus);

        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(
                It.Is<GetVacanciesByVacancyReferenceReviewsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<Recruit.Contracts.ApiResponses.VacancyReview>>(null!, HttpStatusCode.NotFound, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().NotBeNull();
        actual.VacancyReviews.Should().BeEmpty();
    }
}