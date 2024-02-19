using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingGetDeleteVolunteeringOrWorkExperienceApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid id,
        Guid candidateId,
        Guid applicationId)
    {
        var actual = new GetDeleteVolunteeringOrWorkExperienceApiRequest(applicationId, candidateId, id);

        actual.GetUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/volunteeringorworkexperience/{id}");
    }
}
