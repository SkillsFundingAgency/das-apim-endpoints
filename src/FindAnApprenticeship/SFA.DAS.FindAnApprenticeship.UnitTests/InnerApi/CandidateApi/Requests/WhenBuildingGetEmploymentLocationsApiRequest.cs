using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    [TestFixture]
    public class WhenBuildingGetEmploymentLocationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId)
        {
            var actual = new GetEmploymentLocationsApiRequest(candidateId, applicationId);

            actual.GetUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/employment-locations");
        }
    }
}