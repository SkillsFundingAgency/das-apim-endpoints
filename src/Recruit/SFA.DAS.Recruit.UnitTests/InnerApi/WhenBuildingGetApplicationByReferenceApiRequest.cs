using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetApplicationByReferenceApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed(Guid candidateId, long vacancyReference)
    {
        var actual = new GetApplicationByReferenceApiRequest(candidateId, vacancyReference);
        
        actual.GetUrl.Should().Be($"api/candidates/{candidateId}/applications/GetByReference/{vacancyReference}");
    }
}