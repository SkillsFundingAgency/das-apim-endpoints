using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingExpireEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build()
        {
            var actual = new ExpireEmployerRequestsRequest();

            actual.PutUrl.Should().Be("api/employer-requests/expire");
        }
    }
}
