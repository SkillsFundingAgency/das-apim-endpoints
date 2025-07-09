using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using ApplicationReview = SFA.DAS.Recruit.InnerApi.Responses.ApplicationReview;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries
{
    [TestFixture]
    public class WhenHandingGetApplicationReviewsByVacancyReferenceCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetApplicationReviewsByVacancyReferenceQuery query,
            List<ApplicationReview> apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewsByVacancyReferenceQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsByVacancyReferenceApiRequest(query.VacancyReference);
            recruitApiClient
                .Setup(x => x.Get<List<ApplicationReview>>(
                    It.Is<GetApplicationReviewsByVacancyReferenceApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ApplicationReviews.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Null_Returned(
            GetApplicationReviewsByVacancyReferenceQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewsByVacancyReferenceQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsByVacancyReferenceApiRequest(query.VacancyReference);
            recruitApiClient
                .Setup(x => x.Get<List<ApplicationReview>>(
                    It.Is<GetApplicationReviewsByVacancyReferenceApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
                .ReturnsAsync((List<ApplicationReview>)null!);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Should().BeEquivalentTo(new GetApplicationReviewsByVacancyReferenceQueryResult(null));
        }
    }
}
