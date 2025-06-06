using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingTheGetAllApplicationsByIdApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(GetAllApplicationsByIdApiRequestData payload)
        {
            var actual = new GetAllApplicationsByIdApiRequest(payload);

            actual.PostUrl.Should().Be($"api/applications/getAll");
            actual.Payload.Should().BeEquivalentTo(payload);
        }
    }
}