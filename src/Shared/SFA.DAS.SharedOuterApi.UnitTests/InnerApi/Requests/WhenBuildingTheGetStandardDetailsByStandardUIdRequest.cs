using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetStandardDetailsByStandardUIdRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingRequest_ReturnsCorrectUrl(GetStandardDetailsByIdRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/lookup/{actual.Id}");
    }
}