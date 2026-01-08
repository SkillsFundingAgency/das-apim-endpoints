using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;
public class GetAllStandardsRequestTests
{
    [Test]
    public void CourseTypeIsNull_Constructor_ConstructsRequest()
    {
        var sut = new GetAllStandardsRequest(null);

        sut.GetUrl.Should()
                .Be($"standards");
    }

    [Test, AutoData]
    public void CourseTypeHasValue_Constructor_ConstructsRequest(
     CourseType courseType)
    {
        var sut = new GetAllStandardsRequest(courseType);

        sut.GetUrl.Should()
                .Be($"standards?coursetype={courseType}");
    }
}
