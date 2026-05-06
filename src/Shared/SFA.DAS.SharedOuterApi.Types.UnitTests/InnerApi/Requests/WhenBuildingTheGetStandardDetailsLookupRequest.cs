using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetStandardDetailsLookupRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingLookupRequest_ReturnsExpectedUrl(GetStandardDetailsLookupRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/lookup/{actual.Id}");
    }
}