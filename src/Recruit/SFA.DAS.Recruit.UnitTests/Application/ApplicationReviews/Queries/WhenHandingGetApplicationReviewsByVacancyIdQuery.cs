using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyId;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Generic;
using ApplicationReview = SFA.DAS.Recruit.Domain.ApplicationReview;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries
{
    [TestFixture]
    public class WhenHandingGetApplicationReviewsByVacancyIdQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetApplicationReviewsByVacancyIdQuery query,
            List<ApplicationReview> apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewsByVacancyIdQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsByVacancyIdApiRequest(query.VacancyId);
            recruitApiClient
                .Setup(x => x.Get<List<ApplicationReview>>(
                    It.Is<GetApplicationReviewsByVacancyIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ApplicationReviews.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Null_Returned(
            GetApplicationReviewsByVacancyIdQuery query,
            List<ApplicationReview> apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewsByVacancyIdQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsByVacancyIdApiRequest(query.VacancyId);
            recruitApiClient
                .Setup(x => x.Get<List<ApplicationReview>>(
                    It.Is<GetApplicationReviewsByVacancyIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
                .ReturnsAsync((List<ApplicationReview>)null!);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Should().BeEquivalentTo(new GetApplicationReviewsByVacancyReferenceQueryResult([]));
        }
    }
}
