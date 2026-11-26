using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class DeleteProviderAllowedCourseRequestTests
{
    [Test, MoqAutoData]
    public void WhenBuildingRequest_ThenUrlIsSetCorrectly(
        int ukprn,
        string larsCode,
        string userId,
        string userDisplayName)
    {
        // Act
        var request = new DeleteProviderAllowedCourseRequest(ukprn, larsCode, userId, userDisplayName);

        // Assert
        request.DeleteUrl.Should().Be($"providers/{ukprn}/allowed-courses/{larsCode}?userId={Uri.EscapeDataString(userId)}&userDisplayName={Uri.EscapeDataString(userDisplayName)}");
    }
}
