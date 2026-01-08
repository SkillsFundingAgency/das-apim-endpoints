using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;
public class GetAllCoursesRequestTests
{
    [Test, AutoData]
    public void CourseTypeIsNull_Constructor_ConstructsRequest(
     int ukprn)
    {
        var sut = new GetAllCoursesRequest(ukprn, null);

        sut.GetUrl.Should()
                .Be($"providers/{ukprn}/courses?excludeCoursesWithoutLocation={false}");
    }

    [Test, AutoData]
    public void CourseTypeHasValue_Constructor_ConstructsRequest(
     int ukprn,
     CourseType courseType)
    {
        var sut = new GetAllCoursesRequest(ukprn, courseType);

        sut.GetUrl.Should()
                .Be($"providers/{ukprn}/courses?excludeCoursesWithoutLocation={false}&courseType={courseType}");
    }
}
