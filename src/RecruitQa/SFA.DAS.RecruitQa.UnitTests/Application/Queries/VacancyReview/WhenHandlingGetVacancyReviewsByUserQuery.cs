using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewsByUserQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsByUserQuery query,
        List<Recruit.Contracts.ApiResponses.VacancyReview> apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetVacancyReviewsByUserQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetUsersVacancyreviewsApiRequest(query.UserId, query.AssignationExpiry, query.Status);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(
                It.Is<GetUsersVacancyreviewsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<Recruit.Contracts.ApiResponses.VacancyReview>>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().BeEquivalentTo(apiResponse);
        recruitApiClient.Verify(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(It.IsAny<GetUsersVacancyreviewsApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Empty_List_Returned_When_NotFound(
        GetVacancyReviewsByUserQuery query,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetVacancyReviewsByUserQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetUsersVacancyreviewsApiRequest(query.UserId, query.AssignationExpiry, query.Status);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<List<Recruit.Contracts.ApiResponses.VacancyReview>>(
                It.Is<GetUsersVacancyreviewsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<List<Recruit.Contracts.ApiResponses.VacancyReview>>(null!, HttpStatusCode.NotFound, ""));

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.VacancyReviews.Should().NotBeNull();
        actual.VacancyReviews.Should().BeEmpty();
    }
}