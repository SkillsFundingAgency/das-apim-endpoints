using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingTheGetEmployerAlertsApiRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_The_Request(
        long employerId,
        string userId)
    {
        var actual = new GetEmployerAlertsApiRequest(employerId, userId);
        actual.GetUrl.Should().Be($"api/employer/{employerId}/alerts?userId={userId}");
    }
}
