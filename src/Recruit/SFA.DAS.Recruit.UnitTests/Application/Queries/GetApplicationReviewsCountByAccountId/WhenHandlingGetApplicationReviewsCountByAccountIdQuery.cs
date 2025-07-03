using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetApplicationReviewsCountByAccountId
{
    [TestFixture]
    public class WhenHandlingGetApplicationReviewsCountByAccountIdQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetApplicationReviewsCountByAccountIdQuery query,
            List<ApplicationReviewStats> apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Greedy] GetApplicationReviewsCountByAccountIdQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsCountByAccountIdApiRequest(query.AccountId, query.VacancyReferences, query.ApplicationSharedFilteringStatus);
            recruitApiClient
                .Setup(x => x.PostWithResponseCode<List<ApplicationReviewStats>>(
                    It.Is<GetApplicationReviewsCountByAccountIdApiRequest>(r => r.PostUrl == expectedGetUrl.PostUrl), true))
                .ReturnsAsync(new ApiResponse<List<ApplicationReviewStats>>(apiResponse, HttpStatusCode.OK, string.Empty));

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ApplicationReviewStatsList.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Null_Returned(
            GetApplicationReviewsCountByAccountIdQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetApplicationReviewsCountByAccountIdQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetApplicationReviewsCountByAccountIdApiRequest(query.AccountId, query.VacancyReferences, query.ApplicationSharedFilteringStatus);
            recruitApiClient
                .Setup(x => x.PostWithResponseCode<List<ApplicationReviewStats>>(
                    It.Is<GetApplicationReviewsCountByAccountIdApiRequest>(r => r.PostUrl == expectedGetUrl.PostUrl), true))
                .ReturnsAsync(new ApiResponse<List<ApplicationReviewStats>>(null!, HttpStatusCode.OK, string.Empty));

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ApplicationReviewStatsList.Should().BeEquivalentTo(new List<ApplicationReviewStats>());
        }
    }
}
