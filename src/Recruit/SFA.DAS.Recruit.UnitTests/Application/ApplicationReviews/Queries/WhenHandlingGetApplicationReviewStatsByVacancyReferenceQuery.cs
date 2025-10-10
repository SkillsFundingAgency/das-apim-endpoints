using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewStatsByVacancyReference;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries;
[TestFixture]
internal class WhenHandlingGetApplicationReviewStatsByVacancyReferenceQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetApplicationReviewStatsByVacancyReferenceQuery query,
            GetApplicationReviewsCountApiResponse apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewStatsByVacancyReferenceQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetApplicationReviewsCountByVacancyReferenceApiRequest(query.VacancyReference);
        recruitApiClient
            .Setup(x => x.Get<GetApplicationReviewsCountApiResponse>(
                It.Is<GetApplicationReviewsCountByVacancyReferenceApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned(
        GetApplicationReviewStatsByVacancyReferenceQuery query,
        GetApplicationReviewsCountApiResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetApplicationReviewStatsByVacancyReferenceQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetApplicationReviewsCountByVacancyReferenceApiRequest(query.VacancyReference);
        recruitApiClient
            .Setup(x => x.Get<GetApplicationReviewsCountApiResponse>(
                It.Is<GetApplicationReviewsCountByVacancyReferenceApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync((GetApplicationReviewsCountApiResponse)null!);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(new GetApplicationReviewStatsByVacancyReferenceQueryResult());
    }
}