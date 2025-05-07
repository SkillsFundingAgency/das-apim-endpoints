using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    [TestFixture]
    public class WhenBuildingPostApplicationsCountApiRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Post_Url_Is_Correct(
            Guid candidateId)
        {
            // Act
            var request = new PostApplicationsCountApiRequest(candidateId, new PostApplicationsCountApiRequest.PostApplicationsCountApiRequestData(
                []));
            var actual = request.PostUrl;
            // Assert
            actual.Should().Be($"api/candidates/{candidateId}/applications/count");
        }

        [Test, MoqAutoData]
        public void Then_The_Post_Data_Is_Correct(
            PostApplicationsCountApiRequest request)
        {
            // Act
            var actual = request.Data;
            // Assert
            actual.Should().BeEquivalentTo(request.Data);
        }
    }
}