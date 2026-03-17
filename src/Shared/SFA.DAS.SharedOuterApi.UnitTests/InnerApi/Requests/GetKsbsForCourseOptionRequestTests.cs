using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.Courses;

public class GetKsbsForCourseOptionRequestTests
{
    [Test, AutoData]
    public void GetUrl_WithLarsCode_ReturnsExpectedEndpointAndPreservesLarsCode(string larsCode)
    {
        var request = new GetKsbsForCourseOptionRequest(larsCode);

        request.LarsCode.Should().Be(larsCode);
        request.GetUrl.Should().Be($"api/courses/standards/{larsCode}/options/core/ksbs");
    }
}