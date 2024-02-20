using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingPutUpdateJobsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        Guid jobId)
    {
        var actual = new PutUpsertWorkHistoryApiRequest(applicationId, candidateId, jobId, new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData());

        actual.PutUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/work-history/{jobId}");
    }
}
