using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

internal class WhenBuildingGetEmployerProfilesByAccountIdApiRequest
{
    [Test]
    [MoqAutoData]
    public void Then_Fields_Are_As_Expected(long accountId)
    {
        // Arrange
        var actual = new GetEmployerProfilesByAccountIdApiRequest(accountId);
        // Assert
        actual.GetUrl.Should().Be($"api/employer/{accountId}/profiles");
    }
}