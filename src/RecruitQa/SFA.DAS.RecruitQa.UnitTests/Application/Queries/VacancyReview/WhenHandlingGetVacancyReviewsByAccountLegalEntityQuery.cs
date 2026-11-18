using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByAccountLegalEntity;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewsByAccountLegalEntityQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsByAccountLegalEntityQuery query,
        List<Recruit.Contracts.ApiResponses.VacancyReview> apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetVacancyReviewsByAccountLegalEntityQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetAccountsByAccountLegalEntityIdVacancyreviewsApiRequest(query.AccountLegalEntityId);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(
                It.Is<GetAccountsByAccountLegalEntityIdVacancyreviewsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<Recruit.Contracts.ApiResponses.VacancyReview>>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().BeEquivalentTo(apiResponse);
        recruitApiClient.Verify(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(It.IsAny<GetAccountsByAccountLegalEntityIdVacancyreviewsApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_When_NotFound(
        GetVacancyReviewsByAccountLegalEntityQuery query,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetVacancyReviewsByAccountLegalEntityQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetAccountsByAccountLegalEntityIdVacancyreviewsApiRequest(query.AccountLegalEntityId);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(
                It.Is<GetAccountsByAccountLegalEntityIdVacancyreviewsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<Recruit.Contracts.ApiResponses.VacancyReview>>(null!, HttpStatusCode.NotFound, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().NotBeNull();
        actual.VacancyReviews.Should().BeEmpty();
    }
}