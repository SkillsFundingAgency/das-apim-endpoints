using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewsCountByAccountLegalEntityQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsCountByAccountLegalEntityQuery query,
        int apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        GetVacancyReviewsCountByAccountLegalEntityQueryHandler handler)
    {
        var expectedRequest = new GetAccountsByAccountLegalEntityIdVacancyreviewsCountApiRequest(query.AccountLegalEntityId, query.Status, query.ManualOutcome, query.EmployerNameOption);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<int>(
                It.Is<GetAccountsByAccountLegalEntityIdVacancyreviewsCountApiRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<int>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Count.Should().Be(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Zero_Returned_When_NotFound(
        GetVacancyReviewsCountByAccountLegalEntityQuery query,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        GetVacancyReviewsCountByAccountLegalEntityQueryHandler handler)
    {
        var expectedRequest = new GetAccountsByAccountLegalEntityIdVacancyreviewsCountApiRequest(query.AccountLegalEntityId, query.Status, query.ManualOutcome, query.EmployerNameOption);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<int>(
                It.Is<GetAccountsByAccountLegalEntityIdVacancyreviewsCountApiRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<int>(0, HttpStatusCode.NotFound, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Count.Should().Be(0);
    }
}