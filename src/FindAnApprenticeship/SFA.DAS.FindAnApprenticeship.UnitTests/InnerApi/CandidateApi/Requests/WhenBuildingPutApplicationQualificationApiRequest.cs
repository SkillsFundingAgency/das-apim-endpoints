using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;

public class WhenBuildingPutApplicationQualificationApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_And_Data_Are_Correct(
        Guid candidateId,
        Guid applicationId,
        PutApplicationQualificationApiRequestData putApplicationQualificationApiRequestData)
    {
        var actual = new PutApplicationQualificationApiRequest(candidateId, applicationId, putApplicationQualificationApiRequestData);

        actual.PutUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/Qualifications");
        ((PutApplicationQualificationApiRequestData)actual.Data).Should().BeEquivalentTo(putApplicationQualificationApiRequestData);
    }
}