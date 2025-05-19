using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetDashboardByAccountIdApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(long accountId, ApplicationReviewStatus status)
        {
            //Act
            var actual = new GetDashboardByAccountIdApiRequest(accountId);
            
            //Assert
            actual.GetUrl.Should().Be($"api/employer/{accountId}/applicationReviews/dashboard");
        }
    }
}