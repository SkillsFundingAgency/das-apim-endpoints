using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetStandardDetailsByStandardUIdRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingByIdRequest_ReturnsExpectedUrl(GetStandardDetailsByIdRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/standards/{actual.Id}");
    }
}