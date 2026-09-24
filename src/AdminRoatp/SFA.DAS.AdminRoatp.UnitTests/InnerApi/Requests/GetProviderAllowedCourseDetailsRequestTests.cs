using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetProviderAllowedCourseDetailsRequestTests
{
    [Test]
    public void GetUrl_ReturnsExpectedUrl()
    {
        // Arrange
        int ukprn = 12345678;
        string larsCode = "ABC123";
        var request = new GetProviderAllowedCourseDetailsRequest(ukprn, larsCode);
        // Act
        string url = request.GetUrl;
        // Assert
        Assert.That($"providers/{ukprn}/allowed-courses/{larsCode}", Is.EqualTo(url));
    }
}
