using AutoFixture.NUnit3;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetProvidersNotAllowedRequestTests
{
    [Test, AutoData]
    public void WhenCreatingRequest_ThenSetsCorrectUrl(string larsCode)
    {
        // Arrange
        var expected = $"/courses/{larsCode}/providers/not-allowed";
        GetProvidersNotAllowedRequest sut = new(larsCode);

        // Act
        var actual = sut.GetUrl;

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
