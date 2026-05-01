using System;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

public class WhenBuildingPatchCandidateApplicationRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(Guid applicationId, Guid candidateId)
    {
        var actual = new PatchApplicationApiRequest(applicationId, candidateId, null);

        actual.PatchUrl.Should().Be($"api/Candidates/{candidateId}/applications/{applicationId}");
    }
}