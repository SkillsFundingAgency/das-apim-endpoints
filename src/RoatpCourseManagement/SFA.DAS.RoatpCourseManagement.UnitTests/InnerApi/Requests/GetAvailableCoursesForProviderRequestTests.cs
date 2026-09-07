using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class GetAvailableCoursesForProviderRequestTests
{
    [Test]
    public void GetAvailableCoursesForProviderRequest_Returns_Expected_Url()
    {
        // Arrange
        var ukprn = 12345678;
        var courseType = CourseType.Apprenticeship;
        var expectedUrl = $"providers/{ukprn}/course-types/{courseType}/available-courses";
        // Act
        var request = new GetAvailableCoursesForProviderRequest(ukprn, courseType);
        // Assert
        Assert.That(request.GetUrl, Is.EqualTo(expectedUrl));
    }
}
