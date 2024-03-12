using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingGetCandidateSkillsAndStrengthsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new GetCandidateSkillsAndStrengthsItemApiRequest(applicationId, candidateId);

        actual.GetUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/about-you");
    }
}
