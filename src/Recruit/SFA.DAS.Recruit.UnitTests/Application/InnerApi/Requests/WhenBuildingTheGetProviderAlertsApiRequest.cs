using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingTheGetProviderAlertsApiRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_The_Request(
        int ukprn,
        string userId)
    {
        var actual = new GetProviderAlertsApiRequest(ukprn, userId);
        actual.GetUrl.Should().Be($"api/provider/{ukprn}/alerts?userId={userId}");
    }
}