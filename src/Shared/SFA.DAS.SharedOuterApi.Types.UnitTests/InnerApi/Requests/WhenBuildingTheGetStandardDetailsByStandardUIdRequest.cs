using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetStandardDetailsByStandardUIdRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingByIdRequest_ReturnsExpectedUrl(GetStandardDetailsByIdRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/standards/{actual.Id}");
    }
}