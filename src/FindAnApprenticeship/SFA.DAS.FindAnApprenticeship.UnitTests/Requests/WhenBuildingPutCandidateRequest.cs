using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.PutCandidateApiRequest;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;
public class WhenBuildingPutCandidateRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(Guid id, PutCandidateApiRequestData data)
    {
        var actual = new PutCandidateApiRequest(id, data);

        actual.PutUrl.Should().Be($"/api/candidates/{id}");
    }
}
