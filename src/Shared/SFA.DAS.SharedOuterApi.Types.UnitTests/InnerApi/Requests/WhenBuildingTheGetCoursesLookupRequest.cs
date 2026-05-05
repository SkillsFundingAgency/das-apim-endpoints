using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetCoursesLookupRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingLookupRequest_ReturnsExpectedUrl(GetCoursesLookupRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/lookup/{actual.Id}");
    }
}