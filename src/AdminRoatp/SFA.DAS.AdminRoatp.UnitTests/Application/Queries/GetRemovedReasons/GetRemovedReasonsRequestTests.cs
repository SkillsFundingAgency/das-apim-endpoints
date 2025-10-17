using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetRemovedReasons;
public class GetRemovedReasonsRequestTests
{
    [Test]

    public void GetRemovedReasonsRequest_GetUrl_ReturnsCorrectGetUrl()
    {
        // Arrange
        GetRemovedReasonsRequest sut = new();
        var exapectedUrl = "removed-reasons";

        // Act
        var url = sut.GetUrl;

        // Assert
        url.Should().Be(exapectedUrl);
    }
}