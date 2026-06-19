using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

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
