using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    [TestFixture]
    public class WhenBuildingGetApplicationsCountApiRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Get_Url_Is_Correct(
            Guid candidateId,
            ApplicationStatus status)
        {
            // Act
            var request = new GetApplicationsCountApiRequest(candidateId, status);
            var actual = request.GetUrl;
            // Assert
            actual.Should().Be($"api/candidates/{candidateId}/applications/count?status={status}");
        }
    }
}