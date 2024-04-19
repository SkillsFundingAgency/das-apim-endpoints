using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CoursesApi.Requests;

public class WhenBuildingGetAllFrameworksApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetAllFrameworksApiRequest();

        actual.GetUrl.Should().Be("api/courses/frameworks");
    }
}