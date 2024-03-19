using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;

public class WhenBuildingPutQualificationApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_And_Data_Are_Correct(
        Guid candidateId,
        Guid applicationId,
        PutQualificationApiRequestData putQualificationApiRequestData)
    {
        var actual = new PutQualificationApiRequest(candidateId, applicationId, putQualificationApiRequestData);

        actual.PutUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/Qualifications");
        ((PutQualificationApiRequestData)actual.Data).Should().BeEquivalentTo(putQualificationApiRequestData);
    }
}