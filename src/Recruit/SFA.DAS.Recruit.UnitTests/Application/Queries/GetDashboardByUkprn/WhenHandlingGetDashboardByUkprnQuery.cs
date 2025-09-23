using SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetDashboardByUkprn
{
    [TestFixture]
    public class WhenHandlingGetDashboardByUkprnQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetDashboardByUkprnQuery query,
            GetProviderDashboardApiResponse apiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetDashboardByUkprnQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetDashboardByUkprnApiRequest(query.Ukprn, query.UserId);
            recruitApiClient
                .Setup(x => x.Get<GetProviderDashboardApiResponse>(
                    It.Is<GetDashboardByUkprnApiRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}
