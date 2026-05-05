using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetCourseLookupRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingLookupRequest_ReturnsExpectedUrl(GetCourseLookupRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/lookup/{actual.Id}");
    }
}