using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class GetAllProviderCoursesRequestTests
{
    [Test, AutoData]
    public void Constructor_ConstructsRequest(
     int ukprn,
     CourseType courseType)
    {
        var sut = new GetAllProviderCoursesRequest(ukprn, courseType);

        sut.GetUrl.Should()
                .Be($"providers/{ukprn}/courses?excludeCoursesWithoutLocation={false}&courseType={courseType}");
    }
}
