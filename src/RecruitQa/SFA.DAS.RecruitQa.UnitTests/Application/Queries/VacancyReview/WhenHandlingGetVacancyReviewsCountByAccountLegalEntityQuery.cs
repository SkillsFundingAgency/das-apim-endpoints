using System.Net;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewsCountByAccountLegalEntityQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacancyReviewsCountByAccountLegalEntityQuery query,
        int apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacancyReviewsCountByAccountLegalEntityQueryHandler handler)
    {
        var expectedRequest = new GetVacancyReviewsCountByAccountLegalEntityRequest(query.AccountLegalEntityId, query.Status, query.ManualOutcome, query.EmployerNameOption);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<int>(
                It.Is<GetVacancyReviewsCountByAccountLegalEntityRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<int>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Count.Should().Be(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Zero_Returned_When_NotFound(
        GetVacancyReviewsCountByAccountLegalEntityQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacancyReviewsCountByAccountLegalEntityQueryHandler handler)
    {
        var expectedRequest = new GetVacancyReviewsCountByAccountLegalEntityRequest(query.AccountLegalEntityId, query.Status, query.ManualOutcome, query.EmployerNameOption);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<int>(
                It.Is<GetVacancyReviewsCountByAccountLegalEntityRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<int>(0, HttpStatusCode.NotFound, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Count.Should().Be(0);
    }
}
