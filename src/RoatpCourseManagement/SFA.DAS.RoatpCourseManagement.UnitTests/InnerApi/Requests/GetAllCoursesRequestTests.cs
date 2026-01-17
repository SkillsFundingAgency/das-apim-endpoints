using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;
public class GetAllCoursesRequestTests
{
    [Test, AutoData]
    public void Constructor_ConstructsRequest(
     CourseType courseType)
    {
        var sut = new GetAllCoursesRequest(courseType);

        sut.GetUrl.Should()
                .Be($"standards?coursetype={courseType}");
    }
}