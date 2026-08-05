using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

internal class WhenBuildingGetEmployerProfileByLegalEntityIdApiRequest
{
    [Test]
    [MoqAutoData]
    public void Then_Fields_Are_As_Expected(long accountLegalEntityId)
    {
        // Arrange
        var actual = new GetEmployerProfileByLegalEntityIdApiRequest(accountLegalEntityId);
        // Assert
        actual.GetUrl.Should().Be($"api/employer/profiles/{accountLegalEntityId}");
    }
}