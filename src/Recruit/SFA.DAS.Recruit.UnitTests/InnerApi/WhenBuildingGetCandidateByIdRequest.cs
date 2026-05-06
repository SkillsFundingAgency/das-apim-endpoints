using System;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetCandidateByIdRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed(Guid candidateId)
    {
        var actual = new GetCandidateByIdApiRequest(candidateId);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}");
    }
}