using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingPutSkillsAndStrengthsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        Guid id)
    {
        var actual = new PutUpsertAboutYouItemApiRequest(applicationId, candidateId, id, new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData());

        actual.PutUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/about-you/{id}");
    }
}
