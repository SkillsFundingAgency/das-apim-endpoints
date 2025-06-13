using SFA.DAS.Recruit.InnerApi.Requests;
using System;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

public class WhenBuildingPatchRecruitApplicationReviewStatusRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(Guid applicationId)
    {
        var actual = new PatchRecruitApplicationReviewStatusApiRequest(applicationId, null);

        actual.PatchUrl.Should().Be($"api/applicationReviews/{applicationId}");
    }
}