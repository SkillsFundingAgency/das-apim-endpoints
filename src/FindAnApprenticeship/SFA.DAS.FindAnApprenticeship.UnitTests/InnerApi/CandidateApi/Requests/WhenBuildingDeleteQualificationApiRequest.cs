using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;

public class WhenBuildingDeleteQualificationApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(Guid id, Guid applicationId, Guid candidateId)
    {
        var actual = new DeleteQualificationApiRequest(candidateId, applicationId, id);

        actual.DeleteUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/Qualifications/{id}");
    }
}