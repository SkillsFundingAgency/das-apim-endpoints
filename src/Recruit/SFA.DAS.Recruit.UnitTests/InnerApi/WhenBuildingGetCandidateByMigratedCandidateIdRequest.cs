using System;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetCandidateByMigratedCandidateIdRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed(Guid migratedCandidateId)
    {
        var actual = new GetCandidateByMigratedCandidateIdApiRequest(migratedCandidateId);

        actual.GetUrl.Should().Be($"api/candidates/migrated/{migratedCandidateId}");
    }
}