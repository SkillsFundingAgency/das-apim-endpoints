using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetCandidateByIdRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed(Guid candidateId)
    {
        var actual = new GetCandidateByIdRequest(candidateId);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}");
    }
}