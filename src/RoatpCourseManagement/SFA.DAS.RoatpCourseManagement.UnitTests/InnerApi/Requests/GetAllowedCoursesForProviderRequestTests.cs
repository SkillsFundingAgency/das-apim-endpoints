using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class GetAllowedCoursesForProviderRequestTests
{
    [Test]
    public void GetUrl_ReturnsExpectedValue()
    {
        int ukprn = 10012002;
        CourseType courseType = CourseType.ShortCourse;
        GetAllowedCoursesForProviderRequest sut = new(ukprn, courseType);

        sut.GetUrl.Should().Be("providers/10012002/allowed-courses?courseType=ShortCourse");
    }
}
