using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetCandidateApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid candidateId)
    {
        var actual = new GetCandidateApiRequest(candidateId.ToString());

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}");
    }
}