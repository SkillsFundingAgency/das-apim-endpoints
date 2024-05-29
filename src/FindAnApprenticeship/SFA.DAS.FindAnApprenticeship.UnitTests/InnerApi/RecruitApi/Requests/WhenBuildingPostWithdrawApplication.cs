using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.RecruitApi.Requests;

public class WhenBuildingPostWithdrawApplicationRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(Guid candidateId, long vacancyReference)
    {
        var actual = new PostWithdrawApplicationRequest(candidateId, vacancyReference);

        actual.PostUrl.Should().Be($"api/applications/{candidateId}/withdraw/{vacancyReference}");
    }
}