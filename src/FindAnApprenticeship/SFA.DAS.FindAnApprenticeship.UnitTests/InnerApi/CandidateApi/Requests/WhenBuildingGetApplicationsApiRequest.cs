using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingGetApplicationsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(Guid candidateId)
    {
        var actual = new GetApplicationsApiRequest(candidateId);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}/applications");
    }

    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(Guid candidateId, ApplicationStatus status)
    {
        var actual = new GetApplicationsApiRequest(candidateId, status);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}/applications?status={status}");
    }

}
