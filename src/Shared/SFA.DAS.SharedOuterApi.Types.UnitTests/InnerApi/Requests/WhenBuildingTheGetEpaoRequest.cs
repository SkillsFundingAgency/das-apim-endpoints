using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEpaoRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetEpaoRequest actual)
        {
            actual.GetUrl.Should().Be($"api/ao/assessment-organisations/{actual.EpaoId}");
        }
    }
}