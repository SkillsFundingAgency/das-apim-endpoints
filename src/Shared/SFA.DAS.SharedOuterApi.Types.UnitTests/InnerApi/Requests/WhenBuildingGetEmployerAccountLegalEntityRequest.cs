using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetEmployerAccountLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string href)
        {
            var actual = new GetEmployerAccountLegalEntityRequest(href);

            actual.GetUrl.Should().Be($"{href}");
        }
    }
}