using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetApplicationReviewsCountByUkprn
{
    [TestFixture]
    public class WhenHandlingGetApplicationReviewsCountByUkprnQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetApplicationReviewsCountByUkprnQuery query,
            List<ApplicationReviewStats> apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Greedy] GetApplicationReviewsCountByUkprnQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsCountByUkprnApiRequest(query.Ukprn, query.VacancyReferences);
            recruitApiClient
                .Setup(x => x.PostWithResponseCode<List<ApplicationReviewStats>>(
                    It.Is<GetApplicationReviewsCountByUkprnApiRequest>(r => r.PostUrl == expectedGetUrl.PostUrl), true))
                .ReturnsAsync(new ApiResponse<List<ApplicationReviewStats>>(apiResponse, HttpStatusCode.OK, string.Empty));

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ApplicationReviewStatsList.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Null_Returned(
            GetApplicationReviewsCountByUkprnQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Greedy] GetApplicationReviewsCountByUkprnQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsCountByUkprnApiRequest(query.Ukprn, query.VacancyReferences);
            recruitApiClient
                .Setup(x => x.PostWithResponseCode<List<ApplicationReviewStats>>(
                    It.Is<GetApplicationReviewsCountByUkprnApiRequest>(r => r.PostUrl == expectedGetUrl.PostUrl), true))
                .ReturnsAsync(new ApiResponse<List<ApplicationReviewStats>>(null!, HttpStatusCode.OK, string.Empty));

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ApplicationReviewStatsList.Should().BeEquivalentTo(new List<ApplicationReviewStats>());
        }
    }
}