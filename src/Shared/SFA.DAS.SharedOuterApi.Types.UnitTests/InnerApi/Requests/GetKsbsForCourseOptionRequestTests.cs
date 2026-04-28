using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class GetKsbsForCourseOptionRequestTests
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingGetKsbsForCourseOptionRequest_ReturnsCorrectUrl(string larsCode)
    {
        var request = new GetKsbsForCourseOptionRequest(larsCode);

        request.LarsCode.Should().Be(larsCode);
        request.GetUrl.Should().Be($"api/courses/standards/{larsCode}/options/core/ksbs");
    }
}