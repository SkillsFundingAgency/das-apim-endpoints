using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class GetAvailableCourseForProviderRequestTests
{
    [Test]
    public void GetAvailableCourseForProviderRequest_Returns_Expected_Url()
    {
        // Arrange
        var ukprn = 12345678;
        var courseType = CourseType.Apprenticeship;
        var larsCode = "LARS123";
        var expectedUrl = $"providers/{ukprn}/course-types/{courseType}/available-courses/{larsCode}";
        // Act
        var request = new GetAvailableCourseForProviderRequest(ukprn, courseType, larsCode);
        // Assert
        Assert.That(request.GetUrl, Is.EqualTo(expectedUrl));
    }
}
