using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    [TestFixture]
    public class WhenBuildingPutUpsertEmploymentLocationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId,
            Guid id)
        {
            var actual = new PutUpsertEmploymentLocationsApiRequest(applicationId, candidateId, id, new PutUpsertEmploymentLocationsApiRequest.PutUpsertEmploymentLocationsApiRequestData());
            actual.PutUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/employment-locations/{id}");
        }
    }
}