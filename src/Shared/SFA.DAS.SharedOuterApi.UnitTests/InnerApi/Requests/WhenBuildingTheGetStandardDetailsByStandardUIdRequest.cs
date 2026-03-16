using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardDetailsByStandardUIdRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(GetStandardDetailsByIdRequest actual)
        {
            actual.GetUrl.Should().Be($"api/courses/lookup/{actual.Id}");
        }
    }
}