using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

[TestFixture]
public class GetStandardsLookupRequestTests
{
    [Test]
    public void GetUrl_ReturnsExpectedPath()
    {
        // Arrange
        var request = new GetStandardsLookupRequest();

        // Act
        var url = request.GetUrl;

        // Assert
        url.Should().Be("api/courses/search?filter=Active");
    }

    [Test]
    public void Implements_IGetApiRequest()
    {
        // Arrange
        var request = new GetStandardsLookupRequest();

        // Act & Assert
        (request is IGetApiRequest).Should().BeTrue();
    }
}