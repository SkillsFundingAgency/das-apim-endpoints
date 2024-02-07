using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.PostDeleteJobRequest;


namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    public class WhenBuildingDeleteJobRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        PostDeleteJobRequestData data)
        {
            var actual = new PostDeleteJobRequest(applicationId, candidateId, data);

            actual.PostUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/work-history/delete");
        }
    }
}
