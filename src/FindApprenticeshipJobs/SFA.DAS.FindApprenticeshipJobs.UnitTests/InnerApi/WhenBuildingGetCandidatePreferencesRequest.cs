using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetCandidatePreferencesRequest
{
    [Test, AutoData]
    public void Then_The_Query_Is_Correctly_Constructed()
    {
        var actual = new GetCandidatePreferencesRequest();

        actual.GetUrl.Should().Be("api/referencedata/preferences");
    }
}