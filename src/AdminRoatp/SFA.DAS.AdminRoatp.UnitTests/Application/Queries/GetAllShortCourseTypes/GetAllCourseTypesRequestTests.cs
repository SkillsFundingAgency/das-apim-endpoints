using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetAllShortCourseTypes;
public class GetAllCourseTypesRequestTests
{
    [Test]
    public void GetAllCourseTypesRequest_GetUrl_ReturnsCorrectGetUrl()
    {
        // Arrange
        var expectedGetUrl = "course-types";
        GetAllCourseTypesRequest sut = new();

        // Act
        var url = sut.GetUrl;

        // Assert
        url.Should().Be(expectedGetUrl);
    }
}