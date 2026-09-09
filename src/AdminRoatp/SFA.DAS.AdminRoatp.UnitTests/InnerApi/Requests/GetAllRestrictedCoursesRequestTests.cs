using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetAllRestrictedCoursesRequestTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void WhenCreatingRequest_ThenSetsCorrectUrl(bool restricted)
    {
        // Act
        var sut = new GetAllRestrictedCoursesRequest(restricted);

        // Assert
        sut.GetUrl.Should().Be($"restricted-courses?restricted={restricted}");
    }
}
