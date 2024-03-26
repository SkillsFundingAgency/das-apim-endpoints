using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingGetCandidatePostcodeApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(Guid candidateId)
    {
        var actual = new GetCandidatePostcodeApiRequest(candidateId);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}/addresses");
    }
}
